using R3;

namespace RelatedMoney
{
    /// <summary>
    /// 現在の所持金を表すReactiveProperty。値が変更されると、購読者に通知される。
    /// </summary>
    public class HaveMoneyStatus
    {
        private readonly ReactiveProperty<Money> _currentMoney;
        private readonly Subject<Money> _onMoneyChanged;
        public ReadOnlyReactiveProperty<Money> CurrentMoney => _currentMoney;
        public Observable<Money> OnMoneyChanged => _onMoneyChanged;
        
        public HaveMoneyStatus()
        {
            _currentMoney = new ReactiveProperty<Money>();
            _onMoneyChanged = new Subject<Money>();
            _currentMoney.Value = new Money(40);
        }
        
        public void AddMoney(Money money)
        {
            _currentMoney.Value += money;
            _onMoneyChanged.OnNext(money);
        }
        
        public void SubtractMoney(Money money)
        {
            _currentMoney.Value -= money;
        }
    }
}