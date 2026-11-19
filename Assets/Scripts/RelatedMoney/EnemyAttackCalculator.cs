using System.Collections.Generic;
using Skill.ConcreteSkill.SkillInterface;

namespace RelatedMoney
{
    public class EnemyAttackCalculator
    {
        private readonly List<IReduceEnemyAttack> _reduceEnemyAttackList = new List<IReduceEnemyAttack>();

        public Money CalculateReducedAttack(Money baseMoney)
        {
            var resultMoney = baseMoney;
            foreach (var skill in _reduceEnemyAttackList)
            {
                resultMoney = skill.Execute(resultMoney);
            }
            return resultMoney;
        }
        
        public void AddReduceEnemyAttackSkill(IReduceEnemyAttack reduceEnemyAttackSkill)
        {
            _reduceEnemyAttackList.Add(reduceEnemyAttackSkill);
        }
    }
}