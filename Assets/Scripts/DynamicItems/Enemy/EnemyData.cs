using DynamicItems.EnemyBullet;
using RelatedMoney;
using UnityEngine;

namespace DynamicItems.Enemy
{
    /// <summary>
    /// EnemyDataクラスは、敵のデータを定義するために使用されます。
    /// </summary>
    [CreateAssetMenu]
    public class EnemyData : ScriptableObject, IEnemyData
    {
        [SerializeField] private EnemyEnum.EnemyType _enemyType;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _enemyHp;
        [SerializeField] private float _speed;
        [SerializeField] private EnemyBulletData _enemyBulletData;
        [SerializeField] private int _moneyDropAmount;
        
        /// <summary>
        /// EnemyTypeプロパティは、敵の種類を取得します。
        /// </summary>
        public EnemyEnum.EnemyType EnemyType => _enemyType;

        /// <summary>
        /// Prefabプロパティは、敵のプレハブを取得します。
        /// </summary>
        public GameObject Prefab => _prefab;
        
        /// <summary>
        /// EnemyHpプロパティは、敵のHPを取得します。
        /// </summary>
        public Money EnemyHp => new Money(_enemyHp);
        
        /// <summary>
        /// Speedプロパティは、敵の移動速度を取得します。
        /// </summary>
        public float Speed => _speed;
        
        /// <summary>
        /// EnemyBulletDataプロパティは、敵の弾丸データを取得します。
        /// </summary>
        public EnemyBulletData EnemyBulletData => _enemyBulletData;
        
        /// <summary>
        /// MoneyDropAmountプロパティは、敵が倒されたときにドロップする金額を取得します。
        /// </summary>
        public int MoneyDropAmount => _moneyDropAmount;
    }
}