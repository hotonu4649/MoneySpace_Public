using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using DynamicItems.Enemy;
using R3;
using UnityEngine;

namespace DynamicItems.EnemyBullet.EnemyShotPatternFile
{
    /// <summary>
    /// 敵がプレイヤーの方向に向かって弾を撃つパターン
    /// </summary>
    public class ShotStraightToPlayer : EnemyShotPattern
    {
        public ShotStraightToPlayer(GameObject playerObject, GameObject enemyPrefab) 
            : base(playerObject, enemyPrefab)
        {
            player = playerObject;
            enemy = enemyPrefab;
            duration = enemyPrefab.GetComponent<EnemyIdentify>().EnemyData.EnemyBulletData.BulletDuration;
        }
        
        public override async UniTask ShootSequence(CancellationToken token)
        {
            Initialize();
            shotSubject.OnNext(Unit.Default);
            await UniTask.Delay(TimeSpan.FromSeconds(duration), cancellationToken: token);
        }
    }
}