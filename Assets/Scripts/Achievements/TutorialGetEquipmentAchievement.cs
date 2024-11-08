using ZombieSurvival.Equipments;
using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class TutorialGetEquipmentAchievement : TutorialAchievement, IGetEquipmentHandler
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnGetEquipment(Equipment equipment)
        {
            AddCounter(1);
        }
    }
}
