using System;
using UnityEngine;

namespace Util
{
  public class VisibilityTrigger : MonoBehaviour
  {
    [SerializeField] private GameObject objectToToggle;
    [SerializeField] private bool makeVisible = true;
    [SerializeField] private bool controlVisibilityOnStart = true;


    private void Start()
    {
      if (controlVisibilityOnStart) objectToToggle.SetActive(!makeVisible);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
      if (!other.gameObject.CompareTag("Player")) return;
      objectToToggle.SetActive(makeVisible);
    }
  
  
  }
}
