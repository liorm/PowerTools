namespace LiorTech.PowerTools.ViewModel
{
    /// <summary>
    /// Base class for a dialog view model (Default implementation of <see cref="IDialogViewModel"/>)
    /// </summary>
    public abstract class DialogViewModelBase : ViewModelBase, IDialogViewModel
    {
        #region Implementation of IDialogViewModel

        /// <summary>
        /// Set by the infrastructure with the current dialog view for this view model.
        /// </summary>
        IDialogView IDialogViewModel.DialogView { set { m_dialogView = value; } }

        /// <summary>
        /// Called by the infrastructure when the dialog is closing - the VM should return true if the dialog can close or false.
        /// </summary>
        /// <returns>true to close the dialog, false to keep it open</returns>
        public virtual bool DialogIsClosing()
        {
            return true;
        }

        #endregion

        /// <summary>
        /// Return the current associated view
        /// </summary>
        protected IDialogView DialogView { get { return m_dialogView; } }

        private IDialogView m_dialogView;
    }
}

