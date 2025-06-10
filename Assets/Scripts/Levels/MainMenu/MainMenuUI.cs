using System.IO;
using Saving;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Levels.MainMenu
{
    public class MainMenuUI : MonoBehaviour
    {
        public AudioMixer audioMixer;
        public Slider musicSlider;
        public Slider sfxSlider;

        private const float DefaultVolume = 0f;    // 0 dB (max volume)
        private const float MinVolume = -80f;      // -80 dB (min volume)
        
        [SerializeField] private TextMeshProUGUI newGameText;
        [SerializeField] private GameObject continueGameButton;
        [SerializeField] private TextMeshProUGUI continueGameText;
        
        private string savePath;

        private void Start()
        {
            LoadVolume();
            MusicManager.Instance.PlayMusic("StartMenu");
            savePath = Application.persistentDataPath + "/savefile.dat";
            
            //Check if save file exists
            if (File.Exists(savePath))
            {
                continueGameButton.GetComponent<Button>().interactable = true;
            }
            else
            {
                continueGameButton.GetComponent<Button>().interactable = false;
                continueGameText.color = Color.gray;
            }
        }

        public void UpdateMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume", volume);
        }

        public void UpdateSoundVolume(float volume)
        {
            audioMixer.SetFloat("SFXVolume", volume);
        }

        public void SaveVolume()
        {
            audioMixer.GetFloat("MusicVolume", out float musicVolume);
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);

            audioMixer.GetFloat("SFXVolume", out float sfxVolume);
            PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        }

        public void LoadVolume()
        {
            // Music
            if (PlayerPrefs.HasKey("MusicVolume"))
            {
                float musicVolume = PlayerPrefs.GetFloat("MusicVolume");
                if (musicSlider != null)
                    musicSlider.value = musicVolume;
                audioMixer.SetFloat("MusicVolume", musicVolume);
            }
            else
            {
                if (musicSlider != null)
                    musicSlider.value = DefaultVolume;
                audioMixer.SetFloat("MusicVolume", DefaultVolume);
            }

            // SFX
            if (PlayerPrefs.HasKey("SFXVolume"))
            {
                float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
                if (sfxSlider != null)
                    sfxSlider.value = sfxVolume;
                audioMixer.SetFloat("SFXVolume", sfxVolume);
            }
            else
            {
                if (sfxSlider != null)
                    sfxSlider.value = DefaultVolume;
                audioMixer.SetFloat("SFXVolume", DefaultVolume);
            }
        }

        public void ResetToDefaults()
        {
            if (musicSlider != null)
                musicSlider.value = DefaultVolume;
            if (sfxSlider != null)
                sfxSlider.value = DefaultVolume;

            audioMixer.SetFloat("MusicVolume", DefaultVolume);
            audioMixer.SetFloat("SFXVolume", DefaultVolume);

            SaveVolume();
        }

        public void NewGame()
        {
            if (newGameText.text == "Are you sure?" || !File.Exists(savePath))
            {
                File.Delete(savePath);
                MusicManager.Instance.StopMusic();
                SceneManager.LoadScene("Intro Cutscene");
            }
            else
            {
                newGameText.SetText("Are you sure?");
            }
        }

        public void ContinueGame()
        {
            MusicManager.Instance.StopMusic();
            SceneManager.LoadScene("Level 0");
        }

        public void OnSkipClick()
        {
            MusicManager.Instance.StopMusic();
            SceneManager.LoadScene("Level 0");
        }

        public void OnExitClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }
    }
}