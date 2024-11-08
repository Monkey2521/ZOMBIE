using UnityEngine;

namespace ZombieSurvival.Objects.Indication
{
    public sealed class BurnedSpriteIndicator : ObjectDamagedIndicator
    {
        [Header("Burned sprite indicator settings")]
        [SerializeField] private SpriteRenderer _spriteToBurn;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _burnedMaterial;

        protected override void OnIndicationStart(float intensivity)
        {
            _spriteToBurn.material = _burnedMaterial;
        }

        protected override void OnIndicationEnd()
        {
            if (_indicationObject.IsDied) return;

            _spriteToBurn.material = _defaultMaterial;
        }
    }
}