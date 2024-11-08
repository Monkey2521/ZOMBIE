using UnityEngine;

using ZombieSurvival.Abilities;
using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class MakeDamageWithWeaponAchievement : Achievement, IWeaponMakeDamageHandler
    {
        [Header("MakeDamageWithWeapon achievement settings")]
        [SerializeField] private Weapon _requiredWeapon;
        [Tooltip("Enable/Disable counting by SUPER variant")]
        [SerializeField] private bool _countSuper;
        [Tooltip("Field can be NULL if CountSuper is false")]
        [SerializeField] private Weapon _superWeapon;
        public override string Name => _name + " " + _requiredWeapon.Name;

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnWeaponMakeDamage(Weapon weapon, float damage)
        {
            if (weapon.Name.Equals(_requiredWeapon.Name) ||
                (_countSuper && _superWeapon != null && weapon.Name.Equals(_superWeapon.Name)))
            {
                AddCounter((int)damage);
            }
        }
    }
}
