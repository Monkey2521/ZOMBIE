using UnityEngine;

using ZombieSurvival.Interfaces;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class DamageableObjectStats : IObjectStats
    {
        [Header("DamageableObject stats")]
        [SerializeField] protected Health _health;
        [SerializeField] protected Regeneration _regeneration;
        [SerializeField] protected DamageReceiver _damageReceiver;

        public Health Health => _health;
        public Regeneration Regeneration => _regeneration;
        public DamageReceiver DamageReceiver => _damageReceiver;


        public virtual void Initialize()
        {
            _health.Initialize();
            _regeneration.Initialize();
            _damageReceiver.Initialize();
        }

        public virtual void GetUpgrade(Upgrade upgrade)
        {
            _health.Upgrade(upgrade);
            _regeneration.Upgrade(upgrade);
            _damageReceiver.Upgrade(upgrade);
        }

        public virtual void DispelUpgrade(Upgrade upgrade)
        {
            _health.DispelUpgrade(upgrade);
            _regeneration.DispelUpgrade(upgrade);
            _damageReceiver.DispelUpgrade(upgrade);
        }
    }
}