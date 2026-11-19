using RelatedMoney;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Add
{
    public class AddTwentyAtSixtyPercentFromEnemy : IAddMoneyFromEnemy
    {
        public Money Execute(Money value)
        {
            var randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 60)
            {
                value += new Money(20);
            }
            return value;
        }
    }
}