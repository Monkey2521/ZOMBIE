using System;
using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Rewards;
using ZombieSurvival.UI;

using Zenject;

namespace ZombieSurvival.General.Inventories
{
    public sealed class TripForSuppliesInventory : Inventory
    {
        [Header("TripForSupplies inventory settings")]
        [SerializeField] private int _tripTimeLimit;

        private DateTime _lastRewardTime;
        private DateTime _lastMaterialRewardTime;
        private DateTime _lastEquipmentRewardTime;

        public DateTime LastRewardTime => _lastRewardTime;
        public DateTime LastMaterialRewardTime => _lastMaterialRewardTime;
        public DateTime LastEquipmentRewardTime => _lastEquipmentRewardTime;

        public int TripTimeLimit => _tripTimeLimit;

        [Inject] private MainMenu _mainMenu;

        public override bool Add(ConcreteReward reward)
        {
            if (reward is TripForSuppliesReward tripReward)
            {
                List<ConcreteReward> rewards = new List<ConcreteReward>();

                DateTime currentTime;
                DateTime maxTime;
                DateTime now = DateTime.Now;

                #region Gold & Exp
                currentTime = _lastRewardTime.AddSeconds(tripReward.RewardData.TickTime);
                maxTime = _lastRewardTime.AddSeconds(_tripTimeLimit);

                while (currentTime <= maxTime && currentTime <= now)
                {
                    rewards.AddRange(tripReward.RewardData.GoldPerTick);
                    rewards.AddRange(tripReward.RewardData.ExpPerTick);

                    _lastRewardTime = currentTime;

                    currentTime = currentTime.AddSeconds(tripReward.RewardData.TickTime);
                }

                if (maxTime < now)
                {
                    _lastRewardTime = now;
                }
                #endregion

                #region EquipmentMaterials
                if (tripReward.RewardData.MaterialReward != null && tripReward.RewardData.MaterialReward.Count > 0)
                {
                    currentTime = _lastMaterialRewardTime.AddSeconds(tripReward.RewardData.RequiredTimeForMaterial);
                    maxTime = _lastMaterialRewardTime.AddSeconds(_tripTimeLimit);

                    while (currentTime <= maxTime && currentTime <= now)
                    {
                        rewards.AddRange(tripReward.RewardData.MaterialReward);

                        _lastMaterialRewardTime = currentTime;

                        currentTime = currentTime.AddSeconds(tripReward.RewardData.RequiredTimeForMaterial);
                    }

                    if (maxTime < now)
                    {
                        _lastMaterialRewardTime = now;
                    }
                }
                #endregion

                #region Equipment
                if (tripReward.RewardData.EquipmentReward != null && tripReward.RewardData.EquipmentReward.Count > 0)
                {
                    currentTime = _lastEquipmentRewardTime.AddSeconds(tripReward.RewardData.RequiredTimeForEquipment);
                    maxTime = _lastEquipmentRewardTime.AddSeconds(_tripTimeLimit);

                    while (currentTime <= maxTime && currentTime <= now)
                    {
                        rewards.AddRange(tripReward.RewardData.EquipmentReward);

                        _lastEquipmentRewardTime = currentTime;

                        currentTime = currentTime.AddSeconds(tripReward.RewardData.RequiredTimeForEquipment);
                    }

                    if (maxTime < now)
                    {
                        _lastEquipmentRewardTime = now;
                    }
                }
                #endregion

                if (rewards.Count > 0)
                {
                    OnInventoryChanged();

                    _mainMenu.ShowRewards(rewards);

                    return true;
                }
                else
                {
                    _mainMenu.ShowPopupMessage("Hero already in trip");
                }
            }

            return false;
        }

        public override bool IsEnough(ConcreteReward reward) => false;

        public override bool Spend(ConcreteReward unreward) => false;

        #region Serialization
        protected override InventoryData GetData()
        {
            TripData data = new TripData();

            data.lastRewardTime = _lastRewardTime;
            data.lastMaterialRewardTime = _lastMaterialRewardTime;
            data.lastEquipmentRewardTime = _lastEquipmentRewardTime;

            return data;
        }

        public override bool LoadData()
        {
            if (!base.LoadData())
            {
                _lastRewardTime = DateTime.Now;
                _lastMaterialRewardTime = DateTime.Now;
                _lastEquipmentRewardTime = DateTime.Now;

                OnInventoryChanged();

                return false;
            }

            return true;
        }

        protected override bool UnpackData(InventoryData data)
        {
            if (data is TripData tripData) 
            {
                _lastRewardTime = tripData.lastRewardTime;
                _lastMaterialRewardTime = tripData.lastMaterialRewardTime;
                _lastEquipmentRewardTime = tripData.lastEquipmentRewardTime;

                return true;
            }

            return false;
        }

        [Serializable]
        private class TripData : InventoryData
        {
            public DateTime lastRewardTime;
            public DateTime lastMaterialRewardTime;
            public DateTime lastEquipmentRewardTime;
        }
        #endregion
    }
}