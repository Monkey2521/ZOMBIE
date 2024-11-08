using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.Upgrades;

namespace ZombieSurvival.UI.Camp
{
    public class CampUpgradeButton : MonoBehaviour
    {
        [SerializeField] private Image _iconImage;
        [SerializeField] private Image _lockedImage;

        [SerializeField] private Text _levelText;
        [SerializeField] private Text _valueText;

        private CampUpgrade _upgrade;

        public void Initialize(CampUpgrade upgrade)
        {
            _upgrade = upgrade;

            _iconImage.sprite = _upgrade.UnlockedIcon;
            _lockedImage.sprite = _upgrade.LockedIcon;

            UpdateValues();
        }

        public void UpdateValues()
        {
            if (_upgrade == null) return;

            if (_upgrade.Level.Value == _upgrade.Level.MaxValue)
            {
                _levelText.text = "MAX";
            }
            else
            {
                _levelText.text = "Level " + (int)_upgrade.Level.Value;
            }

            if (!_upgrade.Unlocked)
            {
                _lockedImage.enabled = true;
                _iconImage.enabled = false;
                _valueText.enabled = false;
                _levelText.enabled = false;
            }
            else
            {
                _lockedImage.enabled = false;
                _iconImage.enabled = true;
                _valueText.enabled = true;
                _levelText.enabled = true;

                int value = 0;
                if (_upgrade.Level.Value > 0)
                {
                    foreach (var data in _upgrade.CurrentUpgrade.Upgrades)
                    {
                        value += (int)data.UpgradeValue;
                    }
                }

                _valueText.text = (value > 0 ? "+ " : "") + value;
            }
        }
    }
}