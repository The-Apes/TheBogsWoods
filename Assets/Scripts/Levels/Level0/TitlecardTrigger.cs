using System.Collections;
using Managers;
using Player;
using UnityEngine;

namespace Levels.Level0
{
    public class TitleCardTrigger : MonoBehaviour
    {
        [SerializeField] private AudioClip titleCardSong;
        [SerializeField] private AudioClip forestSong;
        [SerializeField] private GameObject titleCard;
        [SerializeField] private GameObject canvas;
        
        private bool triggered = false;
        private void OnTriggerEnter2D(Collider2D other){
            
            if (!other.gameObject.CompareTag("Player")) return;
            if (triggered) return;
            StartCoroutine(TitleCardSequence());
            triggered = true;


        }
        private IEnumerator TitleCardSequence()
        {
            CameraManager.instance.SetDamping(35);
            CameraManager.instance.LookAtLocation(new Vector3(20,25,0));
            CameraManager.instance.LerpZoom(7, 0.075f);
            
            AudioManager.instance.PlayMusic(titleCardSong);
            yield return new WaitForSeconds(15f);
            GameObject title = Instantiate(titleCard,canvas.transform);
            yield return new WaitForSeconds(10f);
            Destroy(title.gameObject);
            yield return new WaitForSeconds(1f);
            CameraManager.instance.SetDamping(1);
            CameraManager.instance.LookAt(RuriMovement.instance.transform);
            CameraManager.instance.LerpZoom(12, 0.25f);
            SaveManager.instance.ChangeFlag("TitleCard", false);
            SaveManager.instance.SaveGame();
            
            AudioManager.instance.FadeOutMusic(3f);
            yield return new WaitForSeconds(3f);
            AudioManager.instance.PlayMusic(forestSong, 0.5f);
            Destroy(gameObject);
        }
    }
}
