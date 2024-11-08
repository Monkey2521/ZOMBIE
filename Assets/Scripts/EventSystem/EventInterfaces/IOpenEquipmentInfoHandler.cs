namespace ZombieSurvival.Events
{
    public interface IOpenEquipmentInfoHandler : ISubscriber
    {
        public void OnOpenEquipmentInfo();
    }
}