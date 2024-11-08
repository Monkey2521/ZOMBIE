using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Equipments;
using ZombieSurvival.Events;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.General.Inventories
{
    public class EquipmentInventory : Inventory
    {
        [Header("EquipmentInventory settings")]
        [SerializeField] private Transform _parent;
        [SerializeField] private EquipmentList _equipmentList;
        [SerializeField] private bool _instantiateUnequippedEquipment = true;

        private List<Equipment> _equipment;

        public List<Equipment> EquippedEquipment => _equipment.FindAll(item => item.IsEquipped);
        public List<Equipment> UnequippedEquipment => _equipment.FindAll(item => !item.IsEquipped);
        public List<Equipment> Equipment => _equipment;
        public int Count => _equipment.Count;

        public Equipment this[int index]
        {
            get
            {
                if (index < 0 || index > Count - 1) return null;

                return _equipment[index];
            }
        }

        public override void Initialize(MainInventory mainInventory = null)
        {
            base.Initialize(mainInventory);

            _equipmentList.SetIDs();

            _equipment = new List<Equipment>();
        }

        public override bool Add(ConcreteReward reward)
        {
            if (reward is NewEquipmentReward equipmentReward)
            {
                if (equipmentReward.Amount < 1) return false; 

                for (int i = 0; i < equipmentReward.Amount; i++)
                {
                    Equipment newEquipment;

                    if (equipmentReward.EquipmentData is WeaponEquipmentData weaponData)
                    {
                        newEquipment = new WeaponEquipment(weaponData);
                    }
                    else
                    {
                        newEquipment = new Equipment(equipmentReward.EquipmentData);
                    }

                    EventBus.Publish<IGetEquipmentHandler>(handler => handler.OnGetEquipment(newEquipment));

                    _equipment.Add(newEquipment);
                }

#if DEBUG
                if (_isDebug) Debug.Log(name + ": Add new equipment = " + equipmentReward.EquipmentData.name + " x" + equipmentReward.Amount);
#endif

                OnInventoryChanged();

                return true;
            }
            else if (reward is ConcreteEquipmentUpgradeReward upgradeReward)
            {
                if (upgradeReward.Amount < 1 || !_equipment.Contains(upgradeReward.UpgradedEquipment)) return false;

                for (int i = 0; i < upgradeReward.Amount; i++)
                {
#if DEBUG
                    if (_isDebug) Debug.Log(name + ": Add level to " + upgradeReward.UpgradedEquipment);
#endif
                    upgradeReward.UpgradedEquipment.Level.LevelUp();
                }

                OnInventoryChanged();

                return true;
            }
            else if (reward is ConcreteEquipmentResetReward resetReward)
            {
                if (!_equipment.Contains(resetReward.Equipment)) return false;

#if DEBUG
                if (_isDebug) Debug.Log(name + ": Reset level " + resetReward.Equipment);
#endif

                resetReward.Equipment.Level.SetValue(1);

                OnInventoryChanged();

                return true;
            }

            return false;
        }

        public override bool Spend(ConcreteReward unreward)
        {
            if (unreward is NewEquipmentReward equipmentUnreward)
            {
                if (equipmentUnreward.Amount < 1) return false;

                else if (IsEnough(equipmentUnreward, out List<Equipment> removingEquipments))
                {
                    for (int i = removingEquipments.Count - 1; 
                            i > removingEquipments.Count - 1 - equipmentUnreward.Amount && i >= 0; i--)
                    {
#if DEBUG
                        if (_isDebug) Debug.Log(name + ": Spend equipment = " + removingEquipments[i]);
#endif

                        _equipment.Remove(removingEquipments[i]);
                    }

                    OnInventoryChanged();

                    return true;
                }

                return false;
            }
            else if (unreward is ConcreteEquipmentReward concreteEquipmentUnreward)
            {
                if (concreteEquipmentUnreward.Amount != 1) return false;

                else
                {
                    Equipment removingEquipment = _equipment.Find(item => item.Equals(concreteEquipmentUnreward.Equipment));

                    if (removingEquipment == null) return false;
#if DEBUG
                    if (_isDebug) Debug.Log(name + ": Spend concrete equipment = " + removingEquipment);
#endif
                    _equipment.Remove(removingEquipment);

                    OnInventoryChanged();

                    return true;
                }
            }

            return false;
        }

        public override bool IsEnough(ConcreteReward reward)
        {
            if (reward is NewEquipmentReward equipmentReward)
            {
                return _equipment.FindAll(item => 
                    item.EquipmentData.Equals(equipmentReward.EquipmentData)).Count >= reward.Amount;
            }
            else if (reward is ConcreteEquipmentReward concreteEquipmentReward)
            {
                return concreteEquipmentReward.Amount == 1 && 
                    _equipment.Find(item => item.Equals(concreteEquipmentReward.Equipment)) != null;
            }

            return false;
        }

        protected bool IsEnough(NewEquipmentReward equipmentReward, out List<Equipment> foundedEquipment)
        {
            if(equipmentReward.Amount < 1)
            {
                foundedEquipment = null;
                return false;
            }

            foundedEquipment = _equipment.FindAll(item => item.EquipmentData.Equals(equipmentReward.EquipmentData));

            return foundedEquipment.Count > equipmentReward.Amount;
        }

        public bool Equip(Equipment equipment)
        {
            if (_equipment.Contains(equipment))
            {
                if (equipment.Equip())
                {
                    OnInventoryChanged();

                    return true;
                }
            }

            return false;
        }

        public bool Unequip(Equipment equipment)
        {
            if (_equipment.Contains(equipment))
            {
                if (equipment.Unequip())
                {
                    OnInventoryChanged();

                    return true;
                }
            }

            return false;
        }

        #region Serialization
        protected override InventoryData GetData()
        {
            EquipmentInventoryData data = new EquipmentInventoryData();

            foreach (Equipment equipment in _equipment)
            {
                data.Add(equipment);
            }

            return data;
        }

        protected override bool UnpackData(InventoryData data)
        {
            if (data == null) return false;

            if (data is EquipmentInventoryData equipmentData)
            {
                for (int i = 0; i < equipmentData.equipments.Count; i++)
                {
                    Equipment equipment;

                    if (equipmentData.equipments[i].isWeapon)
                    {
                        equipment = new WeaponEquipment(_equipmentList[equipmentData[i].id]);
                    }
                    else
                    {
                        equipment = new Equipment(_equipmentList[equipmentData[i].id]);
                    }

                    equipment.Level.SetValue(equipmentData[i].level);
                    
                    if (equipmentData[i].isEquipped)
                    {
                        equipment.Equip();
                    }

                    _equipment.Add(equipment);
                }

                return true;
            }

            return false;
        }

        public override void ResetData()
        {
            base.ResetData();

            if (_equipment == null) return;

            _equipment = new List<Equipment>();
        }

        [System.Serializable]
        private class EquipmentInventoryData : InventoryData
        {
            public List<SingleEquipmentData> equipments;

            public EquipmentInventoryData()
            {
                equipments = new List<SingleEquipmentData>();
            }

            public SingleEquipmentData this[int index]
            {
                get
                {
                    if (index > equipments.Count)
                    {
                        return null;
                    }
                    else return equipments[index];
                }
            }

            public void Add(Equipment equipment)
            {
                SingleEquipmentData data = new SingleEquipmentData();

                data.id = equipment.ID;
                data.level = (int)equipment.Level.Value;
                data.isEquipped = equipment.IsEquipped;

                if (equipment is WeaponEquipment)
                {
                    data.isWeapon = true;
                }
                else
                {
                    data.isWeapon = false;
                }

                equipments.Add(data);
            }

            [System.Serializable]
            public class SingleEquipmentData
            {
                public int id;
                public int level;
                public bool isEquipped;
                public bool isWeapon;
            }
        }
        #endregion
    }
}