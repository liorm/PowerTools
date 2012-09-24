using System;
using System.Globalization;

namespace LiorTech.PowerTools.Utilities.BooleanConverters
{
    /// <summary>
    /// Converts the value to true if it's null, false if it's not null.
    /// </summary>
    public class NullToBooleanConverter : ConverterMarkupExtension
    {
        #region Implementation of IValueConverter

        /// <summary>
        /// Convert to <see cref="bool"/> if <paramref name="value"/> is null
        /// </summary>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value == null;
        }

        #endregion
    }
}