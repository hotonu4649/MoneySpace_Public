using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameSystem
{
    public class HitStop
    {
        public static async UniTask Play(float timeSpeed, float duration)
        {
            Time.timeScale = timeSpeed;
            await UniTask.Delay(TimeSpan.FromSeconds(duration), DelayType.UnscaledDeltaTime);
            Time.timeScale = 1f;
        }
    }
}