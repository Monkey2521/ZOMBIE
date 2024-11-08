using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Stats;

namespace ZombieSurvival.Rewards
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Rewards/Expirience reward data", fileName = "New expirience reward data")]
    public sealed class ExpirienceRewardData : RewardData
    {
        [SerializeField] private Sprite _icon;
        [SerializeField] private Sprite _background;
        [SerializeField] private Expirience _expirienceAmount;

        public Expirience ExpirienceAmount => _expirienceAmount;

        public override List<ConcreteReward> GetConcreteRewards()
        {
            List<ConcreteReward> reward = new List<ConcreteReward>();

            _expirienceAmount.Initialize();

            reward.Add(new ExpirienceReward(_expirienceAmount, _icon, _background));

            return reward;
        }
    }
}
