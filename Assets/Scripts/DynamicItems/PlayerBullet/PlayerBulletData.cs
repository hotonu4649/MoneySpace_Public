using RelatedMoney;
using UnityEngine;

namespace DynamicItems.PlayerBullet
{
    /// <summary>
    /// PlayerBulletDataクラスは、プレイヤーの弾丸のデータを定義するために使用されます。
    /// </summary>
    [CreateAssetMenu]
    public class PlayerBulletData : ScriptableObject, IPlayerBulletData
    {
        [SerializeField] private PlayerBulletEnum.PlayerBulletType _bulletType;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private float _speed;
        [SerializeField] private int _damage; 
        [SerializeField] private int _consumeMoney;
        
        /// <summary>
        /// BulletTypeプロパティは、弾丸の種類を取得します。
        /// </summary>
        public PlayerBulletEnum.PlayerBulletType BulletType => _bulletType;

        /// <summary>
        /// Prefabプロパティは、弾丸のプレハブを取得します。
        /// </summary>
        public GameObject Prefab => _prefab;
        
        /// <summary>
        /// Speedプロパティは、弾丸の速度を取得します。
        /// </summary>
        public float Speed => _speed;
        
        /// <summary>
        /// Damageプロパティは、弾丸のダメージ量を取得します。
        /// </summary>
        public Money Damage => new Money(_damage);

        /// <summary>
        /// ConsumeMoneyプロパティは、弾丸の消費金額を取得します。
        /// </summary>
        public Money ConsumeMoney => new Money(_consumeMoney);

        // ここに他のプロパティやメソッドを追加することができます。
    }
}