using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace LiorTech.PowerTools.Utilities
{
    /// <summary>
    /// Use this in the Expander.HeaderTemplate to force the Header of the expander to stretch!
    /// </summary>
    /// <code>
    /// <Expander HeaderTemplate="{Utilities:StretchedHeaderTemplate}" Header="Stretched text">
    /// </Expander>
    /// </code>
    public class StretchedHeaderTemplate : MarkupExtension
    {
        class DataTemplateImpl : DataTemplate
        {
            static DataTemplateImpl()
            {
                //set up the grid
                FrameworkElementFactory gridFactory = new FrameworkElementFactory(typeof(Grid));
                gridFactory.SetValue(Grid.HorizontalAlignmentProperty, HorizontalAlignment.Stretch);
                gridFactory.AddHandler(Grid.LoadedEvent, (RoutedEventHandler)OnExpanderHeaderItemLoaded);

                //set up the content presenter
                FrameworkElementFactory presenterHolder = new FrameworkElementFactory(typeof(ContentPresenter));
                presenterHolder.SetBinding(ContentPresenter.ContentProperty,
                                           new Binding("Content") { RelativeSource = RelativeSource.TemplatedParent });
                gridFactory.AppendChild(presenterHolder);

                GridFactory = gridFactory;
            }

            private static readonly FrameworkElementFactory GridFactory;

            private static void OnExpanderHeaderItemLoaded(object a_sender, RoutedEventArgs a_e)
            {
                var item = a_sender as FrameworkElement;
                if (item == null)
                    return;

                var presenter = item.TemplatedParent as ContentPresenter;
                if (presenter == null)
                    return;

                presenter.HorizontalAlignment = HorizontalAlignment.Stretch;
            }

            public DataTemplateImpl()
            {
                //set the visual tree of the data template
                VisualTree = GridFactory;
            }
        }

        #region Overrides of MarkupExtension

        public override object ProvideValue( IServiceProvider a_serviceProvider )
        {
            return new DataTemplateImpl();
        }

        #endregion
    }
}
