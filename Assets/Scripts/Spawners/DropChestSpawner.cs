using UnityEngine;

using ZombieSurvival.Characters;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.Objects;
using ZombieSurvival.Objects.Pickables;

using Zenject;

namespace ZombieSurvival.Spawners
{
    public sealed class DropChestSpawner : Spawner, IGameStartHandler, IBossEventHandler, IBossEventEndedHandler
    {
        [Header("Chest settings")]
        [SerializeField] private PickablesDropChest _rewardChestPrefab;
        [SerializeField] private int _poolSize = 5;
        [SerializeField] private ChanceCombiner<PickableObject> _pickablesSpawnChances;
        [Tooltip("Spawn interval in seconds")]
        [SerializeField] private int _spawnCooldown;

        private float _timer;
        private bool _onBossEvent;

        private ObjectSpawner<PickablesDropChest> _pool;

        [Inject] private Player _player;

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }
        
        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public void OnGameStart()
        {
            if (_pool != null)
            {
                _pool.ClearPool();
            }

            _pool = new ObjectSpawner<PickablesDropChest>(_rewardChestPrefab, _poolSize);

            _pickablesSpawnChances.Initialize();
            _timer = _spawnCooldown;
            _onBossEvent = false;
        }

        public override void OnUpdate()
        {
            if (_onBossEvent) return;

            _timer -= Time.deltaTime;

            if (_timer <= 0)
            {
                Spawn(_player.transform.position);

                _timer = _spawnCooldown;
            }
        }
        
        public override void OnFixedUpdate() 
        {
            if (_pool == null || _pool.SpawnCount == 0) return;

            for (int i = 0; i < _pool.SpawnCount; i++)
            {
                if (_pool.SpawnedObjects[i] != null)
                {
                    _pool.SpawnedObjects[i].OnFixedUpdate();
                }
                else
                {
                    _pool.SpawnedObjects.Remove(_pool.SpawnedObjects[i], canNotModify: true);
                }
            }

            _pool.SpawnedObjects.Cleanup();
        }

        protected override void Spawn(Vector3 position)
        {
            if (_isDebug) Debug.Log("Spawn chest");

            PickablesDropChest chest = _pool.Spawn(position + GetDeltaPos());

            chest.Initialize(this);
        }

        private Vector3 GetDeltaPos()
        {
            float angle = Random.Range(0, 2 * Mathf.PI);

            return new Vector3
                (
                    Mathf.Cos(angle) * _spawnDeltaDistance,
                    0f,
                    Mathf.Sin(angle) * _spawnDeltaDistance
                );
        }

        public void OnChestDestoyed(PickablesDropChest chest) // TODO rework
        {
            PickableObject obj = _pickablesSpawnChances.GetStrikedObject();

            Vector3 spawnPos = chest.transform.position;

            _pool.Release(chest);

            if (obj is ExpCrystal expCrystal)
            {
                EventBus.Publish<ISpawnExpCrystalHandler>(handler => handler.OnSpawnExpCrystal(spawnPos));
            }

            else if (obj is MonoPickableReward pickableReward)
            {
                EventBus.Publish<ISpawnPickableRewardHandler>(handler => handler.OnSpawnPickableReward(pickableReward, spawnPos));
            }
            else if (obj is AbilityChest)
            {
                EventBus.Publish<ISpawnAbilityChestHandler>(handler => handler.OnSpawnAbilityChest(spawnPos, 1));
            }
            else
            {
                EventBus.Publish<ISpawnCommonPickableHandler>(handler => handler.OnSpawnCommonPickable(obj, spawnPos));
            }

            if (_isDebug) Debug.Log("Chest destoyed, spawning " + obj.name);
        }

        public void OnBossEvent()
        {
            _onBossEvent = true;
        }

        public void OnBossEventEnd()
        {
            _onBossEvent = false;
        }
    }
}