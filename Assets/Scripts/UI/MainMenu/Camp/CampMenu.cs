using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.UI.Shop;

using Zenject;
using ZombieSurvival.Rewards;
using ZombieSurvival.General.Inventories;
using ZombieSurvival.Events;

namespace ZombieSurvival.UI.Camp
{
    public class CampMenu : UIMenu
    {
        [Header("Camp menu settings")]
        [SerializeField] private CampTalentsMenu _talents;
        [SerializeField] private CampUpgradesMenu _upgrades;
        [SerializeField] private List<CampBuilding> _buildings;
        [SerializeField] private RouletteMarker _rouletteMarker;

        [Space(5)]
        [SerializeField] private Text _campLevelText;

        [Space(5)]
        [SerializeField] private GameObject _cantUpgradeTooltip;
        [SerializeField] private Text _tooltipText;

        [Space(5)]
        [SerializeField] private GameObject _upgradeButton;
        [SerializeField] private Image _requiredCurrencyIconImage;
        [SerializeField] private Text _requiredCurrencyAmountText;

        [Header("Upgrades settings")]
        [SerializeField] private ChanceCombiner<CampRoulleteChance> _buildingChances;
        [SerializeField] private List<CampUpgradeLimit> _campLevelsLimit;
        [SerializeField] private Currency _requiredCurrency;
        [SerializeField] private float _currencyMultiplierPerLevel;

        private Currency _currentCost;

        [Inject] private MainInventory _mainInventory;
        [Inject] private PlayerLevelInventory _playerLevelInventory;
        [Inject] private CampInventory _campInventory;

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _talents.Initialize(_mainMenu, this);
            _upgrades.Initialize(_mainMenu, this);
        }

        public override void Display(bool playAnimation = false)
        {
            EventBus.Publish<IOpenCampHandler>(handler => handler.OnOpenCamp());

            _talents.Hide();
            _upgrades.Hide();

            UpdateUpgrades();

            base.Display(playAnimation);
        }

        public override void Hide(bool playAnimation = false)
        {
            _talents.Hide();
            _upgrades.Hide();

            base.Hide(playAnimation);
        }

        public void OnUpgradeClick()
        {
            if (_mainInventory.Spend(new CurrencyReward(_currentCost)))
            {
                if (!_mainMenu.DisplayRoulette(_buildingChances, _rouletteMarker))
                {
                    _mainMenu.ShowRewards(new CampUpgradeReward(_buildingChances.GetStrikedObject(), new CircleRouletteSlotData()));
                }
            }
            else
            {
                _mainMenu.ShowPopupMessage("Not enough resources!");
            }
        }

        public void UpdateUpgrades()
        {
            int currentLimitIndex = _campLevelsLimit.FindLastIndex(item => 
                item.RequiredPlayerLevel <= _playerLevelInventory.PlayerLevel.Value);

            int currentLevel = 0;

            for (int i = 0; i < _campInventory.Upgrades.Count; i++)
            {
                currentLevel += (int)_campInventory.Upgrades[i].Level.Value;
            }

            _campLevelText.text = "Camp lvl " + currentLevel;

            if (currentLimitIndex == _campLevelsLimit.Count - 1 && currentLevel >= _campLevelsLimit[currentLimitIndex].MaxCampLevel)
            {
                _upgradeButton.SetActive(false);
                _cantUpgradeTooltip.SetActive(true);

                _tooltipText.text = "Reached max camp level for now!";
            }
            else if (currentLimitIndex == -1)
            {
                _upgradeButton.SetActive(false);
                _cantUpgradeTooltip.SetActive(true);

                _tooltipText.text = "Requires lvl " + _campLevelsLimit[0].RequiredPlayerLevel + " to start upgrading camp";
            }
            else
            {
                if (currentLevel >= _campLevelsLimit[currentLimitIndex].MaxCampLevel)
                {
                    _upgradeButton.SetActive(false);
                    _cantUpgradeTooltip.SetActive(true);

                    _tooltipText.text = "Requires lvl " + _campLevelsLimit[currentLimitIndex + 1].RequiredPlayerLevel + " to continue upgrading camp";
                }
                else
                {
                    _upgradeButton.SetActive(true);
                    _cantUpgradeTooltip.SetActive(false);

                    _currentCost = new Currency
                        (
                            _requiredCurrency.CurrencyData,
                            _requiredCurrency.CurrencyValue * (int)((currentLevel + 1) * _currencyMultiplierPerLevel)
                        );

                    _requiredCurrencyAmountText.text = _currentCost.CurrencyValue.ToString();
                    _requiredCurrencyIconImage.sprite = _currentCost.CurrencyData.Icon;
                }
            }
        }
    }
}