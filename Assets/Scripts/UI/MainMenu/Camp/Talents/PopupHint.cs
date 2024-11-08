using UnityEngine;
using UnityEngine.UI;

using ZombieSurvival.General;
using ZombieSurvival.General.Enums;
using ZombieSurvival.General.Sounds;

namespace ZombieSurvival.UI.Camp
{
    public class PopupHint : ConfirmationMessage
    {
        [Header("Hint settings")]
        [SerializeField] private Image _currencyIcon;
        [SerializeField] private Text _currencyText;
        [SerializeField] private SoundList _soundList;

        public void Show(Currency currency)
        {
            _currencyIcon.sprite = currency.CurrencyData.Icon;
            _currencyText.text = currency.CurrencyValue.ToString();
        }

        public override void OnCancel()
        {
            (_parentMenu as CampTalentsMenu).HideHint();
        }

        public override void OnConfirm()
        {
            (_parentMenu as CampTalentsMenu).Unlock();
            (_parentMenu as CampTalentsMenu).HideHint();

            _soundList.PlaySound(SoundTypes.GetReward);
        }
    }
}