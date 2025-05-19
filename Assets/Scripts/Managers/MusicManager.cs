using System.Collections;
using UnityEngine;

// Watched Game Dev Library and Brackeys on Youtube
public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [SerializeField]
    private MusicLibrary musicLibrary;
    [SerializeField]
    private AudioSource musicSource;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlayMusic(string trackName, float fadeDuration = 0.5f)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(AnimateMusicCrossfade(musicLibrary.GetClipFromName(trackName), fadeDuration));
    }

    public void StopMusic(float fadeDuration = 0.5f)
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeOutAndStop(fadeDuration));
    }

    private IEnumerator AnimateMusicCrossfade(AudioClip nextTrack, float fadeDuration = 0.5f)
    {
        // Fade out current music
        float percent = 0;
        float startVolume = musicSource.volume;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0, percent);
            yield return null;
        }

        musicSource.clip = nextTrack;
        musicSource.Play();

        // Fade in new music
        percent = 0;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(0, 1f, percent);
            yield return null;
        }
        musicSource.volume = 1f;
        fadeCoroutine = null;
    }

    private IEnumerator FadeOutAndStop(float fadeDuration = 0.5f)
    {
        float percent = 0;
        float startVolume = musicSource.volume;
        while (percent < 1)
        {
            percent += Time.deltaTime / fadeDuration;
            musicSource.volume = Mathf.Lerp(startVolume, 0, percent);
            yield return null;
        }
        musicSource.Stop();
        musicSource.clip = null;
        fadeCoroutine = null;
    }
}