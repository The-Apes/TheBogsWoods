using System;
using Managers;
using UnityEngine;

public class DestroyableObject : MonoBehaviour
{
  public int health = 1;
  
  [Header("sounds")]
  [SerializeField] private AudioClip HitSound;
  [SerializeField] private AudioClip destroySound;
  private void OnTriggerEnter2D(Collider2D other)
  {
   if (other.GetComponent<HitBox>())
   {
    health--;
    if (health <= 0)
    {
     AudioManager.instance.PlaySound(destroySound);
     Destroy(gameObject);
    }
    else
    {
     AudioManager.instance.PlaySound(HitSound);
    }
   }
  }
 }

