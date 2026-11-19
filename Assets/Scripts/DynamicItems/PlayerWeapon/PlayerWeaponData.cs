using DynamicItems.PlayerBullet;
using UnityEngine;

namespace DynamicItems.PlayerWeapon
{
    [CreateAssetMenu]
    public class PlayerWeaponData : ScriptableObject
    {
        [SerializeField] private string _weaponName;
        [SerializeField] private PlayerBulletData _bulletData;
        [SerializeField] private Sprite _weaponIcon;
        
        /// <summary>
        /// 武器の名前を取得します。
        /// </summary>
        public string WeaponName => _weaponName;
        
        /// <summary>
        /// 武器が発射する弾のデータを取得します。
        /// </summary>
        public PlayerBulletData BulletData => _bulletData;
        
        /// <summary>
        /// 武器のアイコンを取得します。
        /// </summary>
        public Sprite WeaponIcon => _weaponIcon;
    }
}