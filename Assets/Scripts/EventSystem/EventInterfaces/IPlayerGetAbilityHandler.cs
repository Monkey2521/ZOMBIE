using ZombieSurvival.UI.Abilities;

namespace ZombieSurvival.Events
{
    public interface IPlayerGetAbilityHandler : ISubscriber
    {
        public void OnPlayerGetAbility(AbilityCooldown ability);
    }
}