using UnityEngine;

namespace ZombieSurvival.Objects.Indication
{
    public sealed class BloodDieIndicator : ObjectDieIndicator
    {
        [Header("Blood indicator settings")]
        [SerializeField] private ParticleSystem _bloodParticle;

        protected override void OnIndicationStart(float intensivity)
        {
            base.OnIndicationStart(intensivity);

            _bloodParticle.Play();
        }

        protected override void OnIndicationEnd()
        {
            base.OnIndicationEnd();

            _bloodParticle.Stop();
        }
    }
}