using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using ZombieSurvival.Characters;
using ZombieSurvival.Events;
using ZombieSurvival.General;
using ZombieSurvival.UI.Abilities;

namespace ZombieSurvival.UI.GameMenus.Pause
{
    public sealed class PauseMenu : UIMenu
    {
        [Header("PauseMenu settings")]
        [SerializeField] private ConfirmationMessage _confirmationMessage;
        [SerializeField] private SettingsMenu _settingsMenu;

        [Header("Abilities settings")]
        [SerializeField] private AbilityInfoSlot _abilitySlotPrefab;
        [SerializeField] private Transform _weaponSlotsParent;
        [SerializeField] private Transform _passiveSlotsParent;

        [Inject] private Player _player;

        private List<AbilityInfoSlot> _weaponSlots;
        private List<AbilityInfoSlot> _passiveSlots;

        private bool _onLoad;

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            _confirmationMessage.Initialize(mainMenu, parentMenu);
            _settingsMenu.Initialize(mainMenu, parentMenu);

            _confirmationMessage.Hide();
            _settingsMenu.Hide();

            _weaponSlots = new List<AbilityInfoSlot>(_player.AbilityInventory.MaxActiveAbilitiesCount);
            _passiveSlots = new List<AbilityInfoSlot>(_player.AbilityInventory.MaxPassiveAbilitiesCount);

            for (int i = 0; i < _player.AbilityInventory.MaxActiveAbilitiesCount; i++)
            {
                _weaponSlots.Add(Instantiate(_abilitySlotPrefab, _weaponSlotsParent));
                _weaponSlots[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < _player.AbilityInventory.MaxPassiveAbilitiesCount; i++)
            {
                _passiveSlots.Add(Instantiate(_abilitySlotPrefab, _passiveSlotsParent));
                _passiveSlots[i].gameObject.SetActive(false);
            }

            _onLoad = false;
        }

        public override void Display(bool playAnimation = false)
        {
            UpdateInventory();

            base.Display(playAnimation);

            _confirmationMessage.Hide();
            _settingsMenu.Hide();
        }

        public override void Hide(bool playAnimation = false)
        {
            base.Hide(playAnimation);

            _confirmationMessage.Hide();
            _settingsMenu.Hide();

            for (int i = 0; i < _weaponSlots.Count; i++)
            {
                _weaponSlots[i].gameObject.SetActive(false);
            }

            for (int i = 0; i < _passiveSlots.Count; i++)
            {
                _passiveSlots[i].gameObject.SetActive(false);
            }
        }

        public void OnContinue()
        {
            _mainMenu.DisplayDefault();
        }

        public void OnExit()
        {
            _confirmationMessage.Display(true);
            _settingsMenu.Hide();
        }

        public void OnConfirmationExit()
        {
            if (_onLoad) return;

            _onLoad = true;

            var operation = SceneManager.LoadSceneAsync(GameData.MainMenuScene);

            operation.completed += new System.Action<AsyncOperation>((o) => Time.timeScale = 1);
        }

        public void OnConfirmationCancel()
        {
            _confirmationMessage.Hide(true);
            _settingsMenu.Hide();
        }

        public void OnSettings()
        {
            _confirmationMessage.Hide();
            _settingsMenu.Display(true);
        }

        public void OnSettingsExit()
        {
            _confirmationMessage.Hide();
            _settingsMenu.Hide(true);
        }

        private void UpdateInventory()
        {
            for (int i = 0; i < _player.AbilityInventory.Weapons.Count; i++)
            {
                _weaponSlots[i].gameObject.SetActive(true);
                _weaponSlots[i].Initialize(_player.AbilityInventory.Weapons[i]);
            }

            for (int i = 0; i < _player.AbilityInventory.PassiveAbilities.Count; i++)
            {
                _passiveSlots[i].gameObject.SetActive(true);
                _passiveSlots[i].Initialize(_player.AbilityInventory.PassiveAbilities[i]);
            }
        }
    }
}