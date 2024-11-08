using UnityEngine;
using static UnityEngine.Mathf;

using ZombieSurvival.Characters;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.General.Breakpoints;
using ZombieSurvival.Objects;
using ZombieSurvival.UI.Abilities;

using Zenject;

namespace ZombieSurvival.Spawners
{
    public sealed class EliteZombiesSpawner : EnemySpawner, IBossEventHandler
    {
        [Header("Elite zombie spawner settings")]
        [SerializeField][Range(1, 5)] private int _poolSize;

        private BreakpointList<EliteZombieBreakpoint> _breakpoints;
        private EliteZombieBreakpoint _currentBreakpoint;

        private bool _onBossEvent;

        [Inject] private AbilityGiver _abilityGiver;

        protected override void OnEnable()
        {
            base.OnEnable();

            _breakpoints = new BreakpointList<EliteZombieBreakpoint>(_levelContext.EliteZombieBreakpoints);
        }

        public override void OnUpdate()
        {
            if (!_onBossEvent && CurrentSpawned > 0)
            {
                base.OnUpdate();
                TryClearPool();
            }
            else return;
        }

        public override void OnFixedUpdate()
        {
            if (!_onBossEvent && CurrentSpawned > 0)
            {
                base.OnFixedUpdate();
                TryClearPool();
            }
            else return;
        }

        public override void OnLevelProgressUpdate(int progress)
        {
            Breakpoint breakpoint = _breakpoints.CheckReaching(progress);

            if (breakpoint != null)
            {
                if (_isDebug) Debug.Log("Elite zombie incoming!");

                _currentBreakpoint = breakpoint as EliteZombieBreakpoint;

                _maxUnitsOnScene = _poolSize;

                ReplacePools();
                DispelUpgrades();

                _currentSpawners.Add(new ObjectSpawner<Enemy>(_currentBreakpoint.EnemyToSpawnPrefab, _maxUnitsOnScene));

                for (int i = 0; i < _poolSize; i++)
                {
                    Spawn(GetSpawnPosition());
                }

                _currentUpgrade = _currentBreakpoint.EliteZombieUpgrade;

                GetUpgrade();
            }
        }

        protected override void Spawn(Vector3 position)
        {
            if (_currentSpawners != null && _currentSpawners.Count > 0 && position != -Vector3.one)
            {
                Enemy spawnedEnemy = _currentSpawners[0].Spawn(new Vector3
                        (
                            position.x,
                            _levelContext.LevelBuilder.GridHeight + _currentSpawners[0].Prefab.Collider.height * _currentSpawners[0].Prefab.transform.localScale.y * 0.5f,
                            position.z
                        ));

                spawnedEnemy.Initialize(_player, _currentSpawners[0]);
                (spawnedEnemy as EliteZombie).InitializeSpawner(this);

                _totalSpawned++;
            }
            else return;
        }

        protected override Vector3 GetSpawnPosition()
        {
            if (_currentSpawners != null && _currentSpawners.Count > 0)
            {
                Vector3 playerPos = _player.transform.position;

                float angle = Random.Range(0f, 2 * PI);

                return new Vector3
                    (
                        Cos(angle) * _spawnDeltaDistance + playerPos.x,
                        _levelContext.LevelBuilder.GridHeight + _currentSpawners[0].Prefab.Collider.height * _currentSpawners[0].Prefab.transform.localScale.y * 0.5f,
                        Sin(angle) * _spawnDeltaDistance + playerPos.z
                    );
            }
            else return -Vector3.one;
        }

        public void OnEliteZombieDies(EliteZombie zombie) // TODO rework
        {
            Vector3 pos = zombie.transform.position;
                
            EventBus.Publish<ISpawnAbilityChestHandler>(handler => handler.OnSpawnAbilityChest
                (
                    new Vector3(pos.x, _levelContext.LevelBuilder.GridHeight, pos.z),
                    1
                ));
        }

        public void OnBossEvent()
        {
            if (_currentSpawners != null)
            {
                foreach (var spawner in _currentSpawners)
                {
                    for (int i = 0; i < spawner.SpawnCount; i++)
                    {
                        spawner.SpawnedObjects[i]?.Die(true);
                    }
                }
            }

            if (_prevSpawners != null)
            {
                foreach (var spawner in _prevSpawners)
                {
                    for (int i = 0; i < spawner.SpawnCount; i++)
                    {
                        spawner.SpawnedObjects[i]?.Die(true);
                    }
                }
            }

            ClearPools();
        }

        private void TryClearPool()
        {
            if (CurrentSpawned == 0)
            {
                ClearPools();
            }
        }
    }
}