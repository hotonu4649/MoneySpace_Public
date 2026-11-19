using System;
using R3;
using Cysharp.Threading.Tasks;
using GameSystem.Audio;

namespace Effects.RelatedFabulousEffect
{
    public class SubFabulousAudio : IDisposable
    {
        private SubFabulousTrigger _subFabulousTrigger;
        private readonly AudioManager _audioManager;
        private CompositeDisposable _disposables;
        
        public SubFabulousAudio(
            SubFabulousTrigger subFabulousTrigger,
            AudioManager audioManager)
        {
            _subFabulousTrigger = subFabulousTrigger;
            _audioManager = audioManager;
            _disposables = new CompositeDisposable();
        }
        
        public void Initialize()
        {
            Subscribe();
        }
        
        private void Subscribe()
        {
            _subFabulousTrigger.OnSubFabulousEffectTriggered
                .Subscribe(_ =>
                {
                    _audioManager.PlaySeAsync("ShortFabulousPatinko").Forget();
                }).AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}