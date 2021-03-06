﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Disposables;
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

        private CompositeDisposable m_disposables;

        /// <summary>
        /// Add the specified disposable to the collection of disposed objects when <see cref="CleanupViewModel"/> is called.
        /// </summary>
        /// <param name="a_disposable"></param>
        protected void AddDisposable(IDisposable a_disposable)
        {
            if ( m_disposables == null )
                m_disposables = new CompositeDisposable();

            m_disposables.Add(a_disposable);
        }

        /// <summary>
        /// Removes a disposable previously added with <see cref="AddDisposable"/>
        /// </summary>
        /// <param name="a_disposable">Disposable to remove</param>
        /// <param name="a_dispose">Should we dispose of the object automatically while removing it?</param>
        protected void RemoveDisposable(IDisposable a_disposable, bool a_dispose = false)
        {
            if ( m_disposables == null )
                return;

            if ( m_disposables.Remove(a_disposable) && a_dispose )
                a_disposable.Dispose();
        }

        /// <summary>
        /// Cleanup disposables associated with this VM (using <see cref="AddDisposable"/>).
        /// </summary>
        /// <remarks>
        /// <see cref="WindowViewModelBase"/> and <see cref="DialogViewModelBase"/> do this automatically.
        /// </remarks>
        protected void CleanupViewModel()
        {
            if ( m_disposables == null ) 
                return;

            m_disposables.Dispose();
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
