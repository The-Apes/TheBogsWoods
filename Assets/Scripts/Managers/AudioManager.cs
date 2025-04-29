
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
                Destroy(gameObject); // REMOVE YOURSELF!!
            }
        }

        public void PlaySound(AudioClip soundClip)
        {
            _sfxSource.PlayOneShot(soundClip);
        }
        public void PlaySound(AudioClip soundClip, float pitch)
        {
            _sfxSource.pitch = pitch;
            _sfxSource.PlayOneShot(soundClip);
            _sfxSource.pitch = 1f; // Reset pitch to default for the other clip
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
        
        
        //funky stuff
        //All my homies hate Coroutines!!!11!!!1
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
