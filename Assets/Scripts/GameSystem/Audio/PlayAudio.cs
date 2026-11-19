using Cysharp.Threading.Tasks;
using DynamicItems.PlayerWeapon;
using Player;
using Player.AutoShot;
using R3;
using RelatedMoney;
using Skill;

namespace GameSystem.Audio
{
    public class PlayAudio
    {
        private readonly AudioManager _audioManager;
        private readonly PlayerCollision _playerCollision;
        private readonly SkillSelectPresenter _skillSelectPresenter;
        private readonly IsAutoShotButton _isAutoShotButton;
        private readonly PlayerWeaponList _playerWeaponList;
        private readonly TimeStopper _timeStopper;
        private readonly HaveMoneyStatus _haveMoneyStatus;
        private readonly PlayerShot _playerShot;

        public PlayAudio(
            AudioManager audioManager,
            PlayerCollision playerCollision,
            SkillSelectPresenter skillSelectPresenter,
            IsAutoShotButton isAutoShotButton,
            PlayerWeaponList playerWeaponList,
            TimeStopper timeStopper,
            HaveMoneyStatus haveMoneyStatus,
            PlayerShot playerShot)
        {
            _audioManager = audioManager;
            _playerCollision = playerCollision;
            _skillSelectPresenter = skillSelectPresenter;
            _isAutoShotButton = isAutoShotButton;
            _playerWeaponList = playerWeaponList;
            _timeStopper = timeStopper;
            _haveMoneyStatus = haveMoneyStatus;
            _playerShot = playerShot;
        }
        
        public void Initialize()
        {
            AudioSubscribe();
        }
        
        private void AudioSubscribe()
        {
            // プレイヤーがコインに衝突したときの音を再生する
            _haveMoneyStatus.OnMoneyChanged
                .Subscribe(_ =>
                {
                    _audioManager.PlaySeAsync("GettingCoinSound").Forget();
                }).AddTo(_audioManager);
            
            // プレイヤーが敵の弾に衝突したときの音を再生する
            _playerCollision.EnemyBulletCollisionObservable
                .Subscribe(_ =>
                {
                    _audioManager.PlaySeAsync("PlayerDamageSound").Forget();
                }).AddTo(_audioManager);    
            
            // スキルが選択されたときの音を再生する
            _skillSelectPresenter.OnSkillSelected
                .Subscribe(_ =>
                {
                    _audioManager.PlaySeAsync("GettingSkillSound").Forget();
                }).AddTo(_audioManager);
            
            // オートショットボタンが押されたときの音を再生する
            _isAutoShotButton.IsAutoShot
                .Where(isAutoShot => !isAutoShot)
                .Skip(1)
                .Subscribe(_ =>
                {
                    _audioManager.PlaySeAsync("ButtonOff").Forget();
                }).AddTo(_audioManager);
            
            _isAutoShotButton.IsAutoShot
                .Where(isAutoShot => isAutoShot)
                .Subscribe(_ =>
                {
                    _audioManager.PlaySeAsync("ButtonOn").Forget();
                }).AddTo(_audioManager);
                
            // 武器が変更されたときの音を再生する
            _playerWeaponList.OnWeaponChange
                .Subscribe(_ =>
                {
                    _audioManager.PlaySeAsync("WeaponChange").Forget();
                }).AddTo(_audioManager);
            
            // 一時停止ボタンが押されたときの音を再生する
            _timeStopper.TimeStopped
                .Where(isTimeStopped => !isTimeStopped)
                .Skip(1)
                .Subscribe(_ =>
                {
                    _audioManager.PlaySeAsync("ButtonOff").Forget();
                }).AddTo(_audioManager);
            
            _timeStopper.TimeStopped
                .Where(isTimeStopped => isTimeStopped)
                .Subscribe(_ =>
                {
                    _audioManager.PlaySeAsync("ButtonOn").Forget();
                }).AddTo(_audioManager);
            
            _playerShot.OnPlayerShot
                .Subscribe(_ =>
                {
                    _audioManager.PlaySeAsync("PlayerShoot").Forget();
                }).AddTo(_audioManager);
        }
    }
}