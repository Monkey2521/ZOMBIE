using UnityEngine;
using ZombieSurvival.Stats;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Abilities
{
    public class ActiveAbilityStats : AbilityStats
    {
        [SerializeField] protected Cooldown _cooldown;
        public Cooldown Cooldown => _cooldown;

        public override void Initialize()
        {
            base.Initialize();

            _cooldown.Initialize();
        }

        public override void GetUpgrade(Upgrade upgrade)
        {
            base.GetUpgrade(upgrade);

            _cooldown.Upgrade(upgrade);
        }

        public override void DispelUpgrade(Upgrade upgrade)
        {
            base.DispelUpgrade(upgrade);

            _cooldown.DispelUpgrade(upgrade);
        }
    }
}