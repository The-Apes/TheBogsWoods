using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    private const float DefaultVolume = 0f;    // 0 dB (max volume)
    private const float MinVolume = -80f;      // -80 dB (min volume)

    private void Start()
    {
        LoadVolume();
        MusicManager.Instance.PlayMusic("StartMenu");
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
        if (musicSlider != null && PlayerPrefs.HasKey("MusicVolume"))
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        if (sfxSlider != null && PlayerPrefs.HasKey("SFXVolume"))
            sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
    }

    public void ResetToDefaults()
    {
        // Set sliders to default value
        if (musicSlider != null)
            musicSlider.value = DefaultVolume;
        if (sfxSlider != null)
            sfxSlider.value = DefaultVolume;

        // Set AudioMixer values to default
        audioMixer.SetFloat("MusicVolume", DefaultVolume);
        audioMixer.SetFloat("SFXVolume", DefaultVolume);

        // Save defaults
        SaveVolume();
    }

    public void OnStartClick()
    {
        MusicManager.Instance.StopMusic(); // Stop StartMenu music before loading new scene
        SceneManager.LoadScene("Intro Cutscene");
    }
    public void OnSkipClick()
    {
        MusicManager.Instance.StopMusic(); // Stop StartMenu music before loading new scene
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