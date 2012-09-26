using System;
using System.Windows;

namespace LiorTech.PowerTools.ViewModel
{
    /// <summary>
    /// Convience attribute to be used on view models - it specified the default view type to use when calling <see cref="DialogService.ShowDialog"/>
    /// </summary>
    public class DialogViewTypeAttribute : Attribute
    {
        ///<summary>
        /// Construct a new instance.
        ///</summary>
        ///<param name="a_dialogViewType">The type of the dialog view - it must be of a <see cref="Window"/> and <see cref="IDialogView"/> type</param>
        public DialogViewTypeAttribute(Type a_dialogViewType)
        {
            DialogViewType = a_dialogViewType;
        }

        private Type m_dialogViewType;

        /// <summary>
        /// The type of the dialog view - it must be of a <see cref="Window"/> and <see cref="IDialogView"/> type
        /// </summary>
        public Type DialogViewType
        {
            get { return m_dialogViewType; }
            set
            {
                if (!typeof(Window).IsAssignableFrom(value) || !typeof(IDialogView).IsAssignableFrom(value))
                    throw new ArgumentException("Type must derive from Window and IDialogView");

                m_dialogViewType = value;
            }
        }
    }
}
