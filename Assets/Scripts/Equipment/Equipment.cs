using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General.Enums;
using ZombieSurvival.Stats;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Equipments
{
    [System.Serializable]
    public class Equipment
    {
        protected EquipmentData _equipmentData;
        protected Level _level;

        protected bool _isEquipped;

        public EquipmentData EquipmentData => _equipmentData;
        public Level Level => _level;

        public bool IsEquipped => _isEquipped;

        public int ID => _equipmentData.ID;

        public Sprite Icon => _equipmentData.Icon;
        public EquipSlot EquipSlot => _equipmentData.EquipSlot;
        public EquipmentRarity EquipRarity => _equipmentData.EquipRarity;
        public UpgradingStat UpgradingStat => _equipmentData.UpgradingStat;

        public Upgrade EquipUpgrade => new Upgrade(_equipmentData.EquipmentUpgrades.GetUpgrade((int)_level.Value).UpgradeData);
        public List<Upgrade> RarityUpgrades
        {
            get
            {
                List<Upgrade> upgrades = new List<Upgrade>();
                EquipmentData equipmentData = _equipmentData;

                while (equipmentData.PreviousRarityEquipment != null)
                {
                    upgrades.Add(equipmentData.RarityUpgrade);
                    equipmentData = equipmentData.PreviousRarityEquipment;
                }

                return upgrades;
            }
        }

        public EquipmentUpgrade CurrentUpgrade => _level.Value != _level.MaxValue ? _equipmentData.EquipmentUpgrades.GetUpgrade((int)_level.Value + 1) : null;


        public Equipment(EquipmentData data)
        {
            _equipmentData = data;

            _level = new Level(data.Level);

            _isEquipped = false;
        }

        public bool Equip()
        {
            if (_isEquipped) return false;

            _isEquipped = true;

            return true;
        }

        public bool Unequip()
        {
            if (!_isEquipped) return false;

            _isEquipped = false;

            return true;
        }
    }
}