using UnityEngine;
using ZombieSurvival.Objects.Pickables;

namespace ZombieSurvival.Events
{
    public interface ISpawnPickableRewardHandler : ISubscriber
    {
        public void OnSpawnPickableReward(MonoPickableReward reward, Vector3 position);
    }
}