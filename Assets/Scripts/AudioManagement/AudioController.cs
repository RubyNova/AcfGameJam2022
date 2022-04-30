using UnityEngine;

namespace AudioManagement
{
    public class AudioController : MonoBehaviour
    {
        [SerializeField] private AudioSource _musicTrack;
        [SerializeField] private float _volume;

        private void Awake() => DontDestroyOnLoad(gameObject);

        public float Volume
        {
            get => _volume;
            set
            {
                _volume = _volume switch
                {
                    < 0 => 0,
                    > 1 => 1,
                    _ => value
                };

                _musicTrack.volume = _volume;
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

        public void PlayEffect(AudioClip effect) => AudioSource.PlayClipAtPoint(effect, Vector3.zero, Volume);
    }
}
