using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsControllerForCutscene : MonoBehaviour
{
    public AudioMixer audioMixer;
    private const float DefaultVolume = 0f;

    private void Start()
    {
        LoadVolume();
    }

    public void LoadVolume()
    {
        if (audioMixer == null) return;

        // Music
        float musicVolume = PlayerPrefs.HasKey("MusicVolume") ? PlayerPrefs.GetFloat("MusicVolume") : DefaultVolume;
        audioMixer.SetFloat("MusicVolume", musicVolume);

        // SFX
        float sfxVolume = PlayerPrefs.HasKey("SFXVolume") ? PlayerPrefs.GetFloat("SFXVolume") : DefaultVolume;
        audioMixer.SetFloat("SFXVolume", sfxVolume);
    }
}