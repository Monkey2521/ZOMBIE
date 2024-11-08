using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;
using ZombieSurvival.UI;

using Zenject;

namespace ZombieSurvival.Achievements.UI
{
    public sealed class AchievementsMenu : UIMenu
    {
        [Header("AchievementsMenu settings")]
        [SerializeField] private AchievementSlot _achievementSlotPrefab;
        [SerializeField] private RectTransform _achievementSlotsParent;
        [SerializeField] private GridLayoutGroup _grid;

        private List<AchievementSlot> _slots;

        [Inject] private MainInventory _mainInventory;
        [Inject] private AchievementInventory _achievementInventory;

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _slots = new List<AchievementSlot>();

            foreach (Achievement achievement in _achievementInventory.Achievements)
            {
                if (!achievement.DisplayAchievement) continue;

                AchievementSlot slot = Instantiate(_achievementSlotPrefab, _achievementSlotsParent);

                slot.Initialize(achievement, _mainInventory, mainMenu);

                _slots.Add(slot);
            }

            _achievementSlotsParent.sizeDelta = new Vector2
                (
                    _achievementSlotsParent.sizeDelta.x,
                    GetInventoryHeight(_achievementInventory.Achievements.FindAll(item => item.DisplayAchievement).Count)
                );
        }

        public override void Display(bool playAnimation = false)
        {
            base.Display(playAnimation);

            foreach(AchievementSlot slot in _slots)
            {
                slot.UpdateSlot();
            }

            _achievementSlotsParent.transform.position = new Vector3(0, -_achievementSlotsParent.sizeDelta.y * 0.5f, 0);
        }

        private int GetInventoryHeight(int rows)
        {
            return _grid.padding.top + _grid.padding.bottom + (int)_grid.cellSize.y * rows + (int)_grid.spacing.y * (rows - 1);
        }
    }
}