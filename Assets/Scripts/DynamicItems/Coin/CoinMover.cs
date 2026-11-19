using UnityEngine;

namespace DynamicItems.Coin
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class CoinMover : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        private CoinObjectStatus _coinObjectStatus;
        private float _moveSpeed;
        
        private void Awake()
        {
            TryGetComponent(out _rigidbody2D);
            TryGetComponent(out _coinObjectStatus);
        }

        private void Start()
        {
            _moveSpeed = CoinEnumToData.GetMoveSpeed(_coinObjectStatus.CoinType);
        }

        private void Update()
        {
            _rigidbody2D.linearVelocity = Vector2.down * _moveSpeed;
        }
    }
}
