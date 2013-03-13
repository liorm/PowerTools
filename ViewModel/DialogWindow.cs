using System;
using System.Windows;
using ReactiveUI;

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

        #region Implementation of IViewFor

        public object ViewModel { get; set; }

        #endregion
    }

    /// <summary>
    /// Conviently implement <see cref="IViewFor{T}"/> as <see cref="DialogWindow"/>
    /// </summary>
    /// <typeparam name="TViewModel">The viewmodel representing this view</typeparam>
    public class DialogWindow<TViewModel> : DialogWindow, IViewFor<TViewModel> 
        where TViewModel : class
    {

        #region Implementation of IViewFor<TViewModel>

        new public TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        #endregion
    }

    /// <summary>
    /// Conviently implement <see cref="IWindowView{T}"/> as <see cref="DialogWindow"/>
    /// </summary>
    /// <typeparam name="TViewModel">The viewmodel representing this view</typeparam>
    public class WindowView<TViewModel> : DialogWindow<TViewModel>, IWindowView<TViewModel> 
        where TViewModel : class, IWindowViewModel
    {
    }

    /// <summary>
    /// Conviently implement <see cref="IDialogView{T}"/> as <see cref="DialogWindow"/>
    /// </summary>
    /// <typeparam name="TViewModel">The viewmodel representing this view</typeparam>
    public class DialogView<TViewModel> : DialogWindow<TViewModel>, IDialogView<TViewModel> 
        where TViewModel : class, IDialogViewModel
    {
    }
}
