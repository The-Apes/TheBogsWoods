using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//reference https://www.youtube.com/watch?v=DOP_G5bsySA
namespace Managers
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance;
    
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI dialogueArea;
        public Image characterIcon;
        public Image header;
        public Image background;
        public RectTransform iconTransform;
    
        private Queue<DialogueLine> _lines;
        private DialogueLine _currentLine;
    
        public bool isDialogueActive = false;
        private bool _isTyping;
        public float typingSpeed = 0.2f;
        public AudioClip typeSound;
        public Animator animator;
    
        private Dialogue currentDialogue;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
         
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                _lines = new Queue<DialogueLine>();
            } else
            {
                Destroy(gameObject); 
            }
            
        
        }

        public void StartDialogue(Dialogue dialogue)
        {
            isDialogueActive = true;
            animator.Play("ShowDialogue");
            _lines.Clear();
            currentDialogue = dialogue;
            foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
            {
                _lines.Enqueue(dialogueLine);
            }
            if (RuriMovement.instance != null) RuriMovement.instance.controlling = false;
  
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

        public void DisplayNextDialogueLine()
        {
            if (_lines.Count == 0)
            {
                EndDialogue();
                return;
            }
      
            if (!_isTyping)
            {
                _currentLine = _lines.Dequeue();
        
                characterName.text = _currentLine.character.characterName;
                header.color = _currentLine.character.color;
        
                Color newColor = _currentLine.character.color * 0.25f; // Darken the color
                newColor.a = _currentLine.character.color.a; // Preserve the original alpha
                background.color = newColor; // Assign the new color
        
                if (_currentLine.character.characterSprite == null)
                {
                    characterIcon.gameObject.SetActive(false);
                }
                else
                {
                    characterIcon.gameObject.SetActive(true);
                    characterIcon.sprite = _currentLine.character.characterSprite;
                }

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
            else
            {
                StopAllCoroutines();
                dialogueArea.text = _currentLine.line;
                _isTyping = false;
            }
        
        }
        private IEnumerator TypeSentence(DialogueLine currentLine)
        {
            dialogueArea.text = "";
            foreach (char letter in currentLine.line.ToCharArray())
            {
                dialogueArea.text += letter;
                float randomValue = UnityEngine.Random.Range(0.5f, 1.5f);
                AudioManager.instance.PlaySound(typeSound, randomValue);
                yield return new WaitForSeconds(typingSpeed);
            }
            _isTyping = false;
        }

        void EndDialogue()
        {
            isDialogueActive = false;
            animator.Play("HideDialogue");
            if( RuriMovement.instance != null)RuriMovement.instance.controlling = true;
            GameEvents.DialogueEnded(currentDialogue.dilaogueName);
        }
    }
}
