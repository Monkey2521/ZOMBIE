using UnityEngine;
using ZombieSurvival.UI.Shop;

namespace ZombieSurvival.Rewards
{
    public abstract class CircleRouletteReward : ConcreteReward
    {
        protected CircleRouletteSlotData _slotData;

        public CircleRouletteSlotData SlotData => _slotData;

        protected CircleRouletteReward(Sprite icon, Sprite background, CircleRouletteSlotData slotData, int amount = 1) : 
            base(icon, background, amount)
        {
            _slotData = slotData;
        }
    }
}
