using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.Rewards;
using ZombieSurvival.UI.Shop;

public class NewcomerPackage : ShopPackage
{
    [Header("Equipment package settings")]
    [SerializeField] private NewcomerReward _reward1;
    [SerializeField] private NewcomerReward _reward2;
    [SerializeField] private NewcomerReward _reward3;

    public override void Initialize()
    {
        base.Initialize();

        _reward1.Initialize();
        _reward2.Initialize();
        _reward3.Initialize();
    }

    public override void OnBuyClick()
    {
        if (_mainInventory.Spend(new CurrencyReward(_cost)))
        {
            List<ConcreteReward> rewards = new List<ConcreteReward>();

            rewards.Add(new CurrencyReward(_reward1.Reward * _rewardMultiplier));
            rewards.Add(new CurrencyReward(_reward2.Reward * _rewardMultiplier));
            rewards.Add(new CurrencyReward(_reward3.Reward * _rewardMultiplier));

            _mainMenu.ShowRewards(rewards);
        }
        else
        {
            _mainMenu.ShowPopupMessage("Not enough resources!");
        }
    }

    [System.Serializable]
    private class NewcomerReward
    {
        [SerializeField] private Currency _reward;
        [SerializeField] private Image _rewardIconImage;
        [SerializeField] private Text _rewardAmountText;

        public Currency Reward => _reward;

        public void Initialize()
        {
            _rewardIconImage.sprite = _reward.CurrencyData.Icon;
            _rewardAmountText.text = _reward.CurrencyValue.ToString();
        }
    }
}
