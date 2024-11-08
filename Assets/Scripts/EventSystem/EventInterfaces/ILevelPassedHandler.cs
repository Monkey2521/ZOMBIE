using ZombieSurvival.Levels;

namespace ZombieSurvival.Events
{
    public interface ILevelPassedHandler : ISubscriber
    {
        public void OnLevelPassed(LevelContext level);
    }
}
