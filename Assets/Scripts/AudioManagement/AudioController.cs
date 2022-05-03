using UnityEngine;
using UnityEngine.Serialization;

namespace AudioManagement
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicTrack;
        [FormerlySerializedAs("_volume"), SerializeField, Range(0, 1)] private float _musicVolume;
        [SerializeField, Range(0, 1)] private float _soundEffectVolume;

        private void Awake() => DontDestroyOnLoad(gameObject);

        public float MusicVolume
        {
            get => _musicVolume;
            set
            {
                _musicVolume = _musicVolume switch
                {
                    < 0 => 0,
                    > 1 => 1,
                    _ => value
                };

                _musicTrack.volume = _musicVolume;
            }
        }
        
        public float SoundEffectVolume
        {
            get => _soundEffectVolume;
            set
            {
                _soundEffectVolume = _soundEffectVolume switch
                {
                    < 0 => 0,
                    > 1 => 1,
                    _ => value
                };
            }
        }

        public void PlayMusic(AudioClip track, bool shouldLoop = true)
        {
            if (_musicTrack.isPlaying)
            {
                _musicTrack.Stop();
            }
            
            _musicTrack.loop = shouldLoop;
            _musicTrack.clip = track;
            _musicTrack.Play();
        }

        public void PlayEffect(AudioClip effect) => AudioSource.PlayClipAtPoint(effect, Vector3.zero, SoundEffectVolume);

        public void StopMusic() => _musicTrack.Stop();
    }
}
