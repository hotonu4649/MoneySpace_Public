using R3;
using UnityEngine;
using VContainer.Unity;

namespace GameSystem
{
    public class GameTimer : ITickable
    {
        private bool isRunning = true;
        private readonly ReactiveProperty<float> _timer;
        public ReadOnlyReactiveProperty<float> Timer => _timer;
        
        public GameTimer()
        {
            _timer = new ReactiveProperty<float>(0f);
        }
        
        public void Tick()
        {
            if (!isRunning) return;
            _timer.Value += Time.deltaTime;
        }
        
        public void Stop()
        {
            isRunning = false;
        }
        
        public void Start()
        {
            isRunning = true;
        }
    }
}