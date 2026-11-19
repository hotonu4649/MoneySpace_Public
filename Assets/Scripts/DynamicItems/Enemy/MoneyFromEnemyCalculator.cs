using System.Collections.Generic;
using RelatedMoney;
using Skill.ConcreteSkill.MoneyFromEnemySkill;

namespace DynamicItems.Enemy
{
    public class MoneyFromEnemyCalculator
    {
        private List<IAddMoneyFromEnemy> _addMoneyFromEnemySkills = new ();
        private List<IMultiFromEnemy> _multiFromEnemySkills = new ();
        
        public Money CalculateMoneyFromEnemy(Money baseMoney)
        {
            var resultMoney = baseMoney;
            foreach (var skill in _addMoneyFromEnemySkills)
            {
                resultMoney = skill.Execute(resultMoney);
            }
            foreach (var skill in _multiFromEnemySkills)
            {
                resultMoney = skill.Execute(resultMoney);
            }
            return resultMoney;
        }
        
        public void AddMoneyFromEnemySkill(IAddMoneyFromEnemy moneyFromEnemySkill)
        {
            _addMoneyFromEnemySkills.Add(moneyFromEnemySkill);
        }
        
        public void AddMultiFromEnemySkill(IMultiFromEnemy multiFromEnemySkill)
        {
            _multiFromEnemySkills.Add(multiFromEnemySkill);
        }
    }
}