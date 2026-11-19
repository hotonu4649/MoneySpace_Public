using System.Collections.Generic;
using R3;
using UnityEngine;
using VContainer;

namespace DynamicItems.PlayerWeapon
{
    public class PlayerWeaponPresenter : MonoBehaviour
    {
        [SerializeField] private List<PlayerWeaponView> _playerWeaponViewList;
        [SerializeField] private PlayerWeaponData _initialWeaponData;
        private PlayerWeaponList _weaponList;
        private readonly List<Observable<int>> _weaponClickObservables = new List<Observable<int>>();
        
        [Inject]
        public void Construct(PlayerWeaponList weaponList)
        {
            _weaponList = weaponList;
        }
        
        private void Start()
        {
            Initialize();
        }
        
        private void Initialize()
        {
            foreach (var t in _playerWeaponViewList)
            {
                _weaponClickObservables.Add(t.OnWeaponClick);
            }
            
            Observable.Merge(_weaponClickObservables)
                .Subscribe(index =>
                {
                    _weaponList.ChangeCurrentWeapon(index);
                }).AddTo(this);
                
            _weaponList.OnWeaponChange
                .Subscribe(index =>
                {
                    var currentWeaponData = _weaponList.GetIndexWeapon(0);
                    var changedWeaponData = _weaponList.GetIndexWeapon(index);
                    _playerWeaponViewList[0].UpdateSprite(currentWeaponData.WeaponIcon);
                    _playerWeaponViewList[index].UpdateSprite(changedWeaponData.WeaponIcon);
                }).AddTo(this);
            
            _weaponList.OnWeaponAdd
                .Subscribe(index =>
                {
                    var newWeaponData = _weaponList.GetIndexWeapon(index);
                    _playerWeaponViewList[index].UpdateSprite(newWeaponData.WeaponIcon);
                }).AddTo(this);
            
            _weaponList.InitializeWeaponList(_initialWeaponData);
        }
    }
}