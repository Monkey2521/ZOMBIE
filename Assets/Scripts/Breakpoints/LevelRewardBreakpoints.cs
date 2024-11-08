using System.Collections.Generic;
using UnityEngine;

namespace ZombieSurvival.General.Breakpoints
{
    [System.Serializable]
    public class LevelRewardBreakpoints
    {
        [SerializeField] private List<LevelBreakpoint> _breakpoints;

        public int ReachedProgress
        {
            get
            {
                int progress = 0;
                foreach (Breakpoint breakpoint in Breakpoints)
                {
                    if (breakpoint.IsReached) progress = breakpoint.RequiredProgress;

                    else break;
                }

                return progress;
            }
        }
        public List<LevelBreakpoint> Breakpoints => _breakpoints;
        public bool HaveUnclaimedRewards => _breakpoints.Find(item => !item.wasClaimed) != null;

        public Breakpoint CheckReaching(int progress)
        {
            foreach (Breakpoint breakpoint in _breakpoints)
            {
                if (breakpoint.IsReached) continue;

                else if (breakpoint.RequiredProgress <= progress)
                {
                    breakpoint.SetReached(true);

                    return breakpoint;
                }
            }
            return null;
        }

        public bool Claim(LevelBreakpoint breakpoint)
        {
            if (_breakpoints.Contains(breakpoint) && !breakpoint.wasClaimed)
            {
                breakpoint.wasClaimed = true;

                return true;
            }

            return false;
        }
    }
}