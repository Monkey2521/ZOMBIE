using UnityEngine;

using ZombieSurvival.General;

namespace ZombieSurvival.UI
{
    public abstract class UIMenu : ZSMonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] protected CanvasGroup _canvasGroup;
        [Tooltip("Button that displays this menu (can be null)")]
        [SerializeField] protected AnimatedButton _button;

        [Space(5)]
        [SerializeField] protected Animator _animator;

        protected bool _enabled = false;

        protected MainMenu _mainMenu;
        protected UIMenu _parentMenu;

        public bool Enabled => _enabled;

        public virtual void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            _mainMenu = mainMenu;
            _parentMenu = parentMenu;

            if (_animator != null)
            {
                _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
                _animator.cullingMode = AnimatorCullingMode.CullCompletely;
            }

            if (_isDebug) Debug.Log("Initialized");
        }
        public virtual void Display(bool playAnimation = false)
        {
            _enabled = true;

            if (_animator != null && playAnimation)
            {
                if (_isDebug) Debug.Log("Display by animator: " + name);

                _animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                _animator.SetTrigger("Display");
            }
            else
            {
                if (_animator != null)
                    _animator.cullingMode = AnimatorCullingMode.CullCompletely;

                _canvasGroup.alpha = 1;
                _canvasGroup.blocksRaycasts = true;
                _canvasGroup.interactable = true;

                if (_isDebug) Debug.Log("Just display: " + name);
            }

            if (_button != null)
            {
                _button.Enable();
            }
        }

        public virtual void Hide(bool playAnimation = false)
        {
            _enabled = false;

            if (_animator != null && playAnimation)
            {
                if (_isDebug) Debug.Log("Hide by animator: " + name);

                _animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                _animator.SetTrigger("Hide");
            }
            else
            {
                if (_animator != null)
                    _animator.cullingMode = AnimatorCullingMode.CullCompletely;

                _canvasGroup.alpha = 0;
                _canvasGroup.blocksRaycasts = false;
                _canvasGroup.interactable = false;

                if (_isDebug) Debug.Log("Just hide: " + name);
            }

            if (_button != null)
            {
                _button.Disable();
            }
        }

        public void MainMenuDisplay()
        {
            _mainMenu.Display(this);
        }

        public void MainMenuHide()
        {
            _mainMenu.DisplayDefault();
        }

        protected void DisplayButtonMarker()
        {
            if (_button != null)
            {
                _button.DisplayMarker();
            }
        }

        protected void HideButtonMarker()
        {
            if (_button != null)
            {
                _button.HideMarker();
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Display with animation")]
        protected void DisplayAnimation()
        {
            Display(true);
        }

        [ContextMenu("Hide with animation")]
        protected void HideAnimation()
        {
            Hide(true);
        }
#endif
    }
}