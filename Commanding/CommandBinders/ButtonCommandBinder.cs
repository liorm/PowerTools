using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using LiorTech.PowerTools.Commanding.CommandBinders.Utilities;

namespace LiorTech.PowerTools.Commanding.CommandBinders
{
    /// <summary>
    /// Command binder utility for a <see cref="Button"/> using the ViewModel command concept.
    /// </summary>
    public static class ButtonCommandBinder
    {
        #region AppendText attached property

        public static readonly DependencyProperty AppendTextProperty = DependencyProperty.RegisterAttached(
            @"AppendText",
            typeof(Dock?),
            typeof(ButtonCommandBinder),
            new FrameworkPropertyMetadata( null, new PropertyChangedCallback( OnAppendTextChanged ) ) );

        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static Dock? GetAppendText(DependencyObject a_sender)
        {
            return (Dock?)a_sender.GetValue(AppendTextProperty);
        }

        public static void SetAppendText(DependencyObject a_sender, Dock? a_value)
        {
            a_sender.SetValue( AppendTextProperty, a_value );
        }

        private static void OnAppendTextChanged( DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e )
        {
            Button element = a_dependencyObject as Button;
            if ( element == null )
                throw new InvalidOperationException( @"Button required" );
        }

        #endregion

        #region OverrideContent attached property

        /// <summary>
        /// Attached dependency property. Use <see cref="GetOverrideContent"/> and <see cref="SetOverrideContent"/>
        /// When set to false, the <see cref="CommandProperty"/> will override set the content of the button
        /// </summary>
        public static readonly DependencyProperty OverrideContentProperty = DependencyProperty.RegisterAttached(
            @"OverrideContent",
            typeof(bool),
            typeof(ButtonCommandBinder),
            new FrameworkPropertyMetadata( (bool) true ) );

        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static bool GetOverrideContent(DependencyObject a_sender)
        {
            return (bool) a_sender.GetValue( OverrideContentProperty );
        }

        public static void SetOverrideContent( DependencyObject a_sender, bool a_value )
        {
            a_sender.SetValue( OverrideContentProperty, a_value );
        }

        #endregion


        #region Command attached property

        /// <summary>
        /// Set the command for the button.
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            @"Command",
            typeof(ICommand),
            typeof(ButtonCommandBinder),
            new FrameworkPropertyMetadata((ICommand)null, new PropertyChangedCallback(OnCommandChanged)));

        /// <summary>
        /// Gets a value for the attached dependency property <see cref="CommandProperty"/>
        /// </summary>
        [AttachedPropertyBrowsableForType(typeof(Button))]
        public static ICommand GetCommand(DependencyObject a_sender)
        {
            return (ICommand)a_sender.GetValue(CommandProperty);
        }

        /// <summary>
        /// Sets a value for the attached dependency property <see cref="CommandProperty"/>
        /// </summary>
        public static void SetCommand(DependencyObject a_sender, ICommand a_value)
        {
            a_sender.SetValue( CommandProperty, a_value );
        }

        private static void OnCommandChanged( DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e )
        {
            Button button = a_dependencyObject as Button;
            if ( button == null )
                throw new InvalidOperationException( @"Button required" );

            ICommand oldCommand = a_e.OldValue as ICommand;
            if ( oldCommand != null )
            {
                button.Command = null;
                CommandToolTipHelper.ApplyCommandToolTip(button, null);

                ICommandDescriptionProvider oldDescProvider = a_e.OldValue as ICommandDescriptionProvider;
                if (GetOverrideContent(button) && oldDescProvider != null)
                    button.Content = null;
            }

            ICommand newCommand = a_e.NewValue as ICommand;
            if ( newCommand != null )
            {
                button.Command = newCommand;
                CommandToolTipHelper.ApplyCommandToolTip(button, newCommand);

                ICommandDescriptionProvider newDescProvider = a_e.NewValue as ICommandDescriptionProvider;
                if (GetOverrideContent(button) && newDescProvider != null)
                {
                    var textContent = new TextBlock()
                                            {
                                                TextAlignment = TextAlignment.Center,
                                                TextWrapping = TextWrapping.Wrap,
                                            };
                    BindingOperations.SetBinding(
                        textContent,
                        TextBlock.TextProperty,
                        new Binding("Text") { Source = newDescProvider.Description });


                    Uri imageUri = CommandImageHelper.GetCommandImageUri(newCommand);
                    if (imageUri != null)
                    {
                        Dock? useText = GetAppendText(button);

                        ButtonImage image = new ButtonImage()
                            {
                                VerticalAlignment = VerticalAlignment.Center,
                                HorizontalAlignment = HorizontalAlignment.Center,
                                Source = new BitmapImage(imageUri),
                            };

                        if ( useText.HasValue )
                        {
                            Label text = new Label()
                                {
                                    Margin = new Thickness(2),
                                    VerticalAlignment = VerticalAlignment.Center,
                                    HorizontalAlignment = HorizontalAlignment.Center,
                                    Content = textContent,
                                };
                            DockPanel.SetDock( text, useText.Value );

                            DockPanel container = new DockPanel();
                            container.LastChildFill = true;
                            container.Children.Add(text);
                            container.Children.Add(image);

                            button.Content = container;
                        }
                        else
                            button.Content = image;
                    }
                    else
                        button.Content = textContent;
                }

            }
        }

        #endregion
    }
}
