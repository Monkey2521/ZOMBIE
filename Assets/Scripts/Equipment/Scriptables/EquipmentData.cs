using UnityEngine;

using ZombieSurvival.General.Enums;
using ZombieSurvival.Stats;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Equipments
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Equipment/Equipment data", fileName = "New equipment data")]
    public class EquipmentData : ScriptableObject
    {
        [Header("Debug settings")]
        [SerializeField] private bool _isDebug;

        [Header("Equipment settings")]
        [SerializeField] private int _id;
        [SerializeField] private Sprite _icon;
        [SerializeField] private Level _level;
        [SerializeField] private UpgradingStat _upgradingStat;
        [SerializeField] private EquipSlot _equipSlot;
        [SerializeField] private EquipmentRarity _rarity;

        [Space(5)]
        [SerializeField] private EquipmentUpgrades _equipmentUpgrades;

        [Header("Rarity settings")]
        [SerializeField] private bool _displayRarityDescription = true;
        [SerializeField] private string _rarityDescription;
        [SerializeField] private Upgrade _rarityUpgrade;
        [SerializeField] private EquipmentData _previousRarityEquipment;
        [SerializeField] private EquipmentData _nextRarityEquipment;

        public int ID => _id;
        public Sprite Icon => _icon;
        public Level Level => _level;
        public EquipSlot EquipSlot => _equipSlot;
        public EquipmentRarity EquipRarity => _rarity;
        public UpgradingStat UpgradingStat => _upgradingStat;
        public EquipmentUpgrades EquipmentUpgrades => _equipmentUpgrades;
        public EquipmentData MergeResult => _nextRarityEquipment;

        public bool DisplayRarityDescription => _displayRarityDescription;
        public string RarityDescription => _rarityDescription;
        public Upgrade RarityUpgrade => _rarityUpgrade;
        public EquipmentData PreviousRarityEquipment => _previousRarityEquipment;
        public EquipmentData NextRarityEquipment => _nextRarityEquipment;

        public void SetID(int id)
        {
            _id = id;
        }
    }
}