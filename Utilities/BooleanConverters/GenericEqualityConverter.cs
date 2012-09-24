using System;
using System.ComponentModel;
using System.Globalization;

namespace LiorTech.PowerTools.Utilities.BooleanConverters
{
    /// <summary>
    /// Check for equality to the value of the converter parameter and provide the <see cref="TrueValue"/> for true of <see cref="FalseValue"/> for false as a result of the conversion.
    /// </summary>
    public class GenericEqualityConverter : ConverterMarkupExtension
    {
        /// <summary>
        /// Construct the <see cref="GenericEqualityConverter"/>
        /// </summary>
        public GenericEqualityConverter()
        {
            TrueValue = true;
            FalseValue = false;
        }

        /// <summary>
        /// Invert the result ( != ).
        /// </summary>
        public bool Invert { get; set; }

        /// <summary>
        /// Value to provide incase of a true result.
        /// </summary>
        public object TrueValue { get; set; }

        /// <summary>
        /// Value to provide incase of a false result.
        /// </summary>
        public object FalseValue { get; set; }

        #region Implementation of IValueConverter

        /// <summary>
        /// Perform the equality and return a bool value.
        /// </summary>
        public override object Convert(object a_value, Type targetType, object parameter, CultureInfo culture)
        {
            bool boolResult;

            if (parameter == null)
                boolResult = (a_value == null);
            else
            {
                if ( a_value == null )
                    boolResult = false;
                else
                {
                    if ( a_value.GetType() != parameter.GetType() )
                    {
                        object convertedParameter = TypeDescriptor.GetConverter(a_value).ConvertFrom(parameter);
                        boolResult = a_value.Equals(convertedParameter);
                    }
                    else
                        boolResult = a_value.Equals(parameter);
                }
            }

            if (Invert)
                boolResult = !boolResult;

            object result = boolResult ? TrueValue : FalseValue;

            if (!targetType.IsAssignableFrom(result.GetType()))
                return TypeDescriptor.GetConverter(targetType).ConvertFrom(result);

            return result;
        }

        #endregion
   }
}