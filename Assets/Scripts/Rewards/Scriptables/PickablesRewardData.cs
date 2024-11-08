using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Objects;

namespace ZombieSurvival.Rewards
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Rewards/Pickables reward data", fileName = "New pickables reward data")]
    public sealed class PickablesRewardData : RewardData
    {
        [SerializeField] private List<PickableObject> _pickablesRewards;

        public override List<ConcreteReward> GetConcreteRewards()
        {
            List<ConcreteReward> rewards = new List<ConcreteReward>();

            foreach (PickableObject pickable in _pickablesRewards)
            {
                rewards.Add(new PickableReward(pickable));
            }

            return rewards;
        }
    }
}
