using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class TutorialOpenInventoryAchievement : TutorialAchievement, IOpenInventoryHandler
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnOpenInventory()
        {
            AddCounter(1);
        }
    }
}
