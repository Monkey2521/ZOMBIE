using ZombieSurvival.Achievements;

namespace ZombieSurvival.Rewards
{
    public class AchievementObtainReward : ConcreteReward
    {
        protected Achievement _achievement;

        public Achievement Achievement => _achievement;

        public AchievementObtainReward(Achievement achievement, int amount = 1) : 
            base(achievement.Icon, achievement.Background, amount)
        {
            _achievement = achievement;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return false;
        }

        public override ConcreteReward Clone() => new AchievementObtainReward(_achievement, _amount);
    }
}
