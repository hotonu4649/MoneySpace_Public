using UnityEngine;

namespace DynamicItems.DemoPlayerBullet
{
    public class DemoBulletMover : MonoBehaviour 
    {
        private RectTransform _rectTransform;
        private float _canvasScaleFactor;
        private float _worldSpeed;
        
        private void Awake()
        {
            TryGetComponent(out _rectTransform);
        }
        
        public void Initialize(float speed, Canvas canvas)
        {
            _worldSpeed = speed;
            _canvasScaleFactor = canvas.scaleFactor;
        }
        
        private void Update()
        {
            Move();
            if (_rectTransform.anchoredPosition.y > 230f)
            {
                DemoPlayerBulletSpawner.BulletRelease(gameObject);
            }
        }
        
        private void Move()
        {
            var canvasSpeed = _worldSpeed * (Screen.height / (Camera.main.orthographicSize * 2f)) / _canvasScaleFactor;
            var deltaPosition = transform.up * canvasSpeed * Time.unscaledDeltaTime;
            _rectTransform.anchoredPosition += new Vector2(deltaPosition.x, deltaPosition.y);
        }
    }
}