using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Windows;
using LiorTech.PowerTools.Commanding;

namespace LiorTech.PowerTools.Globalization
{
    /// <summary>
    /// Base class for a resource base <see cref="CommandDescriptionBase"/> instance.
    /// </summary>
    public abstract class ResourceCommandDescriptionBase : CommandDescriptionBase, IWeakEventListener
    {
        protected ResourceCommandDescriptionBase(string a_name, string a_commandImagePostfix = null) :
            base(a_name)
        {
            CommandImagePostfix = a_commandImagePostfix;

            ResetValues();

            CultureChangedWeakEventManager.AddListener(this);
        }

        /// Returns the resources manager instance.
        protected abstract ResourceManager ResourceManager { get; }

        private void ResetValues()
        {
            ToolTip = ResourceManager.GetString(Name + "_tooltip") ?? string.Empty;
            Text = ResourceManager.GetString(Name + "_text") ?? string.Empty;
        }

        #region Implementation of IWeakEventListener

        public bool ReceiveWeakEvent(Type a_managerType, object sender, EventArgs e)
        {
            if (a_managerType != typeof(CultureChangedWeakEventManager))
                return false;

            ResetValues();

            return true;
        }

        #endregion
    }
}
