using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Abilities;
using ZombieSurvival.Characters;
using ZombieSurvival.UI.Abilities;

namespace ZombieSurvival.Objects.Pickables
{
    public class AbilityChest : PickableObject
    {
        [Header("Ability chest settings")]
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _abilityUIParent;
        [SerializeField] private AbilityUI _abilityUIPrefab;

        private List<AbilityUI> _abilitiesUI;

        private AbilityGiver _abilityGiver;
        private int _abilitiesRewardCount;

        private const int MIN_ABILITIES_REWARD = 1;

        public void Initialize(MonoPool<PickableObject> pool, AbilityGiver abilityGiver, int abilitiesRewardCount)
        {
            base.Initialize(pool);

            if (_abilitiesUI != null && _abilitiesUI.Count > 0)
            {
                foreach (AbilityUI abilityUI in _abilitiesUI)
                {
                    Destroy(abilityUI.gameObject);
                }

                _abilitiesUI.Clear();
            }

            _abilitiesUI = new List<AbilityUI>();

            _abilityGiver = abilityGiver;
            _abilitiesRewardCount = Random.Range(MIN_ABILITIES_REWARD, abilitiesRewardCount + 1);

            _canvas.gameObject.SetActive(false);
        }

        protected override void OnPickUp(float releaseDelay = 0)
        {
            if (_target.TryGetComponent(out Player player))
            {
                _canvas.gameObject.SetActive(true);

                Time.timeScale = 0;

                for (int i = 0; i < _abilitiesRewardCount; i++)
                {
                    AbilityContainer ability = _abilityGiver.GetAbilitiesUpgrades(1)[0];

                    if (ability != null)
                    {
                        AbilityUI abilityUI = Instantiate(_abilityUIPrefab, _abilityUIParent);
                        abilityUI.Initialize();
                        abilityUI.SetAbility(ability);

                        _abilitiesUI.Add(abilityUI);

                        player.GetAbility(ability);
                    }
                    else if (_isDebug) Debug.Log("Missing ability!");
                }

            }
            else
            {
                if (_isDebug) Debug.Log(name + ": Missing player!");

                base.OnPickUp();
            }
        }

        public void OnAbilitiesGetted()
        {
            _canvas.gameObject.SetActive(false);
            Time.timeScale = 1;

            base.OnPickUp();
        }
    }
}