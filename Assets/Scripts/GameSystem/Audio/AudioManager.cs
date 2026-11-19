using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace GameSystem.Audio
{
    /// <summary>
    /// SE再生を担当するクラス。
    /// 自身のGameObjectにAudioSourceを複数付け、再生中でないものを探して使う。
    /// どのタイミングでどの音を鳴らすかは知らない(呼び出し側が決める)。
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// AudioSourceと「何回目の再生か」を表す番号の組。
        /// AudioSourceを使いまわすため、待機中の処理が
        /// 「自分が鳴らした音がまだこのAudioSourceで鳴っているか」を番号で判定する。
        /// </summary>
        private sealed class Slot
        {
            public AudioSource source;
            public int playId;
        }

        [SerializeField] private SoundLibrary _soundLibrary;
        [SerializeField, Min(1)] private int _initialSourceCount = 8;
        [SerializeField, Min(1)] private int _maxSourceCount = 24;

        private readonly List<Slot> _slots = new List<Slot>();
        private readonly Dictionary<string, AudioClip> _clipsByName = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            BuildClipTable();

            int count = Mathf.Min(_initialSourceCount, _maxSourceCount);
            for (int i = 0; i < count; i++)
            {
                AddSlot();
            }
        }

        /// <summary>
        /// SoundLibraryに登録した名前のSEを1回再生し、再生が終わるまで待機する。
        /// cancellationTokenがキャンセルされると、その音を止めてOperationCanceledExceptionを投げる。
        /// StopAllで止められた場合は、例外なしで待機が完了する。
        /// 名前が未登録の場合、または空きがなく上限にも達している場合は、再生せずにすぐ完了する。
        /// </summary>
        public async UniTask PlaySeAsync(
            string soundName,
            float volume = 1f,
            float pitch = 1f,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (string.IsNullOrEmpty(soundName) || !_clipsByName.TryGetValue(soundName, out AudioClip clip))
            {
                Debug.LogWarning($"AudioManager: \"{soundName}\" はSoundLibraryに登録されていません。", this);
                return;
            }

            Slot slot = GetAvailableSlot();
            if (slot == null)
            {
                Debug.LogWarning($"AudioManager: AudioSourceが上限({_maxSourceCount})に達しているため \"{soundName}\" を再生できません。", this);
                return;
            }

            int playId = ++slot.playId;

            // 使いまわすので、前回の設定が残らないよう毎回すべて設定する
            AudioSource source = slot.source;
            source.clip = clip;
            source.volume = volume;
            source.pitch = pitch;
            source.Play();

            try
            {
                await UniTask.WaitWhile(
                    () => IsStillPlaying(slot, playId),
                    cancellationToken: cancellationToken);
            }
            finally
            {
                // キャンセル時に音を止める。
                // 番号が変わっていたら、このAudioSourceは既に別の音に使われているので触らない。
                if (slot.source != null && slot.playId == playId)
                {
                    slot.source.Stop();
                }
            }
        }

        /// <summary>
        /// 再生中のSEをすべて止める。PlaySEAsyncで待機中の処理は、例外なしで完了する。
        /// </summary>
        public void StopAll()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                // 番号を進めて、待機中の処理に「自分の音は終わった」と判定させる
                _slots[i].playId++;
                _slots[i].source.Stop();
            }
        }

        /// <summary>
        /// SoundLibraryの内容から、名前 → AudioClip の辞書を作る。
        /// </summary>
        private void BuildClipTable()
        {
            if (_soundLibrary == null)
            {
                Debug.LogError("AudioManager: SoundLibraryが設定されていません。", this);
                return;
            }

            IReadOnlyList<SoundLibrary.Entry> entries = _soundLibrary.Entries;
            for (int i = 0; i < entries.Count; i++)
            {
                SoundLibrary.Entry entry = entries[i];

                if (string.IsNullOrEmpty(entry.Name) || entry.Clip == null)
                {
                    Debug.LogWarning($"AudioManager: SoundLibraryの{i}番目は名前かAudioClipが空のため無視します。", _soundLibrary);
                    continue;
                }

                if (_clipsByName.ContainsKey(entry.Name))
                {
                    Debug.LogWarning($"AudioManager: \"{entry.Name}\" が重複しています。{i}番目は無視します。", _soundLibrary);
                    continue;
                }

                _clipsByName.Add(entry.Name, entry.Clip);
            }
        }

        private static bool IsStillPlaying(Slot slot, int playId)
        {
            // Source != null は、AudioManagerが破棄された後に待機が残っていた場合の対策
            return slot.source != null
                   && slot.playId == playId
                   && slot.source.isPlaying;
        }

        /// <summary>
        /// 再生中でないAudioSourceを持つSlotを返す。なければ上限まで追加する。上限に達していればnull。
        /// </summary>
        private Slot GetAvailableSlot()
        {
            for (int i = 0; i < _slots.Count; i++)
            {
                if (!_slots[i].source.isPlaying)
                {
                    return _slots[i];
                }
            }

            if (_slots.Count < _maxSourceCount)
            {
                return AddSlot();
            }

            return null;
        }

        private Slot AddSlot()
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            source.playOnAwake = false;
            source.loop = false;
            source.spatialBlend = 0f; // 位置に依存しない2D音

            Slot slot = new Slot { source = source };
            _slots.Add(slot);
            return slot;
        }
    }
}