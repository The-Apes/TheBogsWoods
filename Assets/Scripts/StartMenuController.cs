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

    public void OnStartClick()
{
    MusicManager.Instance.StopMusic();
    LevelManager.Instance.LoadScene("Intro Cutscene", "CrossFade");
}

public void OnSkipClick()
{
    MusicManager.Instance.StopMusic();
    LevelManager.Instance.LoadScene("Level 0", "CrossFade");
}

    public void OnExitClick()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}