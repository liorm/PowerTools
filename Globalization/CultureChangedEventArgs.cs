using System;
using System.Globalization;

namespace LiorTech.PowerTools.Globalization
{
	public class CultureChangedEventArgs : EventArgs
	{
		public CultureInfo NewCulture { get; internal set; }
	}
}