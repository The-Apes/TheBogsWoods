using Managers;
using UnityEngine;

namespace Saving
{
 public class SaveGameTrigger : MonoBehaviour
 {
  private void OnTriggerEnter2D(Collider2D other)
  {
   if (!other.gameObject.CompareTag("Player")) return;
   SaveManager.instance.SaveGame();
  }
 }
}
