using RelatedMoney;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Add
{
    public class AddEightyAtTwentyPercentFromEnemy : IAddMoneyFromEnemy
    {
        public Money Execute(Money value)
        {
            var randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 20)
            {
                value += new Money(80);
            }
            return value;
        }
    }
}