using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvival.Upgrades
{
    [System.Serializable]
    public class PlayerUpgrades
    {
        [SerializeField] private List<PlayerUpgrade> _upgrades;

        public PlayerUpgrade GetUpgrade(int level) => _upgrades.FindLast(item => item.RequiredLevel <= level);
    }
}