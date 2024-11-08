using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Interfaces;

namespace ZombieSurvival.Spawners
{
    public abstract class Spawner : ZSMonoBehaviour, IUpdatable, IFixedUpdatable
    {
        [Header("Spawner settings")]
        [SerializeField][Range(0, 50)] protected float _spawnDeltaDistance;

        public abstract void OnUpdate();

        public abstract void OnFixedUpdate();

        protected abstract void Spawn(Vector3 position);
    }
}