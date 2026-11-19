using System.Collections.Generic;
using UnityEngine;

namespace DynamicItems.EnemyBullet
{
    public class EnemyBulletEnumToData : MonoBehaviour
    {
        [SerializeField] private List<EnemyBulletData> _enemyBulletDataList;
        private static readonly Dictionary<EnemyBulletEnum.EnemyBulletType, EnemyBulletData> EnemyBulletDataMap
            = new Dictionary<EnemyBulletEnum.EnemyBulletType, EnemyBulletData>();
        
        private void Awake()
        {
            foreach (var enemyBulletData in _enemyBulletDataList)
            {
                EnemyBulletDataMap[enemyBulletData.EnemyBulletType] = enemyBulletData; // シーンを読み直しても重複キーで例外にならないよう上書き
            }
        }
        
        public static GameObject GetPrefab(EnemyBulletEnum.EnemyBulletType enemyBulletType)
        {
            if (EnemyBulletDataMap.TryGetValue(enemyBulletType, out EnemyBulletData enemyBulletData))
            {
                return enemyBulletData.Prefab;
            }
            else
            {
                Debug.LogError($"{enemyBulletType}はEnemyBulletEnumToDataのDictionaryに登録されていません。");
                return null;
            }
        }
        
        public static float GetSpeed(EnemyBulletEnum.EnemyBulletType enemyBulletType)
        {
            if (EnemyBulletDataMap.TryGetValue(enemyBulletType, out EnemyBulletData enemyBulletData))
            {
                return enemyBulletData.Speed;
            }
            else
            {
                Debug.LogError($"{enemyBulletType}はEnemyBulletEnumToDataのDictionaryに登録されていません。");
                return 0f;
            }
        }
        
        public static float GetDamage(EnemyBulletEnum.EnemyBulletType enemyBulletType)
        {
            if (EnemyBulletDataMap.TryGetValue(enemyBulletType, out EnemyBulletData enemyBulletData))
            {
                return enemyBulletData.Damage;
            }
            else
            {
                Debug.LogError($"{enemyBulletType}はEnemyBulletEnumToDataのDictionaryに登録されていません。");
                return 0f;
            }
        }
        
        public static float GetDamageDuration(EnemyBulletEnum.EnemyBulletType enemyBulletType)
        {
            if (EnemyBulletDataMap.TryGetValue(enemyBulletType, out EnemyBulletData enemyBulletData))
            {
                return enemyBulletData.DamageUpDuration;
            }
            else
            {
                Debug.LogError($"{enemyBulletType}はEnemyBulletEnumToDataのDictionaryに登録されていません。");
                return 0f;
            }
        }
        
        public static float GetDamageDurationAcceleration(EnemyBulletEnum.EnemyBulletType enemyBulletType)
        {
            if (EnemyBulletDataMap.TryGetValue(enemyBulletType, out EnemyBulletData enemyBulletData))
            {
                return enemyBulletData.DamageUpDurationAcceleration;
            }
            else
            {
                Debug.LogError($"{enemyBulletType}はEnemyBulletEnumToDataのDictionaryに登録されていません。");
                return 0f;
            }
        }
    }
}