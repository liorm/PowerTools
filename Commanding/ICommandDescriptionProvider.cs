
namespace LiorTech.PowerTools.Commanding
{
    public interface ICommandDescriptionProvider
    {
        /// <summary>
        /// A better command description
        /// </summary>
        CommandDescriptionBase Description { get; }
    }
}