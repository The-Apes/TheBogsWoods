using System;
using UnityEngine;

public class StickPickup : MonoBehaviour
{
   [SerializeField] private GameObject attackControlsPrompt;

   private void Start()
   {
      attackControlsPrompt.SetActive(false);
   }

   //public GameObject eligiblePlayer;
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("Player"))
      {
         RuriMovement ruri = collision.GetComponent<RuriMovement>();
         if (ruri)
         {
            ruri.hasWeapon = true;
            attackControlsPrompt.SetActive(true);
            Destroy(gameObject);
         }
      }  
   }
}
