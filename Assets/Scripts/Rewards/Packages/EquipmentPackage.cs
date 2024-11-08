using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Shop
{
    public sealed class EquipmentPackage : ShopPackage
    {
        [Header("Equipment package settings")]
        [SerializeField] private EquipmentRewardData _reward;
        [SerializeField] private RouletteMarker _rouletteMarker;

        [Space(5)]
        [SerializeField][Range(2, 10)] private int _multipleBuyAmount;
        [SerializeField] private Currency _multipleBuyCost;
        [SerializeField] private Image _multipleBuyCurrenceCostIconImage;
        [SerializeField] private Text _multipleBuyCostText;
        [SerializeField] private Text _multipleBuyDescriptionText;

        private const string MULTIPLE_BUY_DESCRIPTION = "Buy x";

        [Space(5)]
        [SerializeField] private bool _enableAdReward;
        [SerializeField] private GameObject _adButton;

        public override void Initialize()
        {
            base.Initialize();

            if (!_enableAdReward)
            {
                _adButton?.SetActive(false);
            }
            else
            {
                _adButton.SetActive(true);
            }

            _multipleBuyCurrenceCostIconImage.sprite = _multipleBuyCost.CurrencyData.Icon;
            _multipleBuyDescriptionText.text = MULTIPLE_BUY_DESCRIPTION + _multipleBuyAmount.ToString();
        }

        public override void UpdatePackage()
        {
            base.UpdatePackage();

            if (_displayInvontoryAmount && _inventory != null)
            {
                _multipleBuyCostText.text = (_inventory.Total + _inventoryAmountSeparator.ToString() + 
                    _multipleBuyCost.CurrencyValue).ToString();
            }
            else
            {
                _multipleBuyCostText.text = _multipleBuyCost.CurrencyValue.ToString();
            }
        }

        public override void OnBuyClick()
        {
            if (_mainInventory.Spend(new CurrencyReward(_cost)))
            {
                if (!_mainMenu.DisplayRoulette(_reward.ReawardChances, _rouletteMarker))
                {
                    _mainMenu.ShowRewards(_reward.GetConcreteRewards());
                }
            }
            else
            {
                _mainMenu.ShowPopupMessage("Not enough resources!");
            }
            
        }

        public void OnMultipleBuyClick()
        {
            if (_mainInventory.Spend(new CurrencyReward(_multipleBuyCost)))
            {
                if (!_mainMenu.DisplayRoulette(_reward.ReawardChances, _rouletteMarker, _multipleBuyAmount))
                {
                    List<ConcreteReward> rewards = new List<ConcreteReward>();

                    for (int i = 0; i < _multipleBuyAmount; i++)
                    {
                        rewards.AddRange(_reward.GetConcreteRewards());
                    }

                    _mainMenu.ShowRewards(rewards);
                }
            }
            else
            {
                _mainMenu.ShowPopupMessage("Not enough resources!");
            }
        }

        public void OnAdClick()
        {
            if (!_mainMenu.DisplayRoulette(_reward.ReawardChances, _rouletteMarker))
            {
                _mainMenu.ShowRewards(_reward.GetConcreteRewards());
            }
        }
    }
}