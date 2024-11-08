using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class PickUpRadius : Radius
    {
        public PickUpRadius(StatData statData, UpgradeList upgradeList = null, bool isDebug = false) : 
            base(statData, upgradeList, isDebug) { }
    }
}