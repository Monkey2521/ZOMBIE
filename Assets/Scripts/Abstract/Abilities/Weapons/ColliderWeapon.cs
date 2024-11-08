using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Events;
using ZombieSurvival.General.Enums;
using ZombieSurvival.Objects;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Abilities
{
    public abstract class ColliderWeapon : Weapon
    {
        [SerializeField] protected WeaponAbilityStats _stats;

        public override AbilityStats Stats => _stats;

        public override void Attack()
        {
            if (_isReady)
            {
                base.Attack();

                _sounds.PlaySound(SoundTypes.Shoot);

                List<GameObject> targets = _targetDetector.Targets;

                if (targets.Count == 0) return;

                for (int i = 0; i < targets.Count; i++)
                {
                    if (targets[i] == null) continue;

                        
                    if (targets[i].TryGetComponent(out DamageableObject target))
                    {
                        float makedDamage = target.TakeDamage(_stats.Damage.GetValue());

                        EventBus.Publish<IWeaponMakeDamageHandler>(handler => handler.OnWeaponMakeDamage(this, makedDamage));

                        if (target.Stats.Health.Value <= 0)
                        {
                            EventBus.Publish<IWeaponKillHandler>(handler => handler.OnWeaponKill(this));
                        }
                    }
                }

                _isReady = false;
                _attackIntervalTimer = _stats.Cooldown.Value;
            }
        }

        public override bool Upgrade(Upgrade upgrade)
        {
            _stats.GetUpgrade(upgrade);

            _targetDetector.UpdateRadius();

            return base.Upgrade(upgrade);
        }
    }
}
