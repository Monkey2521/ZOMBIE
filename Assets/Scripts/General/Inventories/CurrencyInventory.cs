using UnityEngine;

using ZombieSurvival.Rewards;
using ZombieSurvival.UI.Counters;

namespace ZombieSurvival.General.Inventories
{
    public class CurrencyInventory : Inventory
    {
        [Header("CurrencyInventory settings")]
        [SerializeField] protected CurrencyData _currencyData;
        [SerializeField] protected CurrencyCounter _counter;

        [SerializeField] protected Currency _baseValue;

        protected int _total;

        public CurrencyData CurrencyData => _currencyData;
        public int Total => _total;

        public override void Initialize(MainInventory mainInventory = null)
        {
            base.Initialize(mainInventory);

            _total = _baseValue.CurrencyValue;

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
                _total += currencyReward.Amount;
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Add currency. Added = " + reward.Amount + "; Total = " + _total);
#endif  
                OnInventoryChanged();

                return true;
            }
            else if (reward is CircleRouletteCurrencyReward rouletteReward && rouletteReward.Currency.CurrencyData.Equals(_currencyData))
            {
                _total += rouletteReward.Amount;
#if DEBUG
                if (_isDebug) Debug.Log(name + ": Add currency. Added = " + reward.Amount + "; Total = " + _total);
#endif  
                OnInventoryChanged();

                return true;
            }

            return false;
        }

        public override bool IsEnough(ConcreteReward reward)
        {
            if (reward is CurrencyReward currencyReward && currencyReward.Currency.CurrencyData.Equals(_currencyData))
            {
                return _total >= currencyReward.Amount;
            }

            return false;
        }

        public override bool Spend(ConcreteReward unreward)
        {
            if (unreward is CurrencyReward currencyUnreward && currencyUnreward.Currency.CurrencyData.Equals(_currencyData))
            {
                if (IsEnough(unreward))
                {
                    _total -= currencyUnreward.Amount;
#if DEBUG
                    if (_isDebug) Debug.Log(name + ": Spend currency. Spended = " + unreward.Amount + "; Total = " + _total);
#endif  
                    OnInventoryChanged();

                    return true;
                }

                return false;
            }

            return false;
        }

        protected override void OnInventoryChanged(bool needSave = true)
        {
            base.OnInventoryChanged(needSave);

            UpdateCounter();
        }

        protected virtual void UpdateCounter()
        {
            if (_counter != null)
            {
                _counter.UpdateCounter();
            }
        }

        #region Serialization
        protected override InventoryData GetData()
        {
            CurrencyInventoryData data = new CurrencyInventoryData();

            data.total = _total;

            return data;
        }

        protected override bool UnpackData(InventoryData data)
        {
            if (data == null) return false;

            if (data is CurrencyInventoryData currencyData)
            {
                _total = currencyData.total;

                UpdateCounter();

                return true;
            }

            return false;
        }

        public override void ResetData()
        {
            base.ResetData();

            _needSave = true;

            _total = _baseValue.CurrencyValue;

            UpdateCounter();
        }

        [System.Serializable]
        protected class CurrencyInventoryData : InventoryData
        {
            public int total;
        }
        #endregion
    }
}