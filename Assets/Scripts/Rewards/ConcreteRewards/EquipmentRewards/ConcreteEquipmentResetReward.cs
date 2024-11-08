using UnityEngine;

using ZombieSurvival.Equipments;

namespace ZombieSurvival.Rewards
{
    public sealed class ConcreteEquipmentResetReward : ConcreteReward
    {
        private Equipment _equipment;

        public Equipment Equipment => _equipment;

        public ConcreteEquipmentResetReward(Equipment equipment, Sprite background) :
            base(equipment.Icon, background)
        {
            _equipment = equipment;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new ConcreteEquipmentResetReward(_equipment, _background);
    }
}
