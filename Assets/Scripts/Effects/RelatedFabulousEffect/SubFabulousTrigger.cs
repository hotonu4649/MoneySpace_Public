using System;
using RelatedMoney;
using System.Collections.Generic;
using R3;

namespace Effects.RelatedFabulousEffect
{
    public class SubFabulousTrigger : IDisposable
    {
        private readonly HaveMoneyStatus _haveMoneyStatus;
        private readonly FabulousEffectPlayer _fabulousEffectPlayer;
        private readonly List<Money> _getMoneyAtOnce;
        private int _changeIndex;
        private  CompositeDisposable _miniDisposable;
        private readonly CompositeDisposable _disposables;
        private readonly Subject<Unit> _onSubFabulousEffectTriggered;
        public Observable<Unit> OnSubFabulousEffectTriggered => _onSubFabulousEffectTriggered;
        
        public SubFabulousTrigger(
            HaveMoneyStatus haveMoneyStatus,
            FabulousEffectPlayer fabulousEffectPlayer)
        {
            _haveMoneyStatus = haveMoneyStatus;
            _fabulousEffectPlayer = fabulousEffectPlayer;
            _getMoneyAtOnce = new List<Money>();
            _disposables = new CompositeDisposable();
            _onSubFabulousEffectTriggered = new Subject<Unit>();
            _miniDisposable = new CompositeDisposable();
        }
        
        public void Initialize()
        {
            Subscribe();
        }
        
        private void Subscribe()
        {
            _fabulousEffectPlayer.IsPlaying
                .Where(isPlaying => isPlaying)
                .Subscribe(_ =>
                {
                    _haveMoneyStatus.OnMoneyChanged
                        .Subscribe(money =>
                        {
                            ChangeList(money);
                            var averageMoney = GetMoneyAverage();
                            TriggerSubFabulousEffect(money, averageMoney);
                        }).AddTo(_miniDisposable);
                }).AddTo(_disposables);
            
            _fabulousEffectPlayer.IsPlaying
                .Where(isPlaying => !isPlaying)
                .Subscribe(_ =>
                {
                    _miniDisposable?.Dispose();
                    _miniDisposable = new CompositeDisposable();
                    ClearList();
                }).AddTo(_disposables);
        }
        
        private void TriggerSubFabulousEffect(Money money, Money averageMoney)
        {
            if (money > averageMoney)
            {
                _onSubFabulousEffectTriggered.OnNext(Unit.Default);
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
        
        private void ClearList()
        {
            _getMoneyAtOnce.Clear();
            _changeIndex = 0;
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
            _miniDisposable?.Dispose();
            _onSubFabulousEffectTriggered?.Dispose();
        }
    }
}