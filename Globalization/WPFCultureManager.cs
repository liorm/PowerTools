using System;
using System.ComponentModel;
using System.Windows.Markup;

namespace LiorTech.PowerTools.Globalization
{
    /// <summary>
    /// Provides system wide WPF related culture functions.
    /// </summary>
    /// <remarks>This is a WPF complement for <see cref="CultureManager"/></remarks>
    public sealed class WPFCultureManager : INotifyPropertyChanged
    {
        static WPFCultureManager()
        {
            Instance = new WPFCultureManager();
        }

        private WPFCultureManager()
        {
            m_language = XmlLanguage.GetLanguage(CultureManager.ApplicationUICulture.IetfLanguageTag);
            CultureManager.ApplicationUICultureChanged += CultureManager_ApplicationUICultureChanged;
        }

        void CultureManager_ApplicationUICultureChanged(System.Globalization.CultureInfo a_newCulture)
        {
            // Update the langauage.
            Language = XmlLanguage.GetLanguage(a_newCulture.IetfLanguageTag);
        }

        #region Implementation of INotifyPropertyChanged

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged( string a_propertyName )
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if ( handler != null )
            {
                var e = new PropertyChangedEventArgs( a_propertyName );
                handler( this, e );
            }
        }

        #endregion

        #region Language property

        /// <summary>
        /// Return the current xml language 
        /// </summary>
        public XmlLanguage Language
        {
            get { return m_language; }
            private set
            {
                if ( m_language == value )
                    return;

                m_language = value;
                OnPropertyChanged( "Language" );
            }
        }

        private XmlLanguage m_language;

        #endregion

        ///<summary>
        /// Returns the singleton instance of <see cref="WPFCultureManager"/>
        ///</summary>
        public static WPFCultureManager Instance { get; private set; }
    }
}
