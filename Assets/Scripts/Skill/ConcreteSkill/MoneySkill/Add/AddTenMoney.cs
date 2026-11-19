using RelatedMoney;

namespace Skill.ConcreteSkill.MoneySkill.Add
{
    public class AddTenMoney : IAddFromMoney
    {
        public Money Execute(Money value)
        {
            return value + new Money(10);
        }
    }
}