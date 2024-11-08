using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Levels;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.General.Inventories
{
    public class LevelInventory : Inventory
    {
        [Header("LevelsInventory settings")]
        [SerializeField] private List<LevelContext> _levels;

        public List<LevelContext> Levels => _levels;

        public override bool NeedSave => true;

        public override bool Add(ConcreteReward reward)
        {
            if (reward is LevelBreakpointReward breakpointReward)
            {
                LevelContext level = _levels.Find(item => item.LevelRewards.Breakpoints.Contains(breakpointReward.Breakpoint));

                if (level != null)
                {
                    if (level.LevelRewards.Claim(breakpointReward.Breakpoint))
                    {
#if DEBUG
                        if (_isDebug) Debug.Log(name + ": Claim breakpoint reward = " + breakpointReward.Breakpoint);
#endif
                        OnInventoryChanged();

                        return true;
                    }
                }
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Missing level with breakpoint = " + breakpointReward.Breakpoint);
#endif
            }
            else if (reward is LevelSurvivalTimeReward survivalTimeReward)
            {
                LevelContext level = _levels.Find(item => item.Equals(survivalTimeReward.Context));

                if (level != null)
                {
                    level.maxSurvivalTime = survivalTimeReward.SurvivalTime;
#if DEBUG
                    if (_isDebug) Debug.Log(name + ": " + level + " update survival time = " + survivalTimeReward.SurvivalTime);
#endif
                    OnInventoryChanged();
                }
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Missing level = " + survivalTimeReward.Context);
#endif
            }

            return false;
        }

        public override bool IsEnough(ConcreteReward reward) => false;

        public override bool Spend(ConcreteReward unreward) => false;

        #region Serialization
        protected override InventoryData GetData()
        {
            LevelsInventoryData data = new LevelsInventoryData();

            foreach (LevelContext level in _levels)
            {
                data.Add(level.SaveData());
            }

            return data;
        }

        public override bool LoadData()
        {
            if (!base.LoadData())
            {
                ResetData();

                _needSave = true;

                return false;
            }

            return true;
        }

        protected override bool UnpackData(InventoryData data)
        {
            if (data == null) return false;

            if (data is LevelsInventoryData levelsData)
            {
                for (int i = 0; i < levelsData.levelsData.Count; i++)
                {
                    _levels[i].LoadData(levelsData.levelsData[i]);
                }

                return true;
            }

            return false;
        }

        public override void ResetData()
        {
            base.ResetData();

            foreach(LevelContext level in _levels)
            {
                level.ResetLevel();
            }
        }

        [System.Serializable]
        private class LevelsInventoryData : InventoryData
        {
            public List<SerializableData> levelsData;

            public LevelsInventoryData()
            {
                levelsData = new List<SerializableData>();
            }

            public void Add(SerializableData data)
            {
                levelsData.Add(data);
            }
        }
        #endregion
    }
}