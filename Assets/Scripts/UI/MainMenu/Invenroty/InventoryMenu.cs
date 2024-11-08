using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.Abilities;
using ZombieSurvival.Characters;
using ZombieSurvival.Equipments;
using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;
using ZombieSurvival.Rewards;
using ZombieSurvival.Upgrades;

using Zenject;
using ZombieSurvival.Events;

namespace ZombieSurvival.UI.InventoryMenu
{
    public sealed class InventoryMenu : UIMenu
    {
        #region Properties
        #region SerializeFields
        [Header("InventoryMenu settings")]
        [SerializeField] private ItemUpgradeMenu _upgradeMenu;
        [SerializeField] private HeroListMenu _heroListMenu;

        [Header("Merge settings")]
        [SerializeField] private Animator _mergeAnimator;
        [SerializeField] private float _mergeResultShowDelay;

        [Header("Equipment settings")]
        [SerializeField] private EquipSlot _weaponSlot;

        [Space(5)]
        [SerializeField] private MarkerList _playerDamageMarkers;
        [SerializeField] private MarkerList _playerHealthMarkers;
        [SerializeField] private Text _damageText;
        [SerializeField] private Text _healthText;

        [Space(5)]
        [SerializeField] private LevelContextInstaller _contextInstaller;

        [Space(5)]
        [SerializeField] private List<EquipmentSlot> _equippedEquipmentSlots;

        [Space(5)]
        [SerializeField] private EquipmentSlot _equipmentSlotPrefab;
        [SerializeField] private float _additionalViewSize = 200f;
        [SerializeField] private Transform _unequippedEquipmentParent;
        [SerializeField] private RectTransform _unequippedInventoryTransform;
        [SerializeField] private GridLayoutGroup _unequippedInventoryGrid;

        [Header("Player animation settings")]
        [Space(5)]
        [SerializeField] private Animator _playerAnimator;
        private enum AnimationStates
        {
            WithBlade,
            WithShotgun,
            WithPistol,

            Girl
        }

        private void SetAnimatorBools()
        {
            _playerAnimator.SetBool(AnimationStates.Girl.ToString(), true); // TODO add more characters

            if (_equipmentInventory == null) return;

            WeaponEquipment weapon = _equippedEquipmentInventory[_weaponSlot] as WeaponEquipment;
            if (weapon != null)
            {
                if (weapon.BaseWeapon as Shotgun != null)
                {
                    _playerAnimator.SetBool(AnimationStates.WithShotgun.ToString(), true);
                }
                else if (weapon.BaseWeapon as Blade != null)
                {
                    _playerAnimator.SetBool(AnimationStates.WithBlade.ToString(), true);
                }
                else if (weapon.BaseWeapon as Pistol != null)
                {
                    _playerAnimator.SetBool(AnimationStates.WithPistol.ToString(), true);
                }
            }
            else
            {
                _playerAnimator.SetBool(AnimationStates.WithShotgun.ToString(), false);
                _playerAnimator.SetBool(AnimationStates.WithBlade.ToString(), false);
                _playerAnimator.SetBool(AnimationStates.WithPistol.ToString(), false);
            }
        }
        #endregion

        private EquippedEquipmentInventory _equippedEquipmentInventory;
        private UnequippedEquipmentInventory _unequippedEquipmentInventory;

        [Inject] private EquipmentTypesData _equipmentTypesData;
        [Inject] private MainInventory _mainInventory;
        [Inject] private PlayerLevelInventory _playerLevelInventory;
        [Inject] private CampInventory _campInventory;
        [Inject] private EquipmentInventory _equipmentInventory;

        [Header("Test")]
        [SerializeField] private Player _player;
        #endregion

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _upgradeMenu.Initialize(mainMenu, this);
            _heroListMenu.Initialize(mainMenu, this);

            _equippedEquipmentInventory = new EquippedEquipmentInventory();
            _unequippedEquipmentInventory = new UnequippedEquipmentInventory
                (
                    _equipmentSlotPrefab, 
                    _unequippedEquipmentParent, 
                    _equipmentTypesData,
                    this
                );

            foreach (EquipmentSlot slot in _equippedEquipmentSlots)
            {
                slot.Initialize(_equipmentTypesData);
            }

            SetAnimatorBools();
        }

        public override void Display(bool playAnimation = false)
        {
            EventBus.Publish<IOpenInventoryHandler>(handler => handler.OnOpenInventory());

            LoadEquipment();

            _upgradeMenu.Hide();
            _heroListMenu.Hide();

            base.Display(playAnimation);

            UpdateValues();
        }

        public override void Hide(bool playAnimation = false)
        {
            _upgradeMenu.Hide();
            _heroListMenu.Hide();

            base.Hide(playAnimation);
        }

        public void OnTutorialClick()
        {
            EquipmentSlot slot = _unequippedEquipmentInventory.FirstSlot;

            if (slot != null)
            {
                OnItemClick(slot);
            }
        }

        public void OnItemClick(EquipmentSlot slot)
        {
            if (slot.Equipment == null)
            {
                if (_isDebug) Debug.Log("Missing equipment");

                return;
            }

            _upgradeMenu.SetEquipment(slot.Equipment);
            _upgradeMenu.Display(true);
        }

        public void UpdateValues()
        {
            int totalDamage = (int)_player.LevelUpgrades.GetUpgrade((int)_playerLevelInventory.PlayerLevel.Value).DamageData.UpgradeValue;
            int totalHP = (int)_player.Stats.Health.MaxHP.BaseValue +
                          (int)_player.LevelUpgrades.GetUpgrade((int)_playerLevelInventory.PlayerLevel.Value).HealthData.UpgradeValue;

            UpgradeList damageUpgrades = new UpgradeList();
            UpgradeList healthUpgrades = new UpgradeList();

            #region Calculate equipment upgrades
            foreach (EquipmentSlot slot in _equippedEquipmentSlots)
            {
                if (slot.Equipment != null)
                {
                    slot.SetSlot(slot.Equipment);

                    foreach (UpgradeData data in slot.Equipment.EquipUpgrade.Upgrades)
                    {
                        if (data.UpgradingMarkers.IsStrike(_playerDamageMarkers))
                        {
                            damageUpgrades.Add(data);
                        }
                        else if (data.UpgradingMarkers.IsStrike(_playerHealthMarkers))
                        {
                            healthUpgrades.Add(data);
                        }
                    }

                    foreach (Upgrade upgrade in slot.Equipment.RarityUpgrades)
                    {
                        foreach (UpgradeData data in upgrade.Upgrades)
                        {
                            if (data.UpgradingMarkers.IsStrike(_playerDamageMarkers))
                            {
                                damageUpgrades.Add(data);
                            }
                            else if (data.UpgradingMarkers.IsStrike(_playerHealthMarkers))
                            {
                                healthUpgrades.Add(data);
                            }
                        }
                    }
                }
            }
            #endregion

            #region Calculate camp upgrades
            foreach (CampUpgrade campUpgrade in _campInventory.Upgrades)
            {
                if (campUpgrade.CurrentUpgrade == null) continue;

                foreach (UpgradeData data in campUpgrade.CurrentUpgrade.Upgrades)
                {
                    if (data.UpgradingMarkers.IsStrike(_playerDamageMarkers))
                    {
                        damageUpgrades.Add(data);
                    }
                    else if (data.UpgradingMarkers.IsStrike(_playerHealthMarkers))
                    {
                        healthUpgrades.Add(data);
                    }
                }
            }
            #endregion

            #region Calculate talents upgrades
            foreach (Talent campTalent in _campInventory.Talents)
            {
                if (!campTalent.Unlocked || campTalent.Upgrade == null) continue;

                foreach (UpgradeData data in campTalent.Upgrade.Upgrades)
                {
                    if (data.UpgradingMarkers.IsStrike(_playerDamageMarkers))
                    {
                        damageUpgrades.Add(data);
                    }
                    else if (data.UpgradingMarkers.IsStrike(_playerHealthMarkers))
                    {
                        healthUpgrades.Add(data);
                    }
                }
            }
            #endregion

            _damageText.text = ((int)((totalDamage + damageUpgrades.UpgradesValue) * damageUpgrades.UpgradesMultiplier)).ToString();
            _healthText.text = ((int)((totalHP + healthUpgrades.UpgradesValue) * healthUpgrades.UpgradesMultiplier)).ToString();

            _unequippedEquipmentInventory.UpdateInventory();
            _unequippedInventoryTransform.sizeDelta = new Vector2(0, GetInventoryHeight());
        }

        public void Equip(Equipment equipment, bool displayDefault = true)
        {
            if (equipment == null) return;

            EquipmentSlot slot = _equippedEquipmentSlots.Find(item => item.ValidSlot.Equals(equipment.EquipSlot));

            if (slot.Equipment != null)
            {
                Unequip(slot.Equipment);
            }

            _equipmentInventory.Equip(equipment);

            _equippedEquipmentInventory.Add(equipment);
            _unequippedEquipmentInventory.RemoveEquipment(equipment);
            slot.SetSlot(equipment);

            UpdateValues();
            SetAnimatorBools();
        }

        public void Unequip(Equipment equipment)
        {
            if (equipment == null) return;

            EquipmentSlot slot = _equippedEquipmentSlots.Find(item => item.Equipment != null && 
                                                                      item.Equipment.Equals(equipment));

            if (slot != null)
            {
                _unequippedEquipmentInventory.AddEquipment(equipment);
                _equippedEquipmentInventory.Remove(equipment);

                _equipmentInventory.Unequip(equipment);

                slot.SetSlot(null);

                UpdateValues();

                if (equipment.EquipSlot.Equals(_weaponSlot))
                {
                    SetAnimatorBools();
                }
            }
            else return;
        }

        public void RemoveEquipment(Equipment equipment)
        {
            if (equipment.IsEquipped)
            {
                Unequip(equipment);
            }

            _unequippedEquipmentInventory.RemoveEquipment(equipment);
        }

        public void OnMergeClick()
        {
            int index = 0;

            List<EquipmentData> results = new List<EquipmentData>();

            while (index < _equipmentInventory.Count)
            {
                if (_equipmentInventory[index].EquipmentData.NextRarityEquipment == null)
                {
                    index++;
                    continue;
                }

                List<Equipment> combines = _equipmentInventory.Equipment.FindAll(
                    item => item.EquipmentData.Equals(_equipmentInventory[index].EquipmentData));

                int requiredEquipmentAmount = _equipmentInventory[index].EquipmentData.NextRarityEquipment.EquipmentUpgrades.GetUpgrade(1).UpgradeMaterials.RequiredEquipmentAmount;

                if (combines != null && combines.Count >= requiredEquipmentAmount)
                {
                    int combinesCount = combines.Count / requiredEquipmentAmount;
                    EquipmentData mergeResult = combines[0].EquipmentData.MergeResult;

                    for (int i = 0; i < combines.Count / requiredEquipmentAmount * requiredEquipmentAmount; i++)
                    {
                        if (_mainInventory.Spend(new ConcreteEquipmentReward
                            (
                                _equipmentTypesData,
                                combines[i]
                            )))
                        {
                            RemoveEquipment(combines[i]);
                        }
                    }

                    index = 0;

                    for (int i = 0; i < combinesCount; i++)
                    {
                        results.Add(mergeResult);
                    }
                }
                else index++;
            }

            if (results.Count > 0)
            {
                StartCoroutine(WaitMerge(results));

                _mergeAnimator.SetTrigger("Merge");
            }
            else
            {
                _mainMenu.ShowPopupMessage("Nothing to merge!\nNeed at least 3 equals items to merge");
            }
        }

        private IEnumerator WaitMerge(List<EquipmentData> results)
        {
            yield return new WaitForSecondsRealtime(_mergeResultShowDelay);
            
            MainMenuDisplay();

            List<ConcreteReward> rewards = new List<ConcreteReward>();

            foreach (EquipmentData result in results)
            {
                rewards.Add(new NewEquipmentReward(_equipmentTypesData, result));
            }

            _mainMenu.ShowRewards(rewards);
            LoadEquipment();
        }

        public void OnHeroListClick()
        {
            _upgradeMenu.Hide();

            _heroListMenu.Display(true);
        }

        private void LoadEquipment()
        {
            foreach (Equipment equipment in _equipmentInventory.Equipment)
            {
                if (equipment.IsEquipped && !_equippedEquipmentInventory.Contains(equipment))
                {
                    Equip(equipment);
                }
                else if (!_unequippedEquipmentInventory.Contains(equipment) && !_equippedEquipmentInventory.Contains(equipment))
                {
                    _unequippedEquipmentInventory.AddEquipment(equipment);
                }
            }

            UpdateValues();
        }

        private int GetInventoryHeight()
        {
            int rows = _equipmentInventory.UnequippedEquipment.Count / 5 + 
                (_equipmentInventory.UnequippedEquipment.Count % 5 > 0 ? 1 : 0);

            return _unequippedInventoryGrid.padding.top + 
                   _unequippedInventoryGrid.padding.bottom + 
                   (int)_unequippedInventoryGrid.cellSize.y * rows + 
                   (int)_unequippedInventoryGrid.spacing.y * rows +
                   (int)(_additionalViewSize * ScreenScaler.MinDelta);
        }
    }
}