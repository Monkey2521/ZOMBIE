using System.Collections.Generic;

using UnityEngine;

namespace ZombieSurvival.Objects.Indication
{
    public abstract class ObjectDieIndicator : ObjectIndicator
    {
        [Header("ObjectDie indicator settings")]
        [SerializeField] protected List<SpriteRenderer> _spritesToDisable;
        [SerializeField] protected List<Collider> _collidersToDisable;

        protected override void OnIndicationStart(float intensivity)
        {
            foreach (SpriteRenderer rendered in _spritesToDisable)
            {
                rendered.enabled = false;
            }

            foreach (Collider collider in _collidersToDisable)
            {
                collider.enabled = false;
            }
        }

        protected override void OnIndicationEnd()
        {
            foreach (SpriteRenderer rendered in _spritesToDisable)
            {
                rendered.enabled = true;
            }

            foreach (Collider collider in _collidersToDisable)
            {
                collider.enabled = true;
            }
        }

        protected override float GetIndicationIntensivity() => MAX_INDICATION_INTENSIVITY;
    }
}