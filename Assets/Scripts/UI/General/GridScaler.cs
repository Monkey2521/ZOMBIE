using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;

namespace ZombieSurvival.UI.General
{
    [RequireComponent(typeof(GridLayoutGroup))]
    public class GridScaler : MonoBehaviour
    {
        [SerializeField] private RectTransform _gridTransform;

        [Space(5)]
        [SerializeField] private GridLayoutGroup _grid;
        [SerializeField] private Vector2 _referencedCellSize;
        [SerializeField] private Vector2 _referencedSpacing;
        [SerializeField] private Vector4 _referencedPadding;

        private void Awake()
        {
            float min = ScreenScaler.MinDelta;

            _grid.cellSize = new Vector2(_referencedCellSize.x * min, _referencedCellSize.y * min);
            _grid.spacing = new Vector2(_referencedSpacing.x * min, _referencedSpacing.y * min);

            _grid.padding.left = (int)(_referencedPadding.x * min);
            _grid.padding.right = (int)(_referencedPadding.y * min);
            _grid.padding.top = (int)(_referencedPadding.z * min);
            _grid.padding.bottom = (int)(_referencedPadding.w * min);
        }
    }
}