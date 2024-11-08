namespace ZombieSurvival.Events
{
    public interface IPlayerLevelUpHandler : ISubscriber
    {
        public void OnPlayerLevelUp();
    }
}