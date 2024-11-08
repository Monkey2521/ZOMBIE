using ZombieSurvival.Objects.Pickables;

namespace ZombieSurvival.Rewards
{
    public sealed class ZombieChestReward : ConcreteReward
    {
        private AbilityChest _chest;

        public AbilityChest Chest => _chest;

        public ZombieChestReward(AbilityChest chest, int maxAbilitiesReward) : base(null, null, maxAbilitiesReward)
        {
            _chest = chest;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new ZombieChestReward(_chest, _amount);
    }
}
