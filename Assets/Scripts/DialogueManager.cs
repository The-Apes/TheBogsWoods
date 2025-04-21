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
    public static DialogueManager Instance;
    
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;

    private Queue<DialogueLine> lines;
    
    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;
    public Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        lines = new Queue<DialogueLine>();
    }

    public void StartDialogue(Dialogue dialogue)
  {
  isDialogueActive = true;
  animator.Play("ShowDialogue");
  lines.Clear();
  foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
  {
      lines.Enqueue(dialogueLine);
  }

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
      if (lines.Count == 0)
      {
          EndDialogue();
          return;
      }
        DialogueLine currentLine = lines.Dequeue();
        characterIcon.sprite = currentLine.character.characterSprite;
        characterName.text = currentLine.character.characterName;
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
    }
}
