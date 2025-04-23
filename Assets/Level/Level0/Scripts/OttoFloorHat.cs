using System;
using UnityEngine;

public class OttoFloorHat : MonoBehaviour
{
    
    private void DialogueEnd(string text)
    {
        if(text != "LevelStartDialogue") return;
        GameEvents.OnDialogueEnd -= DialogueEnd;
        Destroy(gameObject);
    }
    public void Start()
    {
        GameEvents.OnDialogueEnd += DialogueEnd; // Subscribe to the dialogue end event
    }

    public void OnDisable()
    {
        
    }
}
