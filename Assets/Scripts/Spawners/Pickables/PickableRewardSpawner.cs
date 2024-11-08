using UnityEngine;

using ZombieSurvival.Events;
using ZombieSurvival.Objects;
using ZombieSurvival.Objects.Pickables;
using ZombieSurvival.UI.GameMenus;

using Zenject;

namespace ZombieSurvival.Spawners
{
    public sealed class PickableRewardSpawner : PickableObjectSpawner, ISpawnPickableRewardHandler
    {
        [Header("Pickable reward spawner settings")]
        [SerializeField] private MonoPickableReward _pickableRewardPrefab;

        public override PickableObject PickableObject => _pickableRewardPrefab;

        [Inject] private GameMenu _gameMenu;

        protected override void OnEnable()
        {
            base.OnEnable();

            EventBus.Subscribe(this);
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            EventBus.Unsubscribe(this);
        }

        public void OnSpawnPickableReward(MonoPickableReward reward, Vector3 position)
        {
            if (reward.Equals(_pickableRewardPrefab))
            {
                Spawn(position);
            }
        }

        protected override void Spawn(Vector3 position)
        {
            PickableObject obj = _pool.Spawn(position);

            if (obj is MonoPickableReward reward)
            {
                reward.Initialize(_pool, _gameMenu);
            }
            else
            {
                if (_isDebug) Debug.Log(name + ": Missing reward!");

                _pool.Release(obj);
            }
        }
    }
}