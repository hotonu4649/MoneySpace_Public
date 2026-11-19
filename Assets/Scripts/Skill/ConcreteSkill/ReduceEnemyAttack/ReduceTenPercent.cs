using RelatedMoney;
using Skill.ConcreteSkill.SkillInterface;

namespace Skill.ConcreteSkill.ReduceEnemyAttack
{
    public class ReduceTenPercent : IReduceEnemyAttack
    {
        public Money Execute(Money value)
        {
            return value * 9 / 10;
        }
    }
}