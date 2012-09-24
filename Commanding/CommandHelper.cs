using System;
using System.Windows;

namespace LiorTech.PowerTools.Commanding
{
    static class CommandHelper
    {
        #region IsInputBoundry attached property

        /// <summary>
        /// Attached dependency property. Use <see cref="GetIsInputBoundry"/> and <see cref="SetIsInputBoundry"/>
        /// </summary>
        public static readonly DependencyProperty IsInputBoundryProperty = DependencyProperty.RegisterAttached(
            @"IsInputBoundry",
            typeof(bool),
            typeof(CommandHelper),
            new FrameworkPropertyMetadata(false, new PropertyChangedCallback(OnIsInputBoundryChanged)));

        public static bool GetIsInputBoundry( DependencyObject a_sender )
        {
            return (bool) a_sender.GetValue( IsInputBoundryProperty );
        }

        public static void SetIsInputBoundry( DependencyObject a_sender, bool a_value )
        {
            a_sender.SetValue( IsInputBoundryProperty, a_value );
        }

        private static void OnIsInputBoundryChanged( DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e )
        {
            UIElement element = a_dependencyObject as UIElement;
            if ( element == null )
                throw new InvalidOperationException( @"UIElement required" );

            
        }

        #endregion

    }
}
