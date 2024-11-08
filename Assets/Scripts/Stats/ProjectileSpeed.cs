using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class ProjectileSpeed : Speed
    {
        public ProjectileSpeed(StatData statData, UpgradeList upgradeList = null, bool isDebug = false) : 
            base(statData, upgradeList, isDebug) { }
    }
}