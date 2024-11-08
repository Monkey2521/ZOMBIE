using UnityEngine;
using UnityEngine.UI;
using ZombieSurvival.Abilities;
using ZombieSurvival.Interfaces;

namespace ZombieSurvival.UI.Abilities
{
    public class AbilityCooldown : FillBar, IFixedUpdatable
    {
        [SerializeField] private Image _abilityIcon;

        private AbilityContainer _container;
        private float _previousCooldown;

        public void Initialize(AbilityContainer ability)
        {
            if (_container == null)
            {
                Destroy(gameObject);
                return;
            }

            base.Initialize();

            _container = ability;

            _abilityIcon.sprite = _container.Icon;

            if (_container as Weapon != null && _container.Stats is WeaponAbilityStats stats)
            {
                _fillBar.maxValue = stats.Cooldown.MaxValue;
#if DEBUG
                if (_isDebug) Debug.Log(name + " set MaxFill value = " + stats.Cooldown.MaxValue.ToString());
#endif

            }
#if DEBUG
            if (_isDebug) Debug.Log(name + "Destroed");
#endif
        }

        public void OnFixedUpdate()
        {
            if (_container == null)
            {
#if DEBUG
                if (_isDebug) Debug.Log(name + "Destroed");
#endif
                    Destroy(gameObject);

                return;
            }

            if (_container as Weapon != null && _container.Stats is WeaponAbilityStats stats)
            {
                _previousCooldown = stats.Cooldown.Value / stats.Cooldown.MaxValue;

                if (_previousCooldown <= 0)
                {
                    _fillBar.value = stats.Cooldown.MaxValue;
                }
                else
                {
                    _fillBar.value = _previousCooldown;
                }

                _fillBar.maxValue = stats.Cooldown.MaxValue;
            }
            else
            {
                _fillBar.value = _minFillValue;
            }
        }
    }
}