using System.Threading;
using Cysharp.Threading.Tasks;
using DynamicItems.DemoPlayerBullet;
using R3;
using UnityEngine;

namespace Player.AutoShot
{
    public class AutoShotPresenter
    {
        private readonly IsAutoShotButton _isAutoShotButton;
        private readonly AutoShotSlider _autoShotSlider;
        private readonly AutoShotDuration _autoShotDuration;
        private readonly DemoPlayerShoter _demoPlayerShoter;
        private CancellationTokenSource _cts;
        
        public AutoShotPresenter(IsAutoShotButton isAutoShotButton, AutoShotSlider autoShotSlider, AutoShotDuration autoShotDuration, DemoPlayerShoter demoPlayerShoter)
        {
            _isAutoShotButton = isAutoShotButton;
            _autoShotSlider = autoShotSlider;
            _autoShotDuration = autoShotDuration;
            _demoPlayerShoter = demoPlayerShoter;
            _cts = new CancellationTokenSource();
        }
        
        public void Initialize()
        {
            Subscribe();
        }
        
        private void Subscribe()
        {
            // オートショットボタがオンになったときの処理を購読します。
            _isAutoShotButton.IsAutoShot
                .Where(isAuto => isAuto)
                .Subscribe(_ =>
                {
                    _autoShotDuration.StartAutoShot(_cts.Token).Forget();
                }).AddTo(_isAutoShotButton);
            
            // オートショットボタンがオフになったときの処理を購読します。
            _isAutoShotButton.IsAutoShot
                .Where(isAuto => !isAuto)
                .Subscribe(_ =>
                {
                    _cts.Cancel();
                    _cts.Dispose();
                    _cts = new CancellationTokenSource();
                }).AddTo(_isAutoShotButton);
            
            // オートショットスライダーの値が変化したときの処理を購読します。
            _autoShotSlider.Value
                .Subscribe(value =>
                {
                    var clampedValue = Mathf.Clamp(value * 2, 0.1f, 3f); // 値を0.1から5の範囲に制限
                    _autoShotDuration.SetAutoShotDuration(clampedValue);
                    _demoPlayerShoter.SetAutoShotDuration(clampedValue);
                }).AddTo(_autoShotSlider);
        }
    }
}