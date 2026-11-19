using DG.Tweening;
using RelatedMoney;
using TMPro;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// リザルト画面。GameEndManagerからShowResultが呼ばれると、以下の順にアニメーションする。
    ///   1. 背景パネルがポンと拡大しながらフェードイン
    ///   2. タイトルが上から落ちてくる
    ///   3. 各項目（最大所持金 / 一度に得た最大金額 / 生存時間）が左から順にスライドイン
    ///      → 値は0からカウントアップ
    ///   4. ボタンがポップして出てくる
    /// 初期状態（非表示・透明）はAwakeで作っておく。
    /// </summary>
    public class ResultPanel : MonoBehaviour
    {
        [SerializeField] private Canvas _resultCanvas;
        [SerializeField] private TextMeshProUGUI _maxMoneyText;
        [SerializeField] private TextMeshProUGUI _maxMoneyGottenAtOnceText;
        [SerializeField] private TextMeshProUGUI _survivalTimeText;

        [Header("アニメーション対象")]
        [SerializeField, Tooltip("背景パネル")] private RectTransform _panel;
        [SerializeField, Tooltip("「リザルト」などのタイトル")] private RectTransform _title;
        [SerializeField, Tooltip("上から順に表示する項目（最大所持金・一度に得た最大金額・生存時間）")]
        private RectTransform[] _rows;
        [SerializeField, Tooltip("リトライ・タイトルなどのボタン")] private RectTransform[] _buttons;

        [Header("アニメーション設定")]
        [SerializeField] private float _panelDuration = 0.45f;
        [SerializeField] private float _titleDuration = 0.4f;
        [SerializeField] private float _rowDuration = 0.35f;
        [SerializeField, Tooltip("項目どうしが動き出す間隔")] private float _rowInterval = 0.15f;
        [SerializeField, Tooltip("値のカウントアップにかける秒数")] private float _countUpDuration = 0.8f;
        [SerializeField] private float _buttonDuration = 0.3f;
        [SerializeField, Tooltip("項目が左からスライドしてくる距離")] private float _rowSlideDistance = 300f;
        [SerializeField, Tooltip("タイトルが上から落ちてくる距離")] private float _titleDropDistance = 150f;

        private CanvasGroup _panelGroup;
        private CanvasGroup _titleGroup;
        private CanvasGroup[] _rowGroups;
        private Vector2 _titleDefaultPosition;
        private Vector2[] _rowDefaultPositions;
        private Vector3 _panelDefaultScale;
        private Vector3[] _buttonDefaultScales;
        private Sequence _sequence;

        private void Awake()
        {
            _panelGroup = GetOrAddCanvasGroup(_panel);
            _titleGroup = GetOrAddCanvasGroup(_title);

            _rows ??= new RectTransform[0];
            _buttons ??= new RectTransform[0];

            _rowGroups = new CanvasGroup[_rows.Length];
            _rowDefaultPositions = new Vector2[_rows.Length];
            for (var i = 0; i < _rows.Length; i++)
            {
                _rowGroups[i] = GetOrAddCanvasGroup(_rows[i]);
                _rowDefaultPositions[i] = _rows[i] != null ? _rows[i].anchoredPosition : Vector2.zero;
            }

            _buttonDefaultScales = new Vector3[_buttons.Length];
            for (var i = 0; i < _buttons.Length; i++)
            {
                _buttonDefaultScales[i] = _buttons[i] != null ? _buttons[i].localScale : Vector3.one;
            }

            if (_panel != null) _panelDefaultScale = _panel.localScale;
            if (_title != null) _titleDefaultPosition = _title.anchoredPosition;

            SetHiddenState();
        }

        public void ShowResult(Money maxMoney, Money maxMoneyGottenAtOnce, float survivalTime)
        {
            _resultCanvas.enabled = true;
            ResultAnimation(maxMoney, maxMoneyGottenAtOnce, survivalTime);
        }

        private void ResultAnimation(Money maxMoney, Money maxMoneyGottenAtOnce, float survivalTime)
        {
            _sequence?.Kill();
            SetHiddenState();

            _sequence = DOTween.Sequence();

            // 1. 背景パネル
            if (_panel != null)
            {
                _sequence.Append(_panel.DOScale(_panelDefaultScale, _panelDuration).SetEase(Ease.OutBack));
                _sequence.Join(_panelGroup.DOFade(1f, _panelDuration * 0.6f));
            }

            // 2. タイトル（パネルが開ききる少し前から）
            if (_title != null)
            {
                var titleStart = Mathf.Max(0f, _sequence.Duration() - 0.1f);
                _sequence.Insert(titleStart, _title.DOAnchorPos(_titleDefaultPosition, _titleDuration).SetEase(Ease.OutBounce));
                _sequence.Insert(titleStart, _titleGroup.DOFade(1f, _titleDuration * 0.5f));
            }

            // 3. 各項目のスライドイン＋値のカウントアップ
            var rowStart = _sequence.Duration();
            for (var i = 0; i < _rows.Length; i++)
            {
                if (_rows[i] == null) continue;
                var start = rowStart + i * _rowInterval;
                _sequence.Insert(start, _rows[i].DOAnchorPos(_rowDefaultPositions[i], _rowDuration).SetEase(Ease.OutCubic));
                _sequence.Insert(start, _rowGroups[i].DOFade(1f, _rowDuration));
            }
            var countUpStart = rowStart + _rowDuration * 0.5f;
            _sequence.Insert(countUpStart, CountUp(_maxMoneyText, ToLong(maxMoney)));
            _sequence.Insert(countUpStart + _rowInterval, CountUp(_maxMoneyGottenAtOnceText, ToLong(maxMoneyGottenAtOnce)));
            _sequence.Insert(countUpStart + _rowInterval * 2f, CountUpTime(_survivalTimeText, survivalTime));

            // 4. ボタン
            var buttonStart = _sequence.Duration();
            for (var i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i] == null) continue;
                _sequence.Insert(buttonStart + i * 0.1f,
                    _buttons[i].DOScale(_buttonDefaultScales[i], _buttonDuration).SetEase(Ease.OutBack));
            }

            _sequence
                .OnComplete(() =>
                {
                    // 演出が終わるまではボタンを押せないようにしておく
                    if (_panelGroup != null)
                    {
                        _panelGroup.interactable = true;
                        _panelGroup.blocksRaycasts = true;
                    }
                })
                .SetUpdate(true)      // Time.timeScale = 0 でも再生する
                .SetLink(gameObject); // このGameObject破棄時にTweenも破棄
        }

        /// <summary>アニメーション開始前の状態（透明・縮小・ずらした位置）にする</summary>
        private void SetHiddenState()
        {
            if (_panel != null)
            {
                _panel.localScale = _panelDefaultScale * 0.6f;
                _panelGroup.alpha = 0f;
                _panelGroup.interactable = false;
                _panelGroup.blocksRaycasts = false;
            }

            if (_title != null)
            {
                _title.anchoredPosition = _titleDefaultPosition + Vector2.up * _titleDropDistance;
                _titleGroup.alpha = 0f;
            }

            for (var i = 0; i < _rows.Length; i++)
            {
                if (_rows[i] == null) continue;
                _rows[i].anchoredPosition = _rowDefaultPositions[i] + Vector2.left * _rowSlideDistance;
                _rowGroups[i].alpha = 0f;
            }

            for (var i = 0; i < _buttons.Length; i++)
            {
                if (_buttons[i] == null) continue;
                _buttons[i].localScale = Vector3.zero;
            }

            if (_maxMoneyText != null) _maxMoneyText.text = "0";
            if (_maxMoneyGottenAtOnceText != null) _maxMoneyGottenAtOnceText.text = "0";
            if (_survivalTimeText != null) _survivalTimeText.text = "0.00 秒";
        }

        private Tween CountUp(TextMeshProUGUI text, long target)
        {
            if (text == null) return DOTween.Sequence();
            var current = 0d;
            return DOTween.To(() => current, value =>
                {
                    current = value;
                    text.text = ((long)value).ToString();
                }, target, _countUpDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => text.text = target.ToString()); // 最後は正確な値にそろえる
        }

        private Tween CountUpTime(TextMeshProUGUI text, float target)
        {
            if (text == null) return DOTween.Sequence();
            var current = 0f;
            return DOTween.To(() => current, value =>
                {
                    current = value;
                    text.text = value.ToString("F2") + " 秒";
                }, target, _countUpDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() => text.text = target.ToString("F2") + " 秒");
        }

        private static CanvasGroup GetOrAddCanvasGroup(Component target)
        {
            if (target == null) return null;
            return target.TryGetComponent(out CanvasGroup group) ? group : target.gameObject.AddComponent<CanvasGroup>();
        }

        private static long ToLong(Money money) => long.TryParse(money.ToString(), out var value) ? value : 0;
    }
}
