using System.Collections.Generic;

using UnityEngine;
using static UnityEngine.Mathf;

using ZombieSurvival.Characters;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.General.Breakpoints;
using ZombieSurvival.Objects;
using ZombieSurvival.Objects.Pickables;
using ZombieSurvival.Levels;
using ZombieSurvival.Stats;

using Zenject;

namespace ZombieSurvival.Spawners
{
    public sealed class CrystalSpawner : PickableObjectSpawner, 
        ILevelProgressUpdateHandler, IEnemyKilledHandler, IGameStartHandler, ISpawnExpCrystalHandler
    {
        [Header("Crystal spawner settings")]
        [SerializeField] private ExpCrystal _crystalPrefab;
        [SerializeField][Range(2, 6)] private float _startSpawnMinRange = 2f;
        [SerializeField][Range(4, 10)] private float _startSpawnMaxRange = 5f;

        private ChanceCombiner<CrystalParam> _spawnCombiner;
        private BreakpointList<CrystalBreakpoint> _breakpoints;

        public override PickableObject PickableObject => _crystalPrefab;

        [Inject] private Player _player;
        [Inject] private LevelContext _levelContext;

        protected override void OnEnable()
        {
            base.OnEnable();

            EventBus.Subscribe(this); 
            
            _breakpoints = new BreakpointList<CrystalBreakpoint>(_levelContext.CrystalSpawnBreakpoints);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EventBus.Unsubscribe(this);
        }

        public void OnGameStart()
        {
            _pool = new ObjectSpawner<PickableObject>(_crystalPrefab, 
                                                      _levelContext.StartCrystalsCount);

            _spawnCombiner = new ChanceCombiner<CrystalParam>(_levelContext.StartCrystalStats.CrystalSpawnParams);

            Vector3 playerPos = _player.transform.position;

            for (int i = 0; i < _levelContext.StartCrystalsCount; i++)
            {
                float angle = Random.Range(0f, 2 * PI);
                float range = GetStartRange();

                Vector3 position = new Vector3
                    (
                        Cos(angle) * range + playerPos.x,
                        _levelContext.LevelBuilder.GridHeight + _spawnDeltaDistance,
                        Sin(angle) * range + playerPos.z
                    );

                Spawn(position);
            }
        }

        private float GetStartRange() => Random.Range(_startSpawnMinRange, _startSpawnMaxRange);

        public void OnLevelProgressUpdate(int progress)
        {
            Breakpoint breakpoint = _breakpoints.CheckReaching(progress);

            if (breakpoint != null)
            {
                OnCrystalBreakpoint(breakpoint as CrystalBreakpoint);
            }
        }

        private void OnCrystalBreakpoint(CrystalBreakpoint breakpoint)
        {
            if (_isDebug) Debug.Log("Get breakpoint: " + breakpoint.Name);

            List<PickableObject> crystals = null;

            if (_pool != null)
            {
                _pool.SpawnedObjects.Cleanup();
                crystals = _pool.SpawnedObjects.List;

                foreach (PickableObject crystal in crystals)
                {
                    crystal.transform.parent = null;
                }
            }

            ClearPool();

            _pool = new ObjectSpawner<PickableObject>(_crystalPrefab, _poolSize);
            _spawnCombiner = new ChanceCombiner<CrystalParam>(breakpoint.SpawningCrystalsStats.CrystalSpawnParams);

            if (crystals != null)
            {
                foreach (PickableObject crystal in crystals)
                {
                    if (crystal == null) continue;

                    crystal.ChangePool(_pool);

                    _pool.AddObject(crystal);
                }

                crystals.Clear();
            }
        }

        /// <summary>
        /// Create ExpCrystal in position that Enemy dies
        /// </summary>
        /// <param name="zombie"></param>
        public void OnEnemyKilled(Enemy enemy)
        {
            if (!enemy.HasExpReward) return;

            Spawn(new Vector3
                (
                    enemy.transform.position.x,
                    _levelContext.LevelBuilder.GridHeight + _spawnDeltaDistance,
                    enemy.transform.position.z
                ));
        }

        public void OnSpawnExpCrystal(Vector3 position)
        {
            Spawn(new Vector3
                (
                    position.x,
                    _levelContext.LevelBuilder.GridHeight + _spawnDeltaDistance,
                    position.z
                ));
        }

        protected override void Spawn(Vector3 position)
        {
            PickableObject obj = _pool.Spawn(position);
            
            if (obj is ExpCrystal crystal)
            {
                crystal.Initialize(_pool, _spawnCombiner.GetStrikedObject());
            }
            else
            {
                if (_isDebug) Debug.Log(name + ": Missing crystal!");

                _pool.Release(obj);
            }
        }
    }
}