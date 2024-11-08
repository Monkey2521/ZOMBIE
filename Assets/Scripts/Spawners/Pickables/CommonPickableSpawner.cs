using UnityEngine;

using ZombieSurvival.Events;
using ZombieSurvival.Objects;

namespace ZombieSurvival.Spawners
{
    public sealed class CommonPickableSpawner : PickableObjectSpawner, ISpawnCommonPickableHandler
    {
        [Header("Pickable reward spawner settings")]
        [SerializeField] private PickableObject _pickablePrefab;

        public override PickableObject PickableObject => _pickablePrefab;

        protected override void OnEnable()
        {
            EventBus.Subscribe(this);

            base.OnEnable();
        }

        protected override void OnDisable()
        {
            EventBus.Unsubscribe(this);

            base.OnDisable();
        }

        public void OnSpawnCommonPickable(PickableObject @object, Vector3 position)
        {
            if (@object.Equals(_pickablePrefab))
            {
                Spawn(position);
            }
        }

        protected override void Spawn(Vector3 position)
        {
            PickableObject obj = _pool.Spawn(position);

            if (obj is PickableObject pickable)
            {
                pickable.Initialize(_pool);
            }
            else
            {
                if (_isDebug) Debug.Log(name + ": Missing pickable!");

                _pool.Release(obj);
            }
        }
    }
}