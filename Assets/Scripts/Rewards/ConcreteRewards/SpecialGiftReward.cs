namespace ZombieSurvival.Rewards
{
    public sealed class SpecialGiftReward : ConcreteReward
    {
        private Reward _reward;

        public Reward Reward => _reward;

        public SpecialGiftReward(Reward reward) : base(null, null) 
        {
            _reward = reward;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new SpecialGiftReward(_reward);
    }
}
