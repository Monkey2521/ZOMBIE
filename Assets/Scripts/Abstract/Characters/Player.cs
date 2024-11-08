using System;
using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.Abilities;
using ZombieSurvival.Equipments;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;
using ZombieSurvival.Levels;
using ZombieSurvival.Stats;
using ZombieSurvival.UI.Abilities;
using ZombieSurvival.Upgrades;

using Zenject;

namespace ZombieSurvival.Characters
{
    public abstract class Player : CharacterBase
    {
        [Header("Moving settings")]
        [SerializeField] protected PlayerMoveController _moveController;
        [SerializeField] protected CameraTransformFollow _cameraFollow;
        [SerializeField] protected Animator _animator;

        [HideInInspector] public bool isMoving;

        protected enum AnimatorBools
        {
            Walk,

            WithBlade,
            WithShotgun,
            WithPistol,
        }

        [Header("Colliders")]
        [Tooltip("Self collider")]
        [SerializeField] protected CapsuleCollider _collider;
        [SerializeField] protected ObjectCatcher _pickablesCatcher;

        [Header("Stats settings")]
#if DEBUG
        [Tooltip("Use stats base weapon instead of equipment/random weapon")]
        [SerializeField] protected bool _weaponDebugging;
#endif
        [SerializeField] protected List<Weapon> _startWeaponsList;
        [SerializeField] protected PlayerUpgrades _levelUpgrades;

        [SerializeField] protected PlayerStats _stats;

        [Header("Inventory settings")]
        [SerializeField] protected List<UpgradeableInventory> _upgradeableInventories;
        
        public override DamageableObjectStats Stats => _stats;

        public PlayerUpgrades LevelUpgrades => _levelUpgrades;
        public Vector3 CameraDeltaPos => _cameraFollow.CameraDeltaPos;

        [Inject] protected LevelContext _levelContext;
        [Inject] protected AbilityGiver _abilityGiver;
        [Inject] protected MainInventory _mainInventory;
        [Inject] protected PlayerLevelInventory _playerLevelInventory;
        [Inject] protected CampInventory _campInventory;
        [Inject] protected EquipmentInventory _equipmentInventory;

        public virtual void Initialize()
        {
            transform.position = new Vector3(0, _levelContext.LevelBuilder.GridHeight + _collider.height * 0.5f, 0);

            List<Upgrade> initUpgrades = GetInitializationUpgrades();

            InitializeFields();

            SetAnimatorBool();

            GetAbility(_stats.BaseWeapon);

            foreach (Upgrade upgrade in _levelContext.PlayerUpgrades)
            {
                GetUpgrade(upgrade);
            }

            foreach (Upgrade upgrade in initUpgrades)
            {
                GetUpgrade(upgrade);
            }

            _hpCanvas?.OnFixedUpdate();
        }

        protected virtual void InitializeFields()
        {
            GetBaseWeapon();

            _stats.Initialize();

            _healthBar?.Initialize(_stats.Health);
            _pickablesCatcher.Initialize(_stats.PickUpRadius);
            _abilityInventory.Initialize();

            _upgrades = new List<Upgrade>();

            foreach(UpgradeableInventory inventory in _upgradeableInventories)
            {
                inventory.Initialize();
            }
        }

        protected virtual List<Upgrade> GetInitializationUpgrades()
        {
            List<Upgrade> upgrades = new List<Upgrade>();

            #region Equipment upgrades
            List<Equipment> equip = _equipmentInventory.EquippedEquipment;

            if (equip.Count > 0)
            {
                foreach (Equipment equipment in equip)
                {
                    if (equipment != null)
                    {
                        upgrades.Add(equipment.EquipUpgrade);

                        foreach (Upgrade upgrade in equipment.RarityUpgrades)
                        {
                            upgrades.Add(upgrade);
                        }
                    }
                    else continue;
                }
            }
            #endregion

            #region Camp upgrades
            foreach (var upgrade in _campInventory.CampUpgrades)
            {
                upgrades.Add(upgrade);
            }
            #endregion

            #region Level upgrades
            PlayerUpgrade currentUpgrade = _levelUpgrades.GetUpgrade((int)_playerLevelInventory.PlayerLevel.Value);

            upgrades.Add(new Upgrade(currentUpgrade.DamageData));
            upgrades.Add(new Upgrade(currentUpgrade.HealthData));
            #endregion

            return upgrades;
        }

        protected virtual void GetBaseWeapon()
        {
#if DEBUG
            if (_weaponDebugging)
            {
                return;
            }
#endif
            WeaponEquipment weaponEquipment = _equipmentInventory.EquippedEquipment.Find(item =>
                                                    item as WeaponEquipment != null) as WeaponEquipment;

            if (weaponEquipment != null)
            {
                _stats.SetBaseWeapon(weaponEquipment.BaseWeapon);
            }
            else
            {
                List<Type> startWeaponTypes = new List<Type>();

                foreach (Weapon weapon in _startWeaponsList)
                {
                    startWeaponTypes.Add(weapon.GetType());
                }

                _stats.SetBaseWeapon(_abilityGiver.GetRandomWeapon(startWeaponTypes.Count > 0 ? startWeaponTypes : null));
            }
        }

        protected virtual void SetAnimatorBool()
        {
            if (_stats.BaseWeapon as Shotgun != null)
            {
                _animator.SetBool(AnimatorBools.WithShotgun.ToString(), true);
            }
            else if (_stats.BaseWeapon as Blade != null)
            {
                _animator.SetBool(AnimatorBools.WithBlade.ToString(), true);
            }
            else //if(_stats.BaseWeapon as Pistol != null)
            {
                _animator.SetBool(AnimatorBools.WithPistol.ToString(), true);
            }
            //else
            //{
            //    _animator.SetBool(AnimatorBools.Walk.ToString(), true);
            //}
        }

        private void Update()
        {
            if (_onDie) return;

            OnUpdate();
        }

        private void FixedUpdate()
        {
            if (_onDie) return;

            OnFixedUpdate();
        }

        public override void OnFixedUpdate()
        {
            base.OnFixedUpdate();

            _moveController.OnFixedUpdate();
            _cameraFollow.OnFixedUpdate();

            Vector3 pos = transform.position;
            _renderer.transform.LookAt(new Vector3(pos.x, pos.y + CameraDeltaPos.y, pos.z + CameraDeltaPos.z));

            _animator.SetBool(AnimatorBools.Walk.ToString(), isMoving);
        }

        public override void Move(Vector3 direction)
        {
            if (direction == Vector3.zero) return;

            Vector3 pos = transform.position;

            if (direction.x > 0 && _defaultViewSide.x < 0)
            {
                _renderer.flipX = true;
            }
            else if (direction.x < 0 && _defaultViewSide.x > 0)
            {
                _renderer.flipX = true;
            }
            else
            {
                _renderer.flipX = false;
            }

            transform.LookAt(pos + direction);
            transform.position = Vector3.MoveTowards(pos, pos + direction * _stats.Velocity.Value, 
                                                     _stats.Velocity.Value * Time.fixedDeltaTime);

            _hpCanvas?.OnFixedUpdate();
        }

        protected override void Attack()
        {
            foreach (Weapon weapon in _abilityInventory.Weapons)
            {
                weapon.OnUpdate();
                weapon.Attack();
            }
        }

        /// <summary>
        /// Upgrade player stats and all abilities he has
        /// </summary>
        /// <param name="upgrade"></param>
        public override void GetUpgrade(Upgrade upgrade)
        {
            base.GetUpgrade(upgrade);

            _pickablesCatcher.UpdateRadius();
            _cameraFollow.Upgrade(upgrade);

            foreach (UpgradeableInventory inventory in _upgradeableInventories)
            {
                inventory.Upgrade(upgrade);
            }
        }

        public override void Die(bool instantly = false)
        {
            base.Die(instantly);

            EventBus.Publish<IPlayerDieHandler>(handler => handler.OnPlayerDie());
        }

        public void OnReanimation()
        {
            _onDie = false;
            
            if (_healthBar != null)
            {
                _healthBar.gameObject.SetActive(true);
            }
        }
    }
}