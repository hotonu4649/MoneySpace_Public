using System.Collections.Generic;
using System.Linq;
using GameSystem;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using R3;
using UnityEngine;
using VContainer;

namespace DynamicItems.Coin
{
    public class RandomCoinSpawn : MonoBehaviour
    {
        [Header("コインの出現設定")]
        [SerializeField] private float _positionY = 4.8f;
        [SerializeField] private float _positionX = 6.4f;
        [SerializeField] private float _spawnInterval = 3.0f;
        [Header("コインの出現確率")]
        [SerializeField] private List<float> _coinSpawnProbabilities; // 各コインの出現確率を設定するリスト
        [SerializeField,Header("リストのどこからがレアか判定する基準"),Tooltip("このインデックス以上のコインは強いコインとして扱われます。")]
        private int _rareCoinIndex;
        [Header("レアコインの出現開始時間")]
        [SerializeField] private float _rareCoinSpawnStartTime;
        private float _nextSpawnTime;
        private GameTimer _gameTimer;
        
        [Inject]
        public void Constructor(GameTimer gameTimer)
        {
            _gameTimer = gameTimer;
            _nextSpawnTime = _spawnInterval;
            _gameTimer.Timer
                .Where(time => time >= _nextSpawnTime && time > 0f)
                .Subscribe(_ =>
                {
                    RandomSpawn();
                    _nextSpawnTime += _spawnInterval;
                }).AddTo(this);
        }

        private void RandomSpawn()
        {
            // あまり強くないコインの出現
            if (_gameTimer.Timer.CurrentValue <= _rareCoinSpawnStartTime)
            {
                var normalCoinList = _coinSpawnProbabilities.GetRange(0, _rareCoinIndex);
                var sumProbabilities = normalCoinList.Sum();
                for (var i = 0; i < normalCoinList.Count; i++)
                {
                    normalCoinList[i] = (normalCoinList[i] / sumProbabilities) * 100f;
                }
                var coinType = GetRandomCoinType(normalCoinList);
                var positionX = GetRandomPositionX();
                var coin = CoinSpawner.CoinSpawn(coinType, new Vector2(positionX, _positionY), new Quaternion(0, 0, 0, 0));
            }
            // 強いコインの出現
            else
            {
                var coinType = GetRandomCoinType(_coinSpawnProbabilities);
                var positionX = GetRandomPositionX();
                var coin = CoinSpawner.CoinSpawn(coinType, new Vector2(positionX, _positionY), new Quaternion(0, 0, 0, 0));
            }
        }
        
        private float GetRandomPositionX()
        {
            return Random.Range(-_positionX, _positionX);
        }
        
        private CoinEnum.CoinType GetRandomCoinType(List<float> probabilities)
        {
            var randomValue = Random.Range(0f, 100f);
            var sumProbabilities = probabilities.Sum();
            var cumulativeProbability = 0f;

            for (var i = 0; i < probabilities.Count; i++)
            {
                cumulativeProbability += probabilities[i]/sumProbabilities * 100f;
                if (randomValue <= cumulativeProbability)
                {
                    return (CoinEnum.CoinType)i;
                }
            }

            // 万が一、確率の合計が100未満の場合は、最後のコインタイプを返す
            return (CoinEnum.CoinType)(probabilities.Count - 1);
        }
    }
}