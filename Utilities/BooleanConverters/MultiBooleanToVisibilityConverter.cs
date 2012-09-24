using System;
using System.Globalization;
using System.Windows;

namespace LiorTech.PowerTools.Utilities.BooleanConverters
{
    /// <summary>
    /// Provides conversion between multiple boolean values and a visiblity
    /// </summary>
    public class MultiBooleanToVisibilityConverter : MultiBooleanConverter
    {
        /// <summary>
        /// Should we use <see cref="Visibility.Collapsed"/> instead of <see cref="Visibility.Hidden"/>?
        /// </summary>
        public bool Collapse { get; set; }

        /// <summary>
        /// Convert the boolean to a <see cref="Visibility"/> value.
        /// </summary>
        protected override object ConvertBoolean(bool a_value, Type targetType, object parameter, CultureInfo culture)
        {
            return a_value ? Visibility.Visible : (Collapse ? Visibility.Collapsed : Visibility.Hidden);
        }
    }
}