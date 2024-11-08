using ZombieSurvival.UI;

namespace ZombieSurvival.UI.GameMenus.Pause
{
    public sealed class PauseExitConfirmation : ConfirmationMessage
    {
        public override void OnCancel()
        {
            (_parentMenu as PauseMenu).OnConfirmationCancel();
        }

        public override void OnConfirm()
        {
            (_parentMenu as PauseMenu).OnConfirmationExit();
        }
    }
}