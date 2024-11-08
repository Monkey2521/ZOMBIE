using UnityEngine;
using UnityEngine.UI;

namespace ZombieSurvival.Objects.Indication
{
    public sealed class ScreenIndicator : ObjectDamagedIndicator
    {
        [Header("Screen indicator settings")]
        [SerializeField] private Image _panelImage;
        [SerializeField] private Color _defalutColor;

        private void OnEnable()
        {
            _panelImage.color = _defalutColor;
        }

        protected override void OnIndicationStart(float intensivity)
        {
            Color color = _defalutColor;

            color.a = intensivity;

            _panelImage.color = color;
        }

        protected override void OnIndicationEnd()
        {
            _panelImage.color = _defalutColor;
        }
    }
}