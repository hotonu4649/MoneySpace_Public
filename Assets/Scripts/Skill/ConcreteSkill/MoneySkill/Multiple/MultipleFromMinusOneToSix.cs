using RelatedMoney;

namespace Skill.ConcreteSkill.MoneySkill.Multiple
{
    public class MultipleFromMinusOneToSix : IMultiFromMoney
    {
        public Money Execute(Money value)
        {
            var randomMultiplier = UnityEngine.Random.Range(-1, 7);
            return value * randomMultiplier;
        }
    }
}