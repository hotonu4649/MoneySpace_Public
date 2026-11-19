using System;
using System.Collections.Generic;
using GameSystem;
using R3;

namespace Skill
{
    /// <summary>
    /// スキルの発生タイミングを管理するクラス
    /// </summary>
    public class SkillManager : IDisposable
    {
        private readonly GameTimer _gameTimer;
        private readonly SkillTimeDatabase _skillTimeDatabase;
        private readonly TimeStopper _timeStopper;
        private readonly List<float> _skillTimeList;
        private float _stopTime;
        private readonly CompositeDisposable _disposables;
        private readonly Subject<Unit> _onSkillAvailable = new ();
        public Observable<Unit> OnSkillAvailable => _onSkillAvailable;

        public SkillManager(GameTimer gameTimer, SkillTimeDatabase skillTimeDatabase, TimeStopper timeStopper)
        {
            _gameTimer = gameTimer;
            _skillTimeDatabase = skillTimeDatabase;
            _timeStopper = timeStopper;
            _skillTimeList = _skillTimeDatabase.SkillTimeList;
            _stopTime = _skillTimeList[0];
            _disposables = new CompositeDisposable();
        }

        public void Initialize()
        {
            var numSkills = -1;
            _gameTimer.Timer
                .Where(x => x >= _stopTime)
                .Do(_ =>
                {
                    if (numSkills < _skillTimeList.Count - 1)
                    {
                        numSkills++;
                    }
                    _stopTime += _skillTimeList[numSkills];
                })
                .Subscribe(_ =>
                {
                    _timeStopper.StopTimeFromSkillSelection();
                    _onSkillAvailable.OnNext(Unit.Default);
                }).AddTo(_disposables);
        }

        public void Dispose()
        {
            _disposables?.Dispose();
        }
    }
}