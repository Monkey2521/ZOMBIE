using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class SurvivalTimeAchievement : Achievement, ISurvivalTimeHandler
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnSurvivalTime(int time)
        {
            AddCounter(time);
        }
    }
}
