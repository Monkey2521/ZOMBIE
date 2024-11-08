using UnityEngine;

using ZombieSurvival.Stats;
using ZombieSurvival.UI.Camp;

namespace ZombieSurvival.Upgrades
{
    [System.Serializable]
    public class CampUpgrade
    {
        [SerializeField] private Sprite _unlockedIcon;
        [SerializeField] private Sprite _lockedIcon;
        [SerializeField] private Level _level;
        [SerializeField] private Upgrade _repeatingUpgrade;
        [Tooltip("Field can be null")]
        [SerializeField] private CampBuilding _building;

        public Sprite UnlockedIcon => _unlockedIcon;
        public Sprite LockedIcon => _lockedIcon;
        public Level Level => _level;
        public bool Unlocked => _level.Value > 0;
        public Upgrade CurrentUpgrade => (int)_level.Value > 0 ? _repeatingUpgrade * (int)_level.Value : null;

        public void Initialize()
        {
            _level.Initialize();

            _building?.Initialize(this);
        }

        public void Upgrade()
        {
            _level.LevelUp();

            UpdateValues();
        }

        public void UpdateValues()
        {
            _building?.UpdateValues();
        }
    }
}