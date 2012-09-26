using System;
using System.Windows;
using System.Windows.Data;

namespace LiorTech.PowerTools.Globalization
{
    public class FlowDirectionBindingExtension : Binding
    {
        ///<summary>
        /// Contruct a new binding.
        ///</summary>
        public FlowDirectionBindingExtension()
        {
            Initialize();
        }

        private void Initialize()
        {
            Source = DepObject.Instance;
            Path = new PropertyPath("FlowDirection");
        }

        class DepObject : DependencyObject
        {
            /// <summary>
            /// Dependency objects have thread locality.
            /// </summary>
            [ThreadStatic]
            private static DepObject m_instance;
            public static DepObject Instance
            {
                get
                {
                    if (m_instance == null)
                        m_instance = new DepObject();
                    return m_instance;
                }
            }

            private DepObject()
            {
                // Invoke in the owning dispatcher thread.
                CultureManager.ApplicationUICultureChanged +=
                    a_newCulture => Dispatcher.BeginInvoke(new Action(UpdateFlowDirection));
                UpdateFlowDirection();
            }

            private void UpdateFlowDirection()
            {
                FlowDirection = 
                    CultureManager.ApplicationUICulture.TextInfo.IsRightToLeft ? 
                    FlowDirection.RightToLeft : 
                    FlowDirection.LeftToRight;
            }

            #region FlowDirection dependency property

            private static readonly DependencyPropertyKey FlowDirectionPropertyKey = DependencyProperty.RegisterReadOnly(
                "FlowDirection",
                typeof(FlowDirection),
                typeof(DepObject),
                new FrameworkPropertyMetadata());

            /// <summary>
            /// Dependency property for <see cref="FlowDirection"/>
            /// </summary>
            public static readonly DependencyProperty FlowDirectionProperty = FlowDirectionPropertyKey.DependencyProperty;

            public FlowDirection FlowDirection
            {
                get { return (FlowDirection) GetValue( FlowDirectionProperty ); }
                private set { SetValue( FlowDirectionPropertyKey, value ); }
            }

            #endregion
        }

    }
}
