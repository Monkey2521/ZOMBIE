using UnityEngine;

using ZombieSurvival.Rewards;

namespace ZombieSurvival.Objects.Pickables
{
    public sealed class PickableEquipmentMaterial : MonoPickableReward
    {
        [Header("Equipment material settings")]
        [Tooltip("Reward must have only 1 concrete reward")]
        [SerializeField] private EquipmentMaterialRewardData _equipmentMaterialRewardData;

        protected override RewardData RewardData => _equipmentMaterialRewardData;
    }
}