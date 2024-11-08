using System.Collections;
using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.General.Enums;
using ZombieSurvival.General.Sounds;

namespace ZombieSurvival.UI.Shop
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    public class RouletteDropHandler : ZSMonoBehaviour
    {
        [Header("Drop handler settings")]
        [SerializeField] private RectTransform _selfTransform;
        [SerializeField] private BoxCollider2D _collider;
        [SerializeField] private SoundList _sounds;

        private RouletteSlot _lastReachedSlot;

        public RouletteSlot LastReachedSlot => _lastReachedSlot;

        private void Awake()
        {
            _collider.size = new Vector2(_collider.size.x * ScreenScaler.MinDelta, _collider.size.y * ScreenScaler.MinDelta);
        }

        public void Initialize()
        {
            StartCoroutine(WaitInitialize());
        }

        private IEnumerator WaitInitialize()
        {
            yield return new WaitForSecondsRealtime(0.1f);

            _selfTransform.anchoredPosition = Vector2.zero;
            _selfTransform.sizeDelta = Vector2.zero;
        }

        protected virtual void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out RouletteSlot slot))
            {
                _sounds.PlaySound(SoundTypes.RouletteHandler);

                _lastReachedSlot = slot;
            }
        }
    }
}