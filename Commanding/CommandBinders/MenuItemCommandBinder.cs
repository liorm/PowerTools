using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using LiorTech.PowerTools.Commanding.CommandBinders.Utilities;

namespace LiorTech.PowerTools.Commanding.CommandBinders
{
    public static class MenuItemCommandBinder
    {
        #region OverrideHeader attached property

        /// <summary>
        /// Attached dependency property. Use <see cref="GetOverrideHeader"/> and <see cref="SetOverrideHeader"/>
        /// </summary>
        public static readonly DependencyProperty OverrideHeaderProperty = DependencyProperty.RegisterAttached(
            "OverrideHeader",
            typeof(bool),
            typeof(MenuItemCommandBinder),
            new FrameworkPropertyMetadata( (bool) true, new PropertyChangedCallback( OnOverrideHeaderChanged ) ) );

        public static bool GetOverrideHeader( DependencyObject a_sender )
        {
            return (bool) a_sender.GetValue( OverrideHeaderProperty );
        }

        public static void SetOverrideHeader( DependencyObject a_sender, bool a_value )
        {
            a_sender.SetValue( OverrideHeaderProperty, a_value );
        }

        private static void OnOverrideHeaderChanged( DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e )
        {
            MenuItem element = a_dependencyObject as MenuItem;
            if ( element == null )
                throw new InvalidOperationException( "MenuItem required" );
        }

        #endregion


        #region Command attached property

        /// <summary>
        /// Attached dependency property. Use <see cref="GetCommand"/> and <see cref="SetCommand"/>
        /// </summary>
        public static readonly DependencyProperty CommandProperty = DependencyProperty.RegisterAttached(
            @"Command",
            typeof(ICommand),
            typeof(MenuItemCommandBinder),
            new FrameworkPropertyMetadata((ICommand)null, new PropertyChangedCallback(OnCommandChanged)));

        [AttachedPropertyBrowsableForType(typeof(MenuItem))]
        public static ICommand GetCommand(DependencyObject a_sender)
        {
            return (ICommand)a_sender.GetValue(CommandProperty);
        }

        public static void SetCommand(DependencyObject a_sender, ICommand a_value)
        {
            a_sender.SetValue( CommandProperty, a_value );
        }

        private static void OnCommandChanged( DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e )
        {
            MenuItem menuItem = a_dependencyObject as MenuItem;
            if (menuItem == null)
                throw new InvalidOperationException( @"MenuItem required" );

            ICommand oldCommand = a_e.OldValue as ICommand;
            if ( oldCommand != null )
            {
                menuItem.Command = null;
                menuItem.Icon = null;
                CommandToolTipHelper.ApplyCommandToolTip(menuItem, null);

                ICommandDescriptionProvider oldDescProvider = a_e.OldValue as ICommandDescriptionProvider;
                if (GetOverrideHeader(menuItem) && oldDescProvider != null)
                    BindingOperations.ClearBinding(
                        menuItem,
                        MenuItem.HeaderProperty);
            }

            ICommand newCommand = a_e.NewValue as ICommand;
            if (newCommand != null)
            {
                menuItem.Command = newCommand;

                Uri imageUri = CommandImageHelper.GetCommandImageUri(newCommand);
                if (imageUri != null)
                {
                    MenuItemImage image = new MenuItemImage
                                              {
                                                  Source = new BitmapImage( imageUri )
                                              };
                    menuItem.Icon = image;
                }

                // Header
                ICommandDescriptionProvider newDescProvider = a_e.NewValue as ICommandDescriptionProvider;
                if (GetOverrideHeader(menuItem) && newDescProvider != null)
                {
                    BindingOperations.SetBinding( 
                        menuItem, 
                        MenuItem.HeaderProperty, 
                        new Binding("Text") {Source = newDescProvider.Description} );
                }

                // tooltip
                CommandToolTipHelper.ApplyCommandToolTip(menuItem, newCommand);
            }
        }

        #endregion
    }
}
