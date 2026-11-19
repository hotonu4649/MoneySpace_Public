using System.Collections.Generic;
using DynamicItems.PlayerWeapon;
using Skill.ConcreteSkill.GetPlayerWeapon;
using Skill.ConcreteSkill.MoneyFromEnemySkill.Add;
using Skill.ConcreteSkill.MoneyFromEnemySkill.Multiple;
using Skill.ConcreteSkill.MoneySkill.Add;
using Skill.ConcreteSkill.MoneySkill.Multiple;
using Skill.ConcreteSkill.ReduceEnemyAttack;
using Skill.ConcreteSkill.SkillInterface;

namespace Skill
{
    public class SkillIdToClass
    {
        private readonly PlayerWeaponList _playerWeaponList;
        private readonly PlayerWeaponData _superWeaponData;
        private readonly PlayerWeaponData _hyperWeaponData;
        private readonly Dictionary<int, ISkill> SkillIdToClassMap;
        
        public SkillIdToClass(PlayerWeaponList playerWeaponList, PlayerWeaponData superWeaponData, PlayerWeaponData hyperWeaponData)
        {
            // コンストラクタで必要な依存関係を注入する場合はここに追加
            _playerWeaponList = playerWeaponList;
            _superWeaponData = superWeaponData;
            _hyperWeaponData = hyperWeaponData;
            SkillIdToClassMap = new Dictionary<int, ISkill>
            {
                { 1, new AddTenMoney() },
                { 2, new MultipleFromMinusOneToSix()},
                { 3, new TwiceMoney()},
                { 4, new GetSuperWeapon(_playerWeaponList, _superWeaponData) },
                { 5, new GetHyperWeapon(_playerWeaponList, _hyperWeaponData) },
                { 6, new TwiceFromEnemy() },
                { 7, new ReduceTenPercent() },
                { 8, new AddEightyAtTwentyPercentFromEnemy() },
                { 9, new AddFiveFromEnemy() },
                { 10, new AddFortyAtFortyPercentFromEnemy() },
                { 11, new AddTenAtEightyPercentFromEnemy() },
                { 12, new AddTwentyAtSixtyPercentFromEnemy() },
                { 13, new FiveTimesAtFortyPercentFromEnemy() },
                { 14, new FourTimesAtSixtyPercentFromEnemy() },
                { 15, new MultipleFromOneToThreeFromEnemy() },
                { 16, new SixTimesAtTwentyPercentFromEnemy() },
                { 17, new TenTimesAtFivePercentFromEnemy() },
                { 18, new ThreeTimesAtEightyPercentFromEnemy() },
                { 19, new AddEightyAtFortyPercent() },
                { 20, new AddFortyAtSixtyPercent() },
                { 21, new AddOneHundredFiftyAtTwentyPercent() },
                { 22, new AddThreeHundredAtFivePercent() },
                { 23, new AddTwentyAtEightyPercent() },
                { 24, new FiveTimesAtFortyPercent() },
                { 25, new FourTimesAtSixtyPercent() },
                { 26, new MultipleFromZeroToThree() },
                { 27, new SixTimesAtTwentyPercent() },
                { 28, new ThreeTimesAtEightyPercent() },
                { 29, new TwentyTimesAtFivePercent()} 
            };
        }
        public ISkill GetSkillById(int skillId)
        {
            return SkillIdToClassMap.GetValueOrDefault(skillId);
        }
    }
}