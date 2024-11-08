using System.Collections;

using UnityEngine;

using ZombieSurvival.Interfaces;
using ZombieSurvival.General;
using ZombieSurvival.General.Enums;
using ZombieSurvival.General.Sounds;

namespace ZombieSurvival.Objects
{
    [RequireComponent(typeof(SphereCollider))]
    public abstract class PickableObject : ZSMonoBehaviour, IPoolable, IFixedUpdatable
    {
        [Header("Pickable object settings")]
        [Tooltip("Enable/disable pick up in unscaled time")]
        [SerializeField] protected bool _useUnscaledTime;

        [Space(5)]
        [Tooltip("Enable/disable particle highlighting")]
        [SerializeField] protected bool _useHighlight;
        [Tooltip("Field can be null if UseHighlight is false")]
        [SerializeField] protected ParticleSystem _highlightParticle;
        [SerializeField] protected ParticleSystem _highlightParticle1;
        [SerializeField] protected bool _disableHighlightOnPickUp;

        [Header("Pick up settings")]
        [SerializeField] protected SphereCollider _pickUpCollider;
        [SerializeField] protected TagList _pickUpTags;
        [SerializeField] protected PickableObjectStats _stats;

        [Header("Sounds settings")]
        [SerializeField] protected SoundList _sounds;

        protected bool _isPickUpped;

        protected float _knockbackTimer;
        protected Vector3 _knockbackDirection;

        protected Transform _target;

        protected MonoPool<PickableObject> _pool;

        public bool IsPickUpped => _isPickUpped;

        public virtual void Initialize(MonoPool<PickableObject> pool)
        {
            _stats.Initialize();

            _pool = pool;

            _pickUpCollider.isTrigger = true;
            _pickUpCollider.radius = _stats.PickUpRadius.Value;

            _isPickUpped = false;

            if (_useHighlight)
            {
                if (_highlightParticle != null)
                {
                    _highlightParticle.gameObject.SetActive(true);
                    _highlightParticle.Play();
                }
                else if (_isDebug) Debug.Log(name + ": Missing highlight particle!");

                if (_highlightParticle1 != null)
                {
                    _highlightParticle1.gameObject.SetActive(true);
                    _highlightParticle1.Play();
                }
                else if (_isDebug) Debug.Log(name + ": Missing highlight particle1!");
            }
        }

        public void ChangePool(MonoPool<PickableObject> pool) // TODO rework
        {
            _pool = pool;

            if (_pool != null)
            {
                transform.parent = _pool.Parent;
            }            
        }

        public virtual void ResetObject()
        {
            _isPickUpped = false;
            _knockbackTimer = 0;
            _target = null;
            _pool = null;
        }

        public void OnFixedUpdate()
        {
            if (!_isPickUpped) return;

            if (_target == null)
            {
                if (_isDebug) Debug.Log(name + ": Missing target!");

                _isPickUpped = false;

                return;
            }

            transform.position = GetNextMovePosition();
        }

        protected virtual Vector3 GetNextMovePosition()
        {
            Vector3 position = transform.position;

            if (_knockbackTimer < _stats.KnockbackDuration.Value)
            {
                if (_useUnscaledTime)
                {
                    _knockbackTimer += Time.fixedUnscaledDeltaTime;

                    return Vector3.MoveTowards(position, position + _knockbackDirection * _stats.KnockbackSpeed.Value,
                                                         _stats.KnockbackSpeed.Value * Time.fixedUnscaledDeltaTime);
                }
                else
                {
                    _knockbackTimer += Time.fixedDeltaTime;

                    return Vector3.MoveTowards(position,  position + _knockbackDirection * _stats.KnockbackSpeed.Value,
                                                         _stats.KnockbackSpeed.Value * Time.fixedDeltaTime);
                }
            }
            else
            {
                if (_useUnscaledTime)
                {
                    return Vector3.MoveTowards(position, _target.position,
                                                         _stats.PickUpSpeed.Value * Time.fixedUnscaledDeltaTime);
                }
                else
                {
                    return Vector3.MoveTowards(position, _target.position,
                                                         _stats.PickUpSpeed.Value * Time.fixedDeltaTime);
                }
            }
        }

        public virtual void PickUp(Transform target)
        {
            if (_isDebug) Debug.Log("Pick up " + name);

            _target = target;

            _isPickUpped = true;
            _knockbackTimer = 0;
            _knockbackDirection = (transform.position - _target.position).normalized;

            if (_useHighlight && _disableHighlightOnPickUp)
            {
                if (_highlightParticle != null)
                {
                    _highlightParticle.Stop();
                    _highlightParticle.gameObject.SetActive(false);
                }
                else if (_isDebug) Debug.Log(name + ": Missing highlight particle!");

                if (_highlightParticle1 != null)
                {
                    _highlightParticle1.Stop();
                    _highlightParticle1.gameObject.SetActive(false);
                }
                else if (_isDebug) Debug.Log(name + ": Missing highlight particle1!");
            }
        }

        protected virtual void OnPickUp(float releaseDelay = 0f)
        {
            _target = null;

            StartCoroutine(ReleaseDelay(releaseDelay));
        }

        protected virtual IEnumerator ReleaseDelay(float releaseDelay)
        {
            if (releaseDelay <= 0)
            {
                yield return null;
            }
            else
            {
                if (_useUnscaledTime)
                {
                    yield return new WaitForSecondsRealtime(releaseDelay);
                }
                else
                {
                    yield return new WaitForSeconds(releaseDelay);
                }
            }

            if (_pool != null)
            {
                _pool.Release(this);
            }
            else
            {
                if (_isDebug) Debug.Log(name + ": Missing pool!");

                Destroy(gameObject);
            }
        }

        protected virtual void OnTriggerEnter(Collider other)
        {
            if (_pickUpTags.Contains(other.tag) && _isPickUpped)
            {
                _sounds.PlaySound(SoundTypes.PickUp);

                OnPickUp();
            }
        }
    }
}