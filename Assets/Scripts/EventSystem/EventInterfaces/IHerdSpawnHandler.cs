namespace ZombieSurvival.Events
{
    public interface IHerdSpawnHandler : ISubscriber
    {
        public void OnHerdSpawn();
    }
}