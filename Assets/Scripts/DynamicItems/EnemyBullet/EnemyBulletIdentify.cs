using UnityEngine;

namespace DynamicItems.EnemyBullet
{
    public class EnemyBulletIdentify : MonoBehaviour
    {
        [SerializeField] private EnemyBulletData _enemyBulletType;
        private EnemyBulletBaseInfo _enemyBulletBaseInfo = new ();
        private int _baseDamage;
        private float _enemySpawnedTime;
        
        public EnemyBulletData EnemyBulletData => _enemyBulletType;
        public int BaseDamage => _baseDamage;
        public float EnemySpawnedTime => _enemySpawnedTime;
        
        public void InitializeBulletInfo(float time, float currentTime)
        {
            _enemySpawnedTime = time;
            _baseDamage = _enemyBulletBaseInfo.CalculateBaseDamage(_enemyBulletType, _enemySpawnedTime, currentTime);
        }
    }
}