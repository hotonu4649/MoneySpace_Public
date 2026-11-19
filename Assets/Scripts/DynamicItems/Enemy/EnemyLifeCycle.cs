using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DynamicItems.PlayerBullet;
using GameSystem;
using R3;
using RelatedMoney;
using UnityEngine;
using VContainer;

namespace DynamicItems.Enemy
{
    /// <summary>
    /// Manages the lifecycle of an enemy.
    /// Enemyにアタッチする予定
    /// </summary>
    public class EnemyLifeCycle : MonoBehaviour
    {
        private RandomEnemySpawner _enemySpawner;
        private EnemySpawner _enemyPool;
        private EnemyEvents _enemyEvents;
        private EnemyIdentify _enemyIdentify;
        private EnemyHp _enemyHp;
        private EnemyMove _enemyMove;
        private EnemyShoter _enemyShoter;
        private HaveMoneyStatus _haveMoneyStatus;
        private MoneyFromEnemyCalculator _moneyFromEnemyCalculator;
        private CancellationTokenSource _cts;
        
        [Inject]
        public void Construct(
            RandomEnemySpawner enemySpawner,
            EnemySpawner enemyPool,
            HaveMoneyStatus haveMoneyStatus,
            MoneyFromEnemyCalculator moneyFromEnemyCalculator)
        {
            _enemySpawner = enemySpawner;
            _enemyPool = enemyPool;
            _haveMoneyStatus = haveMoneyStatus;
            _moneyFromEnemyCalculator = moneyFromEnemyCalculator;
        }
        
        private void Awake()
        {
            TryGetComponent(out _enemyEvents);
            TryGetComponent(out _enemyIdentify);
            TryGetComponent(out _enemyMove);
            TryGetComponent(out _enemyShoter);
            _enemyHp = new EnemyHp(_enemyIdentify.EnemyData.EnemyHp);
            Subscribe();
        }

        private async void OnEnable()
        {
            await EnemySequence();
        }
        
        private async UniTask EnemySequence()
        {
            _cts = new CancellationTokenSource();
            try
            {
                // 指定の位置まで移動
                await _enemyMove.SetPosition(_enemySpawner.SpawnPosition, _cts.Token);
                // 弾を撃つ流れを開始する
                _enemyShoter.StartShootSequenceAsync(_cts.Token).Forget();
                while (_enemyHp.CurrentHp > new Money(0))
                {
                    await _enemyMove.MoveToNextPositionAsync(_cts.Token);
                }
            }
            catch (OperationCanceledException)
            {
                
            }
        }
        
        private void OnDisable()
        {
            _cts?.Cancel();
            _cts?.Dispose();
        }

        private void Subscribe()
        {
            _enemyEvents.CollisionObservable
                .Subscribe(other =>
                {
                    if (other.gameObject.TryGetComponent(
                            out PlayerBulletIdentify playerBulletIdentify))
                    {
                        _enemyHp.TakeDamage(playerBulletIdentify.PlayerBulletData.Damage);
                    }
                }).AddTo(this);
            
            _enemyEvents.OnEnableObservable
                .Subscribe(_ =>
                {
                    _enemyHp.InstantiateHp();
                }).AddTo(this);
            
            _enemyHp.IsDead
                .Where(isDead => isDead)
                .Subscribe(_ =>
                {
                    _enemyPool.EnemyRelease(gameObject);
                    _enemySpawner.SubtractEnemyCount(1);
                    var result = _moneyFromEnemyCalculator.CalculateMoneyFromEnemy(new Money(_enemyIdentify.EnemyData.MoneyDropAmount));
                    _haveMoneyStatus.AddMoney(result);
                }).AddTo(this);
        }
    }
}