using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvival.Upgrades
{
    [System.Serializable]
    public struct CurrentUpgrade
    {
        [SerializeField] private List<string> _descriptions;
        [SerializeField] private Upgrade _upgrade;
        [SerializeField] private int _requiredLevel;

        public List<string> Descriptions => _descriptions;
        public Upgrade Upgrade => _upgrade;
        public int RequiredLevel => _requiredLevel;
    }
}