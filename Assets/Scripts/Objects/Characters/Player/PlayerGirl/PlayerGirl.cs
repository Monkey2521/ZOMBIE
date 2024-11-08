using System.Collections.Generic;
using UnityEngine;

using ZombieSurvival.Characters.Players;
using ZombieSurvival.Events;
using ZombieSurvival.Stats;
using ZombieSurvival.Upgrades;

namespace ZombieSurvival.Characters
{
    public sealed class PlayerGirl : Player, IEnemyKilledHandler
    {
        [Header("Girl settings")]
        [Tooltip("MoveSpeed upgrades for each combo")]
        [SerializeField] private GirlComboUpgrades _comboUpgrades;
        [SerializeField] private ParticleSystem _comboParticle;
        [SerializeField] private GirlComboCanvas _comboCanvas;

        private int _comboCounter;
        private bool _onCombo;

        public override void Initialize()
        {
            base.Initialize();

            _comboCounter = 0;

            _comboParticle.Stop();

            _comboCanvas.Initialize();
        }

        protected override void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public override void OnFixedUpdate()
        {
            if (_onDie) return;

            base.OnFixedUpdate();

            if (!isMoving && _onCombo)
            {
                _comboParticle.Stop();
            }
            else if (isMoving && _onCombo && _comboParticle.isStopped)
            {
                _comboParticle.Play();
            }

            _comboCanvas.OnFixedUpdate();
        }

        public void OnEnemyKilled(Enemy enemy)
        {
            _comboCounter++;

            if (_comboUpgrades.TryReachCombo(_comboCounter, out GirlCombo combo))
            {
                GetUpgrade(combo.UpgradeOnCombo);

                SetComboParticleValues(combo);
                _comboParticle.Play();

                _comboCanvas.DisplayCombo(combo.RequiredCount);

                _onCombo = true;
            }
        }

        public override float TakeDamage(ConcreteDamage damage)
        {
            float receivedDamage = base.TakeDamage(damage);

            if (receivedDamage > 0)
            {
                List<GirlCombo> combos = _comboUpgrades.GetReachedCombos();

                foreach(GirlCombo combo in combos)
                {
                    if (combo == null) continue;

                    combo.Reset();

                    DispelUpgrade(combo.UpgradeOnCombo);
                }

                _comboCounter = 0;

                _comboParticle.Stop();

                _onCombo = false;
            }

            return receivedDamage;
        }

        private void SetComboParticleValues(GirlCombo combo)
        {
            var main = _comboParticle.main;

            main.startSize = combo.ParticleSize;
            main.startSpeed = combo.ParticleSpeed;
        }

        [System.Serializable]
        private class GirlComboUpgrades
        {
            [SerializeField] private List<GirlCombo> _combos;

            public void Initialize()
            {
                foreach(GirlCombo combo in _combos)
                {
                    combo.Reset();
                }
            }

            public bool TryReachCombo(int comboCounter, out GirlCombo combo)
            {
                combo = _combos.Find(item => item.RequiredCount <= comboCounter && !item.Reached);

                if (combo != null)
                {
                    combo.Reach();
                }

                return combo != null;
            }

            public List<GirlCombo> GetReachedCombos() => _combos.FindAll(item => item.Reached);
        }

        [System.Serializable]
        private class GirlCombo 
        {
            [SerializeField] private int _requiredCount;
            [SerializeField] private Upgrade _upgradeOnCombo;

            [Space(5)]
            [SerializeField] private float _particleSize;
            [SerializeField] private float _particleSpeed;

            private bool _reached;

            public int RequiredCount => _requiredCount;
            public Upgrade UpgradeOnCombo => _upgradeOnCombo;
            public float ParticleSize => _particleSize;
            public float ParticleSpeed => _particleSpeed;
            public bool Reached => _reached;

            public void Reset()
            {
                _reached = false;
            }

            public void Reach()
            {
                _reached = true;
            }
        }
    }
}