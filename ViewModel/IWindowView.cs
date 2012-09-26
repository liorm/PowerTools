namespace LiorTech.PowerTools.ViewModel
{
    /// <summary>
    /// Represents a window view (works in conjuction with <see cref="IWindowViewModel"/>)
    /// </summary>
    public interface IWindowView
    {
        /// <summary>
        /// Request to close the window.
        /// </summary>
        void CloseWindow();
    }
}