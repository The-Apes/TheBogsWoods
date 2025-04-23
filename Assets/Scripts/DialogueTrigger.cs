using System.Collections.Generic;
using UnityEngine;
//reference https://www.youtube.com/watch?v=DOP_G5bsySA

[System.Serializable] // This attribute allows the class to be edited in the Unity Inspector
public class DialogueCharacter
{
    public string characterName; // Name of the character
    public Sprite characterSprite; // Sprite representing the character
    public Color color = Color.white;
}
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character; // The character speaking the line
    [TextArea(3, 10)]
    public string line; // The actual dialogue line
    public bool right = false; // Whether to align the text to the right
}   

[System.Serializable]
public class Dialogue
{
    public string dilaogueName; // Name of the dialogue
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class DialogueTrigger : ScriptableObject
{
   public Dialogue dialogue;
   public void TriggerDialogue()
   {
       DialogueManager.instance.StartDialogue(dialogue);
   }

}
