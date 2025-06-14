using Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Levels.Level0
{
    public class TitlecardTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip titlecardSong;
        private void OnTriggerEnter2D(Collider2D other){
            
            if (!other.gameObject.CompareTag("Player")) return;
            
            CameraManager.instance.SetDamping(75);
            CameraManager.instance.LookAtLocation(new Vector3(23,24,0));
            CameraManager.instance.LerpZoom(5, 0.025f);
            
            AudioManager.instance.PlayMusic(titlecardSong);
        }
    }
}
