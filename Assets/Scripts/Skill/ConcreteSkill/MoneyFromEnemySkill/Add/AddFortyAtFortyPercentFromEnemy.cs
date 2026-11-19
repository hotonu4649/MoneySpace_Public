using RelatedMoney;

namespace Skill.ConcreteSkill.MoneyFromEnemySkill.Add
{
    public class AddFortyAtFortyPercentFromEnemy : IAddMoneyFromEnemy
    {
        public Money Execute(Money value)
        {
            var randomValue = UnityEngine.Random.Range(0, 100);
            if (randomValue < 40)
            {
                value += new Money(40);
            }
            return value;
        }
    }
}