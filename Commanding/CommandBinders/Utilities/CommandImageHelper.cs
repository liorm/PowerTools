using System;
using System.Reflection;
using System.Windows;
using System.Windows.Input;

namespace LiorTech.PowerTools.Commanding.CommandBinders.Utilities
{
    ///<summary>
    /// A delegate which is used by <see cref="CommandImageHelper"/> utility class to convert a command into an image.
    ///</summary>
    ///<param name="a_command">The command</param>
    ///<param name="a_imageName">The image we're looking for</param>
    public delegate Uri GetCommandImageUri(ICommand a_command, string a_imageName);

    /// <summary>
    /// Helper methods to retrieve an image for a command.
    /// </summary>
    public static class CommandImageHelper
    {
        /// <summary>
        /// Use this property to change the default image obtaining function (and override the default behavior).
        /// </summary>
        public static GetCommandImageUri GetCommandImageFunction
        {
            get { return m_getCommandImageFunction; }
            set { m_getCommandImageFunction = value; }
        }
        private static GetCommandImageUri m_getCommandImageFunction = StandardCommandImageLoader;

        private const string IMAGE_PATH = "CommandImages/";
        private const string IMAGE_EXTENSION = ".png";

        private static Uri StandardCommandImageLoader(ICommand a_command, string a_imageName)
        {
            string imageSource = Assembly.GetEntryAssembly().GetName().Name;
            var result = new Uri(
                string.Concat(
                    "pack://application:,,,/",
                    imageSource,
                    ";Component/",
                    IMAGE_PATH,
                    a_imageName,
                    IMAGE_EXTENSION));

            try
            {
                var resource = Application.GetResourceStream(result);
            }
            catch (Exception)
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// Return the Uri for the image of the specified command.
        /// </summary>
        internal static Uri GetCommandImageUri(ICommand a_command)
        {
            return GetCommandImageUri(a_command, string.Empty);
        }

        /// <summary>
        /// Return the Uri for the image of the specified command.
        /// </summary>
        /// <param name="a_command">The command to get an image for</param>
        /// <param name="a_postfix">A string to append to the end of the image file name we look up</param>
        /// <returns>The Uri or null if no image exists</returns>
        internal static Uri GetCommandImageUri(ICommand a_command, string a_postfix)
        {
            if ( a_command == null )
                throw new ArgumentNullException( "a_command" );

            var imageProvider = a_command as ICommandImageProvider;
            if (imageProvider == null)
                return null;

            Uri imageUri = imageProvider.ImageUriOverride;
            if (imageUri != null)
                return imageUri;

            if ( imageProvider.HasImageResource )
            {
                var descProvider = a_command as ICommandDescriptionProvider;
                if (descProvider == null)
                    return null;

                return m_getCommandImageFunction(a_command, string.Concat(descProvider.Description.Name, descProvider.Description.CommandImagePostfix ?? string.Empty, a_postfix));
            }

            return null;
        }
    }
}
