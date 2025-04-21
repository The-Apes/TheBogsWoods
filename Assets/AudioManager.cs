
using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public static AudioManager Instance;
    private AudioSource _audioSource;
    private void Awake()
    {
        Instance = this;
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlaySound(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }
}
