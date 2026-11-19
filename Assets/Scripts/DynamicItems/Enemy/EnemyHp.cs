using R3;
using RelatedMoney;

namespace DynamicItems.Enemy
{
    public class EnemyHp
    {
        private Money MaxHp { get; }
        public Money CurrentHp { get; private set; }
        
        private readonly ReactiveProperty<bool> _isDead = new ReactiveProperty<bool>();
        public ReadOnlyReactiveProperty<bool> IsDead => _isDead;

        public EnemyHp(Money maxHp)
        {
            MaxHp = maxHp;
            CurrentHp = MaxHp;
        }
        
        public void TakeDamage(Money damage)
        {
            CurrentHp -= damage;
            
            if (CurrentHp <= new Money(0))
            {
                _isDead.Value = true;
            }
        }
        
        public void InstantiateHp()
        {
            CurrentHp = MaxHp;
            _isDead.Value = false;
        }
    }
}