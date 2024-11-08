using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvival.Abilities
{
    [System.Serializable]
    public class AbilityCombination
    {
        [SerializeField] private AbilityContainer _combinedAbility;
        [SerializeField] private AbilityContainer _combineResult;
        [SerializeField] private int _unlockLevel;

        [SerializeField] private List<AbilityContainer> _removingAbilitiesOnCombine;

        [HideInInspector] private AbilitiesCombainer _combainer;

        public AbilityContainer CombainedAbility => _combinedAbility;
        public int UnlockLevel => _unlockLevel;
        public AbilityContainer CombineResult => _combineResult;
        public List<AbilityContainer> RemovingAbilitiesOnCombine => _removingAbilitiesOnCombine;
        public AbilitiesCombainer Combainer => _combainer;

        public void Initialize(AbilitiesCombainer combainer)
        {
            _combainer = combainer;
        }

        public bool AbleToCombine(AbilityCombination combination)
        {
            if (combination == null || combination.Combainer.Container == null ||
                !combination.CombainedAbility.Name.Equals(Combainer.Container.Name) ||
                combination.Combainer.Container.Stats.Level.Value < combination.UnlockLevel)
            {
                return false;
            }

            AbilityCombination comb = Combainer.FindCombination(combination.Combainer.Container);

            if (comb != null && comb.Equals(this) &&
                _combainer.Container.Stats.Level.Value >= _unlockLevel &&
                combination.CombineResult.Equals(_combineResult) &&
                _combainer.Character.AbilityInventory.Find(combination.Combainer.Container) != null &&
                _combainer.Character.AbilityInventory.Find(_combainer.Container) != null)
            {
                return true;
            }

            return false;
        }

        public bool AbleToCombine(AbilityContainer container)
        {
            if (container == null) return false;

            return AbleToCombine(container.Combainer.FindCombination(_combainer.Container));
        }
    }
}