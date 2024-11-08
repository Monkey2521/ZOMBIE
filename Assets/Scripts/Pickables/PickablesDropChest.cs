using UnityEngine;
using ZombieSurvival.General;
using ZombieSurvival.Interfaces;
using ZombieSurvival.Spawners;
using ZombieSurvival.Stats;

namespace ZombieSurvival.Objects.Pickables
{
    [RequireComponent(typeof(Collider))]
    public class PickablesDropChest : DamageableObject, IPoolable
    {
        [Header("Drop chest settings")]
        [SerializeField] private DamageableObjectStats _stats;
        [SerializeField] private TagList _destroyTags;
        [SerializeField] private Collider _collider;

        private DropChestSpawner _chestSpawner;

        public override DamageableObjectStats Stats => _stats;

        public void Initialize(DropChestSpawner chestSpawner)
        {
            _chestSpawner = chestSpawner;
            _collider.isTrigger = true;

            _stats.Initialize();
        }

        public void ResetObject()
        {
            _chestSpawner = null;
        }

        public override void Die(bool instantly = false)
        {
            base.Die(instantly);

            if (_chestSpawner != null)
            {
                _chestSpawner.OnChestDestoyed(this);
            }
            else
            {
                if (_isDebug) Debug.Log(name + ": Missing ChestSpawner!");

                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (_destroyTags.Contains(other.tag))
            {
                Die();
            }
        }
    }
}