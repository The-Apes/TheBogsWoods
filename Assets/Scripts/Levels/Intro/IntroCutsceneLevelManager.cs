using DialogueFramework;
using Managers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace Levels.Intro
{
    public class IntroCutsceneLevelManager : MonoBehaviour
    {
        public DialogueAsset introCutsceneDialogue;
        public AudioClip music;

        private void Start()
        {
            DialogueSystem.onDialogueEnd += OnDialogueEnd;
            Debug.Log("IntroCutsceneLevelManager");
            DialogueManager.instance.StartDialogue(introCutsceneDialogue.dialogue);
            Debug.Log("Tried to start the scene");
            ApplySavedMusicVolume();
            AudioManager.instance.PlayMusic(music);
        }

        private void ApplySavedMusicVolume()
        {
            if (AudioManager.instance != null && AudioManager.instance.audioMixer != null)
            {
                float musicVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : 0f;
                AudioManager.instance.audioMixer.SetFloat("MusicVolume", musicVolume);
            }
        }
        public void SkipCutscene(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnDialogueEnd("");
            }
        }

        private void OnDialogueEnd(string dialogue)
        {
            AudioManager.instance.StopMusic();
            SceneChangeManager.instance.LoadScene("Level 0");
        }

        private void OnDestroy()
        {
            DialogueSystem.onDialogueEnd -= OnDialogueEnd;
        }

        private void OnDisable()
        {
            DialogueSystem.onDialogueEnd -= OnDialogueEnd;
        }
    }
}
