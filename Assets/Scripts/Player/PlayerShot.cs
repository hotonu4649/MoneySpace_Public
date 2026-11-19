using DynamicItems.PlayerBullet;
using DynamicItems.PlayerWeapon;
using R3;
using UnityEngine;
using VContainer;

namespace Player
{
    public class PlayerShot : MonoBehaviour
    {
        [SerializeField] private Transform _shotPoint;
        private Rigidbody2D _bulletRigidbody;
        private Quaternion _playerRotation;
        private PlayerWeaponList _playerWeaponList;
        private PlayerWeaponData _currentWeaponData;
        private Subject<GameObject> _onPlayerShot = new Subject<GameObject>();
        public Observable<GameObject> OnPlayerShot => _onPlayerShot;
        
        [Inject]
        public void Construct(PlayerWeaponList playerWeaponList)
        {
            _currentWeaponData = playerWeaponList.GetIndexWeapon(0);
            _playerWeaponList = playerWeaponList;
            Subscribe();
        }

        private void Subscribe()
        {
            _playerWeaponList.OnWeaponChange
                .Subscribe(_ =>
                {
                    _currentWeaponData = _playerWeaponList.GetIndexWeapon(0);
                }).AddTo(this);
            
            _playerWeaponList.OnWeaponAdd
                .Subscribe(_ =>
                {
                    _currentWeaponData = _playerWeaponList.GetIndexWeapon(0);
                })
                .AddTo(this);
        }
        
        private void Update()
        {
            _playerRotation = transform.rotation;
        }
        
        /// <summary>
        /// プレイヤーが弾を発射する処理を行います。
        /// </summary>
        public GameObject Shot()
        {
            var bullet = PlayerBulletSpawner.BulletSpawn(
                _currentWeaponData.BulletData.BulletType,
                _shotPoint.position,
                _playerRotation);
            bullet.TryGetComponent(out _bulletRigidbody);
            _bulletRigidbody.linearVelocity 
                = bullet.transform.up * PlayerBulletEnumToData.GetSpeed(_currentWeaponData.BulletData.BulletType);
            _onPlayerShot.OnNext(bullet);
            return bullet;
        }
    }
}