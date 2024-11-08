using UnityEngine;
using ZombieSurvival.General;

namespace ZombieSurvival.Equipments
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Equipment/Equipment material", fileName = "New equipment material")]
    public class EquipmentMaterialData : CurrencyData
    {
        [SerializeField] private EquipSlot _validEquipment;

        public EquipSlot ValidEquipment => _validEquipment;
    }
}