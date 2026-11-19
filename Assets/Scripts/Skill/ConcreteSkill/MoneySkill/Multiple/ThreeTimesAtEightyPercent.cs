using RelatedMoney;
using UnityEngine;

namespace Skill.ConcreteSkill.MoneySkill.Multiple
{
    public class ThreeTimesAtEightyPercent : IMultiFromMoney
    {
        public Money Execute(Money value)
        {
            var randomValue = Random.Range(0, 100);
            if (randomValue <= 80)
            {
                return value * 3;
            }
            return value;
        }
    }
}