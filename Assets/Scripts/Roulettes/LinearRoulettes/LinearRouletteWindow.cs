using UnityEngine;

namespace ZombieSurvival.UI.Shop
{
    public class LinearRouletteWindow : RouletteWindow 
    {
        public override void Initialize(float speed, float speedDecreasePerFrame, float speedDecreaseMultiplier, 
                                        Vector3 slotsParentPosition, Vector3 moveDirection)
        {
            _slotsParent.localPosition = slotsParentPosition;

            base.Initialize(speed, speedDecreasePerFrame, speedDecreaseMultiplier, slotsParentPosition, moveDirection);
        }

        /// <summary>
        /// Must be called in FixedUpdate
        /// </summary>
        public override void MoveSlots()
        {
            if (!OnMove) return;

            Vector3 parentPos = _slotsParent.localPosition;

            _slotsParent.localPosition = Vector3.MoveTowards
                (
                    parentPos,
                    parentPos + _moveDirection * _speed,
                    _speed * Time.fixedUnscaledDeltaTime
                );

            base.MoveSlots();
        }
    }
}