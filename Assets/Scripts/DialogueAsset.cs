using System.Collections.Generic;
using Managers;
using UnityEngine;

//reference https://www.youtube.com/watch?v=DOP_G5bsySA

[System.Serializable] // This attribute allows the class to be edited in the Unity Inspector
public class DialogueCharacter
{
    public string characterName; 
    public Sprite characterSprite; // Sprite representing the character
    public Color color = Color.white;
    public float pitch = 1f; 
    public AudioClip voice; // Audio Blip for the character's voice
}
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character; 
    [TextArea(3, 10)]
    public string line; 
    public bool right; // Whether to align the text to the right
    public AudioClip soundEffect;
}   

[System.Serializable]
public class Dialogue
{
    public string dialogueName; 
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class DialogueAsset : ScriptableObject
{
   public Dialogue dialogue;
   public void TriggerDialogue()
   {
       DialogueManager.instance.StartDialogue(dialogue);
   }

}
