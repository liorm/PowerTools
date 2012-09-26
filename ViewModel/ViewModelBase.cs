using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using ReactiveUI;

namespace LiorTech.PowerTools.ViewModel
{
    /// <summary>
    /// Base class for view models.
    /// </summary>
    public abstract class ViewModelBase : ReactiveObject, IViewModel
    {
        static ViewModelBase()
        {
            // We use m_xXXX" property format.
            RxApp.GetFieldNameForPropertyNameFunc = a_prop => "m_" + Char.ToLower(a_prop[0]) + a_prop.Substring(1);
        }

        private static bool? m_isInDesignMode;
        public static bool IsInDesignMode
        {
            get
            {
                if (!m_isInDesignMode.HasValue)
                {
                    var prop = DesignerProperties.IsInDesignModeProperty;
                    m_isInDesignMode
                        = (bool)DependencyPropertyDescriptor
                        .FromProperty(prop, typeof(FrameworkElement))
                        .Metadata.DefaultValue;
                }

                return m_isInDesignMode.GetValueOrDefault();
            }
        }

        /// <summary>
        /// Returns the dispatcher associated with this view model. 
        /// </summary>
        /// <remarks>
        /// This is used by the BeginInvoke methods. 
        /// By default, it's the application dispatcher.
        /// </remarks>
        virtual protected Dispatcher Dispatcher { get { return Application.Current.Dispatcher; } }

        #region BeginInvoke functions

        protected void BeginInvoke(Action a_action)
        {
            Dispatcher.BeginInvoke(a_action);
        }

        protected void BeginInvoke<T1>(Action<T1> a_action, T1 a_arg1)
        {
            Dispatcher.BeginInvoke(a_action, a_arg1);
        }

        protected void BeginInvoke<T1, T2>(Action<T1, T2> a_action, T1 a_arg1, T2 a_arg2)
        {
            Dispatcher.BeginInvoke(a_action, a_arg1, a_arg2);
        }

        protected void BeginInvoke<T1, T2, T3>(Action<T1, T2, T3> a_action, T1 a_arg1, T2 a_arg2, T3 a_arg3)
        {
            Dispatcher.BeginInvoke(a_action, a_arg1, a_arg2, a_arg3);
        }

        protected void BeginInvoke<T1, T2, T3, T4>(Action<T1, T2, T3, T4> a_action, T1 a_arg1, T2 a_arg2, T3 a_arg3, T4 a_arg4)
        {
            Dispatcher.BeginInvoke(a_action, a_arg1, a_arg2, a_arg3, a_arg4);
        }

        #endregion
    }
}
