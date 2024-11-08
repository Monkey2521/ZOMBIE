using UnityEngine;
using UnityEngine.UI;

namespace ZombieSurvival.General.Breakpoints
{
    public class LevelProgressBreakpoint : MonoBehaviour
    {
        [SerializeField] private RectTransform _transform;

        [SerializeField] private Image _iconImage;

        public RectTransform Transform => _transform;

        public void SetBreakpoint(Breakpoint breakpoint)
        {
            if (breakpoint.Icon != null)
            {
                _iconImage.sprite = breakpoint.Icon;
            }
            else return;
        }
    }
}