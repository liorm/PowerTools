using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LiorTech.PowerTools.Globalization
{
    /// <summary>
    /// Implements a resource string wrapper that evaluates dynamically to the proper localized value (useful for UI).
    /// </summary>
    public abstract class DynamicResourceStringBase : INotifyPropertyChanged, IWeakEventListener
    {
        protected DynamicResourceStringBase(string a_name)
        {
            m_name = a_name;
        }

        /// Returns the resources manager instance.
        protected abstract ResourceManager ResourceManager { get; }

        private readonly string m_name;

        /// <summary>
        /// We use this to determine if the class was initialized already (we don't want to subscribe to events automatically).
        /// </summary>
        private bool m_isInitialized;

        private void InitializeValue()
        {
            if (m_isInitialized)
                return;

            ResetValues();
            CultureChangedWeakEventManager.AddListener(this);

            m_isInitialized = true;
        }

        private void ResetValues()
        {
            Value = ResourceManager.GetString(m_name) ?? string.Empty;
        }

        public override string ToString()
        {
            return Value;
        }

        #region Value property

        public string Value
        {
            get
            {
                InitializeValue();
                return m_value;
            }
            private set
            {
                if (m_value == value)
                    return;

                m_value = value;
                OnPropertyChanged("Value");
            }
        }
        private string m_value;

        #endregion

        #region Implementation of INotifyPropertyChanged

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Call this to notify of a changed property.
        /// </summary>
        /// <param name="a_propertyName">Name of the changed property</param>
        protected virtual void OnPropertyChanged(string a_propertyName)
        {
#if DEBUG
            VerifyPropertyName(a_propertyName);
#endif
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
            {
                var e = new PropertyChangedEventArgs(a_propertyName);
                handler(this, e);
            }
        }

        [Conditional("DEBUG")]
        [DebuggerStepThrough]
        public void VerifyPropertyName(string a_propertyName)
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if (TypeDescriptor.GetProperties(this)[a_propertyName] == null)
            {
                string msg = "Invalid property name: " + a_propertyName;

                throw new Exception(msg);
            }
        }

        #endregion

        #region Implementation of IWeakEventListener

        public bool ReceiveWeakEvent(Type a_managerType, object a_sender, EventArgs a_e)
        {
            if (a_managerType != typeof(CultureChangedWeakEventManager))
                return false;

            ResetValues();

            return true;
        }

        #endregion
    }
}
