using UnityEngine;
using ZombieSurvival.Interfaces;

using ZombieSurvival.Upgrades;

namespace ZombieSurvival.General.Inventories
{
    public abstract class UpgradeableInventory : ZSMonoBehaviour, IUpgradable
    {
        [Header("UpgradeableInventory settings")]
        [SerializeField] protected MarkerList _upgradeMarkers;

        private UpgradeList _upgrades;

        public UpgradeList Upgrades => _upgrades;

        public virtual void Initialize()
        {
            _upgrades = new UpgradeList();
        }

        public virtual bool Upgrade(Upgrade upgrade)
        {
            if (upgrade == null) return false;

            bool upgraded = false;

            foreach(UpgradeData data in upgrade.Upgrades)
            {
                if (_upgradeMarkers.IsStrike(data.UpgradingMarkers))
                {
                    _upgrades.Add(data);

                    OnGetUpgrade(data);

                    upgraded = true;
                }
            }

            return upgraded;
        }

        protected abstract void OnGetUpgrade(UpgradeData data);
    }
}