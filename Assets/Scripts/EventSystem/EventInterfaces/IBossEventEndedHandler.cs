namespace ZombieSurvival.Events
{
    public interface IBossEventEndedHandler : ISubscriber
    {
        public void OnBossEventEnd();
    }
}