namespace ZombieSurvival.Events
{
    public interface IStartLevelHandler : ISubscriber
    {
        public void OnStartLevel();
    }
}