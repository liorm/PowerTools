using System.Windows;

namespace LiorTech.PowerTools.ViewModel
{
    /// <summary>
    /// Simply implement the <see cref="IDialogView"/> and <see cref="IWindowView"/> interfaces for a standard window.
    /// </summary>
    public class DialogWindow : Window, IDialogView, IWindowView
    {
        public DialogWindow()
        {
            // Register the window.
            DialogService.SetIsViewRegistered(this, true);
        }

        #region Implementation of IDialogView

        void IDialogView.CloseDialog( bool a_successValue )
        {
            DialogResult = a_successValue;
        }

        #endregion

        #region Implementation of IWindowView

        public void CloseWindow()
        {
            Close();
        }

        #endregion
    }
}
