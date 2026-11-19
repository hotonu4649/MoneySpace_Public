using DG.Tweening;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Skill
{
    public class SkillSelectView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private int _skillIndex;
        [SerializeField] private TMPro.TextMeshProUGUI _skillDescriptionText;
        [SerializeField] private Image _skillIconImage;
        [SerializeField] private float _enterOffsetX = 1500f; // 所定位置から右にどれだけ離れた位置から始めるか
        [SerializeField] private float _enterDuration = 0.5f;
        private RectTransform _rectTransform;
        private float _restX; // 所定位置のX
        private readonly Subject<int> _onSkillClick = new Subject<int>();
        public Observable<int> OnSkillClick => _onSkillClick;

        private void Awake()
        {
            _rectTransform = (RectTransform)transform;
            _restX = _rectTransform.anchoredPosition.x;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onSkillClick.OnNext(_skillIndex);
        }

        public void UpdateSkillContent(SkillData skillData)
        {
            _skillIconImage.sprite = skillData.SkillIcon;
            _skillDescriptionText.text = skillData.SkillDescription;
        }

        // 画面外の開始位置に即座に移動させる
        public void MoveToEnterStartPosition()
        {
            var pos = _rectTransform.anchoredPosition;
            pos.x = _restX + _enterOffsetX;
            _rectTransform.anchoredPosition = pos;
        }

        // 所定位置まで減速しながら移動するTweenを返す(再生タイミングはPresenterが決める)
        public Tween CreateEnterTween()
        {
            return _rectTransform.DOAnchorPosX(_restX, _enterDuration).SetEase(Ease.OutCubic);
        }
    }
}