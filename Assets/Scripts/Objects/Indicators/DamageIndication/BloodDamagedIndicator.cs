using UnityEngine;

namespace ZombieSurvival.Objects.Indication
{
    public sealed class BloodDamagedIndicator : ObjectDamagedIndicator
    {
        [Header("Blood indicator settings")]
        [SerializeField] private ParticleSystem _bloodParticle;

        protected override void OnIndicationStart(float intensivity)
        {
            _bloodParticle.Play();
        }

        protected override void OnIndicationEnd()
        {
            _bloodParticle.Stop();
        }
    }
}