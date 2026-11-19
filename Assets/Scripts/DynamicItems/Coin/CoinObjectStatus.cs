using UnityEngine;

namespace DynamicItems.Coin
{
    /// <summary>
    /// CoinObjectStatusクラスは、コインオブジェクトの状態を管理するMonoBehaviourです。
    /// </summary>
    public class CoinObjectStatus : MonoBehaviour
    {
        [SerializeField] private CoinEnum.CoinType _coinType;
        public CoinEnum.CoinType CoinType => _coinType;
    }
}