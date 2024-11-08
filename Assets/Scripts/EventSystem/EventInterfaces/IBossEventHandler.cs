namespace ZombieSurvival.Events
{
    public interface IBossEventHandler : ISubscriber
    {
        public void OnBossEvent();
    }
}