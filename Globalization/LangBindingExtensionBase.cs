using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace LiorTech.PowerTools.Globalization
{
    /// <summary>
    /// Language specific binding.
    /// </summary>
    /// <typeparam name="T">Resource type to use</typeparam>
    /// <remarks>
    /// Create a assembly specific type with the resource as the type parameter and then use that.
    /// </remarks>
    public abstract class LangBindingExtensionBase<T> : Binding
    {
        ///<summary>
        /// Contruct a new binding.
        ///</summary>
        protected LangBindingExtensionBase()
        {
            Initialize();
        }

        private void Initialize()
        {
            Source = DynamicResource.Instance;
        }

        /// <summary>
        /// Reflect the resource type back as a dependency object for binding to work.
        /// </summary>
        class DynamicResource : DependencyObject
        {
            static DynamicResource()
            {
                DependencyPropertiesMap = new Dictionary<string, DependencyProperty>();

                var properties = typeof(T).GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                foreach (var property in properties)
                {
                    var depProp = DependencyProperty.Register(
                        property.Name,
                        property.PropertyType,
                        typeof(DynamicResource),
                        new FrameworkPropertyMetadata(property.GetValue(null, null)));

                    DependencyPropertiesMap.Add(property.Name, depProp);
                }
            }

            private static readonly Dictionary<string, DependencyProperty> DependencyPropertiesMap;

            /// <summary>
            /// Dependency objects have thread locality.
            /// </summary>
            [ThreadStatic]
            private static DynamicResource m_instance;
            public static DynamicResource Instance
            {
                get
                {
                    if (m_instance == null)
                        m_instance = new DynamicResource();
                    return m_instance;
                }
            }

            private DynamicResource()
            {
                // Invoke in the owning dispatcher thread.
                CultureManager.ApplicationUICultureChanged +=
                    a_newCulture => Dispatcher.BeginInvoke(new Action(ResetValues));
            }

            private void ResetValues()
            {
                var properties = typeof(T).GetProperties(BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);

                foreach (var property in properties)
                    SetValue(DependencyPropertiesMap[property.Name], property.GetValue(null, null));
            }
        }
    }
}
