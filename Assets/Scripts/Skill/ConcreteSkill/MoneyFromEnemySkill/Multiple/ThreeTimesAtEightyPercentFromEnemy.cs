using RelatedMoney;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Multiple
{
    public class ThreeTimesAtEightyPercentFromEnemy : IMultiFromEnemy
    {
        public Money Execute(Money value)
        {
            var randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 80)
            {
                value *= 3;
            }
            return value;
        }
    }
}