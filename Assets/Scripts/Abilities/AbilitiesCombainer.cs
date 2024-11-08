using System.Collections.Generic;
using UnityEngine;
using ZombieSurvival.Characters;

namespace ZombieSurvival.Abilities
{
    [System.Serializable]
    public class AbilitiesCombainer
    {
        [SerializeField] private List<AbilityCombination> _combinations;

        private AbilityContainer _container;
        private CharacterBase _character;

        public AbilityContainer Container => _container;
        public CharacterBase Character => _character;
        public List<AbilityCombination> Combinations => _combinations;

        public void Initialize (AbilityContainer abilityContainer, CharacterBase character)
        {
            _container = abilityContainer;
            _character = character;

            foreach (AbilityCombination combination in _combinations)
            {
                combination.Initialize(this);
            }
        }

        public List<AbilityContainer> GetCombines()
        {
            List<AbilityContainer> combines = new List<AbilityContainer>();

            foreach (AbilityCombination combination in _combinations)
            {
                combines.Add(combination.CombainedAbility);
            }

            return combines;
        }

        public AbilityCombination FindCombination(AbilityContainer ability)
        {
            foreach (AbilityCombination combination in _combinations)
            {
                if (combination.CombainedAbility.Name.Equals(ability.Name))
                {
                    return combination;
                }
            }

            return null;
        }

        public AbilityCombination FindCombinationByResult(AbilityContainer result)
        {
            foreach (AbilityCombination combination in _combinations)
            {
                if (combination.CombineResult.Name.Equals(result.Name))
                {
                    return combination;
                }
            }

            return null;
        }
    }
}
