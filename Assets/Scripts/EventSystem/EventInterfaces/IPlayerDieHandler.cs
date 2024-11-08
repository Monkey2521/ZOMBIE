namespace ZombieSurvival.Events
{
    public interface IPlayerDieHandler : ISubscriber
    {
        public void OnPlayerDie();
    }
}