using System;
using System.Globalization;
using System.Windows;

namespace LiorTech.PowerTools.Globalization
{
	public class CultureChangedWeakEventManager : WeakEventManager
	{
		/// <summary>
		/// Creates a new GenericWeakEventManagerBase instance.
		/// </summary>
		public CultureChangedWeakEventManager()
		{
		}

		/// <summary>
		/// Adds a weak event listener.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static void AddListener( IWeakEventListener a_listener )
		{
			CurrentManager.ProtectedAddListener(null, a_listener);
		}

		/// <summary>
		/// Removes a weak event listener.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
		public static void RemoveListener( IWeakEventListener a_listener )
		{
			CurrentManager.ProtectedRemoveListener(null, a_listener);
		}

		/// <inheritdoc/>
		protected sealed override void StartListening( object a_source )
		{
			CultureManager.ApplicationUICultureChanged += OnCultureChanged;
		}

		/// <inheritdoc/>
		protected sealed override void StopListening( object a_source )
		{
			CultureManager.ApplicationUICultureChanged -= OnCultureChanged;
		}

		private void OnCultureChanged(CultureInfo a_newCulture)
		{
			DeliverEvent(null, new CultureChangedEventArgs() {NewCulture = a_newCulture});
		}

		/// <summary>
		/// Gets the current manager.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1721:PropertyNamesShouldNotMatchGetMethods")]
		protected static CultureChangedWeakEventManager CurrentManager
		{
			get
			{
				Type managerType = typeof(CultureChangedWeakEventManager);
				CultureChangedWeakEventManager manager = (CultureChangedWeakEventManager)GetCurrentManager(managerType);
				if ( manager == null )
				{
					manager = new CultureChangedWeakEventManager();
					SetCurrentManager(managerType, manager);
				}
				return manager;
			}
		}
	}
}