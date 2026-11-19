using R3;
using UnityEngine;
using UnityEngine.UI;

namespace Player.AutoShot
{
    public class AutoShotSlider : MonoBehaviour
    {
        private Slider _slider;
        private ReactiveProperty<float> SliderValue;
        public ReadOnlyReactiveProperty<float> Value => SliderValue;
        
        private void Awake()
        {
            TryGetComponent(out _slider);
            SliderValue = new ReactiveProperty<float>(_slider.value);
            SliderValue.Value = 1;
            _slider.onValueChanged.AddListener(SetSliderValue);
        }

        private void OnDestroy()
        {
            if (_slider != null)
            {
                _slider.onValueChanged.RemoveListener(SetSliderValue);
            }

            if (SliderValue != null)
            {
                SliderValue.Dispose();
            }
        }

        private void SetSliderValue(float value)
        {
            SliderValue.Value = 1 - value;
        }
    }
}