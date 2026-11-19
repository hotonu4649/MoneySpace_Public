using R3;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private InputActionReference _moveAction;
        [SerializeField] private InputActionReference _bulletAction;
        private PlayerMove _playerMove;
        private PlayerShot _playerShot;
        private float _playerAngle;

        private void Awake()
        {
            TryGetComponent(out _playerMove);
            TryGetComponent(out _playerShot);
        }

        private void Update()
        {
            RotatePlayer();
        }

        private void OnEnable()
        {
            _moveAction.action.performed += TakeMoveDirection;
            _moveAction.action.canceled += TakeMoveDirection;

            _bulletAction.action.performed +=　Shot;

            _moveAction.action.Enable();
            _bulletAction.action.Enable();
        }
        private void OnDisable()
        {
            _moveAction.action.performed -= TakeMoveDirection;
            _moveAction.action.canceled -= TakeMoveDirection;
            
            _bulletAction.action.performed -= Shot;

            _moveAction.action.Disable();
            _bulletAction.action.Disable();
            _playerMove.SetMoveDirection(Vector2.zero);
        }
        
        private void RotatePlayer()
        {
            Vector2 mousePosition = Mouse.current.position.ReadValue();
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(
                new Vector3(
                    mousePosition.x,
                    mousePosition.y,
                    -Camera.main.transform.position.z
                )
            );
            Vector2 direction = worldMousePosition - transform.position;
            _playerAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Euler(0f, 0f, _playerAngle);
        }
        
        private void TakeMoveDirection(InputAction.CallbackContext context)
        {
            _playerMove.SetMoveDirection(context.ReadValue<Vector2>());
        }
        
        private void Shot(InputAction.CallbackContext context)
        {
            var playerBullet = _playerShot.Shot();
        }
    }
}