using System.Windows;
using System.Windows.Controls;

namespace LiorTech.PowerTools.Commanding.CommandBinders.Utilities
{
    /// <summary>
    /// Provides a specialized implementation of <see cref="Image"/> for command binders
    /// </summary>
    public class ButtonImage : Image
    {
        static ButtonImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(ButtonImage), 
                new FrameworkPropertyMetadata(typeof(ButtonImage)));

            StretchProperty.OverrideMetadata(typeof(ButtonImage),
											 new FrameworkPropertyMetadata(System.Windows.Media.Stretch.Uniform));
        }
    }
}
