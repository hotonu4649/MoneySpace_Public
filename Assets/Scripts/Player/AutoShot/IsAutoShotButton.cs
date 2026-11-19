using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Player.AutoShot
{
    public class IsAutoShotButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] Sprite _autoShotOnSprite;
        [SerializeField] Sprite _autoShotOffSprite;
        private Image _buttonImage;
        private ReactiveProperty<bool> _isAutoShot = new ReactiveProperty<bool>(false);
        public ReadOnlyReactiveProperty<bool> IsAutoShot => _isAutoShot;
        
        private void Awake()
        {
            TryGetComponent(out _buttonImage);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _isAutoShot.Value = !_isAutoShot.Value;
            ImageChange();
        }
        
        private void ImageChange()
        {
            if (_isAutoShot.Value)
            {
                _buttonImage.sprite = _autoShotOnSprite;
            }
            else
            {
                _buttonImage.sprite = _autoShotOffSprite;
            }
        }
    }
}