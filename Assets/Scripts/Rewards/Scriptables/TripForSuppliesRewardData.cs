using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvival.Rewards
{
    [System.Serializable]
    public class TripForSuppliesRewardData
    {
        [SerializeField] private int _requiredLevel;
        [Tooltip("Time in seconds")]
        [SerializeField] private int _tickTime;

        [Space(5)]
        [SerializeField] private CurrencyRewardData _goldPerTick;
        [SerializeField] private ExpirienceRewardData _expPerTick;

        [Space(5)]
        [SerializeField] private EquipmentMaterialRewardData _materialReward;
        [Tooltip("Time in seconds")]
        [SerializeField] private int _requiredTimeForMaterial;

        [Space(5)]
        [SerializeField] private EquipmentRewardData _equipmentReward;
        [Tooltip("Time in seconds")]
        [SerializeField] private int _requiredTimeForEquipment;

        public int RequiredLevel => _requiredLevel;
        public int TickTime => _tickTime;

        public List<CurrencyReward> GoldPerTick
        {
            get
            {
                List<ConcreteReward> concreteRewards = _goldPerTick.GetConcreteRewards();

                if (concreteRewards.Count == 0) return null;

                List<CurrencyReward> rewards = new List<CurrencyReward>();

                foreach (ConcreteReward concreteReward in concreteRewards)
                {
                    if (concreteReward is CurrencyReward currencyReward)
                    {
                        rewards.Add(currencyReward);
                    }
                }

                return rewards;
            }
        }
        public List<ExpirienceReward> ExpPerTick
        {
            get
            {
                List<ConcreteReward> concreteRewards = _expPerTick.GetConcreteRewards();

                if (concreteRewards.Count == 0) return null;

                List<ExpirienceReward> rewards = new List<ExpirienceReward>();

                foreach (ConcreteReward concreteReward in concreteRewards)
                {
                    if (concreteReward is ExpirienceReward expirienceRewards)
                    {
                        rewards.Add(expirienceRewards);
                    }
                }

                return rewards;
            }
        }

        public List<CurrencyReward> MaterialReward
        {
            get
            {
                List<ConcreteReward> concreteRewards = _materialReward.GetConcreteRewards();

                if (concreteRewards.Count == 0) return null;

                List<CurrencyReward> rewards = new List<CurrencyReward>();

                foreach (ConcreteReward concreteReward in concreteRewards)
                {
                    if (concreteReward is CurrencyReward materialReward)
                    {
                        rewards.Add(materialReward);
                    }
                }

                return rewards;
            }
        }
        public int RequiredTimeForMaterial => _requiredTimeForMaterial;

        public List<NewEquipmentReward> EquipmentReward
        {
            get
            {
                List<ConcreteReward> concreteRewards = _equipmentReward.GetConcreteRewards();

                if (concreteRewards.Count == 0) return null;

                List<NewEquipmentReward> rewards = new List<NewEquipmentReward>();

                foreach (ConcreteReward concreteReward in concreteRewards)
                {
                    if (concreteReward is NewEquipmentReward equipmentReward)
                    {
                        rewards.Add(equipmentReward);
                    }
                }

                return rewards;
            }
        }

        public int RequiredTimeForEquipment => _requiredTimeForEquipment;
    }
}