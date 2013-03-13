using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace LiorTech.PowerTools.Utilities.BooleanConverters
{
    public class EnumConverterItem
    {
        public EnumConverterItem()
        {
            
        }

        public EnumConverterItem(object a_key, object a_value)
        {
            Key = a_key;
            Value = a_value;
        }

        public object Key { get; set; }
        public object Value { get; set; }
    }

    /// <summary>
    /// Converts a list of enums (or any other object) to a target value.
    /// </summary>
    public class EnumConverter : Collection<EnumConverterItem>, IValueConverter
    {
        #region Implementation of IValueConverter

        public object Convert( object a_value, Type a_targetType, object a_parameter, CultureInfo a_culture )
        {
            EnumConverterItem targetItem = null;

            // Lookup the value.
            foreach ( var item in this )
            {
                if ( Equals(a_value, item.Key) )
                {
                    targetItem = item;
                    break;
                }

                if ( a_value == null || item.Key == null )
                    continue;

                if ( a_value.GetType() == item.Key.GetType() ) 
                    continue;

                // Normalize key.
                object convertedKey = TypeDescriptor.GetConverter(a_value).ConvertFrom(item.Key);
                item.Key = convertedKey;

                // Retest after normalization.
                if (Equals(a_value, item.Key))
                {
                    targetItem = item;
                    break;
                }
            }

            if ( targetItem == null )
                return DependencyProperty.UnsetValue;

            if (targetItem.Value == null)
                return null;

            // Normalize value.
            if (!a_targetType.IsInstanceOfType(targetItem.Value))
                targetItem.Value = TypeDescriptor.GetConverter(a_targetType).ConvertFrom(targetItem.Value);

            return targetItem.Value;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
