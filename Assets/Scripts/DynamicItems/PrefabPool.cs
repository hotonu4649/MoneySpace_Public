using UnityEngine;
using UnityEngine.Pool;
using VContainer;
using VContainer.Unity;

namespace DynamicItems
{
    public class PrefabPool
    {
        private readonly GameObject _prefab;
        private readonly IObjectResolver _resolver;
        private readonly ObjectPool<GameObject> _pool;
        
        public PrefabPool(GameObject prefab)
            : this(prefab, null)
        {
        }

        public PrefabPool(GameObject prefab, IObjectResolver resolver)
        {
            _prefab = prefab;
            _resolver = resolver;
            _pool = new ObjectPool<GameObject>(
                CreatePooledItem,
                OnTakeFromPool,
                OnReturnedToPool,
                OnDestroyPoolObject,
                true,
                10,
                100);
        }
        
        private GameObject CreatePooledItem()
        {
            return _resolver == null
                ? Object.Instantiate(_prefab)
                : _resolver.Instantiate(_prefab);
        }
        
        private void OnTakeFromPool(GameObject obj)
        {
            obj.SetActive(true);
        }
        
        private void OnReturnedToPool(GameObject obj) => obj.SetActive(false);
        
        private void OnDestroyPoolObject(GameObject obj) => Object.Destroy(obj);
        
        public GameObject Get(Vector2 position, Quaternion rotation)
        {
            GameObject obj = _pool.Get();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            return obj;
        }
        
        public GameObject GetUIObject(RectTransform position, Transform parent)
        {
            GameObject obj = _pool.Get();
            obj.transform.SetParent(parent);
            obj.TryGetComponent(out RectTransform rectTransform);
            rectTransform.position = position.position;
            rectTransform.rotation = position.rotation;
            return obj;
        }
        
        public void Release(GameObject obj)
        {
            _pool.Release(obj);
        }
    }
}