using System;

namespace LiorTech.PowerTools.Commanding
{
    public interface ICommandImageProvider
    {
        /// <summary>
        /// Should we look for an image for the command? The command binders check this requirement.
        /// </summary>
        bool HasImageResource { get; }

        /// <summary>
        /// The location of the image (if <see cref="HasImageResource"/> is true.
        /// </summary>
        /// <remarks>If it's null, the Uri is guessed according to the command name</remarks>
        Uri ImageUriOverride { get; }
    }
}
