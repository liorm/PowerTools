using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace LiorTech.PowerTools.Commanding.CommandBinders
{
    /// <summary>
    /// Provides functionality to bind command to keyboard events.
    /// </summary>
    public static class GeneralInputCommandBinder
    {
        #region ClickCommand attached property

        /// <summary>
        /// Binding the enter key to the specified command.
        /// Attached dependency property. Use <see cref="GetClickCommand"/> and <see cref="SetClickCommand"/>
        /// </summary>
        public static readonly DependencyProperty ClickCommandProperty = DependencyProperty.RegisterAttached(
            @"ClickCommand",
            typeof(ICommand),
            typeof(GeneralInputCommandBinder),
            new FrameworkPropertyMetadata((ICommand)null, new PropertyChangedCallback(OnClickCommandChanged)));

        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ICommand GetClickCommand(DependencyObject a_sender)
        {
            return (ICommand)a_sender.GetValue(ClickCommandProperty);
        }

        public static void SetClickCommand(DependencyObject a_sender, ICommand a_value)
        {
            a_sender.SetValue(ClickCommandProperty, a_value);
        }

        private static void OnClickCommandChanged(DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e)
        {
            UIElement element = a_dependencyObject as UIElement;
            if (element == null)
                throw new InvalidOperationException(@"UIElement required");

            GenericBinder(element, new MouseGesture(MouseAction.LeftClick), a_e.NewValue as ICommand, a_e.OldValue as ICommand);
        }

        #endregion


        #region DoubleClickCommand attached property

        /// <summary>
        /// Binding the enter key to the specified command.
        /// Attached dependency property. Use <see cref="GetDoubleClickCommand"/> and <see cref="SetDoubleClickCommand"/>
        /// </summary>
        public static readonly DependencyProperty DoubleClickCommandProperty = DependencyProperty.RegisterAttached(
            @"DoubleClickCommand",
            typeof(ICommand),
            typeof(GeneralInputCommandBinder),
            new FrameworkPropertyMetadata((ICommand)null, new PropertyChangedCallback(OnDoubleClickCommandChanged)));

        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ICommand GetDoubleClickCommand(DependencyObject a_sender)
        {
            return (ICommand)a_sender.GetValue(DoubleClickCommandProperty);
        }

        public static void SetDoubleClickCommand(DependencyObject a_sender, ICommand a_value)
        {
            a_sender.SetValue(DoubleClickCommandProperty, a_value);
        }

        private static void OnDoubleClickCommandChanged(DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e)
        {
            UIElement element = a_dependencyObject as UIElement;
            if (element == null)
                throw new InvalidOperationException(@"UIElement required");

            GenericBinder(element, new MouseGesture(MouseAction.LeftDoubleClick), a_e.NewValue as ICommand, a_e.OldValue as ICommand);
        }

        #endregion

        #region EscCommand attached property

        /// <summary>
        /// Binding the enter key to the specified command.
        /// Attached dependency property. Use <see cref="GetEscCommand"/> and <see cref="SetEscCommand"/>
        /// </summary>
        public static readonly DependencyProperty EscCommandProperty = DependencyProperty.RegisterAttached(
            @"EscCommand",
            typeof(ICommand),
            typeof(GeneralInputCommandBinder),
            new FrameworkPropertyMetadata((ICommand)null, new PropertyChangedCallback(OnEscCommandChanged)));

        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ICommand GetEscCommand(DependencyObject a_sender)
        {
            return (ICommand)a_sender.GetValue(EscCommandProperty);
        }

        public static void SetEscCommand(DependencyObject a_sender, ICommand a_value)
        {
            a_sender.SetValue( EscCommandProperty, a_value );
        }

        private static void OnEscCommandChanged( DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e )
        {
            UIElement element = a_dependencyObject as UIElement;
            if ( element == null )
                throw new InvalidOperationException( @"UIElement required" );

            GenericBinder(element, new KeyGesture(Key.Escape), a_e.NewValue as ICommand, a_e.OldValue as ICommand);
        }

        #endregion

        #region EnterCommand attached property

        /// <summary>
        /// Binding the enter key to the specified command.
        /// Attached dependency property. Use <see cref="GetEnterCommand"/> and <see cref="SetEnterCommand"/>
        /// </summary>
        public static readonly DependencyProperty EnterCommandProperty = DependencyProperty.RegisterAttached(
            @"EnterCommand",
            typeof(ICommand),
            typeof(GeneralInputCommandBinder),
            new FrameworkPropertyMetadata((ICommand)null, new PropertyChangedCallback(OnEnterCommandChanged)));

        [AttachedPropertyBrowsableForType(typeof(UIElement))]
        public static ICommand GetEnterCommand(DependencyObject a_sender)
        {
            return (ICommand)a_sender.GetValue(EnterCommandProperty);
        }

        public static void SetEnterCommand(DependencyObject a_sender, ICommand a_value)
        {
            a_sender.SetValue( EnterCommandProperty, a_value );
        }

        private static void OnEnterCommandChanged( DependencyObject a_dependencyObject, DependencyPropertyChangedEventArgs a_e )
        {
            UIElement element = a_dependencyObject as UIElement;
            if ( element == null )
                throw new InvalidOperationException(@"UIElement required");

            GenericBinder(element, new KeyGesture(Key.Enter), a_e.NewValue as ICommand, a_e.OldValue as ICommand);
        }

        #endregion

        /// <summary>
        /// Perform the binding operation for any kind of button
        /// </summary>
        private static void GenericBinder(UIElement a_element, InputGesture a_gesture, ICommand a_newCommand, ICommand a_oldCommand)
        {
            if ( a_oldCommand != null )
            {
                var existingBindings = a_element.InputBindings.Cast<InputBinding>().Where(a_binding => a_binding.Command == a_oldCommand).ToList();
                existingBindings.ForEach(a_element.InputBindings.Remove);
            }

            if ( a_newCommand != null )
            {
                if ( a_gesture is MouseGesture )
                    a_element.InputBindings.Add(new MouseBinding(a_newCommand, a_gesture as MouseGesture));
                else
                    a_element.InputBindings.Add(new InputBinding(a_newCommand, a_gesture));
                
            }
        }
    }
}
