using UnityEngine;

using ZombieSurvival.Characters;
using ZombieSurvival.General;
using ZombieSurvival.General.Sounds;

namespace ZombieSurvival
{
    namespace Abilities
    {
        public abstract class Weapon : ActiveAbility
        {
            [Header("Sounds settings")]
            [SerializeField] protected SoundList _sounds;

            [Header("Ability settings")]
            [SerializeField] protected bool _isSuperWeapon;
            [Tooltip("Field can be null if abiltity cant upgrade to SUPER")]
            [SerializeField] protected PassiveAbility _requiredAbilityToUpgradeToSuper;

            [Space(5)]
            [SerializeField] protected TargetDetector _targetDetector;
            [SerializeField] protected WeaponAbilityUpgradeData _abilityUpgradeData;

            public bool IsSuper => _isSuperWeapon;
            public PassiveAbility RequiredAbilityToUpgradeToSuper => _requiredAbilityToUpgradeToSuper;
            public override AbilityUpgradeData UpgradeData => _abilityUpgradeData;

            public override void Initialize(CharacterBase character)
            {
                base.Initialize(character);

                _isReady = true;

                _targetDetector.Initialize((Stats as WeaponAbilityStats).AttackRange);
            }

            public virtual void SetTargetTags(TagList tags)
            {
                _targetDetector.SetTargetTags(tags);
            }

            /// <summary>
            /// Attack target if ready based on self rules
            /// </summary>
            public virtual void Attack() // need override
            {
                if (_isDebug) Debug.Log(name + " attacks");
            }
        }
    }
}
