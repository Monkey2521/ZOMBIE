using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General.Inventories;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.General
{
    public sealed class MainInventory : Inventory
    {
        [Header("MainInventory settings")]
        [SerializeField] private bool _useAutoSave = true;
        [Tooltip("Interval in seconds")]
        [SerializeField][Range(10, 600)] private int _autoSaveInvterval = 60;
        [SerializeField] private List<Inventory> _inventories;

        float _saveTimer;

        private void OnDisable()
        {
            SaveData();
        }

        private void OnApplicationQuit()
        {
            SaveData();
        }

        private void LateUpdate()
        {
            if (!_useAutoSave) return;

            _saveTimer += Time.unscaledDeltaTime;

            if (_saveTimer >= _autoSaveInvterval)
            {
                SaveData();

                _saveTimer = 0;
            }
        }

        public override void Initialize(MainInventory mainInventory = null)
        {
            _saveTimer = 0;

            foreach(Inventory inventory in _inventories)
            {
                inventory.Initialize(this);
            }

            LoadData();
        }

        public override bool Add(ConcreteReward reward)
        {
            foreach (Inventory inventory in _inventories)
            {
                if (inventory.Add(reward))
                {
                    return true;
                }
            }
#if DEBUG
            if (_isDebug) Debug.Log(name + ": Not found inventory to add " + reward);
#endif
            return false;
        }

        public override bool Spend(ConcreteReward unreward)
        {
            foreach (Inventory inventory in _inventories)
            {
                if (inventory.Spend(unreward))
                {
                    return true;
                }
            }
#if DEBUG
            if (_isDebug) Debug.Log(name + ": Can't spend " + unreward);
#endif
            return false;
        }

        public override bool IsEnough(ConcreteReward reward)
        {
            foreach(Inventory inventory in _inventories)
            {
                if (inventory.IsEnough(reward))
                {
                    return true;
                }
            }

            return false;
        }

        public bool IsEnough(List<ConcreteReward> rewards)
        {
            foreach(ConcreteReward reward in rewards)
            {
                if (!IsEnough(reward))
                {
                    return false;
                }
            }

            return true;
        }

        public bool FindInventory(InventoryMarker inventoryMarker, out Inventory inventory)
        {
            inventory = _inventories.Find(item => item.Marker.Equals(inventoryMarker));
#if DEBUG
            if (inventory == null && _isDebug) Debug.Log(name + ": Inventory not found by marker " + inventoryMarker);
#endif
            return inventory != null;
        }

        public bool FindInventory(CurrencyData currencyData, out CurrencyInventory inventory)
        {
            inventory = _inventories.Find(item => item is CurrencyInventory ci &&
                                                                    ci.CurrencyData.Equals(currencyData)) as CurrencyInventory;
#if DEBUG
            if (inventory == null && _isDebug) Debug.Log(name + ": Inventory not found by currency " + currencyData);
#endif
            return inventory != null;
        }

        #region Serialization
#if DEBUG
        [ContextMenu("Save data")]
#endif
        public override bool SaveData()
        {
            bool saved = false;

            foreach(Inventory inventory in _inventories)
            {
                if (!saved && inventory.NeedSave)
                {
                    saved = inventory.SaveData();

                    continue;
                }

                if (inventory.NeedSave)
                {
                    inventory.SaveData();
                }
            }

            return saved;
        }

#if DEBUG
        [ContextMenu("Load data")]
#endif
        public override bool LoadData()
        {
            bool loaded = false;

            foreach (Inventory inventory in _inventories)
            {
                if (!loaded && inventory)
                {
                    loaded = inventory.LoadData();

                    continue;
                }

                inventory.LoadData();
            }

            return loaded;
        }

#if DEBUG
        [ContextMenu("Reset data")]
#endif
        public override void ResetData()
        {
            foreach(Inventory inventory in _inventories)
            {
                inventory.ResetData();
            }
        }

        protected override InventoryData GetData() => null;

        protected override bool UnpackData(InventoryData data) => false;
        #endregion
    }
}