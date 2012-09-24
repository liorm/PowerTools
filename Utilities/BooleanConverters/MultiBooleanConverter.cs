using System;
using System.Globalization;

namespace LiorTech.PowerTools.Utilities.BooleanConverters
{
    /// <summary>
    /// Provides conversion between multiple boolean values to a single value.
    /// </summary>
    public class MultiBooleanConverter : ConverterMarkupExtension
    {
        /// <summary>
        /// Use logical OR or logical AND.
        /// </summary>
        public bool UseLogicalOr { get; set; }

        /// <summary>
        /// Should we invert the conversion?
        /// </summary>
        public bool Invert { get; set; }

        #region Implementation of IMultiValueConverter

        /// <summary>
        /// Perform the boolean logic according to the parameters and call <see cref="ConvertBoolean"/> to obtain
        /// the result for the boolean value.
        /// </summary>
        public override object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = !UseLogicalOr;

            foreach (object obj in values)
            {
                bool value = false;
                if (obj is bool)
                    value = (bool)obj;

                if (UseLogicalOr)
                    result |= value;
                else
                    result &= value;
            }

            if (Invert) result = !result;

            return ConvertBoolean(result, targetType, parameter, culture);
        }

        #endregion

        /// <summary>
        /// Convert the specified resulting boolean value into an object.
        /// </summary>
        /// <param name="a_value">Boolean value to convert</param>
        /// <param name="targetType">The target type taken from <see cref="Convert"/></param>
        /// <param name="parameter">The converter parameter taken from <see cref="Convert"/></param>
        /// <param name="culture">The conversion culture taken from <see cref="Convert"/></param>
        /// <returns>The resulting object for the conversion</returns>
        protected virtual object ConvertBoolean(bool a_value, Type targetType, object parameter, CultureInfo culture)
        {
            // Default behaviour is to simply return the value.
            return a_value;
        }
    }
}