using System;

namespace LiorTech.PowerTools.ViewModel
{
    /// <summary>
    /// Base class for a window view model (Default implementation of <see cref="IWindowViewModel"/>)
    /// </summary>
    public abstract class WindowViewModelBase : ViewModelBase, IWindowViewModel
    {
        #region Implementation of IWindowViewModel

        /// <summary>
        /// Set by the infrastructure with the current dialog view for this view model.
        /// </summary>
        IWindowView IWindowViewModel.WindowView { set { m_windowView = value; } }

    	public virtual void WindowShown()
    	{
			// Do nothing here.
    	}

    	/// <summary>
        /// Called by the infrastructure when the dialog is closing - the VM should return true if the dialog can close or false.
        /// </summary>
        /// <returns>true to close the dialog, false to keep it open</returns>
        public virtual bool WindowIsClosing()
        {
            return true;
        }

        #endregion

        /// <summary>
        /// Return the current associated view
        /// </summary>
        protected IWindowView WindowView { get { return m_windowView; } }

        private IWindowView m_windowView;
    }
}