using ZombieSurvival.General;

namespace ZombieSurvival.Events
{
    public interface IInventoryUpdateHandler : ISubscriber
    {
        public void OnInvontoryUpdate(Inventory inventory);
    }
}