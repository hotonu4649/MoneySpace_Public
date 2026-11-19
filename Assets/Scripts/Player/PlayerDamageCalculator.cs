using DynamicItems.EnemyBullet;
using UnityEngine;

namespace Player
{
    public class PlayerDamageCalculator
    {
        public int CalculateDamage(EnemyBulletIdentify enemyBulletIdentify, float enemyLifeTime)
        {
            var baseDamage = enemyBulletIdentify.BaseDamage;
            var DamageDuration = enemyBulletIdentify.EnemyBulletData.DamageUpDuration;
            var DamageDurationAcceleration = enemyBulletIdentify.EnemyBulletData.DamageUpDurationAcceleration;
            var damage = baseDamage + (int)(DamageDuration + DamageDurationAcceleration * (enemyLifeTime/10f));
            return Mathf.Max(damage, 0);
        }
    }
}