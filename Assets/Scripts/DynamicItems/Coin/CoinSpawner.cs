using System.Collections.Generic;
using UnityEngine;

namespace DynamicItems.Coin
{
    public static class CoinSpawner
    {
        private static readonly Dictionary<CoinEnum.CoinType, PrefabPool> _prefabPools
            = new  Dictionary<CoinEnum.CoinType, PrefabPool>();
        private static readonly Dictionary<GameObject, PrefabPool> _prefabToPoolMap 
            = new Dictionary<GameObject, PrefabPool>();

        /// <summary>
        /// シーンが切り替わると、プール内のオブジェクトは破棄される。
        /// 破棄済みのオブジェクトを次のシーンで使い回さないよう、シーンを抜けたらプールを作り直す。
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void RegisterSceneReset()
        {
            Clear();
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded -= OnSceneUnloaded;
            UnityEngine.SceneManagement.SceneManager.sceneUnloaded += OnSceneUnloaded;
        }

        private static void OnSceneUnloaded(UnityEngine.SceneManagement.Scene _) => Clear();

        private static void Clear()
        {
            _prefabPools.Clear();
            _prefabToPoolMap.Clear();
        }
        
        public static GameObject CoinSpawn(CoinEnum.CoinType coinType, Vector2 position, Quaternion rotation)
        {
            if (!_prefabPools.ContainsKey(coinType))
            {
                var prefab = CoinEnumToData.GetPrefab(coinType);
                _prefabPools.Add(coinType, new PrefabPool(prefab));
            }
            var coin = _prefabPools[coinType].Get(position, rotation);
            _prefabToPoolMap.Add(coin, _prefabPools[coinType]);
            return coin;
        }

        public static void CoinRelease(GameObject coin)
        {
            if (_prefabToPoolMap.TryGetValue(coin, out var pool))
            {
                pool.Release(coin);
                _prefabToPoolMap.Remove(coin);
            }
            else
            {
                Debug.LogError("このコインはプールに登録されていません。");
            }
        }
    }
}