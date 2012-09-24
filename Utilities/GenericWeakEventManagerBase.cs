using System;
using System.Diagnostics;
using System.Windows;

namespace LiorTech.PowerTools.Utilities
{
    /// <summary>
    /// WeakEventManager with AddListener/RemoveListener and CurrentManager implementation.
    /// Helps implementing the WeakEventManager pattern with less code.
    /// </summary>
    public abstract class GenericWeakEventManagerBase<TManager, TEventSource> : WeakEventManager
        where TManager : GenericWeakEventManagerBase<TManager, TEventSource>, new()
        where TEventSource : class
    {
        /// <summary>
        /// Creates a new GenericWeakEventManagerBase instance.
        /// </summary>
        protected GenericWeakEventManagerBase()
        {
            Debug.Assert(GetType() == typeof(TManager));
        }

        /// <summary>
        /// Adds a weak event listener.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static void AddListener( TEventSource source, IWeakEventListener listener )
        {
            CurrentManager.ProtectedAddListener(source, listener);
        }

        /// <summary>
        /// Removes a weak event listener.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static void RemoveListener( TEventSource source, IWeakEventListener listener )
        {
            CurrentManager.ProtectedRemoveListener(source, listener);
        }

        /// <inheritdoc/>
        protected sealed override void StartListening( object a_source )
        {
            StartListening((TEventSource)a_source);
        }

        /// <inheritdoc/>
        protected sealed override void StopListening( object a_source )
        {
            StopListening((TEventSource)a_source);
        }

        /// <summary>
        /// Attaches the event handler.
        /// </summary>
        protected abstract void StartListening( TEventSource a_source );

        /// <summary>
        /// Detaches the event handler.
        /// </summary>
        protected abstract void StopListening( TEventSource a_source );

        /// <summary>
        /// Gets the current manager.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
        protected static TManager CurrentManager
        {
            get
            {
                Type managerType = typeof(TManager);
                TManager manager = (TManager)GetCurrentManager(managerType);
                if ( manager == null )
                {
                    manager = new TManager();
                    SetCurrentManager(managerType, manager);
                }
                return manager;
            }
        }
    }
}
