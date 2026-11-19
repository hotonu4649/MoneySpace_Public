using DynamicItems.Coin;
using DynamicItems.DemoPlayerBullet;
using DynamicItems.Enemy;
using DynamicItems.PlayerWeapon;
using Effects.RelatedFabulousEffect;
using GameSystem.Audio;
using Player;
using Player.AutoShot;
using RelatedMoney;
using RelatedMoney.Demo;
using Skill;
using Skill.ConcreteSkill.GetPlayerWeapon;
using Skill.ConcreteSkill.SkillInterface;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace GameSystem
{
    public class GameLifeTimeScope : LifetimeScope
    {
        [Header("Player関連")]
        [SerializeField] private PlayerCollision _playerCollision;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerShot _playerShot;
        [Header("AutoShot関連")]
        [SerializeField] private IsAutoShotButton _isAutoShotButton;
        [SerializeField] private AutoShotSlider _autoShotSlider;
        [SerializeField] private DemoPlayerShoter _demoPlayerShoter;
        [Header("Money関連")]
        [SerializeField] private HaveMoneyView _haveMoneyView;
        [SerializeField] private RandomCoinSpawn _randomCoinSpawn;
        [Header("Enemy関連")]
        [SerializeField] private RandomEnemySpawner _randomEnemySpawner;
        [Header("Skill関連")]
        [SerializeField] private SkillDatabase _skillDatabase;
        [SerializeField] private SkillTimeDatabase _skillTimeDatabase;
        [SerializeField] private SkillSelectPresenter _skillSelectPresenter;
        [Header("PlayerWeapon関連")]
        [SerializeField] private PlayerWeaponPresenter _playerWeaponPresenter;
        [SerializeField] private PlayerWeaponData _superWeaponData;
        [SerializeField] private PlayerWeaponData _hyperWeaponData;
        [Header("Timer関連")]
        [SerializeField] private TimeStopper _timeStopper;
        [Header("Result関連")]
        [SerializeField] private ResultPanel _resultPanel;
        [Header("Audio関連")]
        [SerializeField] private AudioManager _audioManager;
        [SerializeField] private BgmPlayer _bgmPlayer;
        [Header("FabulousEffect関連")]
        [SerializeField] private FabulousEffectView _fabulousEffectView;
        [Header("デモ用")]
        [SerializeField] private DemoMoney _demoMoney;//デモ用
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<HaveMoneyStatus>(Lifetime.Singleton);
            builder.Register<EnemySpawner>(Lifetime.Singleton);
            builder.Register<AutoShotDuration>(Lifetime.Singleton);
            builder.Register<AutoShotPresenter>(Lifetime.Singleton);
            builder.Register<PlayerWeaponList>(Lifetime.Singleton);
            builder.Register<SkillLottery>(Lifetime.Singleton);
            builder.Register<CurrentSkillData>(Lifetime.Singleton);
            builder.Register<MoneyCalculator>(Lifetime.Singleton);
            builder.Register<SkillManager>(Lifetime.Singleton);
            builder.Register<WeaponAdder>(Lifetime.Singleton);
            builder.Register<GetSuperWeapon>(Lifetime.Singleton)
                .As<IGetPlayerWeapon>()
                .WithParameter(_superWeaponData);
            builder.Register<GetHyperWeapon>(Lifetime.Singleton)
                .As<IGetPlayerWeapon>()
                .WithParameter(_hyperWeaponData);
            builder.Register<SkillIdToClass>(Lifetime.Singleton)
                .WithParameter("superWeaponData", _superWeaponData)
                .WithParameter("hyperWeaponData", _hyperWeaponData);
            builder.Register<MoneyFromEnemyCalculator>(Lifetime.Singleton);
            builder.Register<GameEndManager>(Lifetime.Singleton);
            builder.Register<ResultMoneyData>(Lifetime.Singleton);
            builder.Register<EnemyAttackCalculator>(Lifetime.Singleton);
            builder.Register<PlayAudio>(Lifetime.Singleton);
            builder.Register<FabulousEffectTrigger>(Lifetime.Singleton);
            builder.Register<FabulousEffectPlayer>(Lifetime.Singleton);
            builder.Register<SubFabulousTrigger>(Lifetime.Singleton);
            builder.Register<SubFabulousAudio>(Lifetime.Singleton);
            
            builder.RegisterEntryPoint<GameTimer>().AsSelf();
            builder.RegisterEntryPoint<PlayerRelease>();
            builder.RegisterEntryPoint<PlayerMoneySubscribes>();
            builder.RegisterEntryPoint<HaveMoneyPresenter>();
            builder.RegisterEntryPoint<GameCycle>();
            
            // RegisterComponent はMonoBehaviourを継承したクラスが Construct で注入もされるし、コンテナに登録もされる
            builder.RegisterComponent(_playerCollision);
            builder.RegisterComponent(_playerController);
            builder.RegisterComponent(_playerShot);
            builder.RegisterComponent(_isAutoShotButton);
            builder.RegisterComponent(_autoShotSlider);
            builder.RegisterComponent(_haveMoneyView);
            builder.RegisterComponent(_randomCoinSpawn);
            builder.RegisterComponent(_randomEnemySpawner);
            builder.RegisterComponent(_playerWeaponPresenter);
            builder.RegisterComponent(_skillSelectPresenter);
            builder.RegisterComponent(_timeStopper);
            builder.RegisterComponent(_demoPlayerShoter);
            // unityroomへのスコア送信（シーン内のUnityroomScoreSenderを探して注入する）
            builder.RegisterComponentInHierarchy<UnityroomScoreSender>();

            // RegisterInstance はインスタンスをコンテナに登録するだけ
            builder.RegisterInstance(_skillDatabase);
            builder.RegisterInstance(_skillTimeDatabase);
            builder.RegisterInstance(_resultPanel);
            builder.RegisterInstance(_audioManager);
            builder.RegisterInstance(_fabulousEffectView);
            builder.RegisterInstance(_bgmPlayer);
            
            builder.RegisterComponent(_demoMoney);//デモ用
        }
    }
}