using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace GameSystem
{
    /// <summary>
    /// クリックすると指定したシーンに移動する。Image（パネル）にアタッチして使う。
    /// 拡縮アニメーションはSkillButonAnimationを一緒に付ける。
    /// 移動先のシーンはBuild Settings（File > Build Profiles のScene List）に入っている必要がある。
    /// </summary>
    public class SceneTransitionButton : MonoBehaviour, IPointerClickHandler
    {
#if UNITY_EDITOR
        [SerializeField, Tooltip("移動先のシーン。ここにシーンファイルをドラッグすると下のシーン名が自動で入る")]
        private UnityEditor.SceneAsset _sceneAsset;
#endif
        [SerializeField, Tooltip("移動先のシーン名（ビルドではこの名前で読み込む）")]
        private string _sceneName;

        private bool _isLoading;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_isLoading) return;
            if (string.IsNullOrEmpty(_sceneName))
            {
                Debug.LogError($"{name}: 移動先のシーンが設定されていません。", this);
                return;
            }
            if (!Application.CanStreamedLevelBeLoaded(_sceneName))
            {
                Debug.LogError($"{name}: シーン「{_sceneName}」がBuild Settingsに登録されていません。", this);
                return;
            }

            _isLoading = true;
            // スキル選択中やポーズ中にtimeScale=0のまま移動すると、次のシーンが止まったままになるため戻しておく
            Time.timeScale = 1f;
            SceneManager.LoadScene(_sceneName);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_sceneAsset != null)
            {
                _sceneName = _sceneAsset.name;
            }
        }
#endif
    }
}
