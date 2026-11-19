using System.Collections.Generic;
using System.Linq;
using GameSystem;
using R3;
using UnityEngine;
using VContainer;

namespace DynamicItems.Enemy
{
    public class RandomEnemySpawner : MonoBehaviour
    {
        [Header("敵の設定")]
        [Header("敵の出現確率を設定するリスト")]
        [SerializeField] private List<float> _enemySpawnProbabilities; // 各敵の出現確率を設定するリスト
        [SerializeField,Header("リストのどこからが強いか判定する基準"),Tooltip("このインデックス以上の敵は強い敵として扱われます。")]
        private int _strongEnemyIndex;
        [Header("敵の生成範囲等")]
        [SerializeField,Header("右の座標")] private Vector2 _spawnRight;
        [SerializeField,Header("左の座標")] private Vector2 _spawnLeft;
        [SerializeField,Header("上の座標")] private Vector2 _spawnTop;
        [SerializeField,Header("下の座標")] private Vector2 _spawnBottom;
        [SerializeField,Header("敵の生成距離")] private float _spawnDistance;
        [Header("敵の出現数")]
        [SerializeField] private SerializableReactiveProperty<int> _enemySpawnCount;
        [Header("強い敵の出現開始時間")]
        [SerializeField] private float _strongEnemySpawnStartTime;
        
        private GameTimer _gameTimer;
        private EnemySpawner _enemySpawner;
        private Vector2 _spawnPosition;
        
        public ReadOnlyReactiveProperty<int> CurrentEnemySpawnCount => _enemySpawnCount;
        public Vector2 SpawnPosition => _spawnPosition;

        [Inject]
        public void Construct(GameTimer gameTimer, EnemySpawner enemySpawner)
        {
            _gameTimer = gameTimer;
            _enemySpawner = enemySpawner;
        }

        private void Start()
        {
            for (var i = 0; i < _enemySpawnCount.Value; i++)
            {
                RandomSpawn();
            }
            CurrentEnemySpawnCount
                .Pairwise()
                .Where(pair => pair.Previous >= pair.Current)
                .Subscribe(_ =>
                {
                    RandomSpawn();
                }).AddTo(this);
        }

        private void RandomSpawn()
        {
            // あまり強くない敵の出現
            if (_gameTimer.Timer.CurrentValue <= _strongEnemySpawnStartTime)
            {
                var normalEnemyList = _enemySpawnProbabilities.GetRange(0, _strongEnemyIndex);
                var sumProbabilities = normalEnemyList.Sum();
                for (var i = 0; i < normalEnemyList.Count; i++)
                {
                    normalEnemyList[i] = (normalEnemyList[i] / sumProbabilities) * 100f;
                }
                var enemyType = GetEnemyType(normalEnemyList);
                (Vector2 spawnPosition, Vector2 startPosition) = GetRandomSpawnPosition();
                _spawnPosition = startPosition;
                var enemy = _enemySpawner.EnemySpawn(enemyType, spawnPosition, Quaternion.identity);
            }
            // 強い敵の出現
            else
            {
                var enemyType = GetEnemyType(_enemySpawnProbabilities);
                (Vector2 spawnPosition, Vector2 startPosition) = GetRandomSpawnPosition();
                _spawnPosition = startPosition;
                var enemy = _enemySpawner.EnemySpawn(enemyType, spawnPosition, Quaternion.identity);
            }
        }
        
        public int SubtractEnemyCount(int count)
        {
            _enemySpawnCount.Value -= count;
            return _enemySpawnCount.Value;
        }
        
        private EnemyEnum.EnemyType GetEnemyType(List<float> probabilities)
        {
            var randomValue = Random.Range(0f, 100f);
            var sumProbabilities = probabilities.Sum();
            var cumulativeProbability = 0f;

            for (var i = 0; i < probabilities.Count; i++)
            {
                cumulativeProbability += probabilities[i] / sumProbabilities * 100f;

                if (randomValue <= cumulativeProbability)
                {
                    return (EnemyEnum.EnemyType)i;
                }
            }

            return (EnemyEnum.EnemyType)(probabilities.Count - 1);
        }
        
        private (Vector2, Vector2) GetRandomSpawnPosition()
        {
            var randomPosition = Random.Range(0, 4);
            Vector2 spawnPosition = Vector2.zero;
            Vector2 startPosition = Vector2.zero;
            switch (randomPosition)
            {
                case 0:
                    spawnPosition = new Vector2(Random.Range(_spawnLeft.x, _spawnRight.x), _spawnTop.y + _spawnDistance);
                    startPosition = spawnPosition - new Vector2(0, _spawnDistance);
                    break;
                case 1:
                    spawnPosition = new Vector2(Random.Range(_spawnLeft.x, _spawnRight.x), _spawnBottom.y - _spawnDistance);
                    startPosition = spawnPosition + new Vector2(0, _spawnDistance);
                    break;
                case 2:
                    spawnPosition = new Vector2(_spawnLeft.x - _spawnDistance, Random.Range(_spawnBottom.y, _spawnTop.y));
                    startPosition = spawnPosition + new Vector2(_spawnDistance, 0);
                    break;
                case 3:
                    spawnPosition = new Vector2(_spawnRight.x + _spawnDistance, Random.Range(_spawnBottom.y, _spawnTop.y));
                    startPosition = spawnPosition - new Vector2(_spawnDistance, 0);
                    break;
            }
            return (spawnPosition, startPosition);
        }
    }
}