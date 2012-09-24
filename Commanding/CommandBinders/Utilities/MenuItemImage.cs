using System.Windows;
using System.Windows.Controls;

namespace LiorTech.PowerTools.Commanding.CommandBinders.Utilities
{
    /// <summary>
    /// Provides a specialized implementation of <see cref="Image"/> for command binders
    /// </summary>
    public class MenuItemImage : Image
    {
        static MenuItemImage()
        {
            DefaultStyleKeyProperty.OverrideMetadata(
                typeof(MenuItemImage),
                new FrameworkPropertyMetadata(typeof(MenuItemImage)));

            StretchProperty.OverrideMetadata(typeof(MenuItemImage),
											 new FrameworkPropertyMetadata(System.Windows.Media.Stretch.Uniform));

            MaxWidthProperty.OverrideMetadata(typeof(MenuItemImage), new FrameworkPropertyMetadata(16d));
            MaxHeightProperty.OverrideMetadata(typeof(MenuItemImage), new FrameworkPropertyMetadata(16d));
        }

    }
}
