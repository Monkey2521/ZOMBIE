using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.General.Breakpoints;
using ZombieSurvival.General.Sounds;
using ZombieSurvival.Levels;

namespace ZombieSurvival.UI.General
{
    public sealed class LevelProgress : FillBar, IGameStartHandler, IBossEventHandler, IBossEventEndedHandler
    {
        [Header("LevelProgress settings")]
        [SerializeField] private Text _breakpointAlertText;
        [SerializeField] private LevelProgressBreakpoint _breakpointPrefab;
        [SerializeField] private Text _progressText;

        private List<AlertBreakpoint> _alertBreakpoints;
        private LevelRewardBreakpoints _levelBreakpoints;

        private int _maxLevelTime;
        private bool _onBossEvent;

        public int Value => _value;

        [Inject] private LevelContext _levelContext;

        private void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        private void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public override void Initialize()
        {
            _value = _minFillValue;

            _alertBreakpoints = new List<AlertBreakpoint>();
            _levelBreakpoints = _levelContext.LevelRewards;

            base.Initialize();
        }

        public void OnGameStart()
        {
            Initialize();

            foreach (Breakpoint breakpoint in _levelContext.AllBreakpoints.Breakpoints)
            {
                if (breakpoint.Icon != null)
                {
                    LevelProgressBreakpoint newBreakpoint = Instantiate(_breakpointPrefab, transform);

                    newBreakpoint.Transform.anchorMin = new Vector2(breakpoint.RequiredProgress * 0.01f, 0.5f);
                    newBreakpoint.Transform.anchorMax = new Vector2(breakpoint.RequiredProgress * 0.01f, 0.5f);

                    newBreakpoint.Transform.anchoredPosition = new Vector2(0f, 0f);

                    newBreakpoint.SetBreakpoint(breakpoint);

                    _alertBreakpoints.Add(new AlertBreakpoint
                        (
                            _levelContext.LevelLenght * breakpoint.RequiredProgress * 0.01f - breakpoint.DisplayTime,
                            breakpoint.DisplayTime,
                            breakpoint.Description,
                            breakpoint.Sound,
                            breakpoint is BossBreakpoint
                        ));
                }
            }

            _maxLevelTime = _levelContext.LevelLenght;
            _breakpointAlertText.enabled = false;
        }

        public void OnBossEvent()
        {
            _onBossEvent = true;
        }

        public void OnBossEventEnd()
        {
            _onBossEvent = false;

            if (_value >= _maxFillValue)
            {
                if (_isDebug) Debug.Log("Level complete!");

                EventBus.Publish<ILevelPassedHandler>(handler => handler.OnLevelPassed(_levelContext));
            }

            if (!_levelContext.wasPassed)
            {
                Breakpoint breakpoint = _levelBreakpoints.CheckReaching(_value);

                if (breakpoint != null)
                {
                    if (_isDebug) Debug.Log("Reached level reward!");
                }

                if (_value == _maxFillValue)
                {
                    _levelContext.wasPassed = true;
                }
            }
        }

        public void OnTimerUpdate(float time)
        {
            if (_onBossEvent) return;

            int newVal = (int)(time / _maxLevelTime * _maxFillValue);

            if (_value != newVal)
            {
                _value = newVal;

                UpdateBar();
            }

            AlertBreakpoint alertBreakpoint = _alertBreakpoints.Find(item => item.Reached == false && 
                                                                     item.Time <= time);

            if (alertBreakpoint != null)
            {
                alertBreakpoint.SetReached(true);

                _breakpointAlertText.text = alertBreakpoint.Description;
                _breakpointAlertText.enabled = true;

                EventBus.Publish<ISoundPlayHandler>(handler => handler.OnSoundPlay(alertBreakpoint.Sound));

                StartCoroutine(WaitAlert(alertBreakpoint));

                if (alertBreakpoint.IsBossAlert)
                {
                    EventBus.Publish<IBossEventAlertHandler>(handler => handler.OnBossEventAlert());
                }
            }
        }

        protected override void UpdateBar()
        {
            if (_isDebug) Debug.Log("Update progress bar");

            base.UpdateBar();

            _progressText.text = _value.ToString();

            EventBus.Publish<ILevelProgressUpdateHandler>(handler => handler.OnLevelProgressUpdate(_value));

            if (_value >= _maxFillValue && !_onBossEvent)
            {
                if (_isDebug) Debug.Log("Level complete!");

                EventBus.Publish<ILevelPassedHandler>(handler => handler.OnLevelPassed(_levelContext));
            }
        }

        private IEnumerator WaitAlert(AlertBreakpoint breakpoint)
        {
            yield return new WaitForSeconds(breakpoint.DisplayTime);

            _breakpointAlertText.enabled = false;
        }

        private class AlertBreakpoint
        {
            public float Time { private set; get; }
            public float DisplayTime { private set; get; }
            public string Description { private set; get; }
            public SoundType Sound { private set; get; }
            public bool Reached { private set; get; }
            public bool IsBossAlert { private set; get; }

            public AlertBreakpoint(float time, float displayTime, string description, SoundType sound, bool isBossAlert)
            {
                Time = time;
                DisplayTime = displayTime;
                Description = description;
                Sound = sound;
                Reached = false;
                IsBossAlert = isBossAlert;
            }

            public void SetReached(bool value)
            {
                Reached = value;
            }
        }
    }
}