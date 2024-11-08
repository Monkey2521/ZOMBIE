using UnityEngine;

using ZombieSurvival.Equipments;

namespace ZombieSurvival.Rewards
{
    public sealed class ConcreteEquipmentReward : ConcreteReward
    {
        private Equipment _equipment;

        public Equipment Equipment => _equipment;

        public ConcreteEquipmentReward(EquipmentTypesData typesData, Equipment equipment) : 
            base(equipment.Icon, typesData[equipment.EquipRarity].RarityBackground)
        {
            _equipment = equipment;
        }

        public ConcreteEquipmentReward(Equipment equipment, Sprite background) : 
            base(equipment.Icon, background)
        {
            _equipment = equipment;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new ConcreteEquipmentReward(_equipment, _background);
    }
}
