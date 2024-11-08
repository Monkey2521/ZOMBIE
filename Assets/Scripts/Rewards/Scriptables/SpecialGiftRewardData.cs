using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvival.Rewards
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Rewards/SpecialGift reward", fileName = "new SpecialGift reward data")]
    public class SpecialGiftRewardData : RewardData
    {
        [SerializeField] private List<CurrencyRewardData> _currencyRewards;
        [SerializeField] private ExpirienceRewardData _expirienceReward;

        public override List<ConcreteReward> GetConcreteRewards()
        {
            List<ConcreteReward> rewards = new List<ConcreteReward>();

            rewards.AddRange(_expirienceReward.GetConcreteRewards());

            foreach (CurrencyRewardData currencyData in _currencyRewards)
            {
                rewards.AddRange(currencyData.GetConcreteRewards());
            }

            return rewards;
        }
    }
}