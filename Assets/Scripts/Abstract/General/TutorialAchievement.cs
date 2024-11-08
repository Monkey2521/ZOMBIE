using ZombieSurvival.General.Inventories;

namespace ZombieSurvival.Achievements
{
    public abstract class TutorialAchievement : Achievement
    {
        public bool WasClaimed => LastCompletedObjective != null && LastCompletedObjective.Claimed;

        public override void Claim()
        {
            base.Claim();

            _level.SetValue(_level.MaxValue);
        }
    }
}
