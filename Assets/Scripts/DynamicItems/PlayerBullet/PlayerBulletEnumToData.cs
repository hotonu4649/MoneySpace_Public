using System.Collections.Generic;
using RelatedMoney;
using UnityEngine;

namespace DynamicItems.PlayerBullet
{
    /// <summary>
    /// PlayerBulletEnumのenum値をGameObjectに変換するクラス
    /// </summary>
    public class PlayerBulletEnumToData : MonoBehaviour
    {
        [SerializeField] private List<PlayerBulletData> _bulletDataList;
        private static readonly Dictionary<PlayerBulletEnum.PlayerBulletType, PlayerBulletData> BulletDataMap
            = new Dictionary<PlayerBulletEnum.PlayerBulletType, PlayerBulletData>();
        
        private void Awake()
        {
            foreach (var bulletType in _bulletDataList)
            {
                // タイトル→ゲームのように別シーンでも登録されるので、Addではなく上書きにする（重複キーで例外になるため）
                BulletDataMap[bulletType.BulletType] = bulletType;
            }
        }
        
        /// <summary>
        /// 指定されたPlayerBulletTypeに対応するPrefabを取得します。
        /// </summary>
        /// <param name="bulletType"></param>
        /// <returns></returns>
        public static GameObject GetPrefab(PlayerBulletEnum.PlayerBulletType bulletType)
        {
            if (BulletDataMap.TryGetValue(bulletType, out PlayerBulletData bulletData))
            {
                return bulletData.Prefab;
            }
            else
            {
                Debug.LogError($"{bulletType}はPlayerBulletEnumToObjのDictionaryに登録されていません。");
                return null;
            }
        }
        
        /// <summary>
        /// 指定されたPlayerBulletTypeに対応する弾丸の速度を取得します。
        /// </summary>
        /// <param name="bulletType"></param>
        /// <returns></returns>
        public static float GetSpeed(PlayerBulletEnum.PlayerBulletType bulletType)
        {
            if (BulletDataMap.TryGetValue(bulletType, out PlayerBulletData bulletData))
            {
                return bulletData.Speed;
            }
            else
            {
                Debug.LogError($"{bulletType}はPlayerBulletEnumToObjのDictionaryに登録されていません。");
                return 0f;
            }
        }
        
        /// <summary>
        /// 指定されたPlayerBulletTypeに対応する弾丸のダメージ量を取得します。
        /// </summary>
        /// <param name="bulletType"></param>
        /// <returns></returns>
        public static Money GetDamage(PlayerBulletEnum.PlayerBulletType bulletType)
        {
            if (BulletDataMap.TryGetValue(bulletType, out PlayerBulletData bulletData))
            {
                return bulletData.Damage;
            }
            else
            {
                Debug.LogError($"{bulletType}はPlayerBulletEnumToObjのDictionaryに登録されていません。");
                return new Money(0);
            }
        }
        
        /// <summary>
        /// 指定されたPlayerBulletTypeに対応する弾丸の消費金額を取得します。
        /// </summary>
        /// <param name="bulletType"></param>
        /// <returns></returns>
        public static Money GetConsumeMoney(PlayerBulletEnum.PlayerBulletType bulletType)
        {
            if (BulletDataMap.TryGetValue(bulletType, out PlayerBulletData bulletData))
            {
                return bulletData.ConsumeMoney;
            }
            else
            {
                Debug.LogError($"{bulletType}はPlayerBulletEnumToObjのDictionaryに登録されていません。");
                return new Money(0);
            }
        }
    }
}