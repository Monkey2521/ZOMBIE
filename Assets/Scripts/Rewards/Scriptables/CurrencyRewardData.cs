using System.Collections.Generic;
using UnityEngine;
using ZombieSurvival.General;

namespace ZombieSurvival.Rewards
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Rewards/Currency reward data", fileName = "New currency reward data")]
    public sealed class CurrencyRewardData : RewardData
    {
        [SerializeField] private List<Currency> _currencyRewards;

        public override List<ConcreteReward> GetConcreteRewards()
        {
            List<ConcreteReward> rewards = new List<ConcreteReward>();

            foreach (Currency currency in _currencyRewards)
            {
                rewards.Add(new CurrencyReward(currency));
            }

            return rewards;
        }
    }
}
