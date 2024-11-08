using UnityEngine;
using ZombieSurvival.Stats;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Abilities
{
    [System.Serializable]
    public class WeaponAbilityStats : ActiveAbilityStats
    {
        [SerializeField] protected Damage _damage;
        [SerializeField] protected Radius _attackRange;

        public Damage Damage => _damage;
        public Radius AttackRange => _attackRange;

        public override void Initialize()
        {
            base.Initialize();

            _damage.Initialize();
            _attackRange.Initialize();
        }

        public override void GetUpgrade(Upgrade upgrade)
        {
            base.GetUpgrade(upgrade);

            _damage.Upgrade(upgrade);
            _attackRange.Upgrade(upgrade);
        }

        public override void DispelUpgrade(Upgrade upgrade)
        {
            base.DispelUpgrade(upgrade);

            _damage.DispelUpgrade(upgrade);
            _attackRange.DispelUpgrade(upgrade);
        }
    }
}