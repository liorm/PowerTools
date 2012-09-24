using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Input;

namespace LiorTech.PowerTools.Commanding
{
    /// <summary>
    /// Provide a base class for the command descripion.
    /// </summary>
    /// <remarks>The actual class is <see cref="CommandDescription"/>, but applications can override this class to provide application specific
    /// logic for obtaining the command description (e.g from a resource file)</remarks>
	public abstract class CommandDescriptionBase : INotifyPropertyChanged
	{
        protected CommandDescriptionBase(string a_name)
            : this(a_name, string.Empty, string.Empty)
        { 
        }

        protected CommandDescriptionBase(string a_name, string a_gestures, string a_gesturesText)
        {
            if (a_name == null)
                throw new ArgumentNullException("a_name");

            Name = a_name;
            SetupGestures(a_gestures, a_gesturesText);
        }

        #region Implementation of INotifyPropertyChanged

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Call this to notify of a changed property.
        /// </summary>
        /// <param name="a_propertyName">Name of the changed property</param>
        protected virtual void OnPropertyChanged( string a_propertyName )
        {
#if DEBUG
            VerifyPropertyName( a_propertyName );
#endif
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if ( handler != null )
            {
                var e = new PropertyChangedEventArgs( a_propertyName );
                handler( this, e );
            }
        }

        [Conditional( "DEBUG" )]
        [DebuggerStepThrough]
        public void VerifyPropertyName( string a_propertyName )
        {
            // Verify that the property name matches a real,  
            // public, instance property on this object.
            if ( TypeDescriptor.GetProperties( this )[a_propertyName] == null )
            {
                string msg = "Invalid property name: " + a_propertyName;

                throw new Exception( msg );
            }
        }

        #endregion

        public string Name { get; private set; }

        #region Text property

        /// <summary>
        /// A short text describing the command.
        /// </summary>
        public string Text
        {
            get { return m_text; }
            protected set
            {
                if ( m_text == value )
                    return;

                m_text = value;
                OnPropertyChanged( "Text" );
            }
        }

        private string m_text;

        #endregion

        #region ToolTip property

        /// <summary>
        /// Tool tip to display for the command.
        /// </summary>
        public string ToolTip
        {
            get { return m_toolTip; }
            protected set
            {
                if ( m_toolTip == value )
                    return;

                m_toolTip = value;
                OnPropertyChanged( "ToolTip" );
            }
        }

        private string m_toolTip;

        #endregion

        /// <summary>
        /// The command binders append this text to command name when looking for the image of the command.
        /// </summary>
        public string CommandImagePostfix { get; protected set; }

        /// <summary>
        /// Input gestures associated with the command.
        /// </summary>
        public InputGestureCollection Gestures { get; private set; }

        protected void SetupGestures(string a_keyGestures, string a_displayStrings)
        {
            if (string.IsNullOrEmpty(a_displayStrings))
            {
                a_displayStrings = a_keyGestures;
            }

            while (!string.IsNullOrEmpty(a_keyGestures))
            {
                string currentDisplay;
                string currentGesture;
                int index = a_keyGestures.IndexOf(";", StringComparison.Ordinal);
                if (index >= 0)
                {
                    currentGesture = a_keyGestures.Substring(0, index);
                    a_keyGestures = a_keyGestures.Substring(index + 1);
                }
                else
                {
                    currentGesture = a_keyGestures;
                    a_keyGestures = string.Empty;
                }

                index = a_displayStrings.IndexOf(";", StringComparison.Ordinal);
                if (index >= 0)
                {
                    currentDisplay = a_displayStrings.Substring(0, index);
                    a_displayStrings = a_displayStrings.Substring(index + 1);
                }
                else
                {
                    currentDisplay = a_displayStrings;
                    a_displayStrings = string.Empty;
                }

                KeyGesture inputGesture = CreateFromResourceStrings(currentGesture, currentDisplay);
                if (inputGesture != null)
                {
                    if (Gestures == null)
                        Gestures = new InputGestureCollection();

                    Gestures.Add(inputGesture);
                }
            }
        }

        private static KeyGesture CreateFromResourceStrings(string keyGestureToken, string keyDisplayString)
        {
            if (!string.IsNullOrEmpty(keyDisplayString))
            {
                keyGestureToken = keyGestureToken + ',' + keyDisplayString;
            }
            return (KeyGestureConverter.ConvertFromInvariantString(keyGestureToken) as KeyGesture);
        }

        private static readonly KeyGestureConverter KeyGestureConverter = new KeyGestureConverter();
    }
}
