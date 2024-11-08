using ZombieSurvival.General;
using ZombieSurvival.Objects;

namespace ZombieSurvival.Rewards
{
    public sealed class PickableReward : ConcreteReward
    {
        private PickableObject _pickable;

        public PickableObject Pickable => _pickable;

        public PickableReward(PickableObject pickable) : base(null, null)
        {
            _pickable = pickable;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            if (other is PickableReward compare)
            {
                return compare._pickable.name.Equals(_pickable.name);
            }

            return false;
        }

        public override ConcreteReward Clone() => new PickableReward(_pickable);
    }
}
