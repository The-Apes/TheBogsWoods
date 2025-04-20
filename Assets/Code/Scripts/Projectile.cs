using UnityEngine;

public class Projectile : MonoBehaviour
{
   [SerializeField] private float lifetime = 2f; // Lifetime of the projectile in seconds
   
   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.isTrigger) return;
      // ReSharper disable once Unity.UnknownLayer
      if (other.gameObject.layer == LayerMask.NameToLayer("Enemy") || other.gameObject.layer == LayerMask.NameToLayer("Player")) return;
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
