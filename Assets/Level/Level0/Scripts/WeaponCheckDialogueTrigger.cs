using UnityEngine;

namespace Level.Level0.Scripts
{
   public class WeaponCheckDialogueTrigger : MonoBehaviour
   {
      [SerializeField] private Dialogue noWeaponDialogue; // The dialogue to be triggered
      [SerializeField] private Dialogue weaponDialogue; // The dialogue to be triggered

      private void OnCollisionEnter2D(Collision2D other)
      {
         if (other.gameObject.CompareTag("Player"))
         {
            RuriMovement ruri = other.gameObject.GetComponent<RuriMovement>();
            DialogueManager.instance.StartDialogue(ruri.hasWeapon ? weaponDialogue : noWeaponDialogue); //freaky ahh itenary
            Destroy(gameObject);
         }
      }
   }
}
