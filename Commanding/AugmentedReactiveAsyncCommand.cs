using System;
using System.Reactive;
using System.Reactive.Subjects;
using ReactiveUI.Xaml;

namespace LiorTech.PowerTools.Commanding
{
	public class AugmentedReactiveAsyncCommand : IReactiveAsyncCommand, IDisposable, ICommandDescriptionProvider, ICommandImageProvider
	{
		public AugmentedReactiveAsyncCommand(ReactiveAsyncCommand a_subCommand, CommandDescriptionBase a_commandDescriptionBase, bool a_hasImageResource = false)
		{
			m_subCommand = a_subCommand;
			Description = a_commandDescriptionBase;
			HasImageResource = a_hasImageResource;
		}

		public AugmentedReactiveAsyncCommand(ReactiveAsyncCommand a_subCommand, CommandDescriptionBase a_commandDescriptionBase, Uri a_imageUriOverride)
		{
			m_subCommand = a_subCommand;
			Description = a_commandDescriptionBase;
			ImageUriOverride = a_imageUriOverride;
		}

		private readonly ReactiveAsyncCommand m_subCommand;

		#region Implementation of ICommandDescriptionProvider

		public CommandDescriptionBase Description { get; private set; }

		#endregion

		#region Implementation of ICommandImageProvider

		public bool HasImageResource { get; private set; }
		public Uri ImageUriOverride { get; private set; }

		#endregion

		#region Implementation of ICommand

		public void Execute(object parameter)
		{
			m_subCommand.Execute(parameter);
		}

		public bool CanExecute(object parameter)
		{
			return m_subCommand.CanExecute(parameter);
		}

		public event EventHandler CanExecuteChanged;

		#endregion

		#region Implementation of IObservable<out object>

		public IDisposable Subscribe(IObserver<object> observer)
		{
			return m_subCommand.Subscribe(observer);
		}

		#endregion

		#region Implementation of IHandleObservableErrors

		public IObservable<Exception> ThrownExceptions
		{
			get { return m_subCommand.ThrownExceptions; }
		}

		#endregion

		#region Implementation of IReactiveCommand

		public IObservable<bool> CanExecuteObservable
		{
			get { return m_subCommand.CanExecuteObservable; }
		}

		#endregion

		#region Implementation of IReactiveAsyncCommand

		public IObservable<int> ItemsInflight
		{
			get { return m_subCommand.ItemsInflight; }
		}

		public ISubject<Unit> AsyncStartedNotification
		{
			get { return m_subCommand.AsyncStartedNotification; }
		}

		public ISubject<Unit> AsyncCompletedNotification
		{
			get { return m_subCommand.AsyncCompletedNotification; }
		}

		#endregion

		#region Implementation of IDisposable

		public void Dispose()
		{
			m_subCommand.Dispose();
		}

		#endregion
	}
}