using System;
using System.Collections.Generic;
using DynamicItems.Enemy;
using DynamicItems.PlayerWeapon;
using R3;
using RelatedMoney;
using Skill.ConcreteSkill.MoneyFromEnemySkill;
using Skill.ConcreteSkill.MoneySkill;
using Skill.ConcreteSkill.SkillInterface;

namespace Skill
{
    /// <summary>
    /// 現在のスキルデータを管理するクラス
    /// スキルを選ぶときにmodelとして使用されている
    /// 選ばれたスキルは、各管理下へと渡される
    /// 例：お金スキルはMoneyCalculatorへ
    /// </summary>
    public class CurrentSkillData : IDisposable
    {
        private readonly SkillLottery _skillLottery;
        private readonly SkillManager _skillManager;
        private readonly MoneyCalculator _moneyCalculator;
        private readonly MoneyFromEnemyCalculator _moneyFromEnemyCalculator;
        private readonly WeaponAdder _weaponAdder;
        private readonly SkillIdToClass _skillIdToClass;
        private readonly EnemyAttackCalculator _enemyAttackCalculator;
        private List<SkillData> _currentSkillDataList;
        private readonly Subject<List<SkillData>> _onSkillDataChanged = new ();
        private IDisposable _skillManagerSubscription;
        public Observable<List<SkillData>> OnSkillDataChanged => _onSkillDataChanged;
        
        public CurrentSkillData(
            SkillLottery skillLottery,
            MoneyCalculator moneyCalculator,
            SkillManager skillManager,
            WeaponAdder weaponAdder,
            SkillIdToClass skillIdToClass,
            EnemyAttackCalculator enemyAttackCalculator,
            MoneyFromEnemyCalculator moneyFromEnemyCalculator)
        {
            _skillLottery = skillLottery;
            _moneyCalculator = moneyCalculator;
            _skillManager = skillManager;
            _weaponAdder = weaponAdder;
            _skillIdToClass = skillIdToClass;
            _moneyFromEnemyCalculator = moneyFromEnemyCalculator;
            _enemyAttackCalculator = enemyAttackCalculator;
            Subscribe();
        }

        private void Subscribe()
        {
            _skillManagerSubscription = _skillManager.OnSkillAvailable
                .Subscribe(_ =>
                {
                    ChangeCurrentSkillData();
                });
        }
        
        /// <summary>
        /// スキルデータを更新する
        /// </summary>
        private void ChangeCurrentSkillData()
        {
            _currentSkillDataList = _skillLottery.GetRandomSkillData();
            _onSkillDataChanged.OnNext(_currentSkillDataList);
        }
        
        /// <summary>
        /// 指定されたインデックスの現在スキル選択としてのスキルデータを取得する
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public SkillData GetCurrentSkillData(int index)
        {
            if (_currentSkillDataList == null || index < 0 || index >= _currentSkillDataList.Count)
            {
                return null;
            }
            return _currentSkillDataList[index];
        }
        
        /// <summary>
        /// スキルを各管理下へと渡す
        /// </summary>
        public void AddSkillToList(int index)
        {
            var skillData = GetCurrentSkillData(index);
            if (skillData == null) return;
            var skill = _skillIdToClass.GetSkillById(skillData.SkillID);
            if (skill is IMoney moneySkill)
            {
                if (skill is IAddFromMoney addFromMoneySkill)
                {
                    _moneyCalculator.AddMAddMoneySkillList(addFromMoneySkill);
                }
                if (skill is IMultiFromMoney multiFromMoneySkill)
                {
                    _moneyCalculator.AddMultiMoneySkillList(multiFromMoneySkill);
                }
                // MoneyCalculatorの管理下へ渡す
            }
            if (skill is IGetPlayerWeapon playerWeaponSkill)
            {
                _weaponAdder.AddList(playerWeaponSkill);
                _skillLottery.AddUsedSkill(skillData);
                // PlayerWeaponの管理下へ渡す処理をここに追加する
            }
            if (skill is IMoneyFromEnemy moneyFromEnemySkill)
            {
                if (skill is IAddMoneyFromEnemy addMoneyFromEnemySkill)
                {
                    _moneyFromEnemyCalculator.AddMoneyFromEnemySkill(addMoneyFromEnemySkill);
                }
                if (skill is IMultiFromEnemy multiFromEnemySkill)
                {
                    _moneyFromEnemyCalculator.AddMultiFromEnemySkill(multiFromEnemySkill);
                }
            }
            if (skill is IReduceEnemyAttack reduceEnemyAttackSkill)
            {
                _enemyAttackCalculator.AddReduceEnemyAttackSkill(reduceEnemyAttackSkill);
            }
        }
        
        public void Dispose()
        {
            _onSkillDataChanged?.Dispose();
            _skillManagerSubscription?.Dispose();
        }
    }
}