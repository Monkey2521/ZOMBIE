using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using ZombieSurvival.General;
using ZombieSurvival.Interfaces;
using ZombieSurvival.Objects;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Shop
{
    public abstract class Roulette : UIMenu, IFixedUpdatable
    {
        [Header("Roulette settings")]
        [SerializeField] protected RouletteMarker _rouletteMarker;
        [SerializeField][Range(1, 10)] protected int _maxRoulettesCountAtOnce = 10;

        [Space(5)]
        [SerializeField][Range(1, 5)] protected float _skipRouletteButtonHideDelay = 3f;
        [SerializeField] protected GameObject _skipButton;

        [Space(5)]
        [SerializeField] protected Transform _rouletteWindowsParent;
        [SerializeField] protected RouletteWindow _rouletteWindowPrefab;
        [SerializeField] protected RouletteSlot _rouletteSlotPrefab;
        [SerializeField] protected int _maxSlotsCount;

        [Space(5)]
        [SerializeField][Range(0f, 5f)] protected float _showRewardsDelay = 0.5f;
        
        [Space(5)]
        [SerializeField][Range(0.01f, 1000f)] protected float _minStartSpeed = 500f;
        [SerializeField][Range(0.01f, 1500f)] protected float _maxStartSpeed = 800f;
        
        [Space(5)]
        [SerializeField][Range(0.001f, 50f)] protected float _speedMinDecreasePerFrame = 0.35f;
        [SerializeField][Range(0.001f, 50f)] protected float _speedMaxDecreasePerFrame = 0.7f;

        [Space(5)]
        [SerializeField][Range(0f, 2f)] protected float _speedMinDecreaseMultiplier = 0.005f;
        [SerializeField][Range(0f, 2f)] protected float _speedMaxDecreaseMultiplier = 0.015f;

        protected bool _onRoulette;
        protected float _skipHideTimer;

        protected MonoPool<RouletteSlot> _slotsPool;
        protected List<RouletteSlot> _slots;

        protected MonoPool<RouletteWindow> _windowPool;

        protected CleanupableList<RouletteWindow> _windows;
        protected List<RouletteWindow> _stoppedWindows;

        protected int _activeRoulettes;
        protected List<ConcreteReward> _totalRewards;

        protected List<ConcreteReward> _skipRewards;

        public RouletteMarker Marker => _rouletteMarker;

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _minStartSpeed *= ScreenScaler.MinDelta;
            _maxStartSpeed *= ScreenScaler.MinDelta;
            _speedMinDecreasePerFrame *= ScreenScaler.MinDelta;
            _speedMaxDecreasePerFrame *= ScreenScaler.MinDelta;

            _windows = new CleanupableList<RouletteWindow>(_maxRoulettesCountAtOnce);
            _stoppedWindows = new List<RouletteWindow>(_maxRoulettesCountAtOnce);

            _slots = new List<RouletteSlot>(_maxSlotsCount * _maxRoulettesCountAtOnce);

            _windowPool = new MonoPool<RouletteWindow>
                (
                    _rouletteWindowPrefab,
                    _maxRoulettesCountAtOnce,
                    _rouletteWindowsParent
                );

            _slotsPool = new MonoPool<RouletteSlot>
                (
                    _rouletteSlotPrefab,
                    _maxSlotsCount * _maxRoulettesCountAtOnce,
                    transform
                );
        }

        public virtual void DisplayRoulette<TChance>(ChanceCombiner<TChance> chances, int roulettesCount = 1, bool autoStart = true) 
            where TChance : class
        {
            ClearWindows();

            _skipHideTimer = 0f;
            _skipButton.SetActive(true);

            _skipRewards = new List<ConcreteReward>();

            ChanceCombiner<TChance> rouletteChances = chances.Clone();

            for (int i = 0; i < roulettesCount; i++)
            {
                _skipRewards.Add(GetConcreteReward(chances));
            }

            chances.Initialize();

            _onRoulette = autoStart;

            _activeRoulettes = roulettesCount;
            _totalRewards = new List<ConcreteReward>();

            for (int i = 0; i < roulettesCount; i++)
            {
                RouletteWindow initializedWindow = InitializeWindow(_windowPool.Pull());

                InitializeSlots(rouletteChances, initializedWindow);

                _windows.Add(initializedWindow);
            }
        }

        public virtual void StartRoullete()
        {
            _onRoulette = true;
        }

        public virtual void OnFixedUpdate()
        {
            if (!_onRoulette) return;

            _skipHideTimer += Time.fixedUnscaledDeltaTime;

            for (int i = 0; i < _windows.Count; i++)
            {
                if (_windows[i] != null)
                {
                    _windows[i].MoveSlots();

                    if (!_windows[i].OnMove)
                    {
                        OnRouletteStop(_windows[i]);
                    }
                }
            }

            _windows.Cleanup();

            if (_skipHideTimer >= _skipRouletteButtonHideDelay)
            {
                _skipButton.SetActive(false);
            }
        }

        protected virtual void OnRouletteStop(RouletteWindow window)
        {
            _activeRoulettes--;

            _totalRewards.Add(window.LastReachedSlot.Reward);

            if (_activeRoulettes <= 0)
            {
                StartCoroutine(ShowRewardDelay(_totalRewards));
            }

            _windows.Remove(window, true);
            _stoppedWindows.Add(window);
        }

        protected abstract ConcreteReward GetConcreteReward<TChance>(ChanceCombiner<TChance> chances, 
                                                                     int concreteChanceIndex = -1)
            where TChance : class;

        protected abstract RouletteWindow InitializeWindow(RouletteWindow window);

        protected abstract void InitializeSlots<TChance>(ChanceCombiner<TChance> chances, RouletteWindow window)
            where TChance : class;

        public virtual void OnSkip()
        {
            StopAllCoroutines();

            _onRoulette = false;

            Hide(true);

            _mainMenu.ShowRewards(_skipRewards);

            ClearWindows();
        }

        #region RewardsDelay
        protected virtual IEnumerator ShowRewardDelay(Reward reward)
        {
            yield return new WaitForSecondsRealtime(_showRewardsDelay);

            Hide(true);
            _mainMenu.ShowRewards(reward);

            ClearWindows();
        }

        protected virtual IEnumerator ShowRewardDelay(List<ConcreteReward> rewards)
        {
            yield return new WaitForSecondsRealtime(_showRewardsDelay);

            Hide(true);
            _mainMenu.ShowRewards(rewards); 
            
            ClearWindows();
        }

        protected virtual IEnumerator ShowRewardDelay(ConcreteReward reward)
        {
            yield return new WaitForSecondsRealtime(_showRewardsDelay);

            Hide(true);
            _mainMenu.ShowRewards(reward);

            ClearWindows();
        }
        #endregion

        protected virtual void ClearWindows()
        {
            if (_windows.Count > 0)
            {
                foreach (var window in _windows.List)
                {
                    _windowPool.Release(window);
                }

                _windows.List.Clear();
            }

            if (_stoppedWindows.Count > 0)
            {
                foreach (var window in _stoppedWindows)
                {
                    _windowPool.Release(window);
                }

                _stoppedWindows.Clear();
            }

            if (_slots.Count > 0)
            {
                foreach (var slot in _slots)
                {
                    _slotsPool.Release(slot);
                }

                _slots.Clear();
            }
        }
    }
}