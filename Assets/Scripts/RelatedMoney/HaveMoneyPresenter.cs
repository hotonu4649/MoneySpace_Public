using System;
using R3;
using VContainer.Unity;

namespace RelatedMoney
{
    /// <summary>
    /// HaveMoneyPresenterクラスは、所持金の表示を担当するプレゼンターです。
    /// </summary>
    public class HaveMoneyPresenter : IStartable, IDisposable
    {
        private readonly HaveMoneyStatus _haveMoneyStatus;
        private readonly HaveMoneyView _haveMoneyView;
        private readonly CompositeDisposable _disposables = new ();
        
        public HaveMoneyPresenter(HaveMoneyStatus haveMoneyStatus, HaveMoneyView haveMoneyView)
        {
            _haveMoneyStatus = haveMoneyStatus;
            _haveMoneyView = haveMoneyView;
        }

        public void Start()
        {
            _haveMoneyStatus.CurrentMoney.Subscribe(money =>
            {
                _haveMoneyView.UpdateMoneyDisplay(money);
            }).AddTo(_disposables);
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}