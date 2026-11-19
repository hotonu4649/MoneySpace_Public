using System;
using System.Collections.Generic;
using R3;
using RelatedMoney;

namespace Effects.RelatedFabulousEffect
{
    public class FabulousEffectTrigger : IDisposable
    {
        private readonly HaveMoneyStatus _haveMoneyStatus;
        private readonly List<Money> _getMoneyAtOnce;
        private int _changeIndex;
        private readonly CompositeDisposable _disposables;
        private readonly Subject<Unit> _onFabulousEffectTriggered;
        public Observable<Unit> OnFabulousEffectTriggered => _onFabulousEffectTriggered;
        
        public FabulousEffectTrigger(HaveMoneyStatus haveMoneyStatus)
        {
            _haveMoneyStatus = haveMoneyStatus;
            _getMoneyAtOnce = new List<Money>();
            _onFabulousEffectTriggered = new Subject<Unit>();
            _disposables = new CompositeDisposable();
        }
        
        public void Initialize()
        {
            Subscribe();
        }

        private void Subscribe()
        {
            _haveMoneyStatus.OnMoneyChanged
                .Subscribe(money =>
                {
                    ChangeList(money);
                    var averageMoney = GetMoneyAverage();
                    TriggerFabulousEffect(money, averageMoney);
                }).AddTo(_disposables);
        }
        
        private void TriggerFabulousEffect(Money money, Money averageMoney)
        {
            if (money > averageMoney * 4)
            {
                _onFabulousEffectTriggered.OnNext(Unit.Default);
            }
        }

        private void ChangeList(Money money)
        {
            
            if (_getMoneyAtOnce.Count < 5)
            {
                _getMoneyAtOnce.Add(money);
            }
            else
            {
                _getMoneyAtOnce[_changeIndex] = money;
                _changeIndex++;
                if (_changeIndex >= 5)
                {
                    _changeIndex = 0;
                }
            }
        }

        private Money GetMoneyAverage()
        {
            var averageMoney = new Money(0);
            foreach (var m in _getMoneyAtOnce)
            {
                averageMoney += m;
            }
            averageMoney /= _getMoneyAtOnce.Count;
            return averageMoney;
        }

        public void Dispose()
        {
            _disposables?.Dispose();
            _onFabulousEffectTriggered?.Dispose();
        }
    }
}