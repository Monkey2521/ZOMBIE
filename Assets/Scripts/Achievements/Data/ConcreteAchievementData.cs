using System;

namespace ZombieSurvival.Achievements
{
    [Serializable]
    public class ConcreteAchievementData
    {
        public int level;
        public int progress;
        public int unclaimedLevel;

        public ConcreteAchievementData(Achievement achievement)
        {
            level = achievement.Level;
            progress = achievement.Progress;
            unclaimedLevel = achievement.UnclaimedObjective != null ? achievement.UnclaimedObjective.Level : -1;
        }
    }
}
