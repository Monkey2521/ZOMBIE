using System.Collections.Generic;
using UnityEngine;
using ZombieSurvival.Abilities;
using ZombieSurvival.Stats;

namespace ZombieSurvival.Objects.Pickables
{
    public class AreaCleaner : PickableObject
    {
        [SerializeField] private Radius _explosionRadius;
        [SerializeField] private TargetDetector _targetDetector;
        [SerializeField] private ParticleSystem _particle;
        [SerializeField] private float _releaseDelay = 1f;

        public override void Initialize(MonoPool<PickableObject> pool)
        {
            base.Initialize(pool);

            _explosionRadius.Initialize();
            _targetDetector.Initialize(_explosionRadius);
            _pickUpCollider.enabled = true;
        }

        protected override void OnPickUp(float releaseDelay = 0)
        {
            base.OnPickUp(_releaseDelay);

            _pickUpCollider.enabled = false;

            _particle.Play();

            List<GameObject> targets = _targetDetector.Targets;

            if (targets.Count == 0)
            {
                return;
            }

            for (int i = 0; i < targets.Count; i++)
            {
                if (targets[i] == null) continue;

                DamageableObject target = targets[i].GetComponent<DamageableObject>();

                if (target != null)
                {
                    target.Die();
                }
            }
        }
    }
}