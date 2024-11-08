using UnityEngine;

using ZombieSurvival.Objects;

namespace ZombieSurvival.Spawners
{
    public abstract class PickableObjectSpawner : Spawner
    {
        [Header("PickableObject spawner settings")]
        [SerializeField] protected int _poolSize;
        [Tooltip("Field can be null")]
        [SerializeField] protected Transform _poolParent;

        protected ObjectSpawner<PickableObject> _pool;

        public abstract PickableObject PickableObject { get; }

        protected virtual void OnEnable()
        {
            if (_pool != null)
            {
                _pool.ClearPool();
            }

            _pool = new ObjectSpawner<PickableObject>(PickableObject, _poolSize, _poolParent);
        }

        protected virtual void OnDisable()
        {
            _pool.ClearPool();
        }

        public override void OnUpdate() { }

        public override void OnFixedUpdate()
        {
            if (_pool == null || _pool.SpawnCount == 0) return;

            for (int i = 0; i < _pool.SpawnCount; i++)
            {
                if (_pool.SpawnedObjects[i] != null)
                {
                    _pool.SpawnedObjects[i]?.OnFixedUpdate();
                }
                else
                {
                    _pool.SpawnedObjects.Remove(_pool.SpawnedObjects[i], canNotModify: true);
                }
            }

            _pool.SpawnedObjects.Cleanup();
        }

        protected virtual void ClearPool()
        {
            if (_pool != null)
            {
                _pool.ClearPool();
            }
        }
    }
}