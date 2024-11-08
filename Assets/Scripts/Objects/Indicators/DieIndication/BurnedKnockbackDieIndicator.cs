using System.Collections;
using UnityEngine;

namespace ZombieSurvival.Objects.Indication
{
    public sealed class BurnedKnockbackDieIndicator : ObjectDieIndicator
    {
        [Header("Burned knockback indicator settings")]
        [SerializeField] private Transform _transformToKnockback;
        [SerializeField] private float _knockbackSpeed;

        [Space(5)]
        [SerializeField] private SpriteRenderer _spriteToBurn;
        [SerializeField] private Material _defaultMaterial;
        [SerializeField] private Material _burnedMaterial;

        protected override void OnIndicationStart(float intensivity)
        {
            base.OnIndicationStart(intensivity);

            _spriteToBurn.material = _burnedMaterial;

            StartCoroutine(WaitFixedUpdate());
        }

        protected override void OnIndicationEnd()
        {
            base.OnIndicationEnd();

            _spriteToBurn.material = _defaultMaterial;
        }

        private IEnumerator WaitFixedUpdate()
        {
            KnockbackTransform();

            if (_indicationInUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(Time.fixedUnscaledDeltaTime);
            }
            else
            {
                yield return new WaitForSeconds(Time.fixedDeltaTime);
            }

            StartCoroutine(WaitFixedUpdate());
        }

        private void KnockbackTransform()
        {
            Vector3 pos = _transformToKnockback.position;

            _transformToKnockback.position = Vector3.MoveTowards
                (
                    pos,
                    pos + _transformToKnockback.TransformDirection(Vector3.back) * _knockbackSpeed,
                    _knockbackSpeed * (_indicationInUnscaledTime ? Time.fixedUnscaledDeltaTime : Time.fixedDeltaTime)
                );
        }
    }
}