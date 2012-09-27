using ReactiveUI;

namespace LiorTech.PowerTools.ViewModel
{
    /// <summary>
    /// Represents a window view (works in conjuction with <see cref="IWindowViewModel"/>)
    /// </summary>
    public interface IWindowView : IViewFor
    {
        /// <summary>
        /// Request to close the window.
        /// </summary>
        void CloseWindow();
    }

    /// <summary>
    /// Represents a window view (works in conjuction with <see cref="IWindowViewModel"/>)
    /// </summary>
    public interface IWindowView<T> : IWindowView, IViewFor<T>
        where T : class, IWindowViewModel
    {
    }
}