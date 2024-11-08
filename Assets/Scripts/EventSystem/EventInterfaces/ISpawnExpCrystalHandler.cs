using UnityEngine;

namespace ZombieSurvival.Events
{
    public interface ISpawnExpCrystalHandler : ISubscriber
    {
        public void OnSpawnExpCrystal(Vector3 position);
    }
}