using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

//reference https://www.youtube.com/watch?v=DOP_G5bsySA
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    
    
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    public Image characterIcon;
    public Image header;
    public Image background;
    public RectTransform iconTransform;
    
    private Queue<DialogueLine> _lines;
    
    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;
    public Animator animator;
    
    private Dialogue currentDialogue;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        _lines = new Queue<DialogueLine>();
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

  RuriMovement.instance.controlling = false;
  
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
        DialogueLine currentLine = _lines.Dequeue();
        
        characterName.text = currentLine.character.characterName;
        header.color = currentLine.character.color;
        
        Color newColor = currentLine.character.color * 0.25f; // Darken the color
        newColor.a = currentLine.character.color.a; // Preserve the original alpha
        background.color = newColor; // Assign the new color
        
        if (currentLine.character.characterSprite == null)
        {
            characterIcon.gameObject.SetActive(false);
        }
        else
        {
            characterIcon.gameObject.SetActive(true);
            characterIcon.sprite = currentLine.character.characterSprite;
        }

        if (currentLine.right)
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
        StartCoroutine(TypeSentence(currentLine));
  }
    private IEnumerator TypeSentence(DialogueLine currentLine)
    {
        dialogueArea.text = "";
        foreach (char letter in currentLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        animator.Play("HideDialogue");
        RuriMovement.instance.controlling = true;
        GameEvents.DialogueEnded(currentDialogue.dilaogueName);
    }
}
