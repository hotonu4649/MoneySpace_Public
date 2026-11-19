using UnityEngine;

namespace Player
{
    public class PlayerMove : MonoBehaviour
    {
        [SerializeField] private float _moveSpeed = 5f;
        [Header("Playerの動ける範囲")] 
        [SerializeField] private float _canMoveTop;
        [SerializeField] private float _canMoveBottom;
        [SerializeField] private float _canMoveRight;
        [SerializeField] private float _canMoveLeft;
        private Rigidbody2D _rigidbody2D;
        private Vector2 _moveDirection;

        private void Awake()
        {
            TryGetComponent(out _rigidbody2D);
        }
        
        private void FixedUpdate()
        {
            Vector2 currentPos = _rigidbody2D.position;
            if (currentPos.x <= _canMoveLeft && _moveDirection.x < 0f) _moveDirection.x = 0f;
            if (currentPos.x >= _canMoveRight && _moveDirection.x > 0f) _moveDirection.x = 0f;
            if (currentPos.y <= _canMoveBottom && _moveDirection.y < 0f) _moveDirection.y = 0f;
            if (currentPos.y >= _canMoveTop && _moveDirection.y > 0f) _moveDirection.y = 0f;
            Move(_moveDirection);
            _rigidbody2D.position = new Vector2(
                Mathf.Clamp(_rigidbody2D.position.x, _canMoveLeft, _canMoveRight),
                Mathf.Clamp(_rigidbody2D.position.y, _canMoveBottom, _canMoveTop)
            );
        }

        public void SetMoveDirection(Vector2 inputDirection)
        {
            _moveDirection = inputDirection;
        }
        
        private void Move(Vector2 direction)
        {
            _rigidbody2D.linearVelocity = direction * _moveSpeed;
        }
    }
}
