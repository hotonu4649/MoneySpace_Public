using RelatedMoney;
using UnityEngine;

namespace Skill.ConcreteSkill.MoneySkill.Add
{
    public class AddEightyAtFortyPercent : IAddFromMoney
    {
        public Money Execute(Money value)
        {
            var randomValue = Random.Range(0, 100);
            if (randomValue <= 40)
            {
                value += new Money(80);
            }
            return value;
        }
    }
}