using Effects.RelatedFabulousEffect;
using GameSystem.Audio;
using Player.AutoShot;
using Skill;
using VContainer.Unity;

namespace GameSystem
{
    public class GameCycle : IStartable
    {
        private readonly AutoShotPresenter _autoShotPresenter;
        private readonly SkillManager _skillManager;
        private readonly GameEndManager _gameEndManager;
        private readonly PlayAudio _playAudio;
        private readonly FabulousEffectTrigger _fabulousEffectTrigger;
        private readonly FabulousEffectPlayer _fabulousEffectPlayer;
        private readonly SubFabulousTrigger _subFabulousTrigger;
        private readonly SubFabulousAudio _subFabulousAudio;
        
        
        public GameCycle(
            AutoShotPresenter autoShotPresenter,
            SkillManager skillManager,
            GameEndManager gameEndManager,
            PlayAudio playAudio,
            FabulousEffectTrigger fabulousEffectTrigger,
            FabulousEffectPlayer fabulousEffectPlayer,
            SubFabulousTrigger subFabulousTrigger,
            SubFabulousAudio subFabulousAudio)
        {
            _autoShotPresenter = autoShotPresenter;
            _skillManager = skillManager;
            _gameEndManager = gameEndManager;
            _playAudio = playAudio;
            _fabulousEffectTrigger = fabulousEffectTrigger;
            _fabulousEffectPlayer = fabulousEffectPlayer;
            _subFabulousTrigger = subFabulousTrigger;
            _subFabulousAudio = subFabulousAudio;
        }
        
        public void Start()
        {
            _autoShotPresenter.Initialize();
            _skillManager.Initialize();
            _gameEndManager.Initialize();
            _playAudio.Initialize();
            _fabulousEffectTrigger.Initialize();
            _fabulousEffectPlayer.Initialize();
            _subFabulousTrigger.Initialize();
            _subFabulousAudio.Initialize();
        }
    }
}