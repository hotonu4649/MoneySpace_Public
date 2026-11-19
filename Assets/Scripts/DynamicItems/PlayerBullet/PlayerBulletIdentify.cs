using UnityEngine;

namespace DynamicItems.PlayerBullet
{
    public class PlayerBulletIdentify : MonoBehaviour
    {
        [SerializeField] private PlayerBulletData _playerBulletData;
        public PlayerBulletData PlayerBulletData => _playerBulletData;
    }
}