using System;
using System.Collections;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace UI
{
    public class GameUI : MonoBehaviour
    {
        public static GameUI instance;
        [FormerlySerializedAs("menuCanvas")] [SerializeField] private GameObject pauseMenu;
        [Header("Saving")]
        [SerializeField] private GameObject saveGameText;
        [SerializeField] private AudioClip saveGameSfx;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
        
        }

        private void Start()
        {
            pauseMenu.SetActive(false);
        }

        private void Update()
        {
            if (!Input.GetKeyDown(KeyCode.Escape)) return;
            if (!pauseMenu.activeSelf && PauseController.IsGamePaused)
            {
                return;
            }
            ToggleMenu();
        }

        public void ToggleMenu()
        {
            bool showMenu = !pauseMenu.activeSelf;
            pauseMenu.SetActive(showMenu);
            PauseController.SetPause(showMenu);
        }

        public void SavedGame()
        {
            StartCoroutine(SavedGameText());
        }
        private IEnumerator SavedGameText()
        {
            saveGameText.SetActive(true);
            AudioManager.instance.PlaySFX(saveGameSfx);
            yield return new WaitForSeconds(3f);
            saveGameText.SetActive(false);
        }

        public void ResumeGame()
        {
            pauseMenu.SetActive(false);
            PauseController.SetPause(false);
        }
    }
}