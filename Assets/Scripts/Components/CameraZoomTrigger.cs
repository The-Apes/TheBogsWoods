using Managers;
using UnityEngine;

namespace Components
{
    public class CameraZoomTrigger : MonoBehaviour
    {
        public int zoom = 15;
    
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            CameraManager.instance.LerpZoom(zoom);
            //maybe add a zoom sound effect
        }
    }
}
