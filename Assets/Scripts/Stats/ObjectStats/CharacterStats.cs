using UnityEngine;
using ZombieSurvival.Abilities;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Stats
{
    [System.Serializable]
    public class CharacterStats : DamageableObjectStats
    {
        [Header("Character stats")]
        [SerializeField] protected Weapon _baseWeapon;
        [SerializeField] protected MoveSpeed _velocity;

        public Weapon BaseWeapon => _baseWeapon;
        public MoveSpeed Velocity => _velocity;

        public override void Initialize()
        {
            base.Initialize();

            _velocity.Initialize();
        }

        public virtual void SetBaseWeapon(Weapon weapon)
        {
            if (weapon != null)
            {
                _baseWeapon = weapon;
            }
        }

        public override void GetUpgrade(Upgrade upgrade)
        {
            base.GetUpgrade(upgrade);
            _velocity.Upgrade(upgrade);
        }

        public override void DispelUpgrade(Upgrade upgrade)
        {
            base.DispelUpgrade(upgrade);
            _velocity.DispelUpgrade(upgrade);
        }
    }
}