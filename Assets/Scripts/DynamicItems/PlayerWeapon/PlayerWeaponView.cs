using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DynamicItems.PlayerWeapon
{
    public class PlayerWeaponView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private int _weaponIndex;
        private Image _weaponImage;
        private readonly Subject<int> _onWeaponClick = new Subject<int>();
        public Observable<int> OnWeaponClick => _onWeaponClick;
        
        private void Awake()
        {
            TryGetComponent(out _weaponImage);
        }
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _onWeaponClick.OnNext(_weaponIndex);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            
        }
        
        public void UpdateSprite(Sprite newSprite)
        {
            _weaponImage.sprite = newSprite;
            var color = _weaponImage.color;
            color.a = 1f;
            _weaponImage.color = color;
        }
    }
}