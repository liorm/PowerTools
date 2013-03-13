namespace LiorTech.PowerTools.ViewModel
{
    /// <summary>
    /// Interface for a dialog view model.
    /// </summary>
    public interface IDialogViewModel : IViewModel
    {
        /// <summary>
        /// Set by the infrastructure with the current dialog view for this view model.
        /// </summary>
        IDialogView DialogView { set; }

        /// <summary>
        /// Called by the infrastructure when the dialog is closing - the VM should return true if the dialog can close or false.
        /// </summary>
        /// <returns>true to close the dialog, false to keep it open</returns>
        bool DialogIsClosing();

        /// <summary>
        /// Called by the infrastructure when the dialog is closed - the VM should free required resources.
        /// </summary>
        void DialogClosed();
    }
}
