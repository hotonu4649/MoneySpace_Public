using System.Collections.Generic;
using DG.Tweening;
using GameSystem;
using R3;
using UnityEngine;
using VContainer;

namespace Skill
{
    public class SkillSelectPresenter : MonoBehaviour
    {
        [SerializeField] private SkillSelectView[] _skillSelectViews;
        [SerializeField] private Canvas _skillSelectCanvas;
        [SerializeField] private float _enterInterval = 0.3f; // 次のパネルが動き出すまでの間隔
        private CurrentSkillData _currentSkillData;
        private TimeStopper _timeStopper;
        private bool _canSelect;
        private Sequence _enterSequence;
        private readonly List<Observable<int>> _skillClickObservables = new ();
        private readonly Subject<Unit> _onSkillSelected = new ();
        public Observable<Unit> OnSkillSelected => _onSkillSelected;

        [Inject]
        public void Construct(CurrentSkillData currentSkillData, TimeStopper timeStopper)
        {
            _currentSkillData = currentSkillData;
            _timeStopper = timeStopper;
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            foreach (var variable in _skillSelectViews)
            {
                _skillClickObservables.Add(variable.OnSkillClick);
            }

            Observable.Merge(_skillClickObservables)
                .Where(_ => _canSelect) // アニメーション完了までクリックを無視
                .Subscribe(index =>
                {
                    _canSelect = false; // 二重選択の防止
                    _onSkillSelected.OnNext(Unit.Default);
                    _currentSkillData.AddSkillToList(index);
                    _skillSelectCanvas.enabled = false;
                    _timeStopper.ResumeTimeFromSkillSelection();
                }).AddTo(this);

            _currentSkillData.OnSkillDataChanged
                .Subscribe(skillDataList =>
                {
                    _skillSelectCanvas.enabled = true;
                    for (var i = 0; i < _skillSelectViews.Length; i++)
                    {
                        _skillSelectViews[i].UpdateSkillContent(skillDataList[i]);
                    }
                    PlayEnterAnimation();
                }).AddTo(this);
        }

        private void PlayEnterAnimation()
        {
            _canSelect = false;
            _enterSequence?.Kill();
            _enterSequence = DOTween.Sequence();

            for (var i = 0; i < _skillSelectViews.Length; i++)
            {
                _skillSelectViews[i].MoveToEnterStartPosition();
                // i * _enterInterval 秒の時点から開始。interval < duration なので前のパネルの減速中に次が動き出す
                _enterSequence.Insert(i * _enterInterval, _skillSelectViews[i].CreateEnterTween());
            }

            _enterSequence
                .SetUpdate(true)     // Time.timeScale = 0 でも再生する
                .SetLink(gameObject) // このGameObject破棄時にTweenも破棄
                .OnComplete(() => _canSelect = true);
        }
    }
}