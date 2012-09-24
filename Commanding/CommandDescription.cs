namespace LiorTech.PowerTools.Commanding
{
    /// <summary>
    /// Holds a description for a command.
    /// </summary>
    public sealed class CommandDescription : CommandDescriptionBase
    {
        public CommandDescription(
            string a_name, 
            string a_text, string a_toolTip = null,
            string a_gestures = null, string a_gesturesText = null,
            string a_commandImagePostfix = null)
            : base(a_name, a_gestures, a_gesturesText)
        {
            Text = a_text ?? string.Empty;
            ToolTip = a_toolTip ?? string.Empty;

            CommandImagePostfix = a_commandImagePostfix;
        }
    }
}
