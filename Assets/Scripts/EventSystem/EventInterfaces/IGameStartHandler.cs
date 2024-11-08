namespace ZombieSurvival.Events
{
    public interface IGameStartHandler : ISubscriber
    {
        public void OnGameStart();
    }
}