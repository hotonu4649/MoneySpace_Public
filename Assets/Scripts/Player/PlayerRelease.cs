using System;
using DynamicItems.Coin;
using VContainer.Unity;
using R3;

namespace Player
{
    /// <summary>
    /// PlayerReleaseクラスは、プレイヤーがコインを取得した際にお金の状態を更新するためのハンドラーです。
    /// </summary>
    public class PlayerRelease : IStartable, IDisposable
    {
        private PlayerCollision _playerCollision;
        private CompositeDisposable _disposables = new CompositeDisposable();
        
        public PlayerRelease(PlayerCollision playerCollision)
        {
            _playerCollision = playerCollision;
        }
        
        public void Start()
        {
            _playerCollision.CoinCollisionObservable
                .Subscribe(coinObjectStatus =>
                {
                    CoinSpawner.CoinRelease(coinObjectStatus.gameObject);
                })
                .AddTo(_disposables);
        }
        
        // VContainerのライフサイクルに従って、Disposeメソッドを実装することで、リソースの解放を行います。
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}