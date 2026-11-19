using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

namespace GameSystem.Audio
{
    public class BgmPlayer : MonoBehaviour
    {
        [SerializeField] private float _fadeInDuration = 1f;
        [SerializeField] private float _baseVolume = 0.5f;
        private AudioSource _audioSource;
        
        private void Awake()
        {
            TryGetComponent(out _audioSource);
        }
        
        public void Stop()
        {
            _audioSource.Stop();
        }

        public async UniTask Play()
        {
            _audioSource.volume = 0.1f;
            _audioSource.Play();
            await _audioSource.DOFade(_baseVolume, _fadeInDuration);
        }
    }
}