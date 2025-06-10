using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;

//reference https://www.youtube.com/watch?v=DOP_G5bsySA
namespace Managers
{
    public class DialogueSystem : MonoBehaviour
    {
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI dialogueArea;
        [SerializeField] private TextMeshProUGUI _continueText;
        public Image characterIcon;
        public Image header;
        public Image background;
        public RectTransform iconTransform;
    
        private Queue<DialogueLine> _lines;
        private DialogueLine _currentLine;
    
        public bool isDialogueActive;
        private bool _isTyping;
        private bool _pause;
        public float typingSpeed = 0.2f;
        
        [FormerlySerializedAs("typeSound")] [SerializeField] private AudioClip defaultTypeSound;
        private AudioClip _typeSound;
        private float _typePitch = 1f;
        private float _typeVolume = 1f;
        private float _typeFrequency = 0.15f;
        public Animator animator;
    
        private Dialogue _currentDialogue;
        
        public static Action<string> onDialogueNextLine;

        private void Awake()
        {
            _typeSound = defaultTypeSound;
            _lines = new Queue<DialogueLine>();
        }
     
        public void StartDialogue(DialogueAsset dialogueAsset)
        {
            SetupDialogue(dialogueAsset.dialogue, dialogueAsset.name);
        }
        public void StartDialogue(Dialogue dialogue)
        {
            SetupDialogue(dialogue);
        }

        private void SetupDialogue(Dialogue dialogue, string dialogueName = "unnamed Dialogue")
        {
            isDialogueActive = true;
            animator.Play("ShowDialogue");
            _lines.Clear();
            _currentDialogue = dialogue;
            foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
            {
                _lines.Enqueue(dialogueLine);
            }
            if (RuriMovement.instance) RuriMovement.instance.controlling = false;
  
            Debug.Log($"Starting Dialogue '{dialogueName}'");
            DisplayNextDialogueLine();
        }

        public void NextLineInput(InputAction.CallbackContext context)
        {
            if (!context.started) return;
            if (isDialogueActive)
            {
                DisplayNextDialogueLine();
            }
        }

        private void DisplayNextDialogueLine()
        {
            if (_lines.Count == 0 && !_isTyping)
            {
                EndDialogue();
                return;
            }
      
            // if not currently typing
            if (!_isTyping) 
            {
                _currentLine = _lines.Dequeue();

                //Set Dialogue colors
                if (_currentLine.character)
                {
                    //if the character has an Overridden color, use it
                    characterName.text = _currentLine.characterOverride.characterName.Equals("")
                        ? _currentLine.character.characterName
                        : _currentLine.characterOverride.characterName;

                    header.color = _currentLine.characterOverride.color.a == 0f
                        ? _currentLine.character.color
                        : _currentLine.characterOverride.color;

                    Color newColor = header.color * 0.25f; // Darken the color
                    newColor.a = header.color.a; // Preserve the original alpha
                    background.color = newColor;

                    //set sprites
                    if (!_currentLine.character.characterSprite && !_currentLine.characterOverride.characterSprite)
                    {
                        characterIcon.gameObject.SetActive(false);
                    }
                    else
                    {
                        characterIcon.gameObject.SetActive(true);

                        characterIcon.sprite = _currentLine.characterOverride.characterSprite
                            ? _currentLine.characterOverride.characterSprite
                            : _currentLine.character.characterSprite;
                    }

                    //voice and pitch
                    if (_currentLine.character.blipSound != null)
                    {
                        _typeSound = _currentLine.character.blipSound.Length > 0
                            ? _currentLine.character.blipSound[Random.Range(0, _currentLine.character.blipSound.Length)]
                            : defaultTypeSound;
                    }
                    
                    _typePitch = !_currentLine.characterOverride.pitch.Equals(0)
                        ? _currentLine.characterOverride.pitch
                        : _currentLine.character.pitch;
                    
                    _typeVolume = !_currentLine.characterOverride.volume.Equals(0)
                        ? _currentLine.characterOverride.volume
                        : _currentLine.character.volume;
                    
                    _typeFrequency = !_currentLine.characterOverride.frequency.Equals(0)
                        ? _currentLine.characterOverride.frequency
                        : _currentLine.character.frequency;
                }


                if (_currentLine.soundEffect)
                {
                    AudioManager.instance.PlaySFX(_currentLine.soundEffect);

                }


                if (!_currentLine.customEvent.Equals(""))
                {
                    onDialogueNextLine?.Invoke(_currentLine.customEvent);   
                }
                
                //alignment
                if (_currentLine.right)
                {
                    iconTransform.anchorMax = new Vector2(1,1);
                    iconTransform.anchorMin = new Vector2(1,1);
                    iconTransform.pivot = new Vector2(1,0);
                }else{
                    iconTransform.anchorMax = new Vector2(0,1);
                    iconTransform.anchorMin = new Vector2(0,1);
                    iconTransform.pivot = new Vector2(0,0);
                }
                
                StopAllCoroutines();
                _isTyping = true;
                StartCoroutine(TypeSentence(_currentLine));
            }
            else //skip typing
            {
                StopAllCoroutines();
                dialogueArea.text = _currentLine.line;
                _isTyping = false;
            }
        
        }
        private IEnumerator TypeSentence(DialogueLine currentLine)
        {
            
            dialogueArea.text = "";
            Coroutine talkSoundCoroutine = StartCoroutine(PlayTalkSound());
            int letterCount = 0;
            int maxLetters = currentLine.line.Trim().ToCharArray().Length;
            foreach (char letter in currentLine.line.ToCharArray())
            {
                _pause = false;
                dialogueArea.text += letter;
                float additionalPause = 0f;
                if ((letterCount+1) != maxLetters){ 
                    additionalPause = letter switch
                    {
                        '—' => 0.15f,
                        ',' => 0.20f,
                        '.' => 0.4f,
                        '?' => 0.4f,
                        _ => 0f
                    };
                }
                

                if (additionalPause > 0f) _pause = true;
                letterCount++;
                yield return new WaitForSeconds(typingSpeed+additionalPause);
            }
            if (talkSoundCoroutine != null)
            {
                StopCoroutine(talkSoundCoroutine);
                talkSoundCoroutine = null;
            }
            _isTyping = false;
        }
        private IEnumerator PlayTalkSound()
        {
            while (true)
            {
                if (!_pause)
                {
                    AudioManager.instance.PlaySFX(_typeSound, _typeVolume, _typePitch);
                }
                yield return new WaitForSeconds(_typeFrequency);
            }
        }

        private void Update()
        {
            _continueText.enabled = !_isTyping;
        }

        void EndDialogue()
        {
            StopAllCoroutines();
            _isTyping = false;
            isDialogueActive = false;
            animator.Play("HideDialogue");
            if(RuriMovement.instance)RuriMovement.instance.controlling = true;
            GameEvents.DialogueEnded(_currentDialogue.dialogueName);
        }
    }
}
