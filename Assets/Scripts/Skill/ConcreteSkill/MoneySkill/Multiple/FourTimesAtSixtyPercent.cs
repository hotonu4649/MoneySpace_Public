using RelatedMoney;
using UnityEngine;

namespace Skill.ConcreteSkill.MoneySkill.Multiple
{
    public class FourTimesAtSixtyPercent : IMultiFromMoney
    {
        public Money Execute(Money value)
        {
            var randomValue = Random.Range(0, 100);
            if (randomValue < 60)
            {
                value *= 4;
            }
            return value;
        }
    }
}