using UnityEngine;

using ZombieSurvival.Abilities;

namespace ZombieSurvival.Equipments
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Equipment/Weapon equipment data", fileName = "New weapon equipment data")]
    public class WeaponEquipmentData : EquipmentData
    {
        [Header("Weapon equipment settings")]
        [SerializeField] private Weapon _playerBaseWeaponOnEquip;

        public Weapon PlayerBaseWeaponOnEquip => _playerBaseWeaponOnEquip;
    }
}