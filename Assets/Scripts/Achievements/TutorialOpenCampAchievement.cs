using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class TutorialOpenCampAchievement : TutorialAchievement, IOpenCampHandler
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnOpenCamp()
        {
            AddCounter(1);
        }
    }
}
