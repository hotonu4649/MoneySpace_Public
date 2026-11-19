using System.Threading;
using Cysharp.Threading.Tasks;
using DynamicItems.PlayerWeapon;
using GameSystem;
using R3;
using UnityEngine;
using VContainer;

namespace DynamicItems.DemoPlayerBullet
{
    public class DemoPlayerShoter : MonoBehaviour
    {
        [SerializeField] private RectTransform _shootPoint;
        [SerializeField] private Canvas _settingCanvas;
        private TimeStopper _timeStopper;
        private PlayerWeaponList _playerWeaponList;
        private PlayerWeaponData _currentWeaponData;
        private float _autoShotDuration = 2f;
        private CancellationTokenSource _cts;
        private readonly Subject<GameObject> _onDemoPlayerShot = new ();
        public Observable<GameObject> OnDemoPlayerShot => _onDemoPlayerShot;

        [Inject]
        public void Construct(TimeStopper timeStopper, PlayerWeaponList playerWeaponList)
        {
            _timeStopper = timeStopper;
            _playerWeaponList = playerWeaponList;
            _cts = new CancellationTokenSource();
        }
        
        private void Start()
        {
            Subscribe();
            _currentWeaponData = _playerWeaponList.GetIndexWeapon(0);
        }
        
        private void Subscribe()
        {
            _timeStopper.TimeStopped
                .Where(isStopped => isStopped)
                .Subscribe(_ =>
                { 
                    StartAutoShotDemo(_cts.Token).Forget();
                }).AddTo(this);
            
            _timeStopper.TimeStopped
                .Where(isStopped => !isStopped)
                .Subscribe(_ =>
                {
                    _cts.Cancel();
                    _cts.Dispose();
                    _cts = new CancellationTokenSource();
                }).AddTo(this);
            
            _playerWeaponList.OnWeaponChange
                .Subscribe(_ =>
                {
                    _currentWeaponData = _playerWeaponList.GetIndexWeapon(0);
                }).AddTo(this);
            
            _playerWeaponList.OnWeaponAdd
                .Subscribe(_ =>
                {
                    _currentWeaponData = _playerWeaponList.GetIndexWeapon(0);
                }).AddTo(this);
        }
        
        private GameObject DemoShot()
        {
            var demoBullet = DemoPlayerBulletSpawner.BulletSpawn(
                _currentWeaponData.BulletData.BulletType,
                _shootPoint,
                _settingCanvas.transform);
            demoBullet.TryGetComponent(out DemoBulletMover demoBulletMover);
            demoBulletMover.Initialize(_currentWeaponData.BulletData.Speed, _settingCanvas);
            return demoBullet;
        }

        private async UniTask AutoShotDemo(CancellationToken token)
        {
            DemoShot();
            await UniTask.Delay(System.TimeSpan.FromSeconds(_autoShotDuration), delayType: DelayType.UnscaledDeltaTime, cancellationToken:token);
        }
        
        public async UniTask StartAutoShotDemo(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {   
                await AutoShotDemo(token);
            }
        }
        
        public void SetAutoShotDuration(float duration)
        {
            _autoShotDuration = duration;
        }
    }
}