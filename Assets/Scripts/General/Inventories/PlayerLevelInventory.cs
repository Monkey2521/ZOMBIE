using UnityEngine;

using ZombieSurvival.Rewards;
using ZombieSurvival.Stats;

namespace ZombieSurvival.General
{
    public sealed class PlayerLevelInventory : Inventory
    {
        [Header("PlayerLevelInventory settings")]
        [SerializeField] private PlayerExpLevel _playerLevel;

        public PlayerExpLevel PlayerLevel => _playerLevel;

        public override void Initialize(MainInventory mainInventory = null)
        {
            base.Initialize(mainInventory);

            _playerLevel.Initialize();
        }

        public override bool Add(ConcreteReward reward)
        {
            if (reward is ExpirienceReward expReward)
            {
                _playerLevel.AddExp(expReward.Expirience);
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Add expirience: = " + expReward.Amount + "; Total = " + _playerLevel.TotalExp);
#endif
                OnInventoryChanged();

                return true;
            }
            
            return false;
        }

        public override bool IsEnough(ConcreteReward reward)
        {
            if (reward is ExpirienceReward expReward)
            {
                return _playerLevel.TotalExp >= expReward.Expirience.Value;
            }

            return false;
        }

        public override void ResetData()
        {
            base.ResetData();

            _playerLevel.ResetData();
        }

        public override bool Spend(ConcreteReward unreward) => false;

        #region Serialization
        protected override InventoryData GetData()
        {
            PlayerLevelData data = new PlayerLevelData();

            data.levelData = _playerLevel.SaveData();

            return data;
        }

        protected override bool UnpackData(InventoryData data)
        {
            if (data is PlayerLevelData levelData)
            {
                return _playerLevel.LoadData(levelData.levelData);
            }
            return false;
        }

        [System.Serializable]
        private class PlayerLevelData : InventoryData
        {
            public SerializableData levelData;
        }
        #endregion
    }
}