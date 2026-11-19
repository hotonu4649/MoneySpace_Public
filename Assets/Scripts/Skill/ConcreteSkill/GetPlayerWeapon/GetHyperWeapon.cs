using DynamicItems.PlayerWeapon;
using RelatedMoney;
using Skill.ConcreteSkill.SkillInterface;

namespace Skill.ConcreteSkill.GetPlayerWeapon
{
    public class GetHyperWeapon : IGetPlayerWeapon
    {
        private PlayerWeaponList _weaponList;
        private PlayerWeaponData _hyperWeaponData;
        
        public GetHyperWeapon(PlayerWeaponList weaponList, PlayerWeaponData hyperWeaponData)
        {
            _weaponList = weaponList;
            _hyperWeaponData = hyperWeaponData;
        }
        
        public Money Execute(Money value)
        {
            _weaponList.AddWeapon(_hyperWeaponData);
            return new Money(0);
        }
    }
}