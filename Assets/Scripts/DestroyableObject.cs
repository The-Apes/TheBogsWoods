using Managers;
using UnityEngine;
using UnityEngine.Serialization;

public class DestroyableObject : MonoBehaviour
{
  public int health = 1;
  
  [FormerlySerializedAs("HitSound")]
  [Header("sounds")]
  [SerializeField] private AudioClip hitSound;
  [SerializeField] private AudioClip destroySound;
  private void OnTriggerEnter2D(Collider2D other)
  {
   if (!other.GetComponent<HitBox>()) return;
   health--;
   if (health <= 0)
   {
    AudioManager.instance.PlaySound(destroySound);
    Destroy(gameObject);
   }
   else
   {
    AudioManager.instance.PlaySound(hitSound);
   }
  }
 }

