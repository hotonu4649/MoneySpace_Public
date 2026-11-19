using System;
using Cysharp.Threading.Tasks;
using GameSystem;
using GameSystem.Audio;
using R3;

namespace Effects.RelatedFabulousEffect
{
    public class FabulousEffectPlayer : IDisposable

    {
        private readonly FabulousEffectTrigger _fabulousEffectTrigger;
        private readonly FabulousEffectView _fabulousEffectView;
        private readonly AudioManager _audioManager;
        private readonly BgmPlayer _bgmPlayer;
        private readonly CompositeDisposable _disposables;
        private readonly ReactiveProperty<bool> _isPlaying;
        public ReadOnlyReactiveProperty<bool> IsPlaying => _isPlaying.ToReadOnlyReactiveProperty();

        public FabulousEffectPlayer(
            FabulousEffectTrigger fabulousEffectTrigger,
            FabulousEffectView fabulousEffectView,
            AudioManager audioManager, 
            BgmPlayer bgmPlayer)
        {
            _fabulousEffectTrigger = fabulousEffectTrigger;
            _fabulousEffectView = fabulousEffectView;
            _audioManager = audioManager;
            _bgmPlayer = bgmPlayer;
            _isPlaying = new ReactiveProperty<bool>(false);
            _disposables = new CompositeDisposable();
        }

        public void Initialize()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _fabulousEffectTrigger.OnFabulousEffectTriggered
                .Where(_ => !_isPlaying.Value)
                .Subscribe(_ =>
                {
                    FabulousEffectSequence().Forget();
                }).AddTo(_disposables);
        }

        private async UniTask FabulousEffectSequence()
        {
            _bgmPlayer.Stop();
            _isPlaying.Value = true;
            _fabulousEffectView.Play(0.5f);
            _audioManager.PlaySeAsync("Patinko").Forget();
            await HitStop.Play(0f, 2.67f);
            _fabulousEffectView.Stop();
            await _audioManager.PlaySeAsync("FabulousBgm", volume: 0.5f);
            _isPlaying.Value = false;
            _bgmPlayer.Play().Forget();
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}