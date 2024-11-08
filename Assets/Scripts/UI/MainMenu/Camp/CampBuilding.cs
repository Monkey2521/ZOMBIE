using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.Upgrades;

using Zenject;

namespace ZombieSurvival.UI.Camp
{
    public class CampBuilding : ZSMonoBehaviour
    {
        [Header("Building settings")]
        [SerializeField] private CampMenu _campMenu;
        [SerializeField] private CampUpgradeButton _upgradeButton;
        [SerializeField] private Animator _animator;
        [SerializeField] private Image _lockedImage;
        [SerializeField] private Image _unlockedImage;

        private CampUpgrade _campUpgrade;
        private bool _unlocked;

        public CampUpgrade CampUpgrade => _campUpgrade;

        public void Initialize(CampUpgrade campUpgrade)
        {
            _campUpgrade = campUpgrade;

            _unlocked = false;
            CheckLocked();

            _upgradeButton.Initialize(_campUpgrade);
        }

        public void CheckLocked()
        {
            if (_unlocked) return;

            if (_campUpgrade.Unlocked)
            {
                _lockedImage.enabled = false;
                _unlockedImage.enabled = true;

                _unlocked = true;
            }
            else
            {
                _unlockedImage.enabled = false;
                _lockedImage.enabled = true;
            }
        }

        public void UpdateValues()
        {
            CheckLocked();

            _upgradeButton.UpdateValues();

            _campMenu.UpdateUpgrades();
        }

        public void OnClick()
        {
            if (_campUpgrade.Unlocked)
            {
                _animator.SetTrigger("Click");
            }
        }
    }
}