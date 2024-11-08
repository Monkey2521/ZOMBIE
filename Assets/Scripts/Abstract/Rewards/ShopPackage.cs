using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;
using ZombieSurvival.Events;

using Zenject;

namespace ZombieSurvival.UI.Shop
{
    public abstract class ShopPackage : ZSMonoBehaviour, IInventoryUpdateHandler
    {
        [Header("Package settings")]
        [SerializeField] protected Currency _cost;
        [SerializeField] protected bool _displayInvontoryAmount;
        [SerializeField] protected char _inventoryAmountSeparator = '/';
        [SerializeField] protected Image _costCurrencyIconImage;
        [SerializeField] protected Text _costText;
        [SerializeField] protected Image _packageBackground;

        [Header("Multiplier settings")]
        [SerializeField] protected GameObject _rewardMultiplierPanel;
        [SerializeField] protected Text _rewardMultiplierText;
        [SerializeField][Range(1, 10)] protected int _rewardMultiplier = 1;
        [SerializeField] protected char _rewardMultiplierChar = 'x';
        [SerializeField] protected bool _haveMultiplierDescription;
        [SerializeField] protected string _rewardMultiplierDescription;

        [Inject] protected MainMenu _mainMenu;
        [Inject] protected MainInventory _mainInventory;

        protected CurrencyInventory _inventory;

        protected virtual void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        protected  virtual void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public virtual void Initialize()
        {
            if (_mainInventory.FindInventory(_cost.CurrencyData, out _inventory))
            {
                _costCurrencyIconImage.sprite = _cost.CurrencyData.Icon;

                if (_rewardMultiplier != 1)
                {
                    _rewardMultiplierPanel.SetActive(true);

                    string multiplierText = _rewardMultiplierChar.ToString() + _rewardMultiplier.ToString();

                    if (_haveMultiplierDescription)
                    {
                        multiplierText += " " + _rewardMultiplierDescription;
                    }

                    _rewardMultiplierText.text = multiplierText;
                }
                else
                {
                    _rewardMultiplierPanel.SetActive(false);
                }

                _packageBackground.pixelsPerUnitMultiplier = _packageBackground.pixelsPerUnitMultiplier / ScreenScaler.MinDelta;

                UpdatePackage();
            }
#if DEBUG
            else if (_isDebug) Debug.Log(name + ": missing inventory");
#endif
        }

        public abstract void OnBuyClick();

        public virtual void UpdatePackage()
        {
            if (_displayInvontoryAmount && _inventory != null)
            {
                _costText.text = (_inventory.Total + _inventoryAmountSeparator.ToString() + _cost.CurrencyValue).ToString();
            }
            else
            {
                _costText.text = _cost.CurrencyValue.ToString();
            }
        }

        public void OnInvontoryUpdate(Inventory inventory)
        {
            if (inventory.Equals(_inventory))
            {
                UpdatePackage();
            }
        }
    }
}