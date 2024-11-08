using System.Collections;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.General.Enums;
using ZombieSurvival.General.Sounds;
using ZombieSurvival.Interfaces;
using ZombieSurvival.Objects.Indication;
using ZombieSurvival.Stats;
using ZombieSurvival.UI;

namespace ZombieSurvival.Objects
{
    public abstract class DamageableObject : ZSMonoBehaviour, IDamageable, IFixedUpdatable
    {
        [Header("Sounds settings")]
        [SerializeField] protected SoundList _sounds;

        [Header("Health settings")]
        [Tooltip("If object is immortal it takes damage but cant die")]
        [SerializeField] protected bool _isImmortal;
        [Tooltip("Field can be null")]
        [SerializeField] protected HPBar _healthBar;

        protected bool _onDie;

        public abstract DamageableObjectStats Stats { get; }

        public virtual int HP => (int)Stats.Health.Value;
        public virtual int MaxHP => (int)Stats.Health.MaxValue;

        public bool IsImmortal => _isImmortal;
        public bool IsDied => _onDie;

        protected virtual void OnEnable()
        {
            _onDie = false;

            if (_healthBar != null)
            {
                _healthBar.gameObject.SetActive(true);
            }
        }

        public virtual void OnFixedUpdate()
        {
            if (_onDie) return;

            Stats.Health.Heal(Stats.Regeneration.Value * Time.fixedDeltaTime);

            if (_healthBar != null) _healthBar.UpdateHealth();
        }

        public virtual float TakeDamage(ConcreteDamage damage)
        {
            if (_onDie) return 0;
#if DEBUG
            if (_isDebug) Debug.Log(name + " start taking damage = " + damage.Value);
#endif
            ReceivedDamage resistancedDamage = Stats.DamageReceiver.GetDamage(damage);

            if (resistancedDamage.Value == 0)
            {
#if DEBUG
                if (_isDebug) Debug.Log(name + " blocked damage");
#endif
                return 0;
            }
            
            float receivedDamage = Stats.Health.TakeDamage(resistancedDamage.Value);
            
#if DEBUG
            if (_isDebug) Debug.Log(name + " received " + receivedDamage + " damage");
#endif

            _healthBar?.UpdateHealth();

            if (HP <= 0 && !_isImmortal)
            {
                _sounds.PlaySound(SoundTypes.Die);

                OnDie();

                ObjectDieIndicator dieIndicator = Stats.DamageReceiver.GetDieIndicator(damage);

                if (dieIndicator != null)
                {
                    dieIndicator.OnIndication();

                    StartCoroutine(DieDelay(dieIndicator.IndicationTime));
                }
                else
                {
                    Die();
                }
            }
            else
            {
                _sounds.PlaySound(SoundTypes.TakeDamage);

                resistancedDamage.Indicator?.OnIndication();
            }

            return receivedDamage;
        }

        protected virtual IEnumerator DieDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            Die();
        }

        public virtual void Die(bool instantly = false)
        {
#if DEBUG
            if (_isDebug) Debug.Log(name + " dies");
#endif
            if (instantly)
            {
                OnDie(instantly);
            }
        }

        protected virtual void OnDie(bool instantly = false)
        {
            _onDie = true;

            if (_healthBar != null)
            {
                _healthBar.gameObject.SetActive(false);
            }
        }
    }
}