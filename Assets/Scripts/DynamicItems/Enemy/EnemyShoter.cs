using System;
using System.Threading;
using R3;
using Cysharp.Threading.Tasks;
using DynamicItems.EnemyBullet;
using DynamicItems.EnemyBullet.EnemyShotPatternFile;
using GameSystem;
using Player;
using UnityEngine;
using VContainer;

namespace DynamicItems.Enemy
{
    /// <summary>
    /// 実際に弾を生成して撃つクラス
    /// </summary>
    public class EnemyShoter : MonoBehaviour
    {
        [SerializeField] private Transform _shotPoint;
        private GameObject _playerObject;
        private EnemyShotPattern _shotPattern;
        private EnemyIdentify _enemyIdentify;
        private GameTimer _gameTimer;
        private IDisposable _shotObservable;
        private float _spawnedTime;
        
        private void Awake()
        {
            TryGetComponent(out _enemyIdentify);
        }
        
        [Inject]
        public void Construct(PlayerController playerController, GameTimer gameTimer)
        {
            _playerObject = playerController.gameObject;
            _gameTimer = gameTimer;
            _shotPattern = new ShotStraightToPlayer(_playerObject, gameObject);
        }

        private void OnEnable()
        {
            _spawnedTime = _gameTimer.Timer.CurrentValue;
            _shotObservable = _shotPattern.ShotObservable
                .Subscribe(_ =>
                {
                    var bullet = EnemyBulletSpawner.BulletSpawn(_enemyIdentify.EnemyData.EnemyBulletData.EnemyBulletType, _shotPoint.position, Quaternion.identity);
                    bullet.TryGetComponent(out EnemyBulletIdentify bulletIdentify);
                    bulletIdentify.InitializeBulletInfo(_spawnedTime, _gameTimer.Timer.CurrentValue);
                    bullet.TryGetComponent(out Rigidbody2D rb);
                    rb.linearVelocity = _enemyIdentify.EnemyData.EnemyBulletData.Speed * transform.up;
                });
        }

        public async UniTask StartShootSequenceAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    await _shotPattern.ShootSequence(token);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
        }
        
        private void OnDisable()
        {
            _shotObservable?.Dispose();
        }
    }
}