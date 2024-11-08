using UnityEngine;
using ZombieSurvival.General.Enums;
using ZombieSurvival.General.Sounds;

namespace ZombieSurvival.UI.General
{
    public class ButtonSoundPlayer : MonoBehaviour
    {
        [SerializeField] private SoundList _sounds;

        public void Click()
        {
            _sounds.PlaySound(SoundTypes.Click);
        }
    }
}