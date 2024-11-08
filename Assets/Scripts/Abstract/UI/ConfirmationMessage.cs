namespace ZombieSurvival.UI
{
    public abstract class ConfirmationMessage : UIMenu
    {
        public abstract void OnCancel();

        public abstract void OnConfirm();
    }
}