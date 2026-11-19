using RelatedMoney;
using UnityEngine;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Multiple
{
    public class FourTimesAtSixtyPercentFromEnemy : IMultiFromEnemy
    {
        public Money Execute(Money value)
        {
            var randomValue = Random.Range(0, 100);
            if (randomValue <= 60)
            {
                return value * 4;
            }
            return value;
        }
    }
}