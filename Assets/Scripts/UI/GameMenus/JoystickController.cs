using UnityEngine;
using UnityEngine.UI;

namespace ZombieSurvival.UI.GameMenus.HUD
{
    public class JoystickController : MonoBehaviour
    {
        [SerializeField] private Image _onStartTouchImage;
        [SerializeField] private Image _onTouchImage;

        [Space(5)]
        [SerializeField] private Sprite _defaultTouchSprite;
        [SerializeField] private Sprite _onTouchTopLeftSprite;
        [SerializeField] private Sprite _onTouchTopRightSprite;
        [SerializeField] private Sprite _onTouchBottomLeftSprite;
        [SerializeField] private Sprite _onTouchBottomRightSprite;
        [SerializeField] private Sprite _onTouchSprite;

        private void OnEnable()
        {
            _onStartTouchImage.enabled = false;
            _onTouchImage.enabled = false;

            _onTouchImage.sprite = _onTouchSprite;
        }

        public void UpdateJoystick(bool onTouch, Vector2 startTouchPosition = default, Vector2 touchPosition = default)
        {
            if (!onTouch)
            {
                _onStartTouchImage.enabled = false;
                _onTouchImage.enabled = false;

                return;
            }

            (_onStartTouchImage.transform as RectTransform).anchoredPosition = startTouchPosition;
            (_onTouchImage.transform as RectTransform).anchoredPosition = touchPosition;

            Sprite current = _onTouchTopLeftSprite;

            if (startTouchPosition == touchPosition)
            {
                current = _defaultTouchSprite;
            }
            else if (startTouchPosition.x > touchPosition.x)
            {
                if (startTouchPosition.y > touchPosition.y)
                {
                    current = _onTouchBottomLeftSprite;
                }
                else
                {
                    current = _onTouchTopLeftSprite;
                }
            }
            else
            {
                if (startTouchPosition.y > touchPosition.y)
                {
                    current = _onTouchBottomRightSprite;
                }
                else
                {
                    current = _onTouchTopRightSprite;
                }
            }

            _onStartTouchImage.sprite = current;

            _onStartTouchImage.enabled = true;

            if (current != _defaultTouchSprite)
            {
                _onTouchImage.enabled = true;
            }

        }
    }
}