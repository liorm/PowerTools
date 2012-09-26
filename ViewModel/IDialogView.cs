namespace LiorTech.PowerTools.ViewModel
{
    /// <summary>
    /// Represents a dialog view (works in conjunction with <see cref="IDialogViewModel"/>)
    /// </summary>
    public interface IDialogView
    {
        ///<summary>
        /// Ask the dialog to close itself (called from the ViewModel).
        ///</summary>
        ///<param name="a_successValue">Return success or failure upon closure</param>
        void CloseDialog(bool a_successValue);
    }
}