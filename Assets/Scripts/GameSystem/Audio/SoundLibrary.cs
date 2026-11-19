using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem.Audio
{
    /// <summary>
    /// 音の名前とAudioClipの対応を保持するScriptableObject。
    /// Projectウィンドウで右クリック → Create → Audio → Sound Library で作成する。
    /// </summary>
    [CreateAssetMenu]
    public class SoundLibrary : ScriptableObject
    {
        [Serializable]
        public class Entry
        {
            [SerializeField] private string _name;
            [SerializeField] private AudioClip _clip;

            public string Name => _name;
            public AudioClip Clip => _clip;
        }

        [SerializeField] private List<Entry> _entries = new List<Entry>();

        public IReadOnlyList<Entry> Entries => _entries;
    }
}