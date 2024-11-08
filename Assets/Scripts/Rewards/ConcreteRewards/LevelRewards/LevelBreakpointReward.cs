using ZombieSurvival.General.Breakpoints;

namespace ZombieSurvival.Rewards
{
    public class LevelBreakpointReward : ConcreteReward
    {
        protected LevelBreakpoint _breakpoint;

        public LevelBreakpoint Breakpoint => _breakpoint;

        public LevelBreakpointReward(LevelBreakpoint breakpoint) : base(null, null)
        {
            _breakpoint = breakpoint;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new LevelBreakpointReward(_breakpoint);
    }
}
