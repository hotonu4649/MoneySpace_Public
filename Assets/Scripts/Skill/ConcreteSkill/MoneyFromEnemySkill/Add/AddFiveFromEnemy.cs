using RelatedMoney;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Add
{
    public class AddFiveFromEnemy : IAddMoneyFromEnemy
    {
        public Money Execute(Money value)
        {
            return value + new Money(5);
        }
    }
}