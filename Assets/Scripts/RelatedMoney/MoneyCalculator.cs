using System.Collections.Generic;
using Skill;
using Skill.ConcreteSkill.MoneySkill;
using Skill.ConcreteSkill.SkillInterface;

namespace RelatedMoney
{
    public class MoneyCalculator
    {
        private readonly List<IAddFromMoney> _addFromMoneyList = new ();
        private readonly List<IMultiFromMoney> _multiFromMoneyList = new ();

        public Money CalculateAddMoney(Money baseMoney)
        {
            var resultMoney = baseMoney;
            foreach (var skill in _addFromMoneyList)
            {
                resultMoney = skill.Execute(resultMoney);
            }

            foreach (var skill in _multiFromMoneyList)
            {
                resultMoney = skill.Execute(resultMoney);
            }
            return resultMoney;
        }
        
        public void AddMAddMoneySkillList(IAddFromMoney moneySkill)
        {
            _addFromMoneyList.Add(moneySkill);
        }
        
        public void AddMultiMoneySkillList(IMultiFromMoney moneySkill)
        {
            _multiFromMoneyList.Add(moneySkill);
        }
    }
}