using UnityEngine;
using ZombieSurvival.Events;
using ZombieSurvival.Levels;

namespace ZombieSurvival.Achievements
{
    public sealed class PassLevelAchievement : Achievement, ILevelPassedHandler
    {
        [Header("PassLevel achievement settings")]
        [SerializeField] private LevelContext _requiredLevel;

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnLevelPassed(LevelContext level)
        {
            if (level.Equals(_requiredLevel))
            {
                AddCounter(1);
            }
        }
    }
}
