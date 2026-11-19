using DynamicItems.Coin;
using DynamicItems.EnemyBullet;
using R3;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// PlayerCollisionクラスは、プレイヤーの衝突を処理するMonoBehaviourです。
    /// </summary>
    public class PlayerCollision : MonoBehaviour
    {
        private readonly Subject<CoinObjectStatus> _coinCollisionSubject = new Subject<CoinObjectStatus>();
        private readonly Subject<EnemyBulletIdentify> _enemyBulletCollisionSubject = new Subject<EnemyBulletIdentify>();
        public Observable<CoinObjectStatus> CoinCollisionObservable => _coinCollisionSubject;
        public Observable<EnemyBulletIdentify> EnemyBulletCollisionObservable => _enemyBulletCollisionSubject;
        
        private void OnCollisionEnter2D(Collision2D collision)
        {
            switch (collision.gameObject.tag)
            {
                case "EnemyBullet":
                    // Handle enemy collision
                    break;
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.tag)
            {
                case "Coin":
                    other.TryGetComponent(out CoinObjectStatus coinObjectStatus);
                    _coinCollisionSubject.OnNext(coinObjectStatus);
                    break;
                case "EnemyBullet":
                    other.TryGetComponent(out EnemyBulletIdentify enemyBulletIdentify);
                    _enemyBulletCollisionSubject.OnNext(enemyBulletIdentify);
                    break;
            }
        }
    }
}