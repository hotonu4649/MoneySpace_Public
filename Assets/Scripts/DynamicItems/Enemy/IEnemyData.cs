using DynamicItems.EnemyBullet;
using RelatedMoney;

namespace DynamicItems.Enemy
{
    public interface IEnemyData : ISpawnData
    {
        public Money EnemyHp { get; }
        public float Speed { get; }
        public EnemyBulletData EnemyBulletData { get; }
        // TODO:敵の攻撃パターンを追加する
    }
}