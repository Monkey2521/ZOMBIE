using UnityEngine;

using ZombieSurvival.Stats;

namespace ZombieSurvival.Rewards
{
    public sealed class ExpirienceReward : ConcreteReward
    {
        private Expirience _exp;

        public Expirience Expirience => _exp;

        public ExpirienceReward(Expirience expirience, Sprite icon, Sprite background) : 
            base(icon, background, (int)expirience.Value) 
        {
            _exp = expirience;
        }
        
        public ExpirienceReward(Expirience expirience, Sprite icon, Sprite background, int amount) : 
            base(icon, background, amount) 
        {
            _exp = expirience;
        }

        public override bool AbleToMerge(ConcreteReward other)
        {
            return other is ExpirienceReward;
        }

        public override ConcreteReward Clone() => new ExpirienceReward(_exp, _icon, _background, _amount);
    }
}
