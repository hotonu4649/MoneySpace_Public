using DynamicItems.Coin;
using DynamicItems.EnemyBullet;
using DynamicItems.PlayerBullet;
using UnityEngine;

namespace DynamicItems
{
    public class WallToRelease : MonoBehaviour
    {
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("PlayerBullet"))
            {
                PlayerBulletSpawner.BulletRelease(collision.gameObject);
            }
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Coin"))
            {
                CoinSpawner.CoinRelease(other.gameObject);
            }

            if (other.gameObject.CompareTag("EnemyBullet"))
            {
                EnemyBulletSpawner.BulletRelease(other.gameObject);
            }
        }
    }
}