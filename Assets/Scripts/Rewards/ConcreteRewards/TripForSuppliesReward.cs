namespace ZombieSurvival.Rewards
{
    [System.Serializable]
    public sealed class TripForSuppliesReward : ConcreteReward
    {
        private TripForSuppliesRewardData _rewardData;

        public TripForSuppliesRewardData RewardData => _rewardData;

        public TripForSuppliesReward(TripForSuppliesRewardData data) : base(null, null) 
        {
            _rewardData = data;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new TripForSuppliesReward(_rewardData);
    }
}
