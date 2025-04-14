using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   [SerializeField] private float lifetime = 2f; // Lifetime of the projectile in seconds
   
   public void onTriggerEnter2D(Collider2D collision)
   {
      Destroy(gameObject);
   }

   private void Update()
   {
      lifetime -= Time.deltaTime;
      if (lifetime <= 0)
      {
         Destroy(gameObject);
      }
   }
}
