using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace Managers
{
    public class SceneChangeManager : MonoBehaviour
    {
        public static SceneChangeManager instance;
        public static bool isSceneChanging;
        
        public GameObject transitionsContainer;
        
        private SceneTransition[] transitions;

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

        private void Start()
        {
            transitions = transitionsContainer.GetComponentsInChildren<SceneTransition>();
        }

        public void Restart()
        {
            LoadScene(SceneManager.GetActiveScene().name);
        }
        
        private IEnumerator LoadingScreen(string sceneName, string transitionName)
        {
            isSceneChanging = true;
            
            var transition = transitions.FirstOrDefault(t => t.name == transitionName);
            if (transition == null)
            {
                Debug.LogWarning($"Transition '{transitionName}' not found.");
                yield break;
            }
            
            var scene = SceneManager.LoadSceneAsync(sceneName);
            scene.allowSceneActivation = false;
            yield return transition.AnimateTransitionIn();

            scene.allowSceneActivation = true;

            yield return transition.AnimateTransitionOut();
            isSceneChanging = false; 
            //@phiwe im assuming that this line will run after the fade out transition is complete
            //if not please make that happen somehow
        }
        
        public void LoadScene(string sceneName, string transitionName = "Crossfade")
        {
           StartCoroutine(LoadingScreen(sceneName, transitionName));
        }
    }
}
