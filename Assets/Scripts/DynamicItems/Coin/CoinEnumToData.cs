using System.Collections.Generic;
using RelatedMoney;
using UnityEngine;

namespace DynamicItems.Coin
{
    /// <summary>
    /// CoinEnumのenum値をGameObjectに変換するクラス
    /// </summary>
    public class CoinEnumToData : MonoBehaviour
    {
        [SerializeField] private List<CoinData> _coinDataList;
        private static readonly Dictionary<CoinEnum.CoinType, CoinData> CoinDataMap
            = new Dictionary<CoinEnum.CoinType, CoinData>();

        private void Awake()
        {
            foreach (var coinType in _coinDataList)
            {
                CoinDataMap[coinType.CoinType] = coinType; // シーンを読み直しても重複キーで例外にならないよう上書き
            }
        }
        
        public static GameObject GetPrefab(CoinEnum.CoinType coinType)
        {
            if (CoinDataMap.TryGetValue(coinType, out CoinData coinData))
            {
                return coinData.Prefab;
            }
            else
            {
                Debug.LogError($"{coinType}はCoinEnumToDataのDictionaryに登録されていません。");
                return null;
            }
        }
        
        public static Money GetValue(CoinEnum.CoinType coinType)
        {
            if (CoinDataMap.TryGetValue(coinType, out CoinData coinData))
            {
                return coinData.Value;
            }
            else
            {
                Debug.LogError($"{coinType}はCoinEnumToDataのDictionaryに登録されていません。");
                return new Money(0);
            }
        }
        
        public static float GetMoveSpeed(CoinEnum.CoinType coinType)
        {
            if (CoinDataMap.TryGetValue(coinType, out CoinData coinData))
            {
                return coinData.MoveSpeed;
            }
            else
            {
                Debug.LogError($"{coinType}はCoinEnumToDataのDictionaryに登録されていません。");
                return 0f;
            }
        }
    }
}