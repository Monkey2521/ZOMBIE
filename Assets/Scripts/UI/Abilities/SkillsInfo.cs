using System.Collections.Generic;
using UnityEngine;
using Zenject;
using ZombieSurvival.Abilities;
using ZombieSurvival.Characters;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.Interfaces;

namespace ZombieSurvival.UI.Abilities
{
    public class SkillsInfo : UIMenu, IFixedUpdatable, IPlayerGetAbilityHandler
    {
        [Header("SkillsInfo settings")]
        [SerializeField] private AbilityCooldown _abilityCooldownPrefab;

        private CleanupableList<AbilityCooldown> _cooldowns;

        [Inject] private Player _player;

        public void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        public void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnPlayerGetAbility(AbilityCooldown ability)
        {
            _cooldowns ??= new CleanupableList<AbilityCooldown>();

            _cooldowns.Add(ability);
        }

        public void FixedUpdate() // test
        {
            OnFixedUpdate();
        }

        public void OnFixedUpdate()
        {
            if (_cooldowns == null || _cooldowns.Count == 0) return;

            foreach(AbilityCooldown cooldown in _cooldowns.List)
            {
                if (cooldown == null)
                {
                    _cooldowns.Remove(cooldown, canNotModify: true);

                    return;
                }

                cooldown.OnFixedUpdate();
            }

            _cooldowns.Cleanup();
        }
    }
}