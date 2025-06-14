using System.Collections;
using Managers;
using Player;
using UnityEngine;

namespace Levels.Level0
{
    public class TitlecardTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip titlecardSong;
        [SerializeField] private GameObject titlecard;
        [SerializeField] private GameObject canvas;
        
        private bool triggered = false;
        private void OnTriggerEnter2D(Collider2D other){
            
            if (!other.gameObject.CompareTag("Player")) return;
            if (triggered) return;
            StartCoroutine(TitlecardSequence());
            triggered = true;


        }
        private IEnumerator TitlecardSequence()
        {
            CameraManager.instance.SetDamping(35);
            CameraManager.instance.LookAtLocation(new Vector3(20,25,0));
            CameraManager.instance.LerpZoom(7, 0.075f);
            
            AudioManager.instance.PlayMusic(titlecardSong);
            yield return new WaitForSeconds(15f);
            GameObject title = Instantiate(titlecard,canvas.transform);
            yield return new WaitForSeconds(10f);
            Destroy(title.gameObject);
            yield return new WaitForSeconds(1f);
            CameraManager.instance.SetDamping(1);
            CameraManager.instance.LookAt(RuriMovement.instance.transform);
            CameraManager.instance.LerpZoom(12, 0.25f);
            Destroy(gameObject);
        }
    }
}
