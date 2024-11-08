using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Interfaces;
using ZombieSurvival.UI.GameMenus.HUD;

namespace ZombieSurvival.Characters
{
    public class PlayerMoveController : ZSMonoBehaviour, IFixedUpdatable
    {
        [Header("Player move controller settings")]
        [SerializeField] private Player _player;

        [SerializeField] private JoystickController _joystickController;

        private bool _isMobile;

        /// <summary>
        /// Position of the first touch on screen
        /// </summary>
        private Vector2 _touchPosition;
        /// <summary>
        /// Equals true while TouchCount > 0
        /// </summary>
        private bool _onTouch;

        private void OnEnable()
        {
            _isMobile = Application.isMobilePlatform;
        }

        public void OnFixedUpdate()
        {
            if (_isMobile)
            {
                if (Input.touchCount > 0)
                {
                    // Move if already touch screen
                    if (_onTouch)
                    {
                        Vector3 moveDirection = new Vector3
                            (
                                Input.GetTouch(0).position.x - _touchPosition.x,
                                0,
                                Input.GetTouch(0).position.y - _touchPosition.y
                            );

                        _player.Move(moveDirection);

                        _joystickController.UpdateJoystick(onTouch: true, startTouchPosition: _touchPosition,
                                                           touchPosition: new Vector2
                                                                (
                                                                    Input.GetTouch(0).position.x,
                                                                    Input.GetTouch(0).position.y
                                                                ));

                        if (moveDirection != Vector3.zero)
                        {
                            _player.isMoving = true;
                        }
                        else
                        {
                            _player.isMoving = false;
                        }
                    }
                    // Set new touch position
                    else
                    {
                        _joystickController.UpdateJoystick(onTouch: true, startTouchPosition: _touchPosition,
                                                           touchPosition: new Vector2
                                                                (
                                                                    Input.GetTouch(0).position.x,
                                                                    Input.GetTouch(0).position.y
                                                                ));

                        _onTouch = true;
                        _touchPosition = Input.GetTouch(0).position;
                    }
                }
                else // Reset flags
                {
                    _player.isMoving = false;
                    _onTouch = false;

                    _joystickController.UpdateJoystick(onTouch: false);
                }
            }
            else
            {
                #region Mouse control (Equals mobile control)
                if (Input.GetMouseButton(0))
                {
                    if (_onTouch)
                    {
                        Vector3 moveDirection = new Vector3
                            (
                                Input.mousePosition.x - _touchPosition.x,
                                0,
                                Input.mousePosition.y - _touchPosition.y
                            ).normalized;

                        if (_isDebug) Debug.Log(moveDirection);

                        _player.Move(moveDirection);

                        _joystickController.UpdateJoystick(onTouch: true, startTouchPosition: _touchPosition,
                                                           touchPosition: new Vector2
                                                                (
                                                                    Input.mousePosition.x,
                                                                    Input.mousePosition.y
                                                                ));

                        if (moveDirection != Vector3.zero)
                        {
                            _player.isMoving = true;
                        }
                        else
                        {
                            _player.isMoving = false;
                        }
                    }
                    else
                    {
                        _joystickController.UpdateJoystick(onTouch: true, startTouchPosition: _touchPosition,
                                                           touchPosition: new Vector2
                                                                (
                                                                    Input.mousePosition.x,
                                                                    Input.mousePosition.y
                                                                ));

                        _onTouch = true;
                        _touchPosition = Input.mousePosition;
                    }
                }
                else
                {
                    _onTouch = false;
                    _player.isMoving = false;

                    _joystickController.UpdateJoystick(onTouch: false);
                }
                #endregion
#if DEBUG
                #region WASD control
                if (Input.GetKey(KeyCode.W))
                {
                    _player.Move(Vector3.forward);
                }
                if (Input.GetKey(KeyCode.S))
                {
                    _player.Move(Vector3.back);
                }
                if (Input.GetKey(KeyCode.A))
                {
                    _player.Move(Vector3.left);
                }
                if (Input.GetKey(KeyCode.D))
                {
                    _player.Move(Vector3.right);
                }
                #endregion
#endif
            }
        }
    }
}