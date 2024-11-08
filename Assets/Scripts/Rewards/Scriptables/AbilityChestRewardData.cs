using System.Collections.Generic;
using UnityEngine;
using ZombieSurvival.Objects.Pickables;

namespace ZombieSurvival.Rewards
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Rewards/ZombieChest reward data", fileName = "New ZombieChest reward data")]
    public sealed class AbilityChestRewardData : RewardData
    {
        [SerializeField] private AbilityChest _zombieChest;
        [SerializeField] private int _maxAbilitiesReward;

        public override List<ConcreteReward> GetConcreteRewards()
        {
            List<ConcreteReward> rewards = new List<ConcreteReward>();

            rewards.Add(new ZombieChestReward(_zombieChest, _maxAbilitiesReward));

            return rewards;
        }
    }
}
