using UnityEngine;

namespace DynamicItems.Enemy
{
    public class EnemyIdentify : MonoBehaviour
    {
        [SerializeField] private EnemyData _enemyData;
        public EnemyData EnemyData => _enemyData;
    }
}