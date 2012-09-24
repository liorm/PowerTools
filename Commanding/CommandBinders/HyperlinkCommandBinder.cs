using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;

namespace LiorTech.PowerTools.Commanding.CommandBinders
{
    /// <summary>
    /// Command binder utility for a <see cref="Hyperlink"/> using the ViewModel command concept.
    /// </summary>
    public static class HyperlinkCommandBinder
    {
        #region SetText attached property

        /// <summary>
        /// Attached dependency property. Use <see cref="GetSetText"/> and <see cref="SetSetText"/>
        /// </summary>
        public static readonly DependencyProperty SetTextProperty = DependencyProperty.RegisterAttached(
            @"SetText",
            typeof(bool),
            typeof(HyperlinkCommandBinder),
            new FrameworkPropertyMetadata( (bool) false, new PropertyChangedCallback( OnSetTextChanged ) ) );

        public static bool GetSetText( DependencyObject a_sender )
        {
            return (bool) a_sender.GetValue( SetTextProperty );
        }

        public static void SetSetText( DependencyObject a_sender, bool a_value )
        {
            a_sender.SetValue( SetTextProperty, a_value );
        }

        private static void OnSetTextChanged( DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e )
        {
            Hyperlink element = a_dependencyObject as Hyperlink;
            if ( element == null )
                throw new InvalidOperationException( @"Hyperlink required" );
        }

        #endregion

        #region Command attached property

        /// <summary>
        /// Set the command for the button.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            @"Command",
            typeof(ICommand),
            typeof(HyperlinkCommandBinder),
            new FrameworkPropertyMetadata((ICommand)null, new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// Gets a value for the attached dependency property <see cref="CommandProperty"/>
        /// </summary>
        public static ICommand GetCommand(DependencyObject a_sender)
        {
            return (ICommand)a_sender.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets a value for the attached dependency property <see cref="CommandProperty"/>
        /// </summary>
        public static void SetCommand(DependencyObject a_sender, ICommand a_value)
        {
            a_sender.SetValue(CommandProperty, a_value);
        }

        private static void OnCommandChanged(DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e)
        {
            Hyperlink hyperlink = a_dependencyObject as Hyperlink;
            if (hyperlink == null)
                throw new InvalidOperationException(@"Hyperlink required");

            ICommand oldCommand = a_e.OldValue as ICommand;
            if (oldCommand != null)
            {
                hyperlink.Command = null;
            }

            ICommand newCommand = a_e.NewValue as ICommand;
            if (newCommand != null)
            {
                hyperlink.Command = newCommand;

                ICommandDescriptionProvider descProvider = newCommand as ICommandDescriptionProvider;
                if (GetSetText(hyperlink) && descProvider != null)
                {
                    var run = new Run();
                    BindingOperations.SetBinding(
                        run,
                        Run.TextProperty,
                        new Binding("Text") { Source = descProvider.Description });
                    hyperlink.Inlines.Clear();
                    hyperlink.Inlines.Add(run);
                }
            }
        }

        #endregion
    }
}
