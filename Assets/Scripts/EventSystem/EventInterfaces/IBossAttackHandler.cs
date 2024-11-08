namespace ZombieSurvival.Events
{
    public interface IBossAttackHandler : ISubscriber
    {
        public void OnBossAttack();
    }
}