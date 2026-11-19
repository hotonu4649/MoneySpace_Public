using DynamicItems.PlayerWeapon;
using RelatedMoney;
using Skill.ConcreteSkill.SkillInterface;

namespace Skill.ConcreteSkill.GetPlayerWeapon
{
    public class GetSuperWeapon : IGetPlayerWeapon
    {
        private PlayerWeaponList _weaponList;
        private PlayerWeaponData _superWeaponData;
        
        public GetSuperWeapon(PlayerWeaponList weaponList, PlayerWeaponData superWeaponData)
        {
            _weaponList = weaponList;
            _superWeaponData = superWeaponData;
        }
        
        public Money Execute(Money value)
        {
            //　アニメーション付けたい
            _weaponList.AddWeapon(_superWeaponData);
            return new Money(0);
        }
    }
}