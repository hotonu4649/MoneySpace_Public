using RelatedMoney;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Multiple
{
    public class FiveTimesAtFortyPercentFromEnemy : IMultiFromEnemy
    {
        public Money Execute(Money value)
        {
            var randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 40)
            {
                value *= 5;
            }
            return value;
        }
    }
}