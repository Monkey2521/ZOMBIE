namespace ZombieSurvival.Events
{
    public interface IGetEquipmentRewardHandler : ISubscriber
    {
        public void OnGetEquipmentReward();
    }
}