using System;
using RelatedMoney;
using R3;

namespace GameSystem
{
    public class ResultMoneyData : IDisposable
    {
        private readonly HaveMoneyStatus _haveMoneyStatus;
        private readonly CompositeDisposable _disposables;
        public Money MaxMoney { get; private set; }
        public Money MaxMoneyGottenAtOnce { get; private set; }
        
        public ResultMoneyData(HaveMoneyStatus haveMoneyStatus)
        {
            _haveMoneyStatus = haveMoneyStatus;
            _disposables = new CompositeDisposable();
            MaxMoney = new Money(0);
            MaxMoneyGottenAtOnce = new Money(0);
            Subscribe();
        }
        
        private void Subscribe()
        {
            _haveMoneyStatus.OnMoneyChanged
                .Subscribe(gottenMoney =>
                {
                    if (gottenMoney > MaxMoneyGottenAtOnce)
                    {
                        MaxMoneyGottenAtOnce = gottenMoney;
                    }
                }).AddTo(_disposables);
            
            _haveMoneyStatus.CurrentMoney
                .Subscribe(currentMoney =>
                {
                    if (currentMoney > MaxMoney)
                    {
                        MaxMoney = currentMoney;
                    }
                }).AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}