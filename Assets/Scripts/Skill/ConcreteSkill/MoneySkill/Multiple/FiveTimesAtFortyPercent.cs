using RelatedMoney;
using UnityEngine;

namespace Skill.ConcreteSkill.MoneySkill.Multiple
{
    public class FiveTimesAtFortyPercent : IMultiFromMoney
    {
        public Money Execute(Money value)
        {
            var randomValue = Random.Range(0, 100);
            if (randomValue < 40)
            {
                value *= 5;
            }

            return value;
        }
    }
}