using ZombieSurvival.General;

namespace ZombieSurvival.Rewards
{
    public class CampTalentReward : ConcreteReward
    {
        private Talent _talent;

        public Talent Talent => _talent;

        public CampTalentReward(Talent talent, int amount = 1) :
            base(talent.UnlockedIcon, talent.UnlockedBackground, amount)
        {
            _talent = talent;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new CampTalentReward(_talent, _amount);
    }
}
