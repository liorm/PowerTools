using System;
using System.Windows.Threading;

namespace LiorTech.PowerTools.Utilities
{
    /// <summary>
    /// Misc utilities for the dispatcher.
    /// </summary>
    public static class DispatcherUtils
    {
        #region DoEvents

        /// <summary>
        /// Process pending actions in the dispatcher
        /// </summary>
        /// <param name="a_level">Upto what level to process actions</param>
        public static void DoEvents(DispatcherPriority a_level = DispatcherPriority.Background)
        {
            DispatcherFrame frame = new DispatcherFrame();
            Dispatcher.CurrentDispatcher.BeginInvoke(a_level,
                                                     new Action<DispatcherFrame>(ExitFrame), frame);
            Dispatcher.PushFrame(frame);
        }

        private static void ExitFrame(DispatcherFrame a_f)
        {
            a_f.Continue = false;
        }

        /// <summary>
        /// Process pending actions in the dispatcher for the specified time.
        /// </summary>
        public static void DoEvents(TimeSpan a_timeout, DispatcherPriority a_level = DispatcherPriority.Background)
        {
            DispatcherFrame frame = new DispatcherFrame();
            DispatcherTimer timer = new DispatcherTimer(
                a_timeout,
                a_level, 
                (a_object, a_e) => ExitTimerFrame(a_object, frame), 
                Dispatcher.CurrentDispatcher);
            timer.Start();

            Dispatcher.PushFrame(frame);
        }

        private static void ExitTimerFrame(object a_timer, DispatcherFrame a_f)
        {
            ((DispatcherTimer)a_timer).Stop();
            a_f.Continue = false;
        }

        #endregion

        /// <summary>
        /// Execute the action or call BeginInvoke if access is denied.
        /// </summary>
        public static void BeginInvokeOrExecute(
            this Dispatcher a_dispatcher,
            Action a_action)
        {
            if (a_dispatcher.CheckAccess())
                a_action();
            else
                a_dispatcher.BeginInvoke(a_action);
        }

        /// <summary>
        /// Execute the action or call BeginInvoke if access is denied.
        /// </summary>
        public static void BeginInvokeOrExecute<T1>(
            this Dispatcher a_dispatcher,
            Action<T1> a_action, T1 a_arg1)
        {
            if (a_dispatcher.CheckAccess())
                a_action(a_arg1);
            else
                a_dispatcher.BeginInvoke(a_action, a_arg1);
        }

        /// <summary>
        /// Execute the action or call BeginInvoke if access is denied.
        /// </summary>
        public static void BeginInvokeOrExecute<T1, T2>(
            this Dispatcher a_dispatcher,
            Action<T1, T2> a_action, T1 a_arg1, T2 a_arg2)
        {
            if (a_dispatcher.CheckAccess())
                a_action(a_arg1, a_arg2);
            else
                a_dispatcher.BeginInvoke(a_action, a_arg1, a_arg2);
        }

        /// <summary>
        /// Execute the action or call BeginInvoke if access is denied.
        /// </summary>
        public static void BeginInvokeOrExecute<T1, T2, T3>(
            this Dispatcher a_dispatcher,
            Action<T1, T2, T3> a_action, T1 a_arg1, T2 a_arg2, T3 a_arg3)
        {
            if (a_dispatcher.CheckAccess())
                a_action(a_arg1, a_arg2, a_arg3);
            else
                a_dispatcher.BeginInvoke(a_action, a_arg1, a_arg2, a_arg3);
        }

        /// <summary>
        /// Execute the action or call BeginInvoke if access is denied.
        /// </summary>
        public static void BeginInvokeOrExecute<T1, T2, T3, T4>(
            this Dispatcher a_dispatcher,
            Action<T1, T2, T3, T4> a_action, T1 a_arg1, T2 a_arg2, T3 a_arg3, T4 a_arg4)
        {
            if (a_dispatcher.CheckAccess())
                a_action(a_arg1, a_arg2, a_arg3, a_arg4);
            else
                a_dispatcher.BeginInvoke(a_action, a_arg1, a_arg2, a_arg3, a_arg4);
        }

        /// <summary>
        /// Convinenece function to perform an action.
        /// </summary>
        /// <param name="a_dispatcher">Dispatcher to perform action on</param>
        /// <param name="a_callback">Action to perform</param>
        public static DispatcherOperation BeginInvoke(
            this Dispatcher a_dispatcher,
            Action a_callback)
        {
            return a_dispatcher.BeginInvoke(a_callback);
        }

        /// <summary>
        /// Convinenece function to perform an action.
        /// </summary>
        /// <param name="a_dispatcher">Dispatcher to perform action on</param>
        /// <param name="a_callback">Action to perform</param>
        /// <param name="a_arg1">First argument to the action</param>
        public static DispatcherOperation BeginInvoke<T1>(
            this Dispatcher a_dispatcher,
            Action<T1> a_callback, 
            T1 a_arg1)
        {
            return a_dispatcher.BeginInvoke(a_callback, a_arg1);
        }

        /// <summary>
        /// Convinenece function to perform an action.
        /// </summary>
        /// <param name="a_dispatcher">Dispatcher to perform action on</param>
        /// <param name="a_callback">Action to perform</param>
        /// <param name="a_arg1">First argument to the action</param>
        /// <param name="a_arg2">Second argument to the action</param>
        public static DispatcherOperation BeginInvoke<T1, T2>(
            this Dispatcher a_dispatcher,
            Action<T1, T2> a_callback,
            T1 a_arg1,
            T2 a_arg2)
        {
            return a_dispatcher.BeginInvoke(a_callback, a_arg1, a_arg2);
        }

        /// <summary>
        /// Convinenece function to perform an action.
        /// </summary>
        /// <param name="a_dispatcher">Dispatcher to perform action on</param>
        /// <param name="a_callback">Action to perform</param>
        /// <param name="a_arg1">First argument to the action</param>
        /// <param name="a_arg2">Second argument to the action</param>
        /// <param name="a_arg3">Third argument to the action</param>
        public static DispatcherOperation BeginInvoke<T1, T2, T3>(
            this Dispatcher a_dispatcher,
            Action<T1, T2, T3> a_callback,
            T1 a_arg1,
            T2 a_arg2,
            T3 a_arg3)
        {
            return a_dispatcher.BeginInvoke(a_callback, a_arg1, a_arg2, a_arg3);
        }
    }
}