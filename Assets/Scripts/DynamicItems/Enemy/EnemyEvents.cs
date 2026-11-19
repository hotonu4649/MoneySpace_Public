using R3;
using UnityEngine;

namespace DynamicItems.Enemy
{
    [RequireComponent(typeof(Collider2D))]
    public class EnemyEvents : MonoBehaviour
    {
        private readonly Subject<Collider2D> _collisionSubject = new Subject<Collider2D>();
        private readonly Subject<Unit> _onEnableSubject = new Subject<Unit>();
        private readonly Subject<Unit> _onDisableSubject = new Subject<Unit>();
        public Observable<Collider2D> CollisionObservable => _collisionSubject;
        public Observable<Unit> OnEnableObservable => _onEnableSubject;
        public Observable<Unit> OnDisableObservable => _onDisableSubject;

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("PlayerBullet"))
            {
                _collisionSubject.OnNext(other);
            }
        }

        private void OnEnable()
        {
            _onEnableSubject.OnNext(Unit.Default);
        }

        private void OnDisable()
        {
            _onDisableSubject.OnNext(Unit.Default);
        }
    }
}