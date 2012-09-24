using System;
using System.Globalization;
using System.Resources;
using System.Windows.Data;
using System.Windows.Markup;

namespace LiorTech.PowerTools.Globalization
{
    /// <summary>
    /// Converts the given enum value to a localized resource string.
    /// </summary>
    /// <remarks>
    /// This is a base class - to use it, derive a project specific class and use that for resource conversion</remarks>
    [ValueConversion(typeof(Enum), typeof(string))]
    public abstract class EnumLocalizationConverterBase : MarkupExtension, IValueConverter
    {
        protected EnumLocalizationConverterBase(ResourceManager a_manager)
        {
            ResourcesSource = a_manager;
        }

        public ResourceManager ResourcesSource { get; set; }

        #region Implementation of IValueConverter

        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            Type valueType = value.GetType();

            string resourceName = "Enum_" + valueType.Name + "_" + value.ToString();
            string localizedString = ResourcesSource.GetString(resourceName);

            return string.IsNullOrEmpty(localizedString) ? value.ToString() : localizedString;
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Overrides of MarkupExtension

        public override object ProvideValue( IServiceProvider serviceProvider )
        {
            return this;
        }

        #endregion
    }
}
