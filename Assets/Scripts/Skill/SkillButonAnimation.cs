using DG.Tweening;
using GameSystem;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Skill
{
    [DefaultExecutionOrder(-100)]
    public class SkillButonAnimation : MonoBehaviour, IBaseButton
    {
        [SerializeField] private Transform _target;              // 拡縮させる対象。未設定ならこのオブジェクト自身
        [SerializeField] private float _hoverScale = 0.9f;       // カーソルが入っている間の倍率(元のサイズ比)
        [SerializeField] private float _duration = 0.15f;        // 拡縮にかける秒数
        [SerializeField] private Ease _ease = Ease.OutQuad;
        private Vector3 _defaultScale;
        private Tween _scaleTween;

        private void Awake()
        {
            if (_target == null) _target = transform;
            _defaultScale = _target.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            PlayScale(_defaultScale * _hoverScale);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            PlayScale(_defaultScale);
        }

        private void PlayScale(Vector3 endScale)
        {
            _scaleTween?.Kill(); // 再生中の拡縮を止めてから、現在のスケールを起点に新しく再生する
            _scaleTween = _target.DOScale(endScale, _duration)
                .SetEase(_ease)
                .SetUpdate(true)      // Time.timeScale = 0 でも再生する
                .SetLink(gameObject); // このGameObject破棄時にTweenも破棄
        }
    }
}