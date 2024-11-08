using System.Collections.Generic;

using UnityEngine;
using static UnityEngine.Mathf;

using ZombieSurvival.Characters;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.General.Breakpoints;
using ZombieSurvival.Objects;
using ZombieSurvival.Levels;
using ZombieSurvival.Upgrades;

using Zenject;
using ZombieSurvival.General.Inventories;

namespace ZombieSurvival.Spawners
{
    public abstract class EnemySpawner : Spawner, ILevelProgressUpdateHandler
    {
        [Header("Enemy spawner settings")]
        [Tooltip("Means distance between player and spawned objects")]
        [SerializeField] protected float _maxDistanceForRespawn;

        protected int _maxUnitsOnScene;
        protected int _totalSpawned;

        protected BreakpointList<UpgradeBreakpoint> _upgradeBreakpoints;

        protected Upgrade _currentUpgrade;
        protected List<Upgrade> _levelUpgrades;

        protected List<ObjectSpawner<Enemy>> _prevSpawners;
        protected List<ObjectSpawner<Enemy>> _currentSpawners;

        protected int CurrentSpawned
        {
            get
            {
                int spawned = 0;

                if (_currentSpawners != null)
                {
                    foreach (ObjectSpawner<Enemy> pool in _currentSpawners)
                    {
                        spawned += pool.SpawnCount;
                    }
                }

                if (_prevSpawners != null)
                {
                    foreach (ObjectSpawner<Enemy> pool in _prevSpawners)
                    {
                        spawned += pool.SpawnCount;
                    }
                }

                return spawned;
            }
        }

        [Inject] protected Player _player;
        [Inject] protected LevelContext _levelContext;
        [Inject] protected MainInventory _mainInventory;
        [Inject] protected CampInventory _campInventory;

        protected virtual void OnEnable()
        {
            EventBus.Subscribe(this);

            _upgradeBreakpoints = new BreakpointList<UpgradeBreakpoint>(_levelContext.EnemyUpgradeBreakpoints);

            _levelUpgrades = _levelContext.EnemiesUpgrades;

            _currentSpawners = new List<ObjectSpawner<Enemy>>();
            _prevSpawners = new List<ObjectSpawner<Enemy>>();
        }

        protected virtual void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public virtual void OnLevelProgressUpdate(int progress)
        {
            Breakpoint upgradeBreakpoint = _upgradeBreakpoints.CheckReaching(progress);

            if (upgradeBreakpoint != null)
            {
                if (_isDebug) Debug.Log("Enemy upgrade!");

                DispelUpgrades();

                _currentUpgrade = (upgradeBreakpoint as UpgradeBreakpoint).Upgrade;

                GetUpgrade();
            }
        }

        public override void OnUpdate()
        {
            if (_currentSpawners != null)
            {
                foreach (var spawner in _currentSpawners)
                {
                    if (spawner.SpawnCount == 0) continue;

                    for (int i = 0; i < spawner.SpawnCount; i++)
                    {
                        spawner.SpawnedObjects[i]?.OnUpdate();
                    }
                    spawner.SpawnedObjects.Cleanup();
                }
            }

            if (_prevSpawners != null)
            {
                foreach (var spawner in _prevSpawners)
                {
                    if (spawner.SpawnCount == 0) continue;

                    for (int i = 0; i < spawner.SpawnCount; i++)
                    {
                        spawner.SpawnedObjects[i]?.OnUpdate();
                    }

                    spawner.SpawnedObjects.Cleanup();

                    if (spawner.SpawnCount == 0)
                    {
                        spawner.ClearPool();
                    }
                }

                _prevSpawners.RemoveAll(item => item.SpawnedObjects == null);
            }
        }

        public override void OnFixedUpdate()
        {
            Vector3 playerPos = _player.transform.position;

            if (_currentSpawners != null)
            {
                foreach (var spawner in _currentSpawners)
                {
                    if (spawner.SpawnCount == 0) continue;

                    for (int i = 0; i < spawner.SpawnCount; i++)
                    {
                        if (spawner.SpawnedObjects[i] != null)
                        {
                            spawner.SpawnedObjects[i].Move(GetMoveDirection(playerPos, spawner.SpawnedObjects[i].transform.position));

                            spawner.SpawnedObjects[i]?.OnFixedUpdate();
                        }
                        else continue;
                    }

                    spawner.SpawnedObjects.Cleanup();
                }
            }

            if (_prevSpawners != null)
            {
                foreach (var spawner in _prevSpawners)
                {
                    if (spawner.SpawnCount == 0) continue;

                    for (int i = 0; i < spawner.SpawnCount; i++)
                    {
                        if (spawner.SpawnedObjects[i] != null)
                        {
                            spawner.SpawnedObjects[i].Move(GetMoveDirection(playerPos, spawner.SpawnedObjects[i].transform.position));

                            spawner.SpawnedObjects[i]?.OnFixedUpdate();
                        }
                        else continue;
                    }

                    spawner.SpawnedObjects.Cleanup();

                    if (spawner.SpawnCount == 0)
                    {
                        spawner.ClearPool();
                    }
                }

                _prevSpawners.RemoveAll(item => item.SpawnedObjects == null);
            }
        }

        protected virtual Vector3 GetMoveDirection(Vector3 playerPos, Vector3 enemyPos)
        {
            return new Vector3
                            (
                                playerPos.x - enemyPos.x,
                                0f,
                                playerPos.z - enemyPos.z
                            );
        }

        protected virtual Vector3 GetSpawnPosition()
        {
            Vector3 playerPos = _player.transform.position;

            float angle = Random.Range(0, 2 * PI);

            return new Vector3
                (
                    Cos(angle) * _spawnDeltaDistance + playerPos.x,
                    0f,
                    Sin(angle) * _spawnDeltaDistance + playerPos.z
                );
        }

        /// <summary>
        /// Removes all enemies from scene (spawned and enemies in pool)
        /// </summary>
        protected virtual void ClearPools()
        {
            if (_currentSpawners != null)
            {
                foreach (ObjectSpawner<Enemy> pool in _currentSpawners)
                {
                    pool.ClearPool();
                }

                _currentSpawners.Clear();
            }

            if (_prevSpawners != null)
            {
                foreach (ObjectSpawner<Enemy> pool in _prevSpawners)
                {
                    pool.ClearPool();
                }

                _prevSpawners.Clear();
            }

            _totalSpawned = 0;
        }

        /// <summary>
        /// Replace current pools to prevPools (it will be destroyed when all enemies return to pool)
        /// </summary>
        protected void ReplacePools()
        {
            if (_totalSpawned == 0) return;

            if (_currentSpawners != null)
            {
                if (_prevSpawners == null)
                {
                    _prevSpawners = new List<ObjectSpawner<Enemy>>();
                }

                foreach (ObjectSpawner<Enemy> pool in _currentSpawners)
                {
                    if (pool.SpawnCount == 0)
                    {
                        pool.ClearPool();
                    }
                    else
                    {
                        _prevSpawners.Add(pool);
                    }
                }

                _currentSpawners.Clear();
            }
            else
            {
                _currentSpawners = new List<ObjectSpawner<Enemy>>();
            }
        }

        /// <summary>
        /// Add upgrade to enemies (spawned and enemies in pool)
        /// </summary>
        /// <param name="upgradeAll">If true, also give upgrades to previous waves</param>
        protected virtual void GetUpgrade(bool upgradeAll = false)
        {
            if (_currentSpawners != null)
            {
                foreach (ObjectSpawner<Enemy> pool in _currentSpawners)
                {
                    foreach (Enemy zombie in pool.Objects)
                    {
                        zombie?.GetUpgrade(_currentUpgrade);

                        foreach (Upgrade levelUpgrade in _levelUpgrades)
                        {
                            zombie?.GetUpgrade(levelUpgrade);
                        }

                        foreach (Upgrade campUpgrade in _campInventory.CampUpgrades)
                        {
                            zombie?.GetUpgrade(campUpgrade);
                        }
                    }

                    foreach (Enemy zombie in pool.SpawnedObjects.List)
                    {
                        zombie?.GetUpgrade(_currentUpgrade);

                        foreach (Upgrade levelUpgrade in _levelUpgrades)
                        {
                            zombie?.GetUpgrade(levelUpgrade);
                        }

                        foreach (Upgrade campUpgrade in _campInventory.CampUpgrades)
                        {
                            zombie?.GetUpgrade(campUpgrade);
                        }
                    }
                }
            }

            if (_prevSpawners != null && upgradeAll)
            {
                foreach (ObjectSpawner<Enemy> pool in _prevSpawners)
                {
                    foreach (Enemy zombie in pool.Objects)
                    {
                        zombie?.GetUpgrade(_currentUpgrade);

                        foreach (Upgrade levelUpgrade in _levelUpgrades)
                        {
                            zombie?.GetUpgrade(levelUpgrade);
                        }

                        foreach (Upgrade campUpgrade in _campInventory.CampUpgrades)
                        {
                            zombie?.GetUpgrade(campUpgrade);
                        }
                    }

                    foreach (Enemy zombie in pool.SpawnedObjects.List)
                    {
                        zombie?.GetUpgrade(_currentUpgrade);

                        foreach (Upgrade levelUpgrade in _levelUpgrades)
                        {
                            zombie?.GetUpgrade(levelUpgrade);
                        }

                        foreach (Upgrade campUpgrade in _campInventory.CampUpgrades)
                        {
                            zombie?.GetUpgrade(campUpgrade);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Dispel upgrade from enemies (spawned and enemies in pool)
        /// </summary>
        /// <param name="dispelAll">If true, also dispel upgrades from previous waves</param>
        protected virtual void DispelUpgrades(bool dispelAll = false)
        {
            if (_currentSpawners != null)
            {
                foreach (ObjectSpawner<Enemy> pool in _currentSpawners)
                {
                    foreach (Enemy zombie in pool.Objects)
                    {
                        zombie?.DispelUpgrade(_currentUpgrade);

                        foreach (Upgrade levelUpgrade in _levelUpgrades)
                        {
                            zombie?.DispelUpgrade(levelUpgrade);
                        }

                        foreach (Upgrade campUpgrade in _campInventory.CampUpgrades)
                        {
                            zombie?.DispelUpgrade(campUpgrade);
                        }
                    }

                    foreach (Enemy zombie in pool.SpawnedObjects.List)
                    {
                        zombie?.DispelUpgrade(_currentUpgrade);

                        foreach (Upgrade levelUpgrade in _levelUpgrades)
                        {
                            zombie?.DispelUpgrade(levelUpgrade);
                        }

                        foreach (Upgrade campUpgrade in _campInventory.CampUpgrades)
                        {
                            zombie?.DispelUpgrade(campUpgrade);
                        }
                    }
                }
            }

            if (_prevSpawners != null && dispelAll)
            {
                foreach (ObjectSpawner<Enemy> pool in _prevSpawners)
                {
                    foreach (Enemy zombie in pool.Objects)
                    {
                        zombie?.DispelUpgrade(_currentUpgrade);

                        foreach (Upgrade levelUpgrade in _levelUpgrades)
                        {
                            zombie?.DispelUpgrade(levelUpgrade);
                        }

                        foreach (Upgrade campUpgrade in _campInventory.CampUpgrades)
                        {
                            zombie?.DispelUpgrade(campUpgrade);
                        }
                    }

                    foreach (Enemy zombie in pool.SpawnedObjects.List)
                    {
                        zombie?.DispelUpgrade(_currentUpgrade);

                        foreach (Upgrade levelUpgrade in _levelUpgrades)
                        {
                            zombie?.DispelUpgrade(levelUpgrade);
                        }

                        foreach (Upgrade campUpgrade in _campInventory.CampUpgrades)
                        {
                            zombie?.DispelUpgrade(campUpgrade);
                        }
                    }
                }
            }
        }
    }
}