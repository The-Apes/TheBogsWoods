
using System.Collections;
using UnityEngine;

namespace Managers
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;
        private AudioSource _sfxSource;
        private AudioSource _musicSource;
        
        [SerializeField] private AudioSource sfxPrefab;
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
        
        public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
        {
            AudioSource audioSource = Instantiate(sfxPrefab);
            
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.Play();
            audioSource.spatialBlend = 0;
            
            Destroy(audioSource.gameObject, audioSource.clip.length);
        }
       
        public void PlaySFXAt(AudioClip clip, Transform spawnTransform, float spatialBlend = 0.7f, float volume = 1f, float pitch = 1f)
        {
            AudioSource audioSource = Instantiate(sfxPrefab, spawnTransform.position, Quaternion.identity);
            
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.pitch = pitch;
            audioSource.spatialBlend = spatialBlend;
            audioSource.Play();
            
            Destroy(audioSource.gameObject, audioSource.clip.length);
        }
        public void PlayRandomSFXAt(AudioClip[] clip, Transform spawnTransform, float volume)
        {
            AudioSource audioSource = Instantiate(sfxPrefab, spawnTransform.position, Quaternion.identity);
            
            audioSource.clip = clip[Random.Range(0, clip.Length)];
            audioSource.volume = volume;
            audioSource.Play();
            
            Destroy(audioSource.gameObject, audioSource.clip.length);
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
            PlayMusic(newTrack);
            _musicSource.volume = 0f;

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
