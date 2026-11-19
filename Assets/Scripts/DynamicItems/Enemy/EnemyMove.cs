using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace DynamicItems.Enemy
{
    [RequireComponent(typeof(EnemyIdentify))]
    public class EnemyMove : MonoBehaviour
    {
        [Header("敵の生成範囲等")]
        [SerializeField,Header("右の座標")] private Vector2 _spawnRight;
        [SerializeField,Header("左の座標")] private Vector2 _spawnLeft;
        [SerializeField,Header("上の座標")] private Vector2 _spawnTop;
        [SerializeField,Header("下の座標")] private Vector2 _spawnBottom;
        
        private EnemyIdentify _enemyIdentify;
        private EnemyMoveRules _enemyMoveRules;
        private float _speed;
        private Rect _bounds;
        
        private void Awake()
        {
            TryGetComponent(out _enemyIdentify);
            _speed = _enemyIdentify.EnemyData.Speed;
            _bounds = new Rect(_spawnLeft.x, _spawnBottom.y, _spawnRight.x - _spawnLeft.x, _spawnTop.y - _spawnBottom.y);
            _enemyMoveRules = new EnemyMoveRules(_bounds);
        }

        public async UniTask MoveToNextPositionAsync(CancellationToken cancellationToken)
        {
            // Task.Delayだとシーン遷移で敵が破棄された直後に続きが走ってMissingReferenceExceptionになるため、
            // プレイヤーループ上で待つUniTask.Delayにしている（待ち時間の扱いは今までと同じ実時間）
            await UniTask.Delay(TimeSpan.FromSeconds(Random.Range(1f, 2f)), DelayType.Realtime, cancellationToken: cancellationToken);
            if (this == null) return;
            var nextPositions = _enemyMoveRules.Generate(transform.position);
            foreach (var point in nextPositions)
            {
                var distance = Vector2.Distance(transform.position, point);
                var duration = distance / _speed;
                await transform.DOMove(point, duration).ToUniTask(cancellationToken: cancellationToken);
            }
        }
        
        public async UniTask SetPosition(Vector2 position, CancellationToken token)
        {
            await transform.DOMove(position, 1f).SetEase(Ease.InExpo).ToUniTask(cancellationToken: token);
        }
    }
}