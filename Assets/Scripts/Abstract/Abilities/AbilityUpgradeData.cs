using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Upgrades;

namespace ZombieSurvival
{
    namespace Abilities
    {
        public abstract class AbilityUpgradeData : ScriptableObject
        {
            [Tooltip("Upgrades for each ability level. Requires upgrade at level 0")]
            [SerializeField] protected List<CurrentUpgrade> _levelUpgrades;

            public List<CurrentUpgrade> Upgrades => _levelUpgrades;
        }
    }
}
