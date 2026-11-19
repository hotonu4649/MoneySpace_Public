using RelatedMoney;
using UnityEngine;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Multiple
{
    public class TenTimesAtFivePercentFromEnemy : IMultiFromEnemy
    {
        public Money Execute(Money value)
        {
            var randomValue = Random.Range(0, 100);
            if (randomValue < 5)
            {
                value *= 10;
            }
            return value;
        }
    }
}