using RelatedMoney;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Multiple
{
    public class MultipleFromOneToThreeFromEnemy : IMultiFromEnemy
    {
        public Money Execute(Money value)
        {
            var randomMultiplier = UnityEngine.Random.Range(1, 4);
            return value * randomMultiplier;
        }
    }
}