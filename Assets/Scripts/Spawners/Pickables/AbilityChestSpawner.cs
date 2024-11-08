using UnityEngine;

using ZombieSurvival.Events;
using ZombieSurvival.Objects;
using ZombieSurvival.Objects.Pickables;
using ZombieSurvival.UI.Abilities;

using Zenject;

namespace ZombieSurvival.Spawners
{
    public sealed class AbilityChestSpawner : PickableObjectSpawner, ISpawnAbilityChestHandler
    {
        [Header("Pickable reward spawner settings")]
        [SerializeField] private AbilityChest _abilityChestPrefab;
        [SerializeField][Range(1, 5)] private int _defaultMaxAbilitiesCount = 1;

        private int _currentMaxCount;

        public override PickableObject PickableObject => _abilityChestPrefab;

        [Inject] private AbilityGiver _abilityGiver;

        protected override void OnEnable()
        {
            base.OnEnable();

            EventBus.Subscribe(this);

            _currentMaxCount = _defaultMaxAbilitiesCount;
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EventBus.Unsubscribe(this);
        }

        public void OnSpawnAbilityChest(Vector3 position, int maxAbilitiesCount)
        {
            _currentMaxCount = maxAbilitiesCount;

            Spawn(position);
        }

        protected override void Spawn(Vector3 position)
        {
            PickableObject obj = _pool.Spawn(position);
            
            if (obj is AbilityChest chest)
            {
                chest.Initialize(_pool, _abilityGiver, _currentMaxCount);
            }
            else
            {
                if (_isDebug) Debug.Log(name + ": Missing ability chest!");

                _pool.Release(obj);
            }

            _currentMaxCount = _defaultMaxAbilitiesCount;
        }
    }
}