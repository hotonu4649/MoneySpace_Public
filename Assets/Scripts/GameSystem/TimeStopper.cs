using Player;
using R3;
using UnityEngine;
using UnityEngine.EventSystems;
using VContainer;

namespace GameSystem
{
    public class TimeStopper : MonoBehaviour, IPointerClickHandler
    {
        private PlayerController _playerController;
        private Rigidbody2D _playerRigidbody2D;
        private bool _isStoppedBySkillSelection;
        private readonly ReactiveProperty<bool> _timeStoppedByButton = new ();
        public ReadOnlyReactiveProperty<bool> TimeStopped => _timeStoppedByButton.ToReadOnlyReactiveProperty();
        
        [Inject]
        public void Construct(PlayerController playerController)
        {
            _playerController = playerController;
            _playerController.TryGetComponent(out _playerRigidbody2D);
        }

        private void StopTimeFromButton()
        {
            _timeStoppedByButton.Value = true;
            if (!_isStoppedBySkillSelection)
            {
                Time.timeScale = 0f;
                _playerController.enabled = false;
                _playerRigidbody2D.linearVelocity = Vector2.zero;
            }
        }
        
        private void ResumeTimeFromButton()
        {
            _timeStoppedByButton.Value = false;
            if (!_isStoppedBySkillSelection)
            {
                Time.timeScale = 1f;
                _playerController.enabled = true;
            }
        }
        
        public void StopTimeFromSkillSelection()
        {
            _isStoppedBySkillSelection = true;
            Time.timeScale = 0f;
            _playerController.enabled = false;
            _playerRigidbody2D.linearVelocity = Vector2.zero;
        }
        
        public void ResumeTimeFromSkillSelection()
        {
            _isStoppedBySkillSelection = false;
            if (!_timeStoppedByButton.Value)
            {
                Time.timeScale = 1f;
                _playerController.enabled = true;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_timeStoppedByButton.Value)
            {
                ResumeTimeFromButton();
            }
            else
            {
                StopTimeFromButton();
            }
        }
    }
}