using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class MoveSpeed : Speed
    {
        public MoveSpeed(StatData statData, UpgradeList upgradeList = null, bool isDebug = false) : 
            base(statData, upgradeList, isDebug) { }
    }
}