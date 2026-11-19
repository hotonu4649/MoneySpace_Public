using System.Collections.Generic;
using DynamicItems.Coin;

namespace Effects.Coin
{
    public class CoinEnumToSpawnEffect
    {
        private static readonly Dictionary<CoinEnum.CoinType, ICoinEffect> CoinEffectMap
            = new Dictionary<CoinEnum.CoinType, ICoinEffect>();

        //TODO: Dictionaryに登録する処理

        public static ICoinEffect GetEffect(CoinEnum.CoinType coinType)
        {
            if (CoinEffectMap.TryGetValue(coinType, out ICoinEffect coinEffect))
            {
                return coinEffect;
            }
            else
            {
                UnityEngine.Debug.LogError($"{coinType}はCoinEnumToSpawnEffectのDictionaryに登録されていません。");
                return null;
            }
        }
    }
}