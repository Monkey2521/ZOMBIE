using UnityEngine;

namespace ZombieSurvival.UI.Shop
{
    public abstract class CirckeRouletteChance
    {
        [SerializeField] private Sprite _segmentSprite;
        [SerializeField] private bool _fillSegmentByAngle;

        public abstract Sprite RewardBackground { get; }
        public Sprite SegmentSprite => _segmentSprite;
        public bool FillSegmentByAngle => _fillSegmentByAngle;
    }
}