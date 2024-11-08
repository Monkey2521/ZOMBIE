using UnityEngine;

using ZombieSurvival.General;

namespace ZombieSurvival.UI.Shop
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BoxRoulleteSlot : RouletteSlot
    {
        [Header("Box roulette slot settings")]
        [SerializeField] protected BoxCollider2D _collider;

        private void Awake()
        {
            if (_collider is BoxCollider2D collider)
            {
                collider.size = new Vector2(collider.size.x * ScreenScaler.MinDelta, collider.size.y * ScreenScaler.MinDelta);
            }
            else if (_isDebug) Debug.Log(name + ": missing collider error!");
        }
    }
}