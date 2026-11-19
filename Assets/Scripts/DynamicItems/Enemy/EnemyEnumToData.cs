using System.Collections.Generic;
using DynamicItems.EnemyBullet;
using RelatedMoney;
using UnityEngine;

namespace DynamicItems.Enemy
{
    public class EnemyEnumToData : MonoBehaviour
    {
        [SerializeField] private List<EnemyData> _enemyDataList;
        private static readonly Dictionary<EnemyEnum.EnemyType, EnemyData> EnemyDataMap
            = new Dictionary<EnemyEnum.EnemyType, EnemyData>();
        
        private void Awake()
        {
            foreach (var enemyData in _enemyDataList)
            {
                EnemyDataMap[enemyData.EnemyType] = enemyData; // シーンを読み直しても重複キーで例外にならないよう上書き
            }
        }
        
        public static GameObject GetPrefab(EnemyEnum.EnemyType enemyType)
        {
            if (EnemyDataMap.TryGetValue(enemyType, out EnemyData enemyData))
            {
                return enemyData.Prefab;
            }
            else
            {
                Debug.LogError($"{enemyType}はEnemyEnumToDataのDictionaryに登録されていません。");
                return null;
            }
        }
        
        public static float GetSpeed(EnemyEnum.EnemyType enemyType)
        {
            if (EnemyDataMap.TryGetValue(enemyType, out EnemyData enemyData))
            {
                return enemyData.Speed;
            }
            else
            {
                Debug.LogError($"{enemyType}はEnemyEnumToDataのDictionaryに登録されていません。");
                return 0f;
            }
        }
        
        public static Money GetEnemyHp(EnemyEnum.EnemyType enemyType)
        {
            if (EnemyDataMap.TryGetValue(enemyType, out EnemyData enemyData))
            {
                return enemyData.EnemyHp;
            }
            else
            {
                Debug.LogError($"{enemyType}はEnemyEnumToDataのDictionaryに登録されていません。");
                return new Money(0);
            }
        }
        
        public static EnemyBulletData GetEnemyBulletData(EnemyEnum.EnemyType enemyType)
        {
            if (EnemyDataMap.TryGetValue(enemyType, out EnemyData enemyData))
            {
                return enemyData.EnemyBulletData;
            }
            else
            {
                Debug.LogError($"{enemyType}はEnemyEnumToDataのDictionaryに登録されていません。");
                return null;
            }
        }
    }
}