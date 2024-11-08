using UnityEngine;
using UnityEngine.UI;

namespace ZombieSurvival.UI
{
    public class AnimatedButton : SoundedButton
    {
        [Header("AnimatedButton settings")]
        [SerializeField] private Animator _animator;

        [SerializeField] protected Image _backgroundImage;
        [SerializeField] protected Sprite _onDisabledSprite;
        [SerializeField] protected Sprite _onEnableSprite;

        protected bool _wasEnabled;

        public void Enable()
        {
            _backgroundImage.sprite = _onEnableSprite;

            if (!_wasEnabled)
            {
                _animator.SetTrigger("Enable");
            }

            _wasEnabled = true;
        }

        public void Disable()
        {
            _backgroundImage.sprite = _onDisabledSprite;

            if (_wasEnabled)
            {
                _animator.SetTrigger("Disable");
            }

            _wasEnabled = false;
        }
    }
}