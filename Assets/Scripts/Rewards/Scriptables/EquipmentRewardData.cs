using System.Collections.Generic;
using UnityEngine;
using ZombieSurvival.Equipments;
using ZombieSurvival.General;

namespace ZombieSurvival.Rewards
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Rewards/Equipment reward data", fileName = "New equipment reward data")]
    public sealed class EquipmentRewardData : RewardData
    {
        [SerializeField] private EquipmentList _equipmentList;
        [SerializeField] private EquipmentTypesData _equipmentTypes;
        [SerializeField] private ChanceCombiner<EquipmentRarity> _equipmentChances;
        [SerializeField] private int _equipmentAmount;

        public ChanceCombiner<EquipmentRarity> ReawardChances => _equipmentChances;

        public override List<ConcreteReward> GetConcreteRewards()
        {
            _equipmentChances.Initialize();

            List<ConcreteReward> rewards = new List<ConcreteReward>();

            for (int i = 0; i < _equipmentAmount; i++)
            {
                rewards.Add(new NewEquipmentReward(_equipmentTypes, _equipmentList.GetRandomEquipment(_equipmentChances.GetStrikedObject())));
            }

            return rewards;
        }
    }
}
