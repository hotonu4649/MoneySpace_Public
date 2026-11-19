namespace DynamicItems.EnemyBullet
{
    public interface IEnemyBulletData : ISpawnData
    {
        public EnemyBulletEnum.EnemyBulletType EnemyBulletType { get; }
        public float Speed { get; }
        public int Damage { get; }
        public float DamageUpDuration { get; }
        public float DamageUpDurationAcceleration { get; }
    }
}