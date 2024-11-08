using UnityEngine;

using ZombieSurvival.Equipments;

namespace ZombieSurvival.Rewards
{
    public sealed class ConcreteEquipmentUpgradeReward : ConcreteReward
    {
        private Equipment _equipment;

        public Equipment UpgradedEquipment => _equipment;

        public ConcreteEquipmentUpgradeReward(Equipment equipment, Sprite background) : 
            base(equipment.Icon, background)
        {
            _equipment = equipment;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new ConcreteEquipmentUpgradeReward(_equipment, _background);
    }
}
