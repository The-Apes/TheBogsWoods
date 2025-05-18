using Managers;
using UnityEngine;

public class OneTimeDialogueTrigger : MonoBehaviour
{
   public DialogueAsset dialogue; 
   private void OnCollisionEnter2D(Collision2D other)
   {
      if (!other.gameObject.CompareTag("Player")) return;
      DialogueManager.instance.StartDialogue(dialogue);
      Destroy(gameObject);
   }
}
