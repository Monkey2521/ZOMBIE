using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class TutorialOpenEquipmentInfoAchievement : TutorialAchievement, IOpenEquipmentInfoHandler
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnOpenEquipmentInfo()
        {
            AddCounter(1);
        }
    }
}
