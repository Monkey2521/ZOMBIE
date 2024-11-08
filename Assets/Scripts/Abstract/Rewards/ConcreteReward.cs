using UnityEngine;

namespace ZombieSurvival.Rewards
{
    [System.Serializable]
    public abstract class ConcreteReward
    {
        protected Sprite _icon;
        protected Sprite _background;
        protected int _amount;

        public Sprite Icon => _icon;
        public Sprite Background => _background;
        public int Amount => _amount;

        public ConcreteReward (Sprite icon, Sprite background, int amount = 1)
        {
            _icon = icon;
            _background = background;
            _amount = amount;
        }

        public abstract bool AbleToMerge(ConcreteReward other);

        public virtual bool Merge(ConcreteReward other)
        {
            if (AbleToMerge(other))
            {
                _amount += other.Amount;

                return true;
            }

            return false;
        }

        public abstract ConcreteReward Clone();
    }
}
