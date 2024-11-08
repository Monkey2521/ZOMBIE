using System;

using UnityEngine;

using ZombieSurvival.Rewards;
using ZombieSurvival.Stats;

namespace ZombieSurvival.General.Inventories
{
    public class EnergyInventory : CurrencyInventory
    {
        [Header("EnergyInventory settings")]
        [SerializeField] private EnergyStat _energy;
        [SerializeField] private int _energyRestoreCooldown;
        [SerializeField] int _energyRestoreAmount;

        public DateTime lastGainedTime = default;
        public int EnergyRestoreCooldown => _energyRestoreCooldown;
        public EnergyStat Energy => _energy;

        public override void Initialize(MainInventory mainInventory = null)
        {
            base.Initialize(mainInventory);

            _energy.Initialize();
            _total = (int)_energy.Value;

            if (_counter != null)
            {
                _counter.Initialize(this);
            }

            UpdateCounter();
        }

        public override bool Add(ConcreteReward reward)
        {
            if (reward is CurrencyReward currencyReward && currencyReward.Currency.CurrencyData.Equals(_currencyData))
            {
                _energy.SetValue(_energy.Value + currencyReward.Amount);

                _total = (int)_energy.Value;

                if (_energy.Value >= _energy.MaxValue)
                {
                    lastGainedTime = default;
                }
                else
                {
                    lastGainedTime = DateTime.Now;
                }
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Add energy. Added = " + reward.Amount + "; Total = " + _total);
#endif  
                OnInventoryChanged();
            }

            return false;
        }

        public override bool Spend(ConcreteReward unreward)
        {
            if (unreward is CurrencyReward currencyUnreward && currencyUnreward.Currency.CurrencyData.Equals(_currencyData))
            {
                if (IsEnough(unreward))
                {
                    _energy.SetValue(_energy.Value - currencyUnreward.Amount);

                    _total = (int)_energy.Value;

                    OnInventoryChanged();
#if DEBUG
                    if (_isDebug) Debug.Log(name + ": Spend energy. Spended = " + unreward.Amount + "; Total = " + _total);
#endif  
                    return true;
                }

                return false;
            }

            return false;
        }

        public void Restore()
        {
#if DEBUG
            if (_isDebug) Debug.Log(name + ": Resotore energy");
#endif  
            Add(new CurrencyReward(_currencyData, _energyRestoreAmount));
        }

        #region Serialization
        protected override bool UnpackData(InventoryData data)
        {
            if (data == null) return false;

            if (data is EnergyData loadedData)
            {
                lastGainedTime = loadedData.lastGainedTime;

                if (lastGainedTime == default)
                {
                    _energy.SetValue(loadedData.total);
                    _total = (int)_energy.Value;

                    UpdateCounter();

                    return false;
                }

                DateTime now = DateTime.Now;

                int totalGained = (int)((now - lastGainedTime).TotalSeconds / _energyRestoreCooldown);
                lastGainedTime = lastGainedTime.AddSeconds(totalGained * _energyRestoreCooldown);

                _energy.SetValue(loadedData.total + totalGained);
                _total = (int)_energy.Value;

                UpdateCounter();

                return true;
            }

            _energy.SetValue(_baseValue.CurrencyValue);
            _total = (int)_energy.Value;
            lastGainedTime = default;

            UpdateCounter();

            if (_energy.Value < _energy.MaxValue)
            {
                lastGainedTime = DateTime.Now;
            }

            return false;
        }

        protected override InventoryData GetData()
        {
            EnergyData data = new EnergyData();

            _total = (int)_energy.Value;
            data.total = _total;
            data.lastGainedTime = lastGainedTime;

            return data;
        }

        public override void ResetData()
        {
            base.ResetData();

            _energy.Initialize();
        }

        [System.Serializable]
        private class EnergyData : CurrencyInventoryData
        {
            public DateTime lastGainedTime;
        }
        #endregion
    }
}