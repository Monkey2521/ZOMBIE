using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class TutorialOpenShopAchievement : TutorialAchievement, IOpenShopHandler
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnOpenShop()
        {
            AddCounter(1);
        }
    }
}
