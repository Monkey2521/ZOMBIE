using UnityEngine;

using ZombieSurvival.Objects;

namespace ZombieSurvival.Events
{
    public interface ISpawnCommonPickableHandler : ISubscriber
    {
        public void OnSpawnCommonPickable(PickableObject @object, Vector3 position);
    }
}