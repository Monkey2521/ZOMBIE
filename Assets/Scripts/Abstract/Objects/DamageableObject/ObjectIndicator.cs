using System.Collections;

using UnityEngine;

using ZombieSurvival.General;

namespace ZombieSurvival.Objects.Indication
{
    public abstract class ObjectIndicator : ZSMonoBehaviour 
    {
        [Header("Object indicator settings")]
        [SerializeField] protected bool _indicationInUnscaledTime;
        [SerializeField][Range(0.01f, 2f)] protected float _indicationTime;
        [SerializeField][Range(0f, 1f)] protected float _minIndicationIntensivity;

        protected const float MAX_INDICATION_INTENSIVITY = 1;

        public float IndicationTime => _indicationTime;

        protected virtual void OnDisable()
        {
            OnIndicationEnd();

            StopAllCoroutines();
        }

        public virtual void OnIndication()
        {
            StopAllCoroutines();

            OnIndicationStart(GetIndicationIntensivity());

            StartCoroutine(WaitIndicationEnd());
        }

        protected abstract float GetIndicationIntensivity();

        protected IEnumerator WaitIndicationEnd()
        {
            if (_indicationInUnscaledTime)
            {
                yield return new WaitForSecondsRealtime(_indicationTime);
            }
            else
            {
                yield return new WaitForSeconds(_indicationTime);
            }

            OnIndicationEnd();
        }

        protected abstract void OnIndicationStart(float intensivity);

        protected abstract void OnIndicationEnd();
    }
}