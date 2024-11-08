using UnityEngine;

namespace ZombieSurvival.Abilities
{
    public class PassiveAbility : AbilityContainer
    {
        [Header("Passive ability settings")]
        [SerializeField] protected StatsAbilityUpgradeData _upgradeData;
        [SerializeField] protected AbilityStats _stats;

        public override AbilityUpgradeData UpgradeData => _upgradeData;
        public override AbilityStats Stats => _stats;
    }
}