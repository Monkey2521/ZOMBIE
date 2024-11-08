namespace ZombieSurvival.Events
{
    public interface IOpenShopHandler : ISubscriber
    {
        public void OnOpenShop();
    }
}