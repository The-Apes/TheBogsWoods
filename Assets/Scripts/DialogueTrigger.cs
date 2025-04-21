using System.Collections.Generic;
using UnityEngine;
//reference https://www.youtube.com/watch?v=DOP_G5bsySA

[System.Serializable] // This attribute allows the class to be edited in the Unity Inspector
public class DialogueCharacter
{
    public string characterName; // Name of the character
    public Sprite characterSprite; // Sprite representing the character
    public string[] dialogueLines; // Array of dialogue lines for the character
}
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character; // The character speaking the line
    [TextArea(3, 10)]
    public string line; // The actual dialogue line
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
public class DialogueTrigger : MonoBehaviour
{
   public Dialogue dialogue;
   public void TriggerDialogue()
   {
       DialogueManager.Instance.StartDialogue(dialogue);
   }

}
