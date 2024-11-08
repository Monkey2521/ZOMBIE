using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class TutorialGetEquipmentRewardAchievement : TutorialAchievement, IGetEquipmentRewardHandler
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnGetEquipmentReward()
        {
            AddCounter(1);
        }
    }
}
