using Managers;
using UnityEngine;

public class PrototypeOttoScript : MonoBehaviour
{
    public void Start()
    {
        GameEvents.onDialogueEnd += OnDialogueComplete;
    }

    public DialogueAsset dialogue; // The dialogue to be triggered
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;
        
        RuriMovement ruri = collision.GetComponent<RuriMovement>();
        if (ruri)
        {
            DialogueManager.instance.StartDialogue(dialogue.dialogue);
        }
    }
    private void OnDialogueComplete(string currentDialogue)
    {
        if (currentDialogue != "Otto") return;
        
        RuriMovement.instance.hasOtto = true;
        RuriMovement.instance.AddOtto();
        GameEvents.onDialogueEnd -= OnDialogueComplete;
        Destroy(gameObject);
    }
    
}
