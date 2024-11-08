using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.General.Inventories;

using Zenject;

namespace ZombieSurvival.UI.Camp
{
    public class CampTalentsMenu : UIMenu
    {
        [Header("TalentsMenu settings")]
        [SerializeField] private PopupHint _hint;
        [SerializeField] private TalentSlot _slotPrefab;
        [SerializeField] private RectTransform _slotsParent;
        [SerializeField] private GridLayoutGroup _grid;

        [SerializeField] private ScrollRect _sr;

        private List<TalentSlot> _slots;

        private TalentSlot _currentSlot;

        [Inject] private MainInventory _mainInventory;
        [Inject] private PlayerLevelInventory _playerLevelInventory;
        [Inject] private CampInventory _camoInventory;

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _hint.Initialize(mainMenu, this);
            _hint.Hide();

            _slots = new List<TalentSlot>();

            _slotsParent.sizeDelta = new Vector2(0, GetInventoryHeight(_camoInventory.Talents.Count));

            foreach (var talent in _camoInventory.Talents)
            {
                TalentSlot slot = Instantiate(_slotPrefab, _slotsParent);

                slot.Initialize(this, talent, _mainInventory, mainMenu);

                talent.SetSlot(slot);
            }
        }

        public override void Display(bool playAnimation = false)
        {
            base.Display(playAnimation);

            _hint.Hide();

            _currentSlot = null;

            _slotsParent.localPosition = new Vector3(_slotsParent.localPosition.x, _slotsParent.sizeDelta.y * 0.5f);

            foreach (var slot in _slots)
            {
                slot.UpdateSlot();
            }
        }

        public void ShowHint(TalentSlot slot)
        {
            if (_playerLevelInventory.PlayerLevel.Value >= slot.Talent.RequiredLevel)
            {
                _currentSlot = slot;

                _hint.Show(slot.Talent.RequiredCurrency);
                _hint.Display(true);
            }
            else
            {
                _mainMenu.ShowPopupMessage("You need " + slot.Talent.RequiredLevel + " level to unlock this talent!");
            }
        }

        public void HideHint()
        {
            _hint.Hide(true);

            _currentSlot = null;
        }

        public void Unlock()
        {
            _hint.Hide(true);

            _currentSlot.Unlock();
            _currentSlot = null;
        }

        private int GetInventoryHeight(int rows)
        {
            return _grid.padding.top + (int)_grid.cellSize.y + _grid.padding.bottom + (int)_grid.cellSize.y * rows + (int)_grid.spacing.y * (rows - 1);
        }
    }
}