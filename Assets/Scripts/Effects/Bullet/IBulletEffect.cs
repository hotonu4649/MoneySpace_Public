namespace Effects.Bullet
{
    /// <summary>
    /// Interface for bullet effects that can be applied before and after spawning a bullet.
    /// </summary>
    public interface IBulletEffect : ISpawnBeforeEffect, ISpawnAfterEffect
    {
        // 銃専用のエフェクトを定義する場合は、ここにメソッドを追加してください。
    }
}