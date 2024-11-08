using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvival.Rewards
{
    public abstract class RewardData : ScriptableObject
    {
        public abstract List<ConcreteReward> GetConcreteRewards();
    }
}
