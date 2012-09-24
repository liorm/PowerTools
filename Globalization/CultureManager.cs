using System;
using System.Globalization;
using System.Threading;

namespace LiorTech.PowerTools.Globalization
{
	/// <summary>
    /// Provides event notifications for culture changes.
    /// </summary>
    public static class CultureManager
    {
        /// <summary>
        /// Represents the method that will handle the <see cref="ApplicationUICultureChanged"/> event   
        /// </summary>
        public delegate void CultureChangedHandler(CultureInfo a_newCulture);

        /// <summary>
        /// Raised when the <see cref="ApplicationUICulture"/> is changed  
        /// </summary>
        public static event CultureChangedHandler ApplicationUICultureChanged;

        /// <summary>
        /// Set/Get the UICulture for whole application. 
        /// </summary>
        /// <remarks>
        /// Setting this property changes the <see cref="Thread.CurrentUICulture"/> 
        /// and sets the <see cref="ApplicationUICulture"/> for all <see cref="CultureManager">CultureManagers</see> 
        /// to the given culture.  
        /// </remarks>
        public static CultureInfo ApplicationUICulture
        {
            get { return Thread.CurrentThread.CurrentUICulture; }
            set
            {
                if (value == null) throw new ArgumentNullException();

                if (!value.Equals(Thread.CurrentThread.CurrentUICulture))
                {
                    Thread.CurrentThread.CurrentUICulture = value;
                    if (ApplicationUICultureChanged != null)
                    {
                        ApplicationUICultureChanged(value);
                    }
                }
            }
        }
    }
}
