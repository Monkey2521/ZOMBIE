using ZombieSurvival.Events;

namespace ZombieSurvival.UI
{
    public sealed class GetEquipmentTutorialButton : TutorialButton
    {
        public override void OnTutorialEnd()
        {
            base.OnTutorialEnd();

            EventBus.Publish<IGetEquipmentRewardHandler>(handler => handler.OnGetEquipmentReward());
        }
    }
}