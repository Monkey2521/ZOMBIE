namespace ZombieSurvival.Events
{
    public interface ISurvivalTimeHandler : ISubscriber
    {
        public void OnSurvivalTime(int time);
    }
}