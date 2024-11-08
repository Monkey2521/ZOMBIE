using System.Collections.Generic;
using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General.Enums;
using ZombieSurvival.Equipments;
using ZombieSurvival.General;
using ZombieSurvival.Upgrades;
using ZombieSurvival.Rewards;
using ZombieSurvival.General.Inventories;
using ZombieSurvival.Events;

using Zenject;

namespace ZombieSurvival.UI.InventoryMenu
{
    public class ItemUpgradeMenu : UIMenu
    {
        [Header("Upgrade menu settings")]
        [SerializeField] private Image _hideButton;
        [SerializeField] private ItemResetMenu _resetMenu;
        [SerializeField] private Sprite _damageIcon;
        [SerializeField] private Sprite _healthIcon;

        [Header("Equipment info menu")]
        [SerializeField] private Text _rarityText;
        [SerializeField] private Image _rarityBackground;
        [SerializeField] private Image _equipmentIcon;
        [SerializeField] private Image _equipmentTypeIcon;
        [SerializeField] private Image _equipmentUpgradeValueIcon;
        [SerializeField] private Text _equipmentUpgradeValueName;
        [SerializeField] private Text _equipmentUpgradeValue;
        [SerializeField] private Text _equipmentLevel;

        [Header("Quality skills menu")]
        [SerializeField] private EquipmentRarityInfo _rarityInfoPrefab;
        [SerializeField] private RectTransform _rarityInfoParent;
        [SerializeField] private EquipmentRarityList _rarities;
        private List<EquipmentRarityInfo> _rarityInfos;

        [Header("Upgrade settings")]
        [SerializeField] private Image _materialIcon;
        [SerializeField] private Text _materialCountText;

        [SerializeField] private Image _currencyIcon;
        [SerializeField] private Text _currencyCountText;

        [Space(5)]
        [SerializeField] private Text _maxLevelTootlipText;
        [SerializeField] private Text _equipUnequipButtonText;

        [SerializeField] private Button _upgradeButton;
        [SerializeField] private Button _quickUpgradeButton;

        private Equipment _equipment;

        [Inject] private EquipmentTypesData _equipmentTypesData;
        [Inject] private MainInventory _mainInventory;

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _rarityInfos = new List<EquipmentRarityInfo>();

            foreach (EquipmentRarity rarity in _rarities.EquipmentRarities)
            {
                EquipmentRarityInfo rarityInfo = Instantiate(_rarityInfoPrefab, _rarityInfoParent);

                rarityInfo.Initialize(rarity, _equipmentTypesData[rarity].RarityCircle);

                _rarityInfos.Add(rarityInfo);
            }

            _resetMenu.Initialize(mainMenu, this);
        }

        public override void Display(bool playAnimation = false)
        {
            EventBus.Publish<IOpenEquipmentInfoHandler>(handler => handler.OnOpenEquipmentInfo());

            _hideButton.raycastTarget = true;

            if (_equipment != null)
            {
                _resetMenu.Hide();

                base.Display(playAnimation);
            }
            else Hide();
        }

        public override void Hide(bool playAnimation = false)
        {
            _hideButton.raycastTarget = false;
            _resetMenu.Hide();

            base.Hide(playAnimation);
        }

        public void SetEquipment(Equipment equipment)
        {
            if (equipment == null) return;

            _equipment = equipment;

            #region Initialization
            if (_equipment.IsEquipped) _equipUnequipButtonText.text = "Unequip";
            else _equipUnequipButtonText.text = "Equip";

            _rarityText.text = _equipment.EquipRarity.ToString();
            _rarityBackground.sprite = _equipmentTypesData[_equipment.EquipRarity].RarityBackground;
            _equipmentIcon.sprite = _equipment.Icon;
            _equipmentTypeIcon.sprite = _equipmentTypesData[_equipment.EquipSlot].SlotIcon;

            _equipmentUpgradeValueIcon.sprite = _equipment.UpgradingStat.Equals(UpgradingStat.Health) ? _healthIcon : _damageIcon;
            #endregion

            #region Equipment values
            float value = 0;

            foreach (UpgradeData data in _equipment.EquipUpgrade.Upgrades)
            {
                value += data.UpgradeValue;
            }

            _equipmentUpgradeValueName.text = _equipment.UpgradingStat.Equals(UpgradingStat.Health) ? "Health" : "Damage";
            _equipmentUpgradeValue.text = ((int)value).ToString();

            _equipmentLevel.text = ((int)_equipment.Level.Value).ToString() + "/" + ((int)_equipment.Level.MaxValue).ToString();

            if (_equipment.Level.Value == _equipment.Level.MaxValue)
            {
                _upgradeButton.interactable = false;
                _quickUpgradeButton.interactable = false;

                _materialIcon.enabled = false;
                _materialCountText.enabled = false;
                _currencyIcon.enabled = false;
                _currencyCountText.enabled = false;

                _maxLevelTootlipText.enabled = true;
            }
            else
            {
                _upgradeButton.interactable = true;
                _quickUpgradeButton.interactable = true;

                _materialIcon.enabled = true;
                _materialCountText.enabled = true;
                _currencyIcon.enabled = true;
                _currencyCountText.enabled = true;

                _maxLevelTootlipText.enabled = false;

                _materialIcon.sprite = equipment.EquipmentData.EquipmentUpgrades.RequiredMaterial.Icon;

                if (_mainInventory.FindInventory(equipment.EquipmentData.EquipmentUpgrades.RequiredMaterial,
                                                 out CurrencyInventory materialInventory))
                {
                    _materialCountText.text = materialInventory.Total.ToString() +
                    "/" + equipment.CurrentUpgrade.UpgradeMaterials.RequiredMaterialAmount.ToString();
                }
                else
                {
                    _materialCountText.text = "0/" + equipment.CurrentUpgrade.UpgradeMaterials.RequiredMaterialAmount.ToString();
                }
                
                _currencyIcon.sprite = equipment.EquipmentData.EquipmentUpgrades.RequiredCurrency.Icon;
                
                if (_mainInventory.FindInventory(equipment.EquipmentData.EquipmentUpgrades.RequiredCurrency, 
                                                 out CurrencyInventory currencyInventory))
                {
                    _currencyCountText.text = currencyInventory.Total.ToString() +
                                        "/" + equipment.CurrentUpgrade.UpgradeMaterials.RequiredCurrencyAmount.ToString();
                }
                else
                {
                    _currencyCountText.text = "0/" + equipment.CurrentUpgrade.UpgradeMaterials.RequiredCurrencyAmount.ToString();
                }
            }
            #endregion

            #region Rarity upgrades
            EquipmentData currentData = equipment.EquipmentData;

            while (currentData.PreviousRarityEquipment != null) // find common equipment
            {
                currentData = currentData.PreviousRarityEquipment;
            }

            int index = 0;

            while (currentData != null)
            {
                if (currentData.DisplayRarityDescription)
                {
                    _rarityInfos[index].gameObject.SetActive(true);
                    _rarityInfos[index].SetInfo(currentData.RarityDescription, equipment.EquipRarity);
                }
                else
                {
                    _rarityInfos[index].gameObject.SetActive(false);
                }

                currentData = currentData.NextRarityEquipment;
                index++;
            }
            #endregion
        }

        public void OnResetClick()
        {
            base.Hide(true);

            _resetMenu.SetEquipment(_equipment);
            _resetMenu.Display(true);
        }

        public void OnEquipUnequipClick()
        {
            if (_equipment == null) return;

            if (_equipment.IsEquipped)
            {
                (_parentMenu as InventoryMenu).Unequip(_equipment);
                _equipUnequipButtonText.text = "Equip";
            }
            else
            {
                (_parentMenu as InventoryMenu).Equip(_equipment);
                _equipUnequipButtonText.text = "Unequip";
            }
        }

        public void OnUpgradeClick()
        {
            if (_equipment != null)
            {
                List<ConcreteReward> unrewards = GetEquipmentUpgradeMaterials(_equipment);

                if (_equipment.Level.Value != _equipment.Level.MaxValue &&
                    _mainInventory.IsEnough(unrewards))
                {
                    foreach(ConcreteReward unreward in unrewards)
                    {
                        _mainInventory.Spend(unreward);
                    }

                    _mainInventory.Add(new ConcreteEquipmentUpgradeReward
                        (
                            _equipment,
                            null
                        ));

                    SetEquipment(_equipment);

                    UpdateInventory();
                }
                else
                {
                    _mainMenu.ShowPopupMessage("Not enough resources!");
                }
            }
        }

        public void OnQuickUpgradeClick()
        {
            if (_equipment != null)
            {
                if (_mainInventory.IsEnough(GetEquipmentUpgradeMaterials(_equipment)))
                {
                    while (_equipment.Level.Value != _equipment.Level.MaxValue &&
                           _mainInventory.IsEnough(GetEquipmentUpgradeMaterials(_equipment)))
                    {
                        OnUpgradeClick();
                    }
                }
                else
                {
                    _mainMenu.ShowPopupMessage("Not enough resources!");
                }
            }
        }

        public void OnEquipmentDowngrade(Equipment equipment)
        {
            _equipment = null;

            (_parentMenu as InventoryMenu).RemoveEquipment(equipment);

            _mainInventory.Spend(new ConcreteEquipmentReward(_equipmentTypesData, equipment));

            StartCoroutine(WaitDisplay());
        }

        private IEnumerator WaitDisplay()
        {
            yield return new WaitForEndOfFrame();

            _parentMenu.Display();
        }

        public void UpdateInventory()
        {
            (_parentMenu as InventoryMenu).UpdateValues();

            if (_equipment != null)
            {
                SetEquipment(_equipment);
            }
        }

        private List<ConcreteReward> GetEquipmentUpgradeMaterials(Equipment equipment)
        {
            List<ConcreteReward> result = new List<ConcreteReward>();

            if (equipment.CurrentUpgrade.UpgradeMaterials.RequiredMaterialAmount > 0)
            {
                result.Add(new CurrencyReward
                        (
                            equipment.EquipmentData.EquipmentUpgrades.RequiredMaterial,
                            equipment.CurrentUpgrade.UpgradeMaterials.RequiredMaterialAmount
                        ));
            }

            if (equipment.CurrentUpgrade.UpgradeMaterials.RequiredCurrencyAmount > 0)
            {
                result.Add(new CurrencyReward
                        (
                            equipment.EquipmentData.EquipmentUpgrades.RequiredCurrency,
                            equipment.CurrentUpgrade.UpgradeMaterials.RequiredCurrencyAmount
                        ));
            }
            if (equipment.CurrentUpgrade.UpgradeMaterials.RequiredEquipmentAmount > 0)
            {
                result.Add(new NewEquipmentReward
                            (
                                _equipmentTypesData,
                                equipment.EquipmentData.PreviousRarityEquipment,
                                equipment.CurrentUpgrade.UpgradeMaterials.RequiredEquipmentAmount
                            ));
            }

            return result;
        }

        public void OnCloseMenu()
        {
            Hide(true);
        }
    }
}