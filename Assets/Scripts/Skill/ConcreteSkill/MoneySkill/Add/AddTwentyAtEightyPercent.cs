using RelatedMoney;

namespace Skill.ConcreteSkill.MoneySkill.Add
{
    public class AddTwentyAtEightyPercent : IAddFromMoney
    {
        public Money Execute(Money value)
        {
            var randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 80)
            {
                value += new Money(20);
            }
            return value;
        }
    }
}