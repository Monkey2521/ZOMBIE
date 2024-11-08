using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ZombieSurvival.Abilities;

namespace ZombieSurvival.UI.Abilities
{
    public class AbilityInfoSlot : MonoBehaviour
    {
        [Header("Debug settings")]
        [SerializeField] private bool _isDebug;

        [Header("Settings")]
        [SerializeField] private Image _abilityIcon;
        [SerializeField] private RectTransform _abilityLevelParent;
        [SerializeField] private LevelUI _levelPrefab;

        private AbilityContainer _ability;
        private List<LevelUI> _levels;

        public void Initialize(AbilityContainer ability)
        {
            if (ability == null)
            {
                if (_isDebug) Debug.Log("Missing ability!");

                return;
            }

            _ability = ability;

            _abilityIcon.sprite = _ability.Icon;

            if (_levels == null)
            {
                _levels = new List<LevelUI>();
            }
            else if (_levels.Count > 0)
            {
                foreach (LevelUI level in _levels)
                {
                    Destroy(level.gameObject);
                }

                _levels.Clear();
            }

            for (int i = 0; i < (int)_ability.Stats.Level.MaxValue; i++)
            {
                LevelUI lvl = Instantiate(_levelPrefab, _abilityLevelParent);

                LevelUI.LevelType type = i < (int)ability.Stats.Level.Value ? LevelUI.LevelType.Unlocked : LevelUI.LevelType.Locked;

                lvl.Initialize(type);

                _levels.Add(lvl);
            }
        }
    }
}