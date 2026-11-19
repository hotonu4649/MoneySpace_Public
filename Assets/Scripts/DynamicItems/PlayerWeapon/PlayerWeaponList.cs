using R3;

namespace DynamicItems.PlayerWeapon
{
    public class PlayerWeaponList
    {
        private readonly PlayerWeaponData[] _weaponList = new PlayerWeaponData[4];
        private readonly Subject<int> _onWeaponChange = new ();
        private readonly Subject<int> _onWeaponAdd = new ();
        public Observable<int> OnWeaponChange => _onWeaponChange;
        public Observable<int> OnWeaponAdd => _onWeaponAdd;
        
        public void InitializeWeaponList(PlayerWeaponData weaponData)
        {
            AddWeapon(weaponData);
        }
        
        public void AddWeapon(PlayerWeaponData weaponData)
        {
            for (var i = 0; i < _weaponList.Length; i++)
            {
                if (_weaponList[i] == null)
                {
                    _weaponList[i] = weaponData;
                    _onWeaponAdd.OnNext(i);
                    break;
                }
            }
        }
        
        public void ChangeCurrentWeapon(int index)
        {
            if (index < 1 || index >= _weaponList.Length || _weaponList[index] == null)
            {
                return;
            }
            _onWeaponChange.OnNext(index);
            (_weaponList[0], _weaponList[index]) = (_weaponList[index], _weaponList[0]);
        }
        
        public PlayerWeaponData GetIndexWeapon(int index)
        {
            if (index < 0 || index >= _weaponList.Length || _weaponList[index] == null)
            {
                return null;
            }
            return _weaponList[index];
        }
    }
}