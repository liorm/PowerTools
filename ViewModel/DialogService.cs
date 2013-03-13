using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using ReactiveUI;

namespace LiorTech.PowerTools.ViewModel
{
    ///<summary>
    /// Provides dialog access methods for ViewModels 
    ///</summary>
    public sealed class DialogService
    {
        private DialogService()
        {
            
        }

        static DialogService()
        {
            Instance = new DialogService();
        }

        ///<summary>
        /// Access the single instance.
        ///</summary>
        public static DialogService Instance { get; private set; }

        private readonly HashSet<FrameworkElement> m_views = new HashSet<FrameworkElement>();

        /// <summary>
        /// Registers a View.
        /// </summary>
        /// <param name="a_view">The registered View.</param>
        private void Register(FrameworkElement a_view)
        {
            // Get owner window
            Window owner = a_view as Window ?? Window.GetWindow(a_view);

            if (owner == null)
            {
                if ( a_view.IsLoaded )
                    throw new InvalidOperationException(@"View is not contained within a Window.");

                // Wait for the control to load.
                a_view.Loaded += View_Loaded;
                return;
            }

            // Register for owner window closing, since we then should unregister View reference,
            // preventing memory leaks
            owner.Closed += OwnerClosed;

            m_views.Add(a_view);
        }

        private void View_Loaded(object a_sender, RoutedEventArgs a_e)
        {
            FrameworkElement element = (FrameworkElement)a_sender;
            element.Loaded -= View_Loaded;

            // Re-register the view.
            if (GetIsViewRegistered(element))
                Register(element);
        }


        /// <summary>
        /// Unregisters a View.
        /// </summary>
        /// <param name="view">The unregistered View.</param>
        private void Unregister(FrameworkElement view)
        {
            m_views.Remove(view);
        }

        #region Public methods.

        public bool ShowOpenFileDialog(IViewModel a_ownerViewModel, string a_title, string a_filters, out string[] a_selectedFiles)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = a_filters,
                FilterIndex = 0,
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = true,
                ValidateNames = true,
                Title = a_title,
                RestoreDirectory = true
            };

            var result = openFileDialog.ShowDialog(FindOwnerWindow(a_ownerViewModel)).Value;
            a_selectedFiles = openFileDialog.FileNames;
            return result;
        }

        public bool ShowOpenFileDialog(IViewModel a_ownerViewModel, string a_title, string a_filters, out string a_selectedFile)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = a_filters,
                    FilterIndex = 0,
                    CheckFileExists = true,
                    CheckPathExists = true,
                    Multiselect = false,
                    ValidateNames = true,
                    Title = a_title,
                    RestoreDirectory = true
                };

            var result = openFileDialog.ShowDialog(FindOwnerWindow(a_ownerViewModel)).Value;
            a_selectedFile = openFileDialog.FileName;
            return result;
        }

        public bool ShowSaveFileDialog(IViewModel a_ownerViewModel, string a_title, string a_filters, ref string a_selectedFile)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = a_filters,
                    FilterIndex = 0,
                    CheckFileExists = false,
                    CheckPathExists = true,
                    ValidateNames = true,
                    Title = a_title,
                    FileName = a_selectedFile,
                    RestoreDirectory = true
                };

            var result = saveFileDialog.ShowDialog(FindOwnerWindow(a_ownerViewModel)).Value;
            a_selectedFile = saveFileDialog.FileName;
            return result;
        }

        #region ShowDialog Functions

        /// <summary>
        /// Shows a dialog.
        /// </summary>
        /// <param name="a_ownerViewModel">A ViewModel that represents the owner window of the
        /// dialog.</param>
        /// <param name="a_viewModel">The ViewModel of the new dialog.</param>
        /// <param name="a_beforeShowingCallback">Function to call before showing the newly created window</param>
        /// <returns>A nullable value of type bool that signifies how a window was closed by the
        /// user.</returns>
        /// <remarks>This method guesses the type of the window to open by using <see cref="RxApp.GetService"/> for <see cref="IDialogView{T}"/> </remarks>
        public bool? ShowDialog<T>(T a_viewModel, IViewModel a_ownerViewModel = null, Action<Window> a_beforeShowingCallback = null)
            where T : class, IDialogViewModel
        {
            var dialogView = RxApp.GetService<IDialogView<T>>();
            Window window = dialogView as Window;
            if (window == null || dialogView == null)
                throw new InvalidOperationException(@"Can't find service for IDialogView<T> or not a Window for view model " + typeof(T).FullName);

            if (a_beforeShowingCallback != null)
                a_beforeShowingCallback(window);

            return ShowDialog( a_ownerViewModel, 
                               a_viewModel,
                               window);
        }

        private bool? ShowDialog(IViewModel a_ownerViewModel, IDialogViewModel a_viewModel, Window a_dialog)
        {
            // Create dialog and set properties
            a_dialog.Owner = a_ownerViewModel != null ? FindOwnerWindow(a_ownerViewModel) : null;
            a_dialog.DataContext = a_viewModel;

            // Bind the closing event to the viewmodel.
            CancelEventHandler cancelEventHandler = (a_sender, a_e) => a_e.Cancel = !a_viewModel.DialogIsClosing();
            a_dialog.Closing += cancelEventHandler;

            a_viewModel.DialogView = a_dialog as IDialogView;
            try
            {
                // Show dialog
                return a_dialog.ShowDialog();
            }
            finally
            {
                a_viewModel.DialogClosed();
                a_viewModel.DialogView = null;
                a_dialog.Closing -= cancelEventHandler;
            }
        }

        #endregion

        #region ShowWindow Functions

        /// <summary>
        /// Shows a window.
        /// This method guesses the type of the window to open by using <see cref="RxApp.GetService{T}"/> for <see cref="IWindowView{T}"/>
        /// </summary>
        /// <param name="a_viewModel">The ViewModel of the new dialog.</param>
        /// <param name="a_windowClosedCallback">Callback to call when dialog is closed</param>
        /// <param name="a_ownerViewModel">The owner view model</param>
        /// <param name="a_beforeShowingCallback">Called before the window is opened</param>
        /// <returns>A nullable value of type bool that signifies how a window was closed by the
        /// user.</returns>
        public void ShowWindow<T>(T a_viewModel, IViewModel a_ownerViewModel = null, Action a_windowClosedCallback = null, Action<Window> a_beforeShowingCallback = null)
            where T : class, IWindowViewModel
        {
            var windowView = RxApp.GetService<IWindowView<T>>();
            Window window = windowView as Window;
            if ( window == null || windowView == null )
                throw new InvalidOperationException(@"Can't find service for IWindowView<T> or not a Window for view model " + typeof(T).FullName);

            // Set the VM.
            windowView.ViewModel = a_viewModel;

            if (a_beforeShowingCallback != null)
                a_beforeShowingCallback(window);

            ShowWindow(
                a_ownerViewModel,
                a_viewModel, 
                window,
                a_windowClosedCallback);

			// Notify that the window is shown.
			a_viewModel.WindowShown();
        }

        private void ShowWindow(IViewModel a_ownerViewModel, IWindowViewModel a_viewModel, Window a_dialog, Action a_windowClosedCallback)
        {
            // Create dialog and set properties
            a_dialog.DataContext = a_viewModel;
            a_dialog.Owner = a_ownerViewModel != null ? FindOwnerWindow(a_ownerViewModel) : null;

            // Bind the closing event to the viewmodel.
            a_dialog.Closing += ShowWindowDialog_Closing;
            a_dialog.Closed += ShowWindowDialog_Closed;
            a_dialog.Tag = a_windowClosedCallback;
            a_viewModel.WindowView = a_dialog as IWindowView;

            // Show dialog
            a_dialog.Show();
        }

        private void ShowWindowDialog_Closing(object a_sender, CancelEventArgs a_e)
        {
            Window window = (Window)a_sender;
            IWindowViewModel viewModel = window.DataContext as IWindowViewModel;
            if (viewModel == null)
                return;

            a_e.Cancel = !viewModel.WindowIsClosing();
        }

        private void ShowWindowDialog_Closed(object a_sender, EventArgs a_e)
        {
            Window window = (Window)a_sender;
            IWindowViewModel viewModel = (IWindowViewModel)window.DataContext;

            viewModel.WindowClosed();

            viewModel.WindowView = null;
            window.Closing -= ShowWindowDialog_Closing;
            window.Closed -= ShowWindowDialog_Closed;

            Action closeAction = window.Tag as Action;
            if (closeAction != null)
            {
                closeAction();
                window.Tag = null;
            }

            // Remove the VM.
            IViewFor viewFor = window as IViewFor;
            if (viewFor != null)
                viewFor.ViewModel = null;
        }

        #endregion

        /// <summary>
        /// Shows a message box.
        /// </summary>
        /// <param name="a_ownerViewModel">A ViewModel that represents the owner window of the message
        /// box.</param>
        /// <param name="a_messageBoxText">A string that specifies the text to display.</param>
        /// <param name="a_caption">A string that specifies the title bar caption to display.</param>
        /// <param name="a_button">A MessageBoxButton value that specifies which button or buttons to
        /// display.</param>
        /// <param name="a_icon">A MessageBoxImage value that specifies the icon to display.</param>
        /// <returns>A MessageBoxResult value that specifies which message box button is clicked by the
        /// user.</returns>
        public MessageBoxResult ShowMessageBox(IViewModel a_ownerViewModel, string a_messageBoxText, string a_caption,
            MessageBoxButton a_button, MessageBoxImage a_icon, MessageBoxResult? a_defaultResult = null)
        {
			if ( a_defaultResult.HasValue )
				return MessageBox.Show(FindOwnerWindow(a_ownerViewModel), a_messageBoxText, a_caption, a_button, a_icon, a_defaultResult.Value);

			return MessageBox.Show(FindOwnerWindow(a_ownerViewModel), a_messageBoxText, a_caption, a_button, a_icon);
        }

        #endregion

        #region IsViewRegistered attached property

        /// <summary>
        /// Attached dependency property. Use <see cref="GetIsViewRegistered"/> and <see cref="SetIsViewRegistered"/>
        /// </summary>
        public static readonly DependencyProperty IsViewRegisteredProperty = DependencyProperty.RegisterAttached(
            @"IsViewRegistered",
            typeof(bool),
            typeof(DialogService),
            new FrameworkPropertyMetadata( false, new PropertyChangedCallback( OnIsViewRegisteredChanged ) ) );

        public static bool GetIsViewRegistered( DependencyObject a_sender )
        {
            return (bool) a_sender.GetValue( IsViewRegisteredProperty );
        }

        public static void SetIsViewRegistered( DependencyObject a_sender, bool a_value )
        {
            a_sender.SetValue( IsViewRegisteredProperty, a_value );
        }

        public static void ClearIsViewRegistered( DependencyObject a_sender )
        {
            a_sender.ClearValue(IsViewRegisteredProperty);
        }

        private static void OnIsViewRegisteredChanged( DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e )
        {
            FrameworkElement element = a_dependencyObject as FrameworkElement;
            if ( element == null )
                throw new InvalidOperationException( @"FrameworkElement required" );

            // The Visual Studio Designer or Blend will run this code when setting the attached
            // property, however at that point there is no IDialogService registered
            // in the ServiceLocator which will cause the Resolve method to throw a ArgumentException.
            if (DesignerProperties.GetIsInDesignMode(element)) return;

            // Cast values
            bool newValue = (bool)a_e.NewValue;

            if (newValue)
                Instance.Register(element);
            else
                Instance.Unregister(element);
        }

        #endregion

        /// <summary>
        /// Finds window corresponding to specified ViewModel.
        /// </summary>
        public Window FindOwnerWindow(IViewModel a_viewModel)
        {
            FrameworkElement view = m_views.LastOrDefault(a_v => ReferenceEquals(a_v.DataContext, a_viewModel));
            if (view == null)
                throw new ArgumentException("Viewmodel is not referenced by any registered View.");

            // Get owner window
            Window owner = view as Window;
            if (owner == null)
                owner = Window.GetWindow(view);

            // Make sure owner window was found
            if (owner == null)
                throw new InvalidOperationException(@"View is not contained within a Window.");

            return owner;
        }


        /// <summary>
        /// Handles owner window closed, View service should then unregister all Views acting
        /// within the closed window.
        /// </summary>
        private void OwnerClosed(object sender, EventArgs e)
        {
            Window owner = sender as Window;
            if (owner != null)
            {
                // Find Views acting within closed window
                IEnumerable<FrameworkElement> windowViews =
                    from view in m_views
                    where Window.GetWindow(view) == owner
                    select view;

                // Unregister Views in window
                foreach (FrameworkElement view in windowViews.ToArray())
                {
                    Unregister(view);
                }
            }
        }
    }
}
