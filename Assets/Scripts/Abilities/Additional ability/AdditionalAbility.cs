using UnityEngine;

namespace ZombieSurvival.Abilities
{
    public class AdditionalAbility : AbilityContainer
    {
        [SerializeField] protected AbilityStats _stats;
        [SerializeField] protected StatsAbilityUpgradeData _upgradeData;

        public override AbilityStats Stats => _stats;
        public override AbilityUpgradeData UpgradeData => _upgradeData;
    }

    public class RewardAbility : AbilityContainer 
    {
        [SerializeField] protected RewardAbilityStats _stats;
        [SerializeField] protected RewardAbilityStatsData _upgradeData;

        public override AbilityStats Stats => _stats;
        public override AbilityUpgradeData UpgradeData => _upgradeData;
    }

    public class RewardAbilityStats : AbilityStats
    {

    }

    public class RewardAbilityStatsData : AbilityUpgradeData
    {

    }
}