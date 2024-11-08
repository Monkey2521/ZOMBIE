using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General;

namespace ZombieSurvival.Equipments
{
    [System.Serializable]
    public class EquipmentUpgrades
    {
        [SerializeField] private CurrencyData _requiredCurrency;
        [SerializeField] private EquipmentMaterialData _requiredMaterial;

        [SerializeField] private List<EquipmentUpgrade> _upgrades;
        public CurrencyData RequiredCurrency => _requiredCurrency;
        public EquipmentMaterialData RequiredMaterial => _requiredMaterial;
        public List<EquipmentUpgrade> Upgrades => _upgrades;

        public EquipmentUpgrade GetUpgrade(int level)
        {
            return _upgrades.Find(item => item.RequiredLevel == level);
        }
    }
}