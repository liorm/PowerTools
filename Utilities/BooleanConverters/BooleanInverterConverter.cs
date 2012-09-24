using System;
using System.Globalization;

namespace LiorTech.PowerTools.Utilities.BooleanConverters
{
    /// <summary>
    /// Logical invert the boolean value.
    /// </summary>
    public class BooleanInverterConverter : ConverterMarkupExtension
    {
        #region Implementation of IValueConverter

        /// <summary>
        /// Invert the bool value
        /// </summary>
        public override object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if (!(value is bool))
                return false;

            return !(bool)value;
        }

        /// <summary>
        /// Invert the bool value
        /// </summary>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is bool))
                return false;

            return !(bool)value;
        }

        #endregion
    }
}