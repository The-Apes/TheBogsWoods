using System.Collections;
using Managers;
using Unity.VisualScripting;
using UnityEngine;

namespace Levels.Level0
{
    public class TitlecardTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip titlecardSong;
        [SerializeField] private GameObject titlecard;
        [SerializeField] private GameObject canvas;
        private void OnTriggerEnter2D(Collider2D other){
            
            if (!other.gameObject.CompareTag("Player")) return;
            StartCoroutine(TitlecardSequence());


        }
        private IEnumerator TitlecardSequence()
        {
            CameraManager.instance.SetDamping(35);
            CameraManager.instance.LookAtLocation(new Vector3(23,24,0));
            CameraManager.instance.LerpZoom(10, 0.25f);
            
            AudioManager.instance.PlayMusic(titlecardSong);
            yield return new WaitForSeconds(15f);
            GameObject title = Instantiate(titlecard,canvas.transform);
            yield return new WaitForSeconds(10f);
            Destroy(title.gameObject);
            yield return new WaitForSeconds(1f);
            CameraManager.instance.SetDamping(1);
            CameraManager.instance.LookAt(RuriMovement.instance.transform);
            CameraManager.instance.LerpZoom(20, 0.25f);
            Destroy(gameObject);
        }
    }
}
