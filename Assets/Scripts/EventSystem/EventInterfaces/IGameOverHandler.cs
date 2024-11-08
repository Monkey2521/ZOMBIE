namespace ZombieSurvival.Events
{
    public interface IGameOverHandler : ISubscriber
    {
        public void OnGameOver();
    }
}