using UnityEngine;

namespace ZombieSurvival.UI.Shop
{
    public struct CircleRouletteSlotData
    {
        public Sprite SegmentSprite { get; }
        public int Segments { get; }
        public float StartAngle { get; }
        public float SegmentAngle { get; }
        public float Radius { get; }
        public bool FillSegmentByAngle { get; }

        public CircleRouletteSlotData(Sprite segmentSprite, int segments, float startAngle, 
                                      float segmentAngle, float radius, bool fillSegmentByAngle)
        {
            SegmentSprite = segmentSprite;
            Segments = segments;
            StartAngle = startAngle;
            SegmentAngle = segmentAngle;
            Radius = radius;
            FillSegmentByAngle = fillSegmentByAngle;
        }
    }
}