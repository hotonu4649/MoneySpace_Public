using System.Collections.Generic;
using UnityEngine;

namespace Skill
{
    /// <summary>
    /// スキルの発生タイミングをインスペクターから指定するクラス
    /// </summary>
    [CreateAssetMenu]
    public class SkillTimeDatabase : ScriptableObject
    {
        [SerializeField] List<float> _skillTimeList = new ();
        public List<float> SkillTimeList => _skillTimeList;
    }
}