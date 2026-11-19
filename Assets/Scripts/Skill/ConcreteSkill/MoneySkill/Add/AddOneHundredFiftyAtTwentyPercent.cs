using RelatedMoney;

namespace Skill.ConcreteSkill.MoneySkill.Add
{
    public class AddOneHundredFiftyAtTwentyPercent : IAddFromMoney
    {
        public Money Execute(Money value)
        {
            var randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 20)
            {
                value += new Money(150);
            }

            return value;
        }
    }
}