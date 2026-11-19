using UnityEngine;

namespace DynamicItems.EnemyBullet
{
    public class EnemyBulletBaseInfo
    {
        public int CalculateBaseDamage(EnemyBulletData bulletData, float enemySpawnedTime, float currentTime)
        {
            var initialDamage = Mathf.Max(1, bulletData.Damage);
            var timeScale = CalculateTimeScale(currentTime);
            var ageScale = CalculateEnemyAgeScale(currentTime - enemySpawnedTime);

            var damageScale = Mathf.Min(timeScale * ageScale, MaxDamageScale);
            var baseDamage = Mathf.RoundToInt(initialDamage * damageScale);
            return Mathf.Max(initialDamage, baseDamage);
        }

        // 難しすぎる/簡単すぎるときは、該当する時間帯の倍率を書き換える。
        private static readonly Vector2[] DamageScaleKeys =
        {
            new Vector2(0f, 1f),
            new Vector2(60f, 4f),
            new Vector2(120f, 12f),
            new Vector2(180f, 30f),
            new Vector2(240f, 60f),
            new Vector2(300f, 110f),
            new Vector2(360f, 200f),
            new Vector2(420f, 400f),
            new Vector2(480f, 1500f),
            new Vector2(540f, 8000f),
            new Vector2(600f, 50000f),
            new Vector2(660f, 300000f),
        };

        /// <summary>
        /// 倒されずに生き残っている敵ほど強くなる上限倍率。
        /// 放置した敵はAgeScaleMaxDuration秒かけて最大×1.5まで強くなる。
        /// </summary>
        private const float MaxEnemyAgeScale = 1.5f;
        private const float AgeScaleMaxDuration = 120f;

        /// <summary>intのオーバーフロー防止用の上限倍率（Hyper弾3ダメージ×1億でもint範囲内）</summary>
        private const float MaxDamageScale = 100_000_000f;

        private static float CalculateTimeScale(float currentTime)
        {
            var time = Mathf.Max(0f, currentTime);
            var last = DamageScaleKeys.Length - 1;

            for (var i = 0; i < last; i++)
            {
                if (time <= DamageScaleKeys[i + 1].x)
                {
                    return LogLerp(DamageScaleKeys[i], DamageScaleKeys[i + 1], time);
                }
            }
            // 表の最後より後は、最後の区間と同じ伸び率で増やし続ける
            return LogLerp(DamageScaleKeys[last - 1], DamageScaleKeys[last], time);
        }

        /// <summary>2点間を対数で補間する（t が範囲外なら外挿）</summary>
        private static float LogLerp(Vector2 from, Vector2 to, float time)
        {
            var t = (time - from.x) / (to.x - from.x);
            return from.y * Mathf.Pow(to.y / from.y, t);
        }

        private static float CalculateEnemyAgeScale(float enemyAge)
        {
            var ageRatio = Mathf.Clamp01(enemyAge / AgeScaleMaxDuration);
            return Mathf.Lerp(1f, MaxEnemyAgeScale, ageRatio);
        }
    }
}