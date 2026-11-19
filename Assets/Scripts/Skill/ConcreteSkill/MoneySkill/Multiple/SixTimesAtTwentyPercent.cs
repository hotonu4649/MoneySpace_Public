using RelatedMoney;
using UnityEngine;

namespace Skill.ConcreteSkill.MoneySkill.Multiple
{
    public class SixTimesAtTwentyPercent : IMultiFromMoney
    {
        public Money Execute(Money value)
        {
            var randomValue = Random.Range(0, 100);
            if (randomValue <= 20)
            {
                return value * 6;
            }
            return value;
        }
    }
}