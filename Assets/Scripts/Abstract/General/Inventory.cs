using System.IO;

using UnityEngine;

using ZombieSurvival.Events;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.General
{
    public abstract class Inventory : ZSMonoBehaviour
    {
        [Header("Inventory settings")]
        [Tooltip("If false inventory will not be saved/loaded")]
        [SerializeField] protected bool _saveInventory = true;
        [Tooltip("if true inventory will be saved on each change")]
        [SerializeField] protected bool _alwaysSaveInventory;
        [Tooltip("Path in format: '{ NAME }.dat")]
        [SerializeField] protected string _dataPath = ".dat";
        [SerializeField] protected InventoryMarker _marker;

        protected bool _needSave;
        protected MainInventory _mainInventory;

        public delegate void OnChanged();

        public virtual bool NeedSave => _needSave;
        public virtual InventoryMarker Marker => _marker;

        public virtual void Initialize(MainInventory mainInventory = null)
        {
#if DEBUG
            if (_isDebug) Debug.Log(name + ": Initialized");
#endif
            _mainInventory = mainInventory;
        }

        public abstract bool Add(ConcreteReward reward);

        public abstract bool Spend(ConcreteReward unreward);

        public abstract bool IsEnough(ConcreteReward reward);

        protected virtual void OnInventoryChanged(bool needSave = true)
        {
            _needSave = needSave;

            EventBus.Publish<IInventoryUpdateHandler>(handler => handler.OnInvontoryUpdate(this));

            if (_alwaysSaveInventory && needSave)
            {
                SaveData();
            }
        }

        #region Serialization
        public virtual bool SaveData()
        {
            if (!NeedSave || !_saveInventory) return false;
#if DEBUG
            if (_isDebug) Debug.Log(name + ": Save data");
#endif
            if (GameData.Save(GameData.DefaultPath + _dataPath, GetData()))
            {
                _needSave = false;

                return true;
            }

            return false;
        }

        public virtual bool LoadData()
        {
            if (!_saveInventory) return false;

            if (GameData.Load(GameData.DefaultPath + _dataPath) is InventoryData data)
            {
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Load data");
#endif
                if (UnpackData(data))
                {
                    OnInventoryChanged(needSave: false);

                    return true;
                }

                return false;
            }
#if DEBUG
            if (_isDebug) Debug.Log(name + ": Nothing to load");
#endif
            return false;
        }

        protected abstract InventoryData GetData();

        protected abstract bool UnpackData(InventoryData data);

        public virtual void ResetData()
        {
            if (File.Exists(GameData.DefaultPath + _dataPath))
            {
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Reset data");
#endif
                File.Delete(GameData.DefaultPath + _dataPath);
            }
#if DEBUG
            else if (_isDebug) Debug.Log(name + ": Nothing to reset");
#endif
        }

        [System.Serializable]
        protected abstract class InventoryData : SerializableData { }
        #endregion
    }
}