using UnityEngine;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class DamageResistance : Resistance
    {
        [Header("Damage resistance settings")]
        [SerializeField] private DamageMarker _resistancedDamageMarker;

        public DamageMarker ResistanceMarker => _resistancedDamageMarker;

        public DamageResistance(StatData statData, UpgradeList upgradeList = null, bool isDebug = false) : 
            base(statData, upgradeList, isDebug) { }

        public int CalculateReceivedDamage(int receivedDamage) => (int)(receivedDamage * _value);
    }
}