using UnityEngine;
using UnityEngine.UI;
using ZombieSurvival.UI;

namespace ZombieSurvival.Achievements.UI
{
    public class AchievementProgress : FillBar
    {
        [SerializeField] private Text _progressText;

        public void Initialize(int minValue, int maxValue)
        {
            _minFillValue = minValue;
            _maxFillValue = maxValue;

            Initialize();
        }

        public void UpdateValue(int value, bool isInfinite = false)
        {
            _value = value;

            if (_value > _maxFillValue && !isInfinite)
            {
                _value = _maxFillValue;
            }

            UpdateBar();
        }

        protected override void UpdateBar()
        {
            base.UpdateBar();

            _progressText.text = _value.ToString() + "/" + _maxFillValue.ToString();
        }
    }
}