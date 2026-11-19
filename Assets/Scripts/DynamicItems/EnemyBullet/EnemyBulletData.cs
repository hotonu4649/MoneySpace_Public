using UnityEngine;

namespace DynamicItems.EnemyBullet
{
    /// <summary>
    /// EnemyBulletDataクラスは、敵の弾丸のデータを定義するために使用されます。
    /// </summary>
    [CreateAssetMenu]
    public class EnemyBulletData : ScriptableObject, IEnemyBulletData
    {
        [SerializeField] private EnemyBulletEnum.EnemyBulletType _enemyBulletType;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _speed;
        [SerializeField] private float _bulletDuration;
        [SerializeField] private int _damage;
        [SerializeField] private float _damageUpDuration;
        [SerializeField] private float _damageUpDurationAcceleration;

        public EnemyBulletEnum.EnemyBulletType EnemyBulletType => _enemyBulletType;
        public GameObject Prefab => _prefab;
        public float Speed => _speed;
        public float BulletDuration => _bulletDuration;
        public int Damage => _damage;
        public float DamageUpDuration => _damageUpDuration;
        public float DamageUpDurationAcceleration => _damageUpDurationAcceleration;
    }
}