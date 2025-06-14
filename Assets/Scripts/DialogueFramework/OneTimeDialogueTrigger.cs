using Managers;
using UnityEngine;

namespace DialogueFramework
{
   public class OneTimeDialogueTrigger : MonoBehaviour
   {
      public Dialogue dialogue; 
      private void OnTriggerEnter2D(Collider2D other)
      {
         if (!other.gameObject.CompareTag("Player")) return;
         DialogueManager.instance.StartDialogue(dialogue);
         Destroy(gameObject);
      }
   }
}
