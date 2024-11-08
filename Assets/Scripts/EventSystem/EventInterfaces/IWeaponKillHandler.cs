using ZombieSurvival.Abilities;

namespace ZombieSurvival.Events
{
    public interface IWeaponKillHandler : ISubscriber
    {
        public void OnWeaponKill(Weapon weapon);
    }
}