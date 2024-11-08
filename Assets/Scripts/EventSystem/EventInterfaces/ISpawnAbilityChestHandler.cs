using UnityEngine;
using ZombieSurvival.Objects.Pickables;

namespace ZombieSurvival.Events
{
    public interface ISpawnAbilityChestHandler : ISubscriber
    {
        public void OnSpawnAbilityChest(Vector3 position, int maxAbilitiesCount);
    }
}