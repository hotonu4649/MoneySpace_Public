using DynamicItems.PlayerBullet;
using UnityEngine;

namespace DynamicItems.DemoPlayerBullet
{
    [CreateAssetMenu]
    public class DemoPlayerBulletData : ScriptableObject
    {
        [SerializeField] private PlayerBulletEnum.PlayerBulletType _bulletType;
        [SerializeField] private GameObject _prefab;
        
        public PlayerBulletEnum.PlayerBulletType BulletType => _bulletType;
        public GameObject Prefab => _prefab;
    }
}