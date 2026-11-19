using RelatedMoney;

namespace Skill.ConcreteSkill.MoneySkill.Add
{
    public class AddThreeHundredAtFivePercent : IAddFromMoney
    {
        public Money Execute(Money value)
        {
            var randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 5)
            {
                value += new Money(300);
            }
            return value;
        }
    }
}