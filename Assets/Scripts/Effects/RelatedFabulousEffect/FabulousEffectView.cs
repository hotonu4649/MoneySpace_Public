using UnityEngine;

namespace Effects.RelatedFabulousEffect
{
    public class FabulousEffectView : MonoBehaviour
    {
        [SerializeField] private Material _fabulousMaterial;
        [SerializeField] private float _speed = 1f;
        private bool _isFabulousEffectPlaying;
        private float _currenHueOffset;
        private static readonly int Intensity = Shader.PropertyToID("_Intensity");
        private static readonly int HueOffset = Shader.PropertyToID("_HueOffset");
        
        private void Awake()
        {
            _fabulousMaterial.SetFloat(Intensity, 0f);
        }
        
        public void Play(float intensity)
        {
            _isFabulousEffectPlaying = true;
            _fabulousMaterial.SetFloat(Intensity, intensity);
        }

        public void Stop()
        {
            _isFabulousEffectPlaying = false;
            _fabulousMaterial.SetFloat(Intensity, 0f);
        }
        
        private void Update()
        {
            if (_isFabulousEffectPlaying)
            {
                _currenHueOffset += Time.unscaledDeltaTime * _speed;
                _currenHueOffset = Mathf.Repeat(_currenHueOffset, 1f);
                _fabulousMaterial.SetFloat(HueOffset, _currenHueOffset);
            }
        }
        
        private void OnDestroy()
        {
            _fabulousMaterial.SetFloat(Intensity, 0f);
        }
    }
}