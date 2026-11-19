using System.Collections.Generic;
using UnityEngine;

namespace DynamicItems.PlayerBullet
{
    public static class PlayerBulletSpawner
    {
        private static readonly Dictionary<PlayerBulletEnum.PlayerBulletType, PrefabPool> _prefabPools
            = new  Dictionary<PlayerBulletEnum.PlayerBulletType, PrefabPool>();
        private static readonly Dictionary<GameObject, PrefabPool> _prefabToPoolMap 
            = new Dictionary<GameObject, PrefabPool>();

        /// <summary>
        /// シーンが切り替わると、プール内の弾（シーン上のオブジェクト）は破棄される。
        /// 破棄済みの弾を次のシーンで使い回さないよう、シーンを抜けたらプールを作り直す。
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

        public static GameObject BulletSpawn(PlayerBulletEnum.PlayerBulletType bulletType, Vector2 position, Quaternion rotation)
        {
            if (!_prefabPools.ContainsKey(bulletType))
            {
                var prefab = PlayerBulletEnumToData.GetPrefab(bulletType);
                _prefabPools.Add(bulletType, new PrefabPool(prefab));
            }
            var bullet = _prefabPools[bulletType].Get(position, rotation);
            _prefabToPoolMap.Add(bullet, _prefabPools[bulletType]);
            return bullet;
        }
        
        public static void BulletRelease(GameObject bullet)
        {
            if (_prefabToPoolMap.TryGetValue(bullet, out PrefabPool pool))
            {
                pool.Release(bullet);
                _prefabToPoolMap.Remove(bullet);
            }
            else
            {
                Debug.LogError("この弾はプールに登録されていません。");
            }
        }
    }
}