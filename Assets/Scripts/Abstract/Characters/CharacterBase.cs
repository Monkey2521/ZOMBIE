using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Interfaces;
using ZombieSurvival.Abilities;
using ZombieSurvival.Upgrades;
using ZombieSurvival.Objects;
using ZombieSurvival.UI;

namespace ZombieSurvival.Characters
{
    public abstract class CharacterBase : DamageableObject, IUpdatable
    {
        [Tooltip("Can be null if HealthBar is null")]
        [SerializeField] protected HPBarCanvas _hpCanvas;

        [Header("Render settings")]
        [SerializeField] protected SpriteRenderer _renderer;
        [SerializeField] protected Vector3 _defaultViewSide = Vector3.right;

        [Header("Abilities settings")]
        [SerializeField] protected AbilityInventory _abilityInventory;

        protected List<Upgrade> _upgrades;

        public AbilityInventory AbilityInventory => _abilityInventory;

        /// <summary>
        /// Heal character based on fixedDeltaTime and regeneration value.
        /// Call OnFixedUpdate for each projectile ability in inventory
        /// </summary>
        public override void OnFixedUpdate()
        {
            if (_onDie) return;

            base.OnFixedUpdate();

            foreach (ProjectileWeapon weapon in _abilityInventory.ProjectileWeapons)
            {
                weapon.OnFixedUpdate();
            }
        }

        /// <summary>
        /// Character attacks with all Weapons it has
        /// </summary>
        public virtual void OnUpdate()
        {
            if (_onDie) return;

            Attack();
        }

        /// <summary>
        /// Moving with current speed in direction
        /// </summary>
        /// <param name="direction">Normalized vector</param>
        public abstract void Move(Vector3 direction);

        protected virtual void Attack()
        {
            foreach (Weapon weapon in _abilityInventory.Weapons)
            {
                weapon.OnUpdate();
                weapon.Attack();
            }
        }

        /// <summary>
        /// Get new ability or upgrade existing
        /// </summary>
        /// <param name="ability"></param>
        /// <returns>Returns added or upgraded ability</returns>
        public virtual AbilityContainer GetAbility(AbilityContainer ability)
        {
            if (ability as AdditionalAbility != null)
            {
                GetUpgrade((ability as AdditionalAbility).CurrentUpgrade.Upgrade);
                return ability;
            }

            if (ability as Weapon != null && (ability as Weapon).IsSuper)
            {
                AbilityCombination combination = _abilityInventory.FindCombinationByResult(ability);

                if (combination != null)
                {
                    if (_isDebug) Debug.Log("Getting SUPER: " + ability.Name);

                    foreach (AbilityContainer removingAbility in combination.RemovingAbilitiesOnCombine)
                    {

                        _abilityInventory.Remove(removingAbility);
                    }

                    AbilityContainer newAbility = _abilityInventory.Add(ability);

                    if (newAbility != null)
                    {
                        foreach (Upgrade upgrade in _upgrades)
                        {
                            newAbility.Upgrade(upgrade);
                        }
                    }
                }
            }

            AbilityContainer abilityContainer = _abilityInventory.Find(ability);

            if (abilityContainer != null)
            {
                if (abilityContainer.IsMaxLevel)
                {
                    if (_isDebug) Debug.Log("This ability is max level!");

                    return abilityContainer;
                }

                if (_isDebug) Debug.Log("Ability already in inventory. Upgrade it");

                GetUpgrade(abilityContainer.CurrentUpgrade.Upgrade);

                return abilityContainer;
            }
            else
            {
                if (_isDebug) Debug.Log("Add new ability");

                AbilityContainer newAbility = _abilityInventory.Add(ability);

                if (newAbility != null)
                {
                    foreach (Upgrade upgrade in _upgrades)
                    {
                        newAbility.Upgrade(upgrade);
                    }

                    GetUpgrade(newAbility.CurrentUpgrade.Upgrade);
                }
                else if (_isDebug) Debug.Log("Adding ability error!");

                return newAbility;
            }
        }

        /// <summary>
        /// Upgrade character stats
        /// </summary>
        /// <param name="upgrade"></param>
        public virtual void GetUpgrade(Upgrade upgrade)
        {
            if (upgrade == null) return;

            Stats.GetUpgrade(upgrade);

            for (int index = 0; index < _abilityInventory.Abilities.Count; index++)
            {
                _abilityInventory.Abilities[index].Upgrade(upgrade);
            }

            _upgrades.Add(upgrade);
        }

        /// <summary>
        /// Dispel upgrade on stats
        /// </summary>
        /// <param name="upgrade"></param>
        public virtual void DispelUpgrade(Upgrade upgrade)
        {
            Stats.DispelUpgrade(upgrade);

            foreach (AbilityContainer ability in _abilityInventory.Abilities)
            {
                ability.DispelUpgrade(upgrade);
            }

            _upgrades.Remove(upgrade);
        }

        public virtual void DispelUpgrades(List<Upgrade> upgrades)
        {
            foreach (Upgrade upgrade in upgrades)
            {
                DispelUpgrade(upgrade);
            }
        }

        public void DestroyAbilities()
        {
            if (_abilityInventory != null && _abilityInventory.Abilities.Count > 0)
            {
                foreach (var ability in _abilityInventory.Abilities)
                {
                    ability.DestroyAbility();
                }
            }
        }
    }
}
