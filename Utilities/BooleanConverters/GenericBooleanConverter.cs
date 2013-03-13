using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiorTech.PowerTools.Utilities.BooleanConverters
{
    /// <summary>
    /// Convert a boolean value into the corresponsing <see cref="TrueValue"/> or <see cref="FalseValue"/>
    /// </summary>
    public class GenericBooleanConverter : ConverterMarkupExtension
    {
        /// <summary>
        /// Construct the <see cref="GenericEqualityConverter"/>
        /// </summary>
        public GenericBooleanConverter()
        {
            TrueValue = true;
            FalseValue = false;
        }

        /// <summary>
        /// Value to provide incase of a true result.
        /// </summary>
        public object TrueValue { get; set; }

        /// <summary>
        /// Value to provide incase of a false result.
        /// </summary>
        public object FalseValue { get; set; }

        #region Implementation of IValueConverter

        public override object Convert(object a_value, Type a_targetType, object a_parameter, CultureInfo a_culture)
        {
            bool boolResult = false;

            if ( a_value is bool )
                boolResult = (bool)a_value;

            object result = boolResult ? TrueValue : FalseValue;

            if (!a_targetType.IsAssignableFrom(result.GetType()))
                return TypeDescriptor.GetConverter(a_targetType).ConvertFrom(result);

            return result;
        }

        #endregion
     }
}
