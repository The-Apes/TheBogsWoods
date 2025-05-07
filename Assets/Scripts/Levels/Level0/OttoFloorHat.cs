using System;
using UnityEngine;

public class OttoFloorHat : MonoBehaviour
{
    
    private void DialogueEnd(string text)
    {
        if(text != "LevelStartDialogue") return;
        GameEvents.onDialogueEnd -= DialogueEnd;
        Destroy(gameObject);
    }
    public void Start()
    {
        GameEvents.onDialogueEnd += DialogueEnd; // Subscribe to the dialogue end event
    }

    public void OnDisable()
    {
        GameEvents.onDialogueEnd -= DialogueEnd;
    }
}
