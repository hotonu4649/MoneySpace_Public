using RelatedMoney;
using UnityEngine;

namespace DynamicItems.Coin
{
    /// <summary>
    /// CoinDataクラスは、コインのデータを定義するために使用されます。
    /// </summary>
    [CreateAssetMenu]
    public class CoinData : ScriptableObject, ICoinData
    {
        [SerializeField] private CoinEnum.CoinType _coinType;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private int _value;
        [SerializeField] private float _moveSpeed;
        
        public CoinEnum.CoinType CoinType => _coinType;
        public GameObject Prefab => _prefab;
        public Money Value => new Money(_value);
        public float MoveSpeed => _moveSpeed;
    }
}