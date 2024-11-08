using System;
using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvival.Rewards
{
    [Serializable]
    public class Reward
    {
        [SerializeField] private List<RewardData> _rewards;

        public List<ConcreteReward> Rewards
        {
            get
            {
                List<ConcreteReward> rewards = new List<ConcreteReward>();

                foreach(RewardData data in _rewards)
                {
                    List<ConcreteReward> currentRewards = data.GetConcreteRewards();

                    if (currentRewards != null && currentRewards.Count > 0)
                    {
                        rewards.AddRange(currentRewards);
                    }
                }

                return rewards;
            }
        }
    }
}
