using RelatedMoney;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Add
{
    public class AddTenAtEightyPercentFromEnemy : IAddMoneyFromEnemy
    {
        public Money Execute(Money value)
        {
            var randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 80)
            {
                value += new Money(10);
            }
            return value;
        }
    }
}