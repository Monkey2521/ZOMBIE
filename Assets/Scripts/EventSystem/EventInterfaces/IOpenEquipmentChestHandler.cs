namespace ZombieSurvival.Events
{
    public interface IOpenEquipmentChestHandler : ISubscriber
    {
        public void OnOpenEquipmentChest();
    }
}