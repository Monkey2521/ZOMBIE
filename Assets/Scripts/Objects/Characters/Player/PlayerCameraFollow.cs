using UnityEngine;

using ZombieSurvival.Events;
using ZombieSurvival.Objects;

namespace ZombieSurvival.Characters
{
    public sealed class PlayerCameraFollow : CameraTransformFollow, IBossEventAlertHandler, IBossAttackHandler
    {
        [Header("PlayerFollow settings")]
        [SerializeField] private PlayerCameraStats _stats;

        private bool _onBossEvent;
        private bool _onBossAttack;
        private float _timer;
        private float _rotationTimer;
        private MoveSide _side;
        public override CameraFollowStats Stats => _stats;

        protected override void OnEnable()
        {
            base.OnEnable();

            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            if (_onBossEvent)
            {
                Move();

                if (_timer >= _stats.ShakeDuration.Value)
                {
                    _onBossEvent = false;

                    transform.localPosition = Vector3.zero;
                }
            }
            else if (_onBossAttack)
            {
                Move();

                if (_timer >= _stats.BossAttackShakeDuration.Value)
                {
                    _onBossAttack = false;

                    transform.localPosition = Vector3.zero;
                }
            }
            else return;

            if (_rotationTimer >= _stats.ShakeDirectionMoveDuration.Value)
            {
                _rotationTimer = 0;

                if (_side.Equals(MoveSide.Up))
                {
                    _side = MoveSide.Down;
                }
                else
                {
                    _side = MoveSide.Up;
                }
            }
        }

        private void Move()
        {
            Vector3 pos = transform.position;
            Vector3 newPos;

            if (_side.Equals(MoveSide.Down))
            {
                newPos = Vector3.MoveTowards
                    (
                        pos,
                        pos + transform.TransformDirection(Vector3.down) * _stats.ShakeSpeed.Value,
                        _stats.ShakeSpeed.Value
                    );
            }
            else
            {
                newPos = Vector3.MoveTowards
                    (
                        pos,
                        pos + transform.TransformDirection(Vector3.up) * _stats.ShakeSpeed.Value,
                        _stats.ShakeSpeed.Value
                    );
            }


            transform.position = newPos;

            _timer += Time.fixedDeltaTime;
            _rotationTimer += Time.fixedDeltaTime;
        }

        public void OnBossEventAlert()
        {
            _timer = 0;
            _rotationTimer = 0;
            _side = (MoveSide)Random.Range(0, 2);
            _onBossEvent = true;
        }

        public void OnBossAttack()
        {
            _timer = 0;
            _rotationTimer = 0;
            _side = (MoveSide)Random.Range(0, 2);
            _onBossAttack = true;
        }

        private enum MoveSide 
        {
            Down,
            Up
        }
    }
}