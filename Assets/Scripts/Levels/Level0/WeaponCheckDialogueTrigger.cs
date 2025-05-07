using Managers;
using UnityEngine;

namespace Level.Level0.Scripts
{
   public class WeaponCheckDialogueTrigger : MonoBehaviour
   {
      [SerializeField] private Dialogue noWeaponDialogue; 
      [SerializeField] private Dialogue weaponDialogue;
      
      private void OnCollisionEnter2D(Collision2D other)
      {
         if (!other.gameObject.CompareTag("Player")) return;
         RuriMovement ruri = other.gameObject.GetComponent<RuriMovement>();
         DialogueManager.instance.StartDialogue(ruri.hasWeapon ? weaponDialogue : noWeaponDialogue);
         Destroy(gameObject);
      }
   }
}
