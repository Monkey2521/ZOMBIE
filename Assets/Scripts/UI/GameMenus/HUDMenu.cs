using UnityEngine;
using ZombieSurvival.UI.Abilities;

namespace ZombieSurvival.UI.GameMenus.HUD
{
    public class HUDMenu : UIMenu
    {
        //[SerializeField] private SkillsInfo _skillInfo;

        public override void Initialize(MainMenu mainMenu, UIMenu parentMenu = null)
        {
            base.Initialize(mainMenu, parentMenu);

            //_skillInfo.Initialize(mainMenu, this);
        }
    }
}