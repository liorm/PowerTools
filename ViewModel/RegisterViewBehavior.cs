using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Interactivity;

namespace LiorTech.PowerTools.ViewModel
{
    /// <summary>
    /// Set <see cref="DialogService.IsViewRegisteredProperty"/> automatically - it's useful for DataTemplates that instanciate dynamically.
    /// </summary>
    public class RegisterViewBehavior : Behavior<FrameworkElement>
    {
        protected override void OnAttached()
        {
            base.OnAttached();

            DialogService.SetIsViewRegistered(AssociatedObject, true);
        }

        protected override void OnDetaching()
        {
            DialogService.SetIsViewRegistered(AssociatedObject, false);

            base.OnDetaching();
        }
    }
}
