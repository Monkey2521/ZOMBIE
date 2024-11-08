using UnityEngine;

namespace ZombieSurvival.UI.Shop
{
    public class CircleRouletteWindow : RouletteWindow
    {
        public override void MoveSlots()
        {
            if (!OnMove) return;

            _slotsParent.transform.Rotate(0f, 0f, _speed * Time.fixedDeltaTime);

            base.MoveSlots();
        }
    }
}