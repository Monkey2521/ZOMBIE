namespace ZombieSurvival.Events
{
    public interface IBossEventAlertHandler : ISubscriber
    {
        public void OnBossEventAlert();
    }
}