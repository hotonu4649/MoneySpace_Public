using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using DynamicItems.PlayerBullet;
using DynamicItems.PlayerWeapon;
using Player;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Title
{
    /// <summary>
    /// タイトル画面のデモ用プレイヤー。以下をくり返す。
    ///   1. 画面左の外から登場して、定位置まで移動する
    ///   2. 上下に動く
    ///   3. 2〜3発撃つ（撃つ処理はゲーム本編と同じPlayerShotを使う）
    ///   4. 右へ飛んで画面の外に出ていく
    ///   5. 1秒待って、また左から出てくる
    /// PlayerShotと同じGameObjectにアタッチする。プレイヤーは右向き（弾も右に飛ぶ）。
    /// </summary>
    [RequireComponent(typeof(PlayerShot))]
    public class TitlePlayerDemo : MonoBehaviour
    {
        [SerializeField, Tooltip("撃つ武器（NormalWeaponDataなど）")] private PlayerWeaponData _weaponData;

        [Header("位置")]
        [SerializeField, Tooltip("上下に動く中心の高さ（ワールド座標）")] private float _centerY = 0.5f;
        [SerializeField, Tooltip("登場して止まる位置のX（ワールド座標）")] private float _stopX = -4f;
        [SerializeField, Tooltip("画面の端からどれだけ外で出入りするか")] private float _offscreenMargin = 1.5f;

        [Header("動き")]
        [SerializeField] private float _enterDuration = 1.0f;
        [SerializeField, Tooltip("上下に動く幅")] private float _bobAmplitude = 1.2f;
        [SerializeField, Tooltip("上下の片道にかける秒数")] private float _bobDuration = 0.4f;
        [SerializeField] private int _minShots = 2;
        [SerializeField] private int _maxShots = 3;
        [SerializeField] private float _shotInterval = 0.2f;
        [SerializeField] private float _exitDuration = 1.2f;
        [SerializeField, Tooltip("画面から出てから再登場するまでの秒数")] private float _respawnDelay = 1.0f;

        private PlayerShot _playerShot;
        private Camera _camera;
        private readonly List<GameObject> _bullets = new ();

        private void Awake()
        {
            _camera = Camera.main;
            TryGetComponent(out _playerShot);

            // ゲーム本編ではVContainerが注入するが、タイトルにはスコープが無いので手動で渡す
            var weaponList = new PlayerWeaponList();
            weaponList.AddWeapon(_weaponData);
            _playerShot.Construct(weaponList);

            // 右向き（transform.upが右）にする。PlayerShotはこの向きに弾を飛ばす
            transform.rotation = Quaternion.Euler(0f, 0f, -90f);
            transform.position = new Vector3(LeftOutsideX(), _centerY, transform.position.z);
        }

        private void Start()
        {
            LoopAsync(destroyCancellationToken).Forget();
        }

        private void Update()
        {
            ReleaseOffscreenBullets();
        }

        private async UniTaskVoid LoopAsync(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    // 1. 左の外から登場
                    transform.position = new Vector3(LeftOutsideX(), _centerY, transform.position.z);
                    await Move(transform.DOMoveX(_stopX, _enterDuration).SetEase(Ease.OutCubic), token);

                    // 2. 上下に動く（上 → 下 → 中央）
                    await Move(transform.DOMoveY(_centerY + _bobAmplitude, _bobDuration).SetEase(Ease.InOutSine), token);
                    await Move(transform.DOMoveY(_centerY - _bobAmplitude, _bobDuration * 2f).SetEase(Ease.InOutSine), token);
                    await Move(transform.DOMoveY(_centerY, _bobDuration).SetEase(Ease.InOutSine), token);

                    // 3. 2〜3発撃つ
                    var shots = Random.Range(_minShots, _maxShots + 1);
                    for (var i = 0; i < shots; i++)
                    {
                        _bullets.Add(_playerShot.Shot());
                        await UniTask.Delay(TimeSpan.FromSeconds(_shotInterval), cancellationToken: token);
                    }

                    // 4. 右へ出ていく
                    await Move(transform.DOMoveX(RightOutsideX(), _exitDuration).SetEase(Ease.InQuad), token);

                    // 5. 少し待ってから再登場
                    await UniTask.Delay(TimeSpan.FromSeconds(_respawnDelay), cancellationToken: token);
                }
            }
            catch (OperationCanceledException)
            {
                // シーン遷移などで破棄されたときは何もしない
            }
        }

        private UniTask Move(Tween tween, CancellationToken token)
        {
            return tween.SetLink(gameObject).ToUniTask(cancellationToken: token);
        }

        /// <summary>画面外に出た弾をプールに戻す（タイトルには弾を消す壁が無いため）</summary>
        private void ReleaseOffscreenBullets()
        {
            var right = RightOutsideX();
            var left = LeftOutsideX();
            var top = _camera.orthographicSize + _offscreenMargin;
            for (var i = _bullets.Count - 1; i >= 0; i--)
            {
                var bullet = _bullets[i];
                if (bullet == null || !bullet.activeSelf)
                {
                    _bullets.RemoveAt(i);
                    continue;
                }
                var p = bullet.transform.position;
                if (p.x > right || p.x < left || Mathf.Abs(p.y) > top)
                {
                    PlayerBulletSpawner.BulletRelease(bullet);
                    _bullets.RemoveAt(i);
                }
            }
        }

        private float HalfWidth => _camera.orthographicSize * _camera.aspect;
        private float LeftOutsideX() => _camera.transform.position.x - HalfWidth - _offscreenMargin;
        private float RightOutsideX() => _camera.transform.position.x + HalfWidth + _offscreenMargin;
    }
}
