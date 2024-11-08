using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Shop
{
    public class SingleCurrencyPackage : ShopPackage
    {
        [Header("Currency package settings")]
        [SerializeField] protected Text _additionalRewardText;
        [SerializeField] protected Currency _currencyReward;
        [SerializeField] protected Image _currencyIconImage;
        [SerializeField] protected Text _rewardAmountText;

        private ConcreteReward _reward;

        public override void Initialize()
        {
            base.Initialize();

            _reward = new CurrencyReward(_currencyReward * _rewardMultiplier);

            _currencyIconImage.sprite = _reward.Icon;
            _rewardAmountText.text = _currencyReward.CurrencyValue.ToString();
            _additionalRewardText.text = "+" + (_currencyReward.CurrencyValue * (_rewardMultiplier - 1)).ToString();
        }

        public override void OnBuyClick()
        {
            if (_mainInventory.Spend(new CurrencyReward(_cost)))
            {
                if (!_mainMenu.ShowRewards(_reward))
                {
                    _mainInventory.Add(_reward);
                }
            }
            else
            {
                _mainMenu.ShowPopupMessage("Not enough resources!");
            }
        }
    }
}