using ZombieSurvival.Levels;

namespace ZombieSurvival.Rewards
{
    public class LevelSurvivalTimeReward : ConcreteReward
    {
        protected LevelContext _context;
        protected int _survivalTime;

        public LevelContext Context => _context;
        public int SurvivalTime => _survivalTime;

        public LevelSurvivalTimeReward(LevelContext context, int survivalTime) : base(null, null)
        {
            _context = context;
            _survivalTime = survivalTime;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new LevelSurvivalTimeReward(_context, _survivalTime);
    }
}
