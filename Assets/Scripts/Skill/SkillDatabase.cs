using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    [CreateAssetMenu]
    public class SkillDatabase : ScriptableObject
    {
        [Header("Skill Data")]
        [SerializeField] private List<SkillData> _skillList = new ();
        [Header("Skill Probabilities")]
        [SerializeField] private List<float> _skillProbabilities = new ();
        [Header("スキルのレア度の段階がどこからか")]
        [SerializeField] private List<int> _skillLevels = new ();
        [Header("レアなスキルが抽選に含まれるまでの時間")]
        [SerializeField] private List<int> _skillLevelsTime = new ();
        
        public List<SkillData> SkillList => _skillList;
        public List<float> SkillProbabilities => _skillProbabilities;
        public List<int> SkillLevels => _skillLevels;
        public List<int> SkillLevelsTime => _skillLevelsTime;
    }
}