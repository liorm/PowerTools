using System;
using System.Globalization;
using System.Windows;

namespace LiorTech.PowerTools.Utilities.BooleanConverters
{
    /// <summary>
    /// Convert a bool value to <see cref="Visibility"/>
    /// </summary>
    public class BooleanToVisibilityConverterEx : ConverterMarkupExtension
    {
        /// <summary>
        /// Should we invert the conversion?
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// Should we use <see cref="Visibility.Collapsed"/> instead of <see cref="Visibility.Hidden"/>?
        /// </summary>
        public bool Collapse { get; set; }

        #region Implementation of IValueConverter

        /// <summary>
        /// Convert the <see cref="bool"/> value to <see cref="Visibility"/>
        /// </summary>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return Collapse ? Visibility.Collapsed : Visibility.Hidden;

            bool val = (bool)value;
            if (Invert) val = !val;

            return val ? Visibility.Visible : (Collapse ? Visibility.Collapsed : Visibility.Hidden);
        }

        /// <summary>
        /// Convert the <see cref="Visibility"/> value back to <see cref="bool"/> value
        /// </summary>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility))
                return false;

            Visibility visibility = (Visibility)value;
            bool val = (visibility == Visibility.Visible);

            return Invert ? !val : val;
        }

        #endregion
    }
}