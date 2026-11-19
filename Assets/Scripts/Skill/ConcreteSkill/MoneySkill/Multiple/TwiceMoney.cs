using RelatedMoney;

namespace Skill.ConcreteSkill.MoneySkill.Multiple
{
    /// <summary>
    /// 金額を2倍にするスキルです。
    /// </summary>
    public class TwiceMoney : IMultiFromMoney
    {
        public Money Execute(Money value)
        {
            return value * 2;
        }
    }
}