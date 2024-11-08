using UnityEngine;
using ZombieSurvival.Characters;
using ZombieSurvival.Objects;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Abilities
{
    public class Aura : ColliderWeapon
    {
        [SerializeField] protected ParticleSystem _particle;

        public override void Initialize(CharacterBase character)
        {
            base.Initialize(character);

            _targetDetector.entered.AddListener(OnEntered);
        }

        protected virtual void OnEntered(GameObject obj)
        {
            DamageableObject target = obj.GetComponent<DamageableObject>();

            if (target != null)
            {
                target.TakeDamage(_stats.Damage.GetValue());
            }
        }

        public override void OnUpdate()
        {
            base.OnUpdate();

            transform.LookAt(transform.position + Vector3.forward);
        }

        public override bool Upgrade(Upgrade upgrade)
        {
            bool isLevelUp = base.Upgrade(upgrade);

            if (_particle != null)
            {
                _particle.transform.localScale = new Vector3(_stats.AttackRange.Value, _stats.AttackRange.Value, _stats.AttackRange.Value);
            }
            else
            {
                transform.localScale = new Vector3(_stats.AttackRange.Value, _stats.AttackRange.Value, _stats.AttackRange.Value);
            }

            return isLevelUp;
        }

        public override void DestroyAbility()
        {
            _targetDetector.entered.RemoveAllListeners();

            base.DestroyAbility();
        }
    }
}
