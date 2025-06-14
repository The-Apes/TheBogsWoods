using System;
using DialogueFramework;
using Managers;
using Player;
using UnityEngine;

namespace Levels.Level0
{
   public class WeaponCheckDialogueTrigger : MonoBehaviour
   {
      [SerializeField] private Dialogue noWeaponDialogue; 
      [SerializeField] private Dialogue weaponDialogue;
      
      private void OnTriggerEnter2D(Collider2D other)
      {
         if (!other.gameObject.CompareTag("Player")) return;
         RuriMovement ruri = other.gameObject.GetComponent<RuriMovement>();
         DialogueManager.instance.StartDialogue(ruri.hasWeapon ? weaponDialogue : noWeaponDialogue);
         if(ruri.hasWeapon) Destroy(gameObject);
      }


      private void Start()
      {
         
      }
   }
}
