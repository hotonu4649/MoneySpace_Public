using DynamicItems.Coin;
using DynamicItems.EnemyBullet;
using DynamicItems.PlayerBullet;
using GameSystem;
using Player;
using VContainer.Unity;
using R3;

namespace RelatedMoney
{
    /// <summary>
    /// playerのコインイベントを購読し、HaveMoneyStatusに反映させるクラスです。
    /// </summary>
    public class PlayerMoneySubscribes : IStartable
    {
        private readonly HaveMoneyStatus _haveMoneyStatus;
        private readonly PlayerCollision _playerCollision;
        private readonly PlayerShot _playerShot;
        private readonly MoneyCalculator _moneyCalculator;
        private readonly GameTimer _gameTimer;
        private readonly PlayerDamageCalculator _playerDamageCalculator = new ();
        private readonly EnemyAttackCalculator _enemyAttackCalculator;

        public PlayerMoneySubscribes(
            HaveMoneyStatus haveMoneyStatus,
            PlayerCollision playerCollision,
            PlayerShot playerShot,
            MoneyCalculator moneyCalculator,
            GameTimer gameTimer,
            EnemyAttackCalculator enemyAttackCalculator
            )
        {
            _haveMoneyStatus = haveMoneyStatus;
            _playerCollision = playerCollision;
            _playerShot = playerShot;
            _moneyCalculator = moneyCalculator;
            _gameTimer = gameTimer;
            _enemyAttackCalculator = enemyAttackCalculator;
        }

        public void Start()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            // プレイヤーがコインを取得したときの処理を購読します。
            _playerCollision.CoinCollisionObservable.Subscribe( coinStatus =>
            {
                var amount = CoinEnumToData.GetValue(coinStatus.CoinType);
                amount = _moneyCalculator.CalculateAddMoney(amount);
                _haveMoneyStatus.AddMoney(amount);
            }).AddTo(_playerCollision);
            
            // プレイヤーが敵の弾に衝突したときの処理を購読します。
            _playerCollision.EnemyBulletCollisionObservable.Subscribe( enemyBulletIdentify =>
            {
                var damage = 
                    _playerDamageCalculator.CalculateDamage(
                    enemyBulletIdentify,
                     _gameTimer.Timer.CurrentValue　- enemyBulletIdentify.EnemySpawnedTime);
                EnemyBulletSpawner.BulletRelease(enemyBulletIdentify.gameObject); // 弾を消す処理
                var amount = _enemyAttackCalculator.CalculateReducedAttack(new Money(damage)); // スキルによる攻撃力減少を計算
                _haveMoneyStatus.SubtractMoney(amount);
            }).AddTo(_playerCollision);
            
            // プレイヤーが弾を撃ったときの処理を購読します。
            _playerShot.OnPlayerShot.Subscribe( bullet =>
            {
                bullet.TryGetComponent(out PlayerBulletIdentify bulletIdentify);
                var amount = bulletIdentify.PlayerBulletData.ConsumeMoney;
                _haveMoneyStatus.SubtractMoney(amount);
            }).AddTo(_playerShot);
        }
    }
}