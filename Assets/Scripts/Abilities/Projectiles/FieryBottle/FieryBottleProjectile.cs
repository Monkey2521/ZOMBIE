using UnityEngine;
using ZombieSurvival.General;

namespace ZombieSurvival.Abilities
{
    public sealed class FieryBottleProjectile : Projectile
    {
        [SerializeField] private ParticleSystem _sparkParticle;

        private Vector3 _startPosition;
        private Vector3 _targetPosition;

        private const float EPSILON = 0.125f;

        public override void Initialize(ProjectileAbilityStats stats, ProjectileWeapon weapon, TagList tags = null)
        {
            base.Initialize(stats, weapon, null);

            if (_sparkParticle != null)
            {
                _sparkParticle.Stop();

                var main = _sparkParticle.main;

                main.startLifetime = _releaseDelay.Value;
                main.duration = _releaseDelay.Value;

                _sparkParticle.transform.localScale = new Vector3(stats.ProjectileSize.Value, stats.ProjectileSize.Value, stats.ProjectileSize.Value);
                _sparkParticle.Play();
            }
            else if (_isDebug) Debug.Log("Missing spark particle");
        }

        public override void Throw(Vector3 position)
        {
            base.Throw(position);

            _targetPosition = position;
            _startPosition = transform.position;

            UpdateMoveDirection();
        }

        protected override void Move()
        {
            if (transform.position.y - EPSILON <= 0f)
            {
                _releaseTimer = -1f;
                return;
            }

            UpdateMoveDirection();

            base.Move();
        }

        private void UpdateMoveDirection()
        {
            Vector3 position = transform.position;

            float yOffset = 0;

            if (new Vector3(_targetPosition.x - position.x, 0f, _targetPosition.z - position.z).magnitude <= EPSILON)
            {
                yOffset = -1;
            }

            _moveDirection = new Vector3(_targetPosition.x - position.x, yOffset, _targetPosition.z - position.z).normalized;
        }
    }
}