using System.Collections.Generic;
using DynamicItems.PlayerBullet;
using UnityEngine;

namespace DynamicItems.DemoPlayerBullet
{
    public class PlayerBulletEnumToDemoData : MonoBehaviour
    {
        [SerializeField] private List<DemoPlayerBulletData> _bulletDataList;
        private static readonly Dictionary<PlayerBulletEnum.PlayerBulletType, DemoPlayerBulletData> BulletDataMap
            = new Dictionary<PlayerBulletEnum.PlayerBulletType, DemoPlayerBulletData>();
        
        private void Awake()
        {
            foreach (var bulletType in _bulletDataList)
            {
                BulletDataMap[bulletType.BulletType] = bulletType; // シーンを読み直しても重複キーで例外にならないよう上書き
            }
        }
        
        /// <summary>
        /// 指定されたPlayerBulletTypeに対応するPrefabを取得します。
        /// </summary>
        /// <param name="bulletType"></param>
        /// <returns></returns>
        public static GameObject GetPrefab(PlayerBulletEnum.PlayerBulletType bulletType)
        {
            if (BulletDataMap.TryGetValue(bulletType, out DemoPlayerBulletData bulletData))
            {
                return bulletData.Prefab;
            }
            else
            {
                Debug.LogError($"{bulletType}はPlayerBulletEnumToObjのDictionaryに登録されていません。");
                return null;
            }
        }
        
        public static PlayerBulletEnum.PlayerBulletType GetBulletTypeByPrefab(GameObject prefab)
        {
            foreach (var kvp in BulletDataMap)
            {
                if (kvp.Value.Prefab == prefab)
                {
                    return kvp.Key;
                }
            }
            Debug.LogError($"{prefab.name}はPlayerBulletEnumToObjのDictionaryに登録されていません。");
            return default;
        }
    }
}