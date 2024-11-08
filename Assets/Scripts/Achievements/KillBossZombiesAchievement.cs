using ZombieSurvival.Characters;
using ZombieSurvival.Events;

namespace ZombieSurvival.Achievements
{
    public sealed class KillBossZombiesAchievement : Achievement, IEnemyKilledHandler
    {
        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnEnemyKilled(Enemy enemy)
        {
            if (enemy as BossZombie != null)
            {
                AddCounter();
            }
        }
    }
}
