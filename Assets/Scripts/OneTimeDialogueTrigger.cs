using System;
using UnityEngine;

public class OneTimeDialogueTrigger : MonoBehaviour
{
   public DialogueTrigger dialogue; // The dialogue to be triggered
   private void OnCollisionEnter2D(Collision2D other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         DialogueManager.Instance.StartDialogue(dialogue.dialogue);
         Destroy(gameObject);
      }
   }
}
