using RelatedMoney;
using UnityEngine;

namespace Skill.ConcreteSkill.MoneySkill.Multiple
{
    public class TwentyTimesAtFivePercent : IMultiFromMoney
    {
        public Money Execute(Money value)
        {
            var randomValue = Random.Range(0, 100);
            if (randomValue < 5)
            {
                value *= 20;
            }
            return value;
        }
    }
}