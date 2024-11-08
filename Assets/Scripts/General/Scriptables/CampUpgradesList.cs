using System.Collections.Generic;

using UnityEngine;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.General
{
    [CreateAssetMenu(menuName = "ZombieSurvival/Camp/Upgrades list", fileName = "New upgrades list")]
    public class CampUpgradesList : ScriptableObject
    {
        [SerializeField] private List<CampUpgrade> _campUpgrades;

        public List<CampUpgrade> Upgrades => _campUpgrades;
    }
}