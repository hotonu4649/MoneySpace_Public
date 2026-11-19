namespace Effects.Coin
{
    /// <summary>
    /// Interface for coin effects that can be applied before and after spawning a coin.
    /// </summary>
    public interface ICoinEffect : ISpawnBeforeEffect, ISpawnAfterEffect
    {
        // コイン専用のエフェクトを定義する場合は、ここにメソッドを追加してください。
    }
}