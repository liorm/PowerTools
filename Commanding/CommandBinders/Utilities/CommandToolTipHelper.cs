using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Controls;

namespace LiorTech.PowerTools.Commanding.CommandBinders.Utilities
{
    /// <summary>
    /// Provide helper methods to retrieve a tooltip for a command.
    /// </summary>
	public static class CommandToolTipHelper
	{
        private const string TOOL_TIP_KEY_GESTURE_FORMAT = "{0} ({1})";
        private static readonly CommandToolTipStyle m_toolTipStyle = CommandToolTipStyle.EnabledWithKeyGesture;

        /// <summary>
        /// Set the command tooltip on the specified element.
        /// </summary>
        public static void ApplyCommandToolTip(FrameworkElement a_element, ICommand a_command )
        {
            if ( a_command == null )
            {
                a_element.ToolTip = null;
                return;
            }

            a_element.ToolTip = FormatToolTip(a_command, GetCommandToolTip(a_command));
            SetShowOnDisabled(a_element);
        }

        private static string GetCommandToolTip(ICommand a_command)
		{
            if ( a_command == null )
                throw new ArgumentNullException("a_command");

            if ( m_toolTipStyle == CommandToolTipStyle.None )
				return null;

			string toolTip = null;

            ICommandDescriptionProvider newDescProvider = a_command as ICommandDescriptionProvider;
            if (newDescProvider == null)
                return null;

            if (!string.IsNullOrEmpty(newDescProvider.Description.ToolTip))
                toolTip = newDescProvider.Description.ToolTip;

			return toolTip;
		}

		private static string FormatToolTip( ICommand a_command, string a_toolTip )
		{
			if ( a_command == null )
			{
				throw new ArgumentNullException( "a_command" );
			}

            if ( m_toolTipStyle == CommandToolTipStyle.None || string.IsNullOrEmpty(a_toolTip) )
			{
				return null;
			}

            if ( m_toolTipStyle != CommandToolTipStyle.AlwaysWithKeyGesture &&
                m_toolTipStyle != CommandToolTipStyle.EnabledWithKeyGesture )
			{
				return a_toolTip;
			}

            ICommandDescriptionProvider newDescProvider = a_command as ICommandDescriptionProvider;
            if (newDescProvider == null)
                return null;

            if (newDescProvider.Description.Gestures.Count > 0)
			{
                KeyGesture keyGesture = newDescProvider.Description.Gestures[0] as KeyGesture;
				if ( keyGesture != null && keyGesture.DisplayString.Length > 0 )
				{
					a_toolTip = string.Format(
						TOOL_TIP_KEY_GESTURE_FORMAT,
						a_toolTip,
						keyGesture.DisplayString );
				}
			}

			return a_toolTip;
		}

		private static void SetShowOnDisabled( DependencyObject a_dependencyObject)
		{
            if ( m_toolTipStyle == CommandToolTipStyle.Always || m_toolTipStyle == CommandToolTipStyle.AlwaysWithKeyGesture )
			{
				ToolTipService.SetShowOnDisabled( a_dependencyObject, true );
			}
		}
	}

}
