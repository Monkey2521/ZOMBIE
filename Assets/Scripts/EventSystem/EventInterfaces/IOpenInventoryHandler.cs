namespace ZombieSurvival.Events
{
    public interface IOpenInventoryHandler : ISubscriber
    {
        public void OnOpenInventory();
    }
}