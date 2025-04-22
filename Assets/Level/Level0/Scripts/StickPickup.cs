using System;
using UnityEngine;

public class StickPickup : MonoBehaviour
{
   [SerializeField] private AttackTutorialPrompt attackControlsPrompt;

   //public GameObject eligiblePlayer;
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("Player"))
      {
         RuriMovement ruri = collision.GetComponent<RuriMovement>();
         if (ruri)
         {
            ruri.hasWeapon = true;
            attackControlsPrompt.ShowPrompt();
            Destroy(gameObject);
         }
      }  
   }
}
