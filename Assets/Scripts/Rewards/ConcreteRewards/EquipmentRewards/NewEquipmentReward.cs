using UnityEngine;

using ZombieSurvival.Equipments;

namespace ZombieSurvival.Rewards
{
    public sealed class NewEquipmentReward : ConcreteReward
    {
        private EquipmentData _equipmentData;

        public EquipmentData EquipmentData => _equipmentData;

        public NewEquipmentReward(EquipmentTypesData typesData, EquipmentData equipmentData, int amount = 1) : 
            base(equipmentData.Icon, typesData[equipmentData.EquipRarity].RarityBackground, amount)
        {
            _equipmentData = equipmentData;
        }

        public NewEquipmentReward(EquipmentData equipmentData, Sprite background, int amount = 1) :
            base(equipmentData.Icon, background, amount)
        {
            _equipmentData = equipmentData;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            if (other is NewEquipmentReward compare)
            {
                return compare._equipmentData.Equals(_equipmentData);
            }

            return false;
        }

        public override ConcreteReward Clone() => new NewEquipmentReward(_equipmentData, _background, _amount);
    }
}
