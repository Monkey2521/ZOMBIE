using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Interfaces
{
    public interface IUpgradable
    {
        public UpgradeList Upgrades { get; }

        public bool Upgrade(Upgrade upgrade);
    }
}