using UnityEngine;

public class StickPickup : MonoBehaviour
{
   //public GameObject eligiblePlayer;
   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.CompareTag("Player"))
      {
         RuriMovement ruri = collision.GetComponent<RuriMovement>();
         if (ruri)
         {
            ruri.hasWeapon = true;
            Destroy(gameObject);
         }
      }  
   }
}
