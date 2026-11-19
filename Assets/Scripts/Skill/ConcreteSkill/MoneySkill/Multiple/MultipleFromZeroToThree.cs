using RelatedMoney;

namespace Skill.ConcreteSkill.MoneySkill.Multiple
{
    public class MultipleFromZeroToThree : IMultiFromMoney
    {
        public Money Execute(Money value)
        {
            var randomMultiplier = UnityEngine.Random.Range(0, 4);
            return value * randomMultiplier;
        }
    }
}