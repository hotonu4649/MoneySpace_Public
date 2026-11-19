using RelatedMoney;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Multiple
{
    public class SixTimesAtTwentyPercentFromEnemy : IMultiFromEnemy
    {
        public Money Execute(Money value)
        {
            var randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 20)
            {
                value *= 6;
            }
            return value;
        }
    }
}