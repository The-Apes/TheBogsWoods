using System.Collections;
using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
    
        public static GameManager instance;
   
        private void Awake()
        {
            instance = this;
        }

        public void HitStop(float duration)
        {
            StartCoroutine(HitStopRoutine(duration));
        }

        private static IEnumerator  HitStopRoutine(float duration) {
            Time.timeScale = 0f;
            yield return new WaitForSecondsRealtime(duration);
            Time.timeScale = 1f;
        }
    }
}
