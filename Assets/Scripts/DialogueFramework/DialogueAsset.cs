using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogueFramework
{
    //reference https://www.youtube.com/watch?v=DOP_G5bsySA

    [Serializable]
    public class DialogueLine
    {
        public DialogueCharacter character;
        public DialogueCharacterOverride characterOverride;
        [TextArea(3, 10)]
        public string line; 
        public bool right; // Whether to align the text to the right
        public AudioClip soundEffect;
        public string customEvent;

    }   

    [Serializable]
    public class Dialogue
    {
        [NonSerialized] public string dialogueName; 
        public List<DialogueLine> dialogueLines = new List<DialogueLine>();
        public string endCustomEvent;
    }


    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
    public class DialogueAsset : ScriptableObject
    {
        public Dialogue dialogue;
   
        private void OnValidate()
        {
            if (dialogue != null)
            {
                dialogue.dialogueName = name; // grabs the SO’s file name
          
            }
        }
    }
}