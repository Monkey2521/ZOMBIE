using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Characters;
using ZombieSurvival.General;
using ZombieSurvival.Interfaces;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Abilities
{
    public abstract class AbilityContainer : ZSMonoBehaviour, IUpgradable
    {
        [Header("Ability settings")]
        [SerializeField] protected string _name;
        [SerializeField] protected Sprite _icon;

        [Header("Combinations")]
        [SerializeField] protected AbilitiesCombainer _abilityCombainer;

        protected UpgradeList _upgrades;
        protected CharacterBase _character;

        public string Name => _name;
        public UpgradeList Upgrades => _upgrades;
        /// <summary>
        /// Return false if ability not reached MaxLevel or MaxLevel is infinite
        /// </summary>
        public bool IsMaxLevel => !Stats.Level.MaxValueIsInfinite && Stats.Level.Value == Stats.Level.MaxValue;
        public Sprite Icon => _icon;
        /// <summary>
        /// Current upgrade of this ability
        /// </summary>
        public CurrentUpgrade CurrentUpgrade => UpgradeData.Upgrades[(int)Stats.Level.Value];
        /// <summary>
        /// Stats of this ability
        /// </summary>
        public abstract AbilityStats Stats { get; }
        /// <summary>
        /// All ability upgrades
        /// </summary>
        public abstract AbilityUpgradeData UpgradeData { get; }

        public AbilitiesCombainer Combainer => _abilityCombainer;

        public virtual void Initialize(CharacterBase character)
        {
            Stats.Initialize();

            _character = character;

            _abilityCombainer.Initialize(this, _character);

            _upgrades = new UpgradeList();
        }

        /// <summary>
        /// Upgrade ability
        /// </summary>
        /// <param name="upgrade"></param>
        /// <returns>Return true if level up</returns>
        public virtual bool Upgrade(Upgrade upgrade)
        {
            if (upgrade == null || upgrade.Upgrades == null || upgrade.Upgrades.Count == 0) return false;

            foreach (UpgradeData data in upgrade.Upgrades)
            {
                _upgrades.Add(data);
            }

            if (upgrade.IsAbilityUpgrade && upgrade.AbilityMarker.Equals(Stats.AbilityMarker) && upgrade.Equals(CurrentUpgrade.Upgrade))
            {
                Stats.Level.LevelUp();

                return true;
            }

            return false;
        }

        public virtual void DispelUpgrade(Upgrade upgrade)
        {
            Stats.DispelUpgrade(upgrade);
        }

        public virtual void DispelUgrades(List<Upgrade> upgrades)
        {
            foreach (Upgrade upgrade in upgrades)
            {
                DispelUpgrade(upgrade);
            }
        }

        public virtual void DestroyAbility()
        {
            Destroy(gameObject);
        }
    }
}
