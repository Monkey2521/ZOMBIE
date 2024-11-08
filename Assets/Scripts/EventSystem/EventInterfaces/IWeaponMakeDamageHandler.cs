using ZombieSurvival.Abilities;

namespace ZombieSurvival.Events
{
    public interface IWeaponMakeDamageHandler : ISubscriber
    {
        public void OnWeaponMakeDamage(Weapon weapon, float damage);
    }
}