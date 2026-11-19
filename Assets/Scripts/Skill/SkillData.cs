using UnityEngine;

namespace Skill
{
    [CreateAssetMenu]
    public class SkillData : ScriptableObject
    {
        [SerializeField] private string _skillName;
        [SerializeField] private Sprite _skillIcon;
        [SerializeField] private string _skillDescription;
        [SerializeField] private int _skillID;
        
        public string SkillName => _skillName;
        public Sprite SkillIcon => _skillIcon;
        public string SkillDescription => _skillDescription;
        public int SkillID => _skillID;
    }
}