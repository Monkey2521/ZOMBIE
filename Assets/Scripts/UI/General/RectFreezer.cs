using UnityEngine;

using ZombieSurvival.General;

namespace ZombieSurvival.UI.General
{
    public class RectFreezer : MonoBehaviour
    {
        [SerializeField] private RectTransform _transformToScale;
        [SerializeField] private Vector2 _referencedSize;
        [SerializeField] private bool _scaleLocalPosition;

        private void Awake()
        {
            _transformToScale.sizeDelta = new Vector2
                (
                    _referencedSize.x * ScreenScaler.MinDelta,
                    _referencedSize.y * ScreenScaler.MinDelta
                );

            if (_scaleLocalPosition)
            {
                Vector2 pos = _transformToScale.anchoredPosition;

                _transformToScale.anchoredPosition = new Vector2
                    (
                        pos.x * ScreenScaler.MinDelta,
                        pos.y * ScreenScaler.MinDelta
                    );
            }
        }
    }
}