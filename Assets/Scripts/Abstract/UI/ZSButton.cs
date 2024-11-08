using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

using ZombieSurvival.General;

namespace ZombieSurvival.UI
{
    [RequireComponent(typeof(Button))]
    public abstract class ZSButton : ZSMonoBehaviour
    {
        [Header("Button settings")]
        [SerializeField] protected Button _button;

        [SerializeField] protected List<UnityEvent> _clickEvents;
        [SerializeField] protected List<UnityEvent> _lockedClickEvents;

        [Space(5)]
        [Tooltip("Enable/Disable content marker")]
        [SerializeField] protected bool _displayMarker;
        [SerializeField] protected Sprite _onAvailableContentMarkerSprite;
        [SerializeField] protected Image _contentMarkerImage;

        protected bool _interactable = true;

        public bool interactable { get => _interactable; set => _interactable = value; }

        protected virtual void Awake()
        {
            if (_displayMarker)
            {
                if (_contentMarkerImage == null)
                {
#if DEBUG
                    Log("Missing marker!");
#endif
                    return;
                }

                _contentMarkerImage.sprite = _onAvailableContentMarkerSprite;

                _contentMarkerImage.enabled = false;
            }
        }

        public virtual void OnButtonClick()
        {
            if (_interactable)
            {
                foreach(UnityEvent @event in _clickEvents)
                {
                    @event?.Invoke();
                }
            }
            else
            {
                foreach (UnityEvent @event in _lockedClickEvents)
                {
                    @event?.Invoke();
                }
            }
        }

        public void InvokeClick()
        {
            OnButtonClick();
        }

        public void DisplayMarker()
        {
            if (_contentMarkerImage == null)
            {
#if DEBUG
                Log("Missing marker!");
#endif
                return;
            }

            _contentMarkerImage.enabled = true;
        }

        public void HideMarker()
        {
            if (_contentMarkerImage == null)
            {
#if DEBUG
                Log("Missing marker!");
#endif
                return;
            }

            _contentMarkerImage.enabled = false;
        }
    }
}