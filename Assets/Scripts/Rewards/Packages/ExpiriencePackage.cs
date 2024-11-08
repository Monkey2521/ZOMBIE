using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.Rewards;
using ZombieSurvival.Stats;

namespace ZombieSurvival.UI.Shop
{
    public sealed class ExpiriencePackage : ShopPackage
    {
        [Header("Currency package settings")]
        [SerializeField] private Expirience _expReward;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _background;
        [SerializeField] private Image _currencyIconImage;
        [SerializeField] private Text _rewardAmountText;

        private ConcreteReward _reward;

        public override void Initialize()
        {
            base.Initialize();

            _expReward.Initialize();

            _reward = new ExpirienceReward(_expReward, _icon, _background);

            _currencyIconImage.sprite = _reward.Icon;
            _rewardAmountText.text = _expReward.Value.ToString();
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