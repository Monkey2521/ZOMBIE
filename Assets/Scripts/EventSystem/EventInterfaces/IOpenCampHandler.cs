namespace ZombieSurvival.Events
{
    public interface IOpenCampHandler : ISubscriber
    {
        public void OnOpenCamp();
    }
}