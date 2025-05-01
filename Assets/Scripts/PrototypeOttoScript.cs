using System;
using Managers;
using UnityEngine;

public class PrototypeOttoScript : MonoBehaviour
{
    public void Start()
    {
        GameEvents.OnDialogueEnd += OnDialogueComplete;
    }

    public DialogueAsset dialogue; // The dialogue to be triggered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            RuriMovement ruri = collision.GetComponent<RuriMovement>();
            if (ruri)
            {
                DialogueManager.Instance.StartDialogue(dialogue.dialogue);
            }
        }  
    }
    private void OnDialogueComplete(string dialogue)
    {
        if (dialogue == "Otto")
        {
            RuriMovement.instance.hasOtto = true;
            RuriMovement.instance.AddOtto();
            GameEvents.OnDialogueEnd -= OnDialogueComplete;
            Destroy(gameObject);
        }
    }
    
}
