using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.Achievements;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;
using ZombieSurvival.Rewards;

using Zenject;

namespace ZombieSurvival.UI
{
    public class TutorialButton : ZSMonoBehaviour, IInventoryUpdateHandler
    {
        [Header("TutorialButton settings")]
        [SerializeField] protected ZSButton _button;
        [SerializeField] protected bool _lockButton;

        [SerializeField] protected bool _changeIcon;
        [SerializeField] protected Image _iconImage;
        [SerializeField] protected Sprite _lockedSprite;
        [SerializeField] protected Sprite _unlockedSprite;

        [SerializeField] protected TutorialMenu _tutorialMenu;
        [SerializeField] protected List<Achievement> _achievementsToDisplay;

        protected bool _unlocked;

        protected List<TutorialAchievement> _tutorials;

        [Inject] protected MainInventory _mainInventory;
        [Inject] protected AchievementInventory _achievementInventory;
        
        protected void Awake()
        {
            if (_changeIcon)
            {
                _iconImage.sprite = _unlocked ? _unlockedSprite : _lockedSprite;
            }

            if (_lockButton)
            {
                _button.interactable = _unlocked;
            }
            else
            {
                _button.interactable = true;
            }

            if (_tutorialMenu != null)
            {
                _tutorialMenu.Initialize(null);
                _tutorialMenu.Hide();
            }
        }

        protected void OnEnable()
        {
            EventBus.Subscribe(this);
        }

        protected void OnDisable()
        {
            EventBus.Unsubscribe(this);
        }

        public virtual void OnTutorialEnd()
        {
            _tutorialMenu?.Hide(true);
            
            foreach(TutorialAchievement tutorial in _tutorials)
            {
                _mainInventory.Add(new AchievementObtainReward(tutorial));
            }
            
            _unlocked = true;

            _button.InvokeClick();
        }

        public void OnInvontoryUpdate(Inventory inventory)
        {
            if (_achievementsToDisplay == null || _achievementsToDisplay.Count == 0) return;

            if (inventory.Equals(_achievementInventory))
            {
                bool isUnlock = false;
                bool isFirst = false;

                _tutorials = new List<TutorialAchievement>();

                foreach (Achievement achievement in _achievementsToDisplay)
                {
                    if (_achievementInventory.Achievements.Find(item =>
                        item.Name.Equals(achievement.Name)) is TutorialAchievement tutorial)
                    {
                        _tutorials.Add(tutorial);

                        if (!tutorial.WasClaimed && tutorial.Unlocked)
                        {
                            isFirst = true;
                        }
                        else if (tutorial.Unlocked)
                        {
                            isUnlock = true;
                        }
                        else return;
                    }
                }

                if (isFirst)
                {
                    _tutorialMenu?.Display(true); 

                    _button.interactable = true;

                    if (_changeIcon)
                    {
                        _iconImage.sprite = _unlockedSprite;
                    }
                }
                else if (isUnlock)
                {
                    _unlocked = true;

                    _button.interactable = true;

                    if (_changeIcon)
                    {
                        _iconImage.sprite = _unlockedSprite;
                    }
                }
            }
        }
    }
}