using UnityEngine;
using UnityEngine.UI;
using ZombieSurvival.Stats;

namespace ZombieSurvival.UI
{
    public class PlayerExpBar : FillBar
    {
        [Header("Settings")]
        [SerializeField] private Text _levelText;
        [SerializeField] private bool _showLVLText;
        private PlayerExpLevel _expirience;

        public void Initialize(PlayerExpLevel expLevel)
        {
            _expirience = expLevel;

            UpdateLevel();
            UpdateExp();
        }

        /// <summary>
        /// Update Slider Value
        /// </summary>
        public void UpdateExp()
        {
            if (_expirience != null)
            {
                _value = (int)_expirience.Exp.Value;

                UpdateBar();
            }
        }

        /// <summary>
        /// Update Slider MaxFillValue
        /// </summary>
        public void UpdateLevel()
        {
            if (_expirience == null) return;

            _maxFillValue = _expirience.ExpForLevel;
            _fillBar.maxValue = _maxFillValue;
            _levelText.text = ((int)_expirience.Value).ToString() + (_showLVLText ? " lvl" : "");
        }
    }
}