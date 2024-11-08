using UnityEngine;
using static UnityEngine.Mathf;

using ZombieSurvival.Characters;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.General.Breakpoints;
using ZombieSurvival.Objects;
using ZombieSurvival.Objects.Pickables;
using ZombieSurvival.Rewards;
using ZombieSurvival.UI.GameMenus;

using Zenject;

namespace ZombieSurvival.Spawners
{
    public sealed class BossSpawner : EnemySpawner
    {
        [Header("Boss spawner settings")]
        [SerializeField][Range(1, 5)] private int _spawnCount = 1;

        [SerializeField][Range(1, 3)] private float _rewardsSpawnDistanceMultiplier = 1.5f;

        private GameObject _fence;

        private BreakpointList<BossBreakpoint> _breakpoints;
        private BossBreakpoint _currentBreakpoint;

        private const float PICKABLES_SPAWN_AXIS = PI * 0.5f;

        [Inject] private GameMenu _gameMenu;

        protected override void OnEnable()
        {
            base.OnEnable();

            _breakpoints = new BreakpointList<BossBreakpoint>(_levelContext.BossBreakpoints);
        }

        public override void OnUpdate()
        {
            if (CurrentSpawned > 0)
            {
                base.OnUpdate();
            }
            else return;
        }

        public override void OnFixedUpdate()
        {
            if (CurrentSpawned > 0)
            {
                base.OnFixedUpdate();
            }
            else return;
        }

        public override void OnLevelProgressUpdate(int progress)
        {
            Breakpoint breakpoint = _breakpoints.CheckReaching(progress);

            if (breakpoint != null)
            {
                _currentBreakpoint = breakpoint as BossBreakpoint;

                if (_isDebug) Debug.Log("Boss event incoming!");

                EventBus.Publish<IBossEventHandler>(handler => handler.OnBossEvent());

                Vector3 position = _player.transform.position;
                _currentSpawners.Add(new ObjectSpawner<Enemy>(_currentBreakpoint.BossPrefab, _spawnCount));

                SpawnFence(position, _currentBreakpoint.BossFence);

                for (int i = 0; i < _spawnCount; i++)
                {
                    Spawn(GetSpawnPosition());
                }

                DispelUpgrades();

                _currentUpgrade = _currentBreakpoint.BossUpgrade;

                GetUpgrade();
            }
        }

        public void OnBossDies(Vector3 position)
        {
            ClearPools();

            if (_fence != null)
            {
                Destroy(_fence);
                _fence = null;
            }

            if (_currentBreakpoint.BossRewards.Rewards.Count > 0)
            {
                if (_isDebug) Debug.Log("Get boss reward");

                Vector3 pickablesPos = new Vector3(position.x, _levelContext.LevelBuilder.GridHeight, position.z);


                float delta = 2 * PI / _currentBreakpoint.BossRewards.Rewards.FindAll(item => item as PickableReward != null ||
                                                                                      item as ZombieChestReward != null).Count;
                int currentPickableIndex = 0;

                foreach (ConcreteReward concreteReward in _currentBreakpoint.BossRewards.Rewards)
                {
                    Vector3 spawnPos = pickablesPos + new Vector3
                                (
                                    Cos(PICKABLES_SPAWN_AXIS + delta * currentPickableIndex) * _rewardsSpawnDistanceMultiplier,
                                    0,
                                    Sin(PICKABLES_SPAWN_AXIS + delta * currentPickableIndex) * _rewardsSpawnDistanceMultiplier
                                );

                    #region NEED REWORK
                    if (concreteReward is PickableReward pickable)
                    {
                        if (pickable.Pickable is ExpCrystal expCrystal)
                        {
                            EventBus.Publish<ISpawnExpCrystalHandler>(handler => 
                                handler.OnSpawnExpCrystal(spawnPos));
                        }
                        else if (pickable.Pickable is MonoPickableReward pickableReward)
                        {
                            EventBus.Publish<ISpawnPickableRewardHandler>(handler => 
                                handler.OnSpawnPickableReward(pickableReward, spawnPos));
                        }
                        else
                        {
                            EventBus.Publish<ISpawnCommonPickableHandler>(handler => 
                                handler.OnSpawnCommonPickable(pickable.Pickable, spawnPos));
                        }

                        currentPickableIndex++;
                    }
                    else if (concreteReward is ZombieChestReward chestReward)
                    {
                        EventBus.Publish<ISpawnAbilityChestHandler>(handler =>
                            handler.OnSpawnAbilityChest(spawnPos, chestReward.Amount));

                        currentPickableIndex++;
                    }
                    else // default rewards
                    {
                        _mainInventory.Add(concreteReward);

                        _gameMenu.AddRewards(concreteReward);
                    }
                    #endregion
                }
            }

            if (_isDebug) Debug.Log("Boss event ended");

            EventBus.Publish<IBossEventEndedHandler>(handler => handler.OnBossEventEnd());
        }

        protected override void Spawn(Vector3 position)
        {
            if (_currentSpawners != null && _currentSpawners.Count > 0)
            {
                Enemy boss = _currentSpawners[0].Spawn(new Vector3
                (
                    position.x,
                    _levelContext.LevelBuilder.GridHeight +
                        _currentSpawners[0].Prefab.Collider.height * _currentSpawners[0].Prefab.transform.localScale.y * 0.5f,
                    position.z + _spawnDeltaDistance
                ));

                boss.Initialize(_player, _currentSpawners[0]);
                (boss as BossZombie).InitializeSpawner(this);

                _totalSpawned++;
            }
        }

        protected override Vector3 GetSpawnPosition()
        {
            Vector3 playerPos = _player.transform.position;

            return new Vector3
                (
                    playerPos.x,
                    0f,
                    playerPos.z
                ) + Vector3.forward;
        }

        private void SpawnFence(Vector3 position, GameObject fence)
        {
            if (fence != null)
            {
                _fence = Instantiate(fence,
                                     new Vector3
                                     (
                                         position.x,
                                         _levelContext.LevelBuilder.GridHeight + fence.transform.localScale.y * 0.5f,
                                         position.z
                                     ), fence.transform.localRotation, transform);
            }
        }
    }
}