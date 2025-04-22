using System;
using UnityEngine;

public class OneTimeDialogueTrigger : MonoBehaviour
{
   [SerializeField] private Dialogue dialogue; // The dialogue to be triggered
   private void OnCollisionEnter2D(Collision2D other)
   {
      if (other.gameObject.CompareTag("Player"))
      {
         DialogueManager.instance.StartDialogue(dialogue);
         Destroy(gameObject);
      }
   }
}
