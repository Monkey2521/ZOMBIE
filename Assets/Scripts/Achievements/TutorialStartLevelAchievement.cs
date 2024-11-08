using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class TutorialStartLevelAchievement : TutorialAchievement, IStartLevelHandler
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnStartLevel()
        {
            AddCounter(1);
        }
    }
}
