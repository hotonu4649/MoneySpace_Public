using System.Threading;
using Cysharp.Threading.Tasks;

namespace Player.AutoShot
{
    public class AutoShotDuration
    {
        private PlayerShot _playerShot;
        private float _duration;
        
        public AutoShotDuration(PlayerShot playerShot)
        {
            _playerShot = playerShot;
            _duration = 2f;
        }
        
        public void SetAutoShotDuration(float duration)
        {
            _duration = duration;
        }
        
        private async UniTask AutoShot(CancellationToken token)
        {
            _playerShot.Shot();
            await UniTask.Delay(System.TimeSpan.FromSeconds(_duration), cancellationToken:token);
        }
        
        public async UniTask StartAutoShot(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {   
                await AutoShot(token);
            }
        }
    }
}