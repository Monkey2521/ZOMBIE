using UnityEngine;

using ZombieSurvival.General.Enums;
using ZombieSurvival.General.Sounds;

namespace ZombieSurvival.UI
{
    public class SoundedButton : ZSButton 
    {
        [Header("Sounds settings")]
        [SerializeField] protected SoundList _sounds;

        public override void OnButtonClick()
        {
            base.OnButtonClick();

            if (_interactable)
            {
                _sounds.PlaySound(SoundTypes.Click);
            }
            else
            {
                _sounds.PlaySound(SoundTypes.LockedClick);
            }
        }
    }
}