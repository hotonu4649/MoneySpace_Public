using System.Threading;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;

namespace DynamicItems.EnemyBullet
{
    public abstract class EnemyShotPattern
    {
        protected GameObject player;
        protected GameObject enemy;
        protected float duration;
        protected readonly Subject<Unit> shotSubject = new Subject<Unit>();
        public Observable<Unit> ShotObservable => shotSubject;

        protected EnemyShotPattern(GameObject player, GameObject enemy)
        {
            this.player = player;
            this.enemy = enemy;
        }
        
        protected void Initialize()
        {
            var direction = player.transform.position - enemy.transform.position;
            var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            enemy.transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        public virtual async UniTask ShootSequence(CancellationToken token)
        {
            await UniTask.CompletedTask;
        }
    }
}