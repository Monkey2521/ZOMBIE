using UnityEngine;

using ZombieSurvival.Equipments;
using ZombieSurvival.Rewards;
using ZombieSurvival.UI.GameMenus;

namespace ZombieSurvival.Objects.Pickables
{
    public sealed class PickableEquipment : MonoPickableReward
    {
        [Header("Equipment settings")]
        [Tooltip("Reward must have only 1 concrete reward")]
        [SerializeField] private EquipmentRewardData _equipmentRewardData;
        [SerializeField] private EquipmentTypesData _equipmentTypesData;

        protected override RewardData RewardData => _equipmentRewardData;

        public override void Initialize(MonoPool<PickableObject> pool, GameMenu gameMenu)
        {
            base.Initialize(pool, gameMenu);

            if (_useHighlight)
            {
                if (_highlightParticle != null)
                {
                    if (_reward is NewEquipmentReward reward)
                    {
                        var main = _highlightParticle.main;

                        main.startColor = _equipmentTypesData[reward.EquipmentData.EquipRarity].RarityColor;
                    }
                    else if (_isDebug) Debug.Log(name + ": Missing equipment reward!");
                }
                else if (_isDebug) Debug.Log(name + ": Missing highlight particle!");
            }
        }
    }
}