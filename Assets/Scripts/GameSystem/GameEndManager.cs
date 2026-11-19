using System;
using Player;
using R3;
using RelatedMoney;
using UnityEngine;

namespace GameSystem
{
    public class GameEndManager : IDisposable
    {
        private readonly ResultMoneyData _resultMoneyData;
        private readonly HaveMoneyStatus _haveMoneyStatus;
        private readonly ResultPanel _resultPanel;
        private readonly GameTimer _gameTimer;
        private readonly PlayerController _playerController;
        private readonly TimeStopper _timeStopper;
        private readonly CompositeDisposable _disposables;
        private readonly Subject<Unit> _onGameEnd;
        public Observable<Unit> OnGameEnd => _onGameEnd;
        
        public GameEndManager(
            ResultMoneyData resultMoneyData,
            ResultPanel resultPanel,
            HaveMoneyStatus haveMoneyStatus,
            GameTimer gameTimer,
            PlayerController playerController,
            TimeStopper timeStopper)
        {
            _resultMoneyData = resultMoneyData;
            _resultPanel = resultPanel;
            _haveMoneyStatus = haveMoneyStatus;
            _gameTimer = gameTimer;
            _playerController = playerController;
            _timeStopper = timeStopper;
            _disposables = new CompositeDisposable();
            _onGameEnd = new Subject<Unit>();
        }
        
        public void Initialize()
        {
            Subscribe();
        }
        
        private void Subscribe()
        {
            _haveMoneyStatus.CurrentMoney
                .Where(money => money <= new Money(0))
                .Subscribe(_ =>
                {
                    _resultPanel.ShowResult(
                        _resultMoneyData.MaxMoney,
                        _resultMoneyData.MaxMoneyGottenAtOnce,
                        _gameTimer.Timer.CurrentValue);
                    _onGameEnd.OnNext(Unit.Default);
                    _gameTimer.Stop();
                    _playerController.enabled = false;
                    _playerController.TryGetComponent(out Collider2D playerCollider);
                    playerCollider.enabled = false;
                    _timeStopper.StopTimeFromSkillSelection();
                }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}