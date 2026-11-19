using System.Collections.Generic;
using DynamicItems.PlayerBullet;
using UnityEngine;

namespace Effects.Bullet
{
    public class PlayerBulletEnumToSpawnEffect
    {
        private static readonly Dictionary<PlayerBulletEnum.PlayerBulletType, IBulletEffect> BulletEffectMap
            = new Dictionary<PlayerBulletEnum.PlayerBulletType, IBulletEffect>();
        
        //TODO: Dictionaryに登録する処理
        
        public static IBulletEffect GetEffect(PlayerBulletEnum.PlayerBulletType bulletType)
        {
            if (BulletEffectMap.TryGetValue(bulletType, out IBulletEffect bulletEffect))
            {
                return bulletEffect;
            }
            else
            {
                Debug.LogError($"{bulletType}はPlayerBulletEnumToSpawnEffectのDictionaryに登録されていません。");
                return null;
            }
        }
    }
}