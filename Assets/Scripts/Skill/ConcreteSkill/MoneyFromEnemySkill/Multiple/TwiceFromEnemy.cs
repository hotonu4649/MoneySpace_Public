using RelatedMoney;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Multiple
{
    public class TwiceFromEnemy : IMultiFromEnemy
    {
        public Money Execute(Money value)
        {
            return value * 2;
        }
    }
}