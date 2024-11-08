using ZombieSurvival.Spawners;

namespace ZombieSurvival.Characters
{
    public sealed class EliteZombie : Enemy
    {
        private EliteZombiesSpawner _spawner;

        public void InitializeSpawner(EliteZombiesSpawner spawner)
        {
            _spawner = spawner;
        }

        protected override void OnDie(bool instantly = false)
        {
            _spawner.OnEliteZombieDies(this);

            base.OnDie(instantly);
        }
    }
}