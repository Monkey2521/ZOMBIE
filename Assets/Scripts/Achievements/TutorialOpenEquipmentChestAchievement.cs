using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class TutorialOpenEquipmentChestAchievement : TutorialAchievement, IOpenEquipmentChestHandler
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnOpenEquipmentChest()
        {
            AddCounter(1);
        }
    }
}
