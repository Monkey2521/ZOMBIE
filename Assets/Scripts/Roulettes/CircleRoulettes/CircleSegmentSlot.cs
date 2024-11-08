using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Mathf;

using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Shop
{
    public class CircleSegmentSlot : RouletteSlot
    {
        [Header("Circle segment slot settins")]
        [SerializeField] private RectTransform _selfTransform;
        [SerializeField] private RectTransform _backgroundTransform;
        [SerializeField] private Image _slotBackground;
        [SerializeField] private EdgeCollider2D _collider;

        protected override void InitializeSlot()
        {
            base.InitializeSlot();

            transform.localPosition = Vector3.zero;
            transform.localRotation = new Quaternion();

            if (_reward is CircleRouletteReward reward)
            {
                UpdateSegment(reward.SlotData);
            }
#if DEBUG
            else if (_isDebug) Debug.Log(name + ": Missing circle reward!");
#endif
        }

        public void UpdateSegment(CircleRouletteSlotData slotData)
        {
            _slotBackground.sprite = slotData.SegmentSprite;
            _selfTransform.sizeDelta = new Vector2(slotData.Radius * 2, slotData.Radius * 2);
            _backgroundTransform.sizeDelta = new Vector2(slotData.Radius * 2, slotData.Radius * 2);

            List<Vector2> arcPoints = new List<Vector2>();

            float angle = 0f;
            float arcLength = slotData.SegmentAngle;

            if (!Approximately(Abs(slotData.SegmentAngle), 360)) arcPoints.Add(Vector2.zero);

            for (int i = 0; i <= slotData.Segments; i++)
            {
                float x = Sin(Deg2Rad * angle) * slotData.Radius;
                float y = Cos(Deg2Rad * angle) * slotData.Radius;

                arcPoints.Add(new Vector2(x, y));

                angle += (arcLength / slotData.Segments);
            }

            if (!Approximately(Abs(slotData.SegmentAngle), 360)) arcPoints.Add(Vector2.zero);

            if (slotData.FillSegmentByAngle)
            {
                _slotBackground.fillAmount = arcLength / 360f;
            }
            else
            {
                _slotBackground.fillAmount = 1;
            }

            transform.Rotate(0f, 0f, -slotData.StartAngle);
            _collider.points = arcPoints.ToArray();
        }
    }
}