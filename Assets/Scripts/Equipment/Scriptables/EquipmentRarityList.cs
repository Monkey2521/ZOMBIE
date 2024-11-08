using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvival.Equipments
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Equipment/Equipment rarity list", fileName = "New equipment rarity list")]
    public class EquipmentRarityList : ScriptableObject
    {
        [SerializeField] private List<EquipmentRarity> _equipmentRarities;

        public List<EquipmentRarity> EquipmentRarities => _equipmentRarities;
        public EquipmentRarity LowestRarity => _equipmentRarities[0];
        public EquipmentRarity HighestRarity => _equipmentRarities[_equipmentRarities.Count - 1];
    }
}