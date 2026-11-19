using System.Collections.Generic;
using Skill.ConcreteSkill;
using Skill.ConcreteSkill.SkillInterface;

namespace DynamicItems.PlayerWeapon
{
    public class WeaponAdder
    {
        private readonly List<IGetPlayerWeapon> _playerWeaponSkillList = new List<IGetPlayerWeapon>();
        
        public void AddList(IGetPlayerWeapon playerWeaponSkill)
        {
            _playerWeaponSkillList.Add(playerWeaponSkill);
            playerWeaponSkill.Execute(new RelatedMoney.Money(0));
        }
    }
}