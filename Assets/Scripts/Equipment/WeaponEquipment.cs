using UnityEngine;
using ZombieSurvival.Abilities;

namespace ZombieSurvival.Equipments
{
    public class WeaponEquipment : Equipment
    {
        private Weapon _playerBaseWeaponOnEquip;

        public WeaponEquipment(EquipmentData data) : base(data)
        {
            if (data is WeaponEquipmentData weaponData) 
            {
                _playerBaseWeaponOnEquip = weaponData.PlayerBaseWeaponOnEquip;
            }
        }

        public Weapon BaseWeapon => _playerBaseWeaponOnEquip;
    }
}