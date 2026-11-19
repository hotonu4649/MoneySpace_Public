using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace DynamicItems.Enemy
{
    public class EnemySpawner
    {
        private readonly IObjectResolver _resolver;
        private readonly Dictionary<EnemyEnum.EnemyType, PrefabPool> _prefabPools
            = new Dictionary<EnemyEnum.EnemyType, PrefabPool>();
        private readonly Dictionary<GameObject, PrefabPool> _prefabToPoolMap
            = new Dictionary<GameObject, PrefabPool>();
        
        public EnemySpawner(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public GameObject EnemySpawn(EnemyEnum.EnemyType enemyType, Vector2 position, Quaternion rotation)
        {
            if (!_prefabPools.ContainsKey(enemyType))
            {
                var prefab = EnemyEnumToData.GetPrefab(enemyType);
                _prefabPools.Add(enemyType, new PrefabPool(prefab, _resolver));
            }
            var enemy = _prefabPools[enemyType].Get(position, rotation);
            _prefabToPoolMap.Add(enemy, _prefabPools[enemyType]);
            return enemy;
        }
        
        public void EnemyRelease(GameObject enemy)
        {
            if (_prefabToPoolMap.TryGetValue(enemy, out var pool))
            {
                pool.Release(enemy);
                _prefabToPoolMap.Remove(enemy);
            }
            else
            {
                Debug.LogError("この敵はプールに登録されていません。");
            }
        }
    }
}