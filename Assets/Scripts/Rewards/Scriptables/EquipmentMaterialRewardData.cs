using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Equipments;
using ZombieSurvival.General;

namespace ZombieSurvival.Rewards
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Rewards/Equipment material reward data", fileName = "New equipment material reward data")]
    public sealed class EquipmentMaterialRewardData : RewardData
    {
        [SerializeField] private ChanceCombiner<EquipmentMaterialData> _materialsChances;
        [SerializeField] private int _materialsAmount;

        public override List<ConcreteReward> GetConcreteRewards()
        {
            _materialsChances.Initialize();

            List<ConcreteReward> rewards = new List<ConcreteReward>();

            for (int i = 0; i < _materialsAmount; i++)
            {
                rewards.Add(new CurrencyReward(_materialsChances.GetStrikedObject()));
            }

            return rewards;
        }
    }
}
