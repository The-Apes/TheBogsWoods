
using System.Collections;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        private AudioSource _sfxSource;
        private AudioSource _musicSource;
        
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
                
                _sfxSource = gameObject.AddComponent<AudioSource>();
                _musicSource = gameObject.AddComponent<AudioSource>();
                _musicSource.loop = true;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void PlaySound(AudioClip soundClip)
        {
            _sfxSource.PlayOneShot(soundClip);
        }
        public void PlaySound(AudioClip soundClip, float pitch)
        {
            AudioSource tempSource = gameObject.AddComponent<AudioSource>();
            tempSource.clip = soundClip;
            tempSource.pitch = pitch;
            tempSource.Play();
            Destroy(tempSource, soundClip.length / pitch); // pitch affects duration
        }
        public void PlayMusic(AudioClip musicClip)
        {
                _musicSource.clip = musicClip;
                _musicSource.Play();
        }
        public void StopMusic()
        {
            _musicSource.Stop();
        }
        public void PauseMusic()
        {
            _musicSource.Pause();
        }
        public void UnpauseMusic()
        {
            _musicSource.UnPause();
        }
        
        public void FadeInMusic(AudioClip newTrack, float duration = 1f)
        {
            StartCoroutine(FadeInCoroutine(newTrack, duration));
        }

        public void FadeOutMusic(float duration = 1f)
        {
            StartCoroutine(FadeOutCoroutine(duration));
        }

        private IEnumerator FadeInCoroutine(AudioClip newTrack, float duration)
        {
            _musicSource.clip = newTrack;
            _musicSource.volume = 0f;
            _musicSource.Play();

            float timer = 0f;
            while (timer < duration)
            {
                _musicSource.volume = Mathf.Lerp(0f, 1f, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
            _musicSource.volume = 1f;
        }

        private IEnumerator FadeOutCoroutine(float duration)
        {
            float startVol = _musicSource.volume;
            float timer = 0f;
            while (timer < duration)
            {
                _musicSource.volume = Mathf.Lerp(startVol, 0f, timer / duration);
                timer += Time.deltaTime;
                yield return null;
            }
            _musicSource.Stop();
            _musicSource.volume = 1f;
        }
    }
}
