using RelatedMoney;
using UnityEngine;

namespace Skill.ConcreteSkill.MoneySkill.Add
{
    public class AddFortyAtSixtyPercent : IAddFromMoney
    {
        public Money Execute(Money value)
        {
            var randomValue = Random.Range(0, 100);
            if (randomValue <= 60)
            {
                return value + new Money(40);
            }
            return value;
        }
    }
}