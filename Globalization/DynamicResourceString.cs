using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LiorTech.PowerTools.Globalization
{
    /// <summary>
    /// Implements a resource string wrapper that evaluates dynamically to the proper localized value (useful for UI).
    /// </summary>
    public abstract class DynamicResourceString : INotifyPropertyChanged, IWeakEventListener
    {
        protected DynamicResourceString(string a_name)
        {
            m_name = a_name;
        }

        protected DynamicResourceString(string a_name, object[] a_args) :
            this(a_name)
        {
            m_args = a_args;
        }

        /// Returns the resources manager instance.
        protected abstract ResourceManager ResourceManager { get; }

        private readonly object[] m_args = new object[0];
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
            var str = ResourceManager.GetString(m_name) ?? string.Empty;

            Value = m_args.Length == 0 ? str : string.Format(str, m_args);
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

    /// <summary>
    /// Convinence class for <see cref="DynamicResourceString"/> implementation.
    /// </summary>
    public static class ResourceString
    {
        /// <summary>
        /// Initialize with a resource property signature.
        /// </summary>
        /// <typeparam name="TObj">The parent resource object</typeparam>
        /// <param name="a_property">A lambda expression that evaluates to the property</param>
        /// <param name="a_args">List of custom arguments to pass to the resource</param>
        /// <returns>A dynamic resource</returns>
        public static DynamicResourceString FromResource<TObj>(Expression<Func<TObj, string>> a_property, params object[] a_args)
        {
            string propName = PropertyName(a_property);

            return new ResourceWrapper<TObj>(propName, a_args);
        }

        private static string PropertyName<TObj, TRet>(Expression<Func<TObj, TRet>> a_property)
        {
            var propExpr = a_property.Body as MemberExpression;
            if (propExpr == null)
                throw new ArgumentException("Property expression must be of the form 'x => x.SomeProperty'");

            return propExpr.Member.Name;
        }

        sealed class ResourceWrapper<TParent> : DynamicResourceString
        {
            static ResourceWrapper()
            {
                var propInfo = typeof(TParent).GetProperty("ResourceManager", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static);
                Manager = (ResourceManager)propInfo.GetValue(null, null);
            }

            public ResourceWrapper(string a_name, object[] a_args)
                : base(a_name, a_args)
            {
            }

            // ReSharper disable StaticFieldInGenericType
            private static readonly ResourceManager Manager;
            // ReSharper restore StaticFieldInGenericType

            #region Overrides of DynamicResourceString

            protected override ResourceManager ResourceManager
            {
                get { return Manager; }
            }

            #endregion
        }
    }
}
