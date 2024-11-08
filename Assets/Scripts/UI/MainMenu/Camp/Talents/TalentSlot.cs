using UnityEngine;
using UnityEngine.UI;
using ZombieSurvival.General;
using ZombieSurvival.Rewards;

namespace ZombieSurvival.UI.Camp
{
    public class TalentSlot : MonoBehaviour
    {
        [SerializeField] private Button _button;

        [SerializeField] private Image _talentBackround;
        [SerializeField] private Image _talentIcon;
        [SerializeField] private Text _talentDescriptionText;
        [SerializeField] private Text _requiredLevelText;

        private CampTalentsMenu _menu;
        private Talent _talent;
        private MainInventory _mainInventory;
        private MainMenu _mainMenu;

        public Talent Talent => _talent;

        public void Initialize(CampTalentsMenu menu, Talent talent, MainInventory mainInventory, MainMenu mainMenu)
        {
            _menu = menu;
            _talent = talent;

            _mainInventory = mainInventory;
            _mainMenu = mainMenu;
        }

        public void UpdateSlot()
        {
            if (_talent.Unlocked)
            {
                _talentBackround.sprite = _talent.UnlockedBackground;
                _talentIcon.sprite = _talent.UnlockedIcon;
                _button.interactable = false;
            }
            else
            {
                _talentBackround.sprite = _talent.LockedBackground;
                _talentIcon.sprite = _talent.LockedIcon;
                _button.interactable = true;
            }

            _talentDescriptionText.text = _talent.Description;
            _requiredLevelText.text = _talent.RequiredLevel.ToString();
        }

        public void OnClick()
        {
            _menu.ShowHint(this);
        }

        public void Unlock()
        {
            if (_mainInventory.Spend(new CurrencyReward(_talent.RequiredCurrency)))
            {
                _mainInventory.Add(new CampTalentReward(_talent));
            }
            else
            {
                _mainMenu.ShowPopupMessage("Not enough resources!");
            }
        }
    }
}