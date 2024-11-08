using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;

namespace ZombieSurvival.UI.Counters
{
    public class CurrencyCounter : ZSMonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected Image _currencyImage;
        [SerializeField] protected Text _currencyText;

        protected CurrencyInventory _inventory;

        public virtual void Initialize(CurrencyInventory currencyInventory)
        {
            _inventory = currencyInventory;

            if (currencyInventory.CurrencyData.Icon != null)
            {
                _currencyImage.sprite = currencyInventory.CurrencyData.Icon;
            }

            UpdateCounter();
        }

        public virtual void UpdateCounter()
        {
            if (_inventory != null)
            {
                _currencyText.text = IntegerFormatter.GetCurrency(_inventory.Total);
            }
            else if (_isDebug) Debug.Log("Missing inventory!");
        }
    }
}