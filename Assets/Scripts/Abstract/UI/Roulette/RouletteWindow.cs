using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Interfaces;

namespace ZombieSurvival.UI.Shop
{
    public abstract class RouletteWindow : ZSMonoBehaviour, IPoolable
    {
        [Header("Roulette window settings")]
        [SerializeField] protected RouletteDropHandler _rouletteDropHandler;
        [SerializeField] protected Transform _slotsParent;

        protected float _speed;
        protected float _speedDecreasePerFrame;
        protected float _speedDecreaseMultiplier;
        protected Vector3 _moveDirection;

        public Transform SlotsParent => _slotsParent;

        public RouletteSlot LastReachedSlot => _rouletteDropHandler.LastReachedSlot;
        public bool OnMove => _speed > 0;

        public virtual void Initialize(float speed, float speedDecreasePerFrame, float speedDecreaseMultiplier,
                                       Vector3 rouletteStartPosition, Vector3 moveDirection)
        {
            _speed = speed;
            _speedDecreasePerFrame = speedDecreasePerFrame;
            _speedDecreaseMultiplier = speedDecreaseMultiplier; 
            
            _moveDirection = moveDirection;

            _rouletteDropHandler.Initialize();
        }

        public void ResetObject()
        {
            _speed = 0;
            _speedDecreasePerFrame = 0;
            _speedDecreaseMultiplier = 0;
            _moveDirection = Vector3.zero;
        }

        /// <summary>
        /// Moving in FixedUpdate
        /// </summary>
        public virtual void MoveSlots()
        {
            if (!OnMove) return;

            _speed -= _speedDecreasePerFrame;
            _speedDecreasePerFrame += _speedDecreasePerFrame * _speedDecreaseMultiplier;
        }
    }
}