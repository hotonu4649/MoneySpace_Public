using System;
using R3;
using RelatedMoney;
using Unityroom.Client;
using UnityEngine;
using VContainer;

namespace GameSystem
{
    /// <summary>
    /// ゲーム終了時に「持っていたお金の最大値」をunityroomのスコアボードへ送るクラス。
    ///   生成時 : UnityroomClientを作り、HmacKeyを設定する
    ///   送信時 : Scoreboards.SendAsyncにScoreboardIdとScoreを渡してawaitする
    ///   破棄時 : OnDestroyでDispose()を呼ぶ
    /// </summary>
    public class UnityroomScoreSender : MonoBehaviour
    {
        [SerializeField, Tooltip("unityroomの「HMACキーをコピー」でコピーした文字列")]
        private string _hmacKey = " ";

        [SerializeField, Tooltip("送り先のスコアボードNo")]
        private int _scoreboardId = 1;

        [SerializeField, Tooltip("Editorで遊んだときも送るか。開発中のスコアで本番ランキングを汚さないよう、普段はオフ")]
        private bool _sendInEditor;

        private UnityroomClient _client;
        private GameEndManager _gameEndManager;
        private ResultMoneyData _resultMoneyData;
        private bool _isSent;

        [Inject]
        public void Construct(GameEndManager gameEndManager, ResultMoneyData resultMoneyData)
        {
            _gameEndManager = gameEndManager;
            _resultMoneyData = resultMoneyData;
        }

        private void Awake()
        {
            _client = new UnityroomClient
            {
                HmacKey = _hmacKey,
            };
        }

        private void Start()
        {
            _gameEndManager.OnGameEnd
                .Subscribe(_ => SendMaxMoney())
                .AddTo(this);
        }

        private void SendMaxMoney()
        {
            if (_isSent) return; // 1プレイにつき1回だけ
            _isSent = true;

            var score = ToFloat(_resultMoneyData.MaxMoney);
            if (Application.isEditor && !_sendInEditor)
            {
                Debug.Log($"[unityroom] Editorなので送信をスキップしました（ボード{_scoreboardId}, スコア{score}）");
                return;
            }
            SendScoreAsync(score);
        }

        /// <summary>スコアを送る。失敗してもゲームは止めず、ログだけ出す。</summary>
        public async void SendScoreAsync(float score)
        {
            try
            {
                var response = await _client.Scoreboards.SendAsync(new SendScoreRequest
                {
                    ScoreboardId = _scoreboardId,
                    Score = score,
                }, destroyCancellationToken);
                Debug.Log($"[unityroom] スコア送信: {score}（ボード{_scoreboardId}, 記録更新={response.ScoreUpdated}）");
            }
            catch (OperationCanceledException)
            {
                // 送信中にシーンを移動した場合など
            }
            catch (Exception e)
            {
                Debug.LogWarning($"[unityroom] スコアの送信に失敗しました: {e.Message}");
            }
        }

        private void OnDestroy()
        {
            _client?.Dispose();
        }

        private static float ToFloat(Money money) => float.TryParse(money.ToString(), out var value) ? value : 0f;
    }
}
