using System.Collections.Generic;
using GameSystem;
using System.Linq;

namespace Skill
{
    public class SkillLottery
    {
        private List<SkillData> _usedSkillList = new ();
        private readonly SkillDatabase _skillDatabase;
        private readonly GameTimer _gameTimer;
        //memo
        //skillDatabaseにスキルの強さのレベルとかを指定するリストとかを作る
        //それを基にランダムに抽選をする
        
        public SkillLottery(SkillDatabase skillDatabase, GameTimer gameTimer)
        {
            _skillDatabase = skillDatabase;
            _gameTimer = gameTimer;
        }
        
        public List<SkillData> GetRandomSkillData()
        {
            var (currentSkillDataList, currentSkillProbabilities) = GetCurrentSkillData();
            var randomSkillData = ReturnRandomSkillData(currentSkillDataList, currentSkillProbabilities);
            return randomSkillData;
        }
        
        public void AddUsedSkill(SkillData skillData)
        {
            _usedSkillList.Add(skillData);
        }
        
        /// <summary>
        /// 現在のスキルに関するデータを取得する
        /// </summary>
        /// <returns></returns>
        private (List<SkillData>, List<float>) GetCurrentSkillData()
        {
            var skillDataList = _skillDatabase.SkillList;
            var skillProbabilities = _skillDatabase.SkillProbabilities;
            var skillLevels = _skillDatabase.SkillLevels;
            var skillLevelsTime = _skillDatabase.SkillLevelsTime;
            var currentTime = _gameTimer.Timer.CurrentValue;
            var currentSkillLevel = 0;
            
            for (var i = 0; i < skillLevels.Count; i++)
            {
                if (i >= skillLevelsTime.Count)
                {
                    currentSkillLevel = i;
                    break;
                }

                if (currentTime >= skillLevelsTime[i] &&
                    (i == skillLevelsTime.Count - 1 ||
                     currentTime < skillLevelsTime[i + 1]))
                {
                    currentSkillLevel = i;
                    break;
                }
            }
            
            var currentSkillDataList = skillDataList.GetRange(0, skillLevels[currentSkillLevel]);
            var currentSkillProbabilities = skillProbabilities.GetRange(0, skillLevels[currentSkillLevel]);
            
            return (currentSkillDataList, currentSkillProbabilities);
        }
        
        /// <summary>
        /// 指定されたスキルデータリストと確率リストからランダムにスキルデータを選択する
        /// </summary>
        /// <param name="currentSkillDataList"></param>
        /// <param name="currentSkillProbabilities"></param>
        /// <returns></returns>
        private List<SkillData> ReturnRandomSkillData(List<SkillData> currentSkillDataList, List<float> currentSkillProbabilities)
        {
            var currentSkillList = currentSkillDataList;
            var currentSkillProbabilitiesList = currentSkillProbabilities;
            var skillList = new List<SkillData>();
            while (skillList.Count < 3)
            {
                // 確率を正規化
                var sumProbabilities = currentSkillProbabilitiesList.Sum();
                for (var i = 0; i < currentSkillProbabilitiesList.Count; i++)
                {
                    currentSkillProbabilitiesList[i] = (currentSkillProbabilitiesList[i] / sumProbabilities) * 100f;
                }
                
                // ランダム値を生成
                var randomValue = UnityEngine.Random.Range(0f, 100f);
                
                // 累積確率を計算してスキルを選択
                var cumulativeProbability = 0f;
                
                for (var i = 0; i < currentSkillList.Count; i++)
                {
                    cumulativeProbability += currentSkillProbabilitiesList[i];
                    if (randomValue <= cumulativeProbability)
                    {
                        if (_usedSkillList.Contains(currentSkillList[i]))
                        {
                            break;
                        }
                        skillList.Add(currentSkillList[i]);
                        currentSkillList.Remove(currentSkillList[i]);
                        currentSkillProbabilitiesList.RemoveAt(i);
                    }
                }
            }
            return skillList;
        }
    }
}