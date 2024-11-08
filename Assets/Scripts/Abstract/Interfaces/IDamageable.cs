using ZombieSurvival.Stats;

namespace ZombieSurvival
{
    namespace Interfaces
    {
        public interface IDamageable
        {
            public int HP { get; }
            public int MaxHP { get; }

            public float TakeDamage(ConcreteDamage damage);

            public void Die(bool instantly);
        }
    }
}
