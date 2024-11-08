using UnityEngine;
using ZombieSurvival.Abilities;
using ZombieSurvival.Stats;

namespace ZombieSurvival.Objects.Pickables
{
    [RequireComponent(typeof(SphereCollider))]
    public class PickableMagnet : PickableObject
    {
        [Header("Magnet settings")]
        [SerializeField] private Radius _magnetRadius;
        [SerializeField] private TargetDetector _targetDetector;

        public override void Initialize(MonoPool<PickableObject> pool)
        {
            base.Initialize(pool);

            _magnetRadius.Initialize();
            _targetDetector.Initialize(_magnetRadius);
        }

        protected override void OnPickUp(float releaseDelay = 0)
        {
            foreach (var target in _targetDetector.Targets)
            {
                PickableObject obj = target.GetComponent<PickableObject>();

                if (obj != null)
                {
                    obj.PickUp(_target);
                }
            }

            base.OnPickUp(releaseDelay);
        }
    }
}