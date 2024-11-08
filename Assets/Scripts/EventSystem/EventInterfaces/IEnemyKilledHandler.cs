using ZombieSurvival.Characters;

namespace ZombieSurvival.Events
{
    public interface IEnemyKilledHandler : ISubscriber
    {
        public void OnEnemyKilled(Enemy enemy);
    }
}