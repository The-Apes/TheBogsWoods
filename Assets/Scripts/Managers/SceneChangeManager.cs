using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class SceneChangeManager : MonoBehaviour
    {
        public static SceneChangeManager instance;
        public static bool isSceneChanging;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void Restart()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private IEnumerator LoadingScreen(string sceneName)
        {
            isSceneChanging = true;
            SceneManager.LoadScene("LoadingScene");
            yield return new WaitForSeconds(3f);
            isSceneChanging = false;
            SceneManager.LoadScene(sceneName);
        }
        
        public void LoadScene(string sceneName)
        {
           StartCoroutine(LoadingScreen(sceneName));
        }
    }
}
