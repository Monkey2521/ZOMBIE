using UnityEngine;
using ZombieSurvival.General;

namespace ZombieSurvival.Equipments
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Equipment/Equipment slot", fileName = "New equipment slot")]
    public class EquipSlot : Marker
    {
        [SerializeField] private int _slotIndex;

        public int SlotIndex => _slotIndex;
    }
}