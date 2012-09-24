using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace LiorTech.PowerTools.Utilities
{
    /// <summary>
    /// Helper methods for UI-related tasks.
    /// </summary>
    public static class UIElementFinder
    {
        #region find parent

        /// <summary>
        /// Return the first instance of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Type to return</typeparam>
        /// <param name="a_o">Object to check</param>
        /// <returns>The type or null if non was found</returns>
        /// <remarks>If the object itself is <typeparamref name="T"/> then return that, otherwise travel the visual tree</remarks>
        public static T AsObjectOrParent<T>(this DependencyObject a_o)
            where T : class
        {
            if ( a_o is T )
                return a_o as T;

            return a_o.TryFindParentInterface<T>();
        }


        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the
        /// queried item.</param>
        /// <returns>The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T TryFindParentInterface<T>(this DependencyObject child)
            where T : class
        {
            //get parent item
            DependencyObject parentObject = GetParentObject(child);

            //we've reached the end of the tree
            if (parentObject == null) return null;

            //check if the parent matches the type we're looking for
            T parent = parentObject as T;
            if (parent != null)
            {
                return parent;
            }
            else
            {
                //use recursion to proceed with next level
                return TryFindParentInterface<T>(parentObject);
            }
        }

        /// <summary>
        /// Finds a parent of a given item on the visual tree.
        /// </summary>
        /// <typeparam name="T">The type of the queried item.</typeparam>
        /// <param name="child">A direct or indirect child of the
        /// queried item.</param>
        /// <returns>The first parent item that matches the submitted
        /// type parameter. If not matching item can be found, a null
        /// reference is being returned.</returns>
        public static T TryFindParent<T>(this DependencyObject child)
            where T : DependencyObject
        {
            return TryFindParentInterface<T>(child);
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetParent"/> method, which also
        /// supports content elements. Keep in mind that for content element,
        /// this method falls back to the logical tree of the element!
        /// </summary>
        /// <param name="child">The item to be processed.</param>
        /// <returns>The submitted item's parent, if available. Otherwise
        /// null.</returns>
        public static DependencyObject GetParentObject(this DependencyObject child)
        {
            if (child == null) return null;

            //handle content elements separately
            ContentElement contentElement = child as ContentElement;
            if (contentElement != null)
            {
                DependencyObject parent = ContentOperations.GetParent(contentElement);
                if (parent != null) return parent;

                FrameworkContentElement fce = contentElement as FrameworkContentElement;
                return fce != null ? fce.Parent : null;
            }

            //also try searching for parent in framework elements (such as DockPanel, etc)
            FrameworkElement frameworkElement = child as FrameworkElement;
            if (frameworkElement != null)
            {
                DependencyObject parent = frameworkElement.Parent;
                if (parent != null) return parent;
            }

            //if it's not a ContentElement/FrameworkElement, rely on VisualTreeHelper
            return VisualTreeHelper.GetParent(child);
        }

        #endregion

        #region find children

        /// <summary>
        /// Find child element by its name.
        /// </summary>
        /// <typeparam name="T">Type to find</typeparam>
        /// <param name="a_source">Parent object</param>
        /// <param name="a_name">The name to look for</param>
        /// <param name="a_inherited">Are we looking for an inherited type of <typeparamref name="T"/> as well?</param>
        /// <returns>The child or null if non was found</returns>
        public static T FindChildByName<T>(this FrameworkElement a_source, string a_name, bool a_inherited) 
            where T : FrameworkElement
        {
            return a_source.FindChildrenInterface<T>(a_inherited, true).SingleOrDefault(a_item => a_item.Name == a_name);
        }

        /// <summary>
        /// Analyzes both visual and logical tree in order to find all elements of a given
        /// type that are descendants of the <paramref name="a_source"/> item.
        /// </summary>
        /// <typeparam name="T">The type of the queried items.</typeparam>
        /// <param name="a_source">The root element that marks the source of the search. If the
        /// source is already of the requested type, it will not be included in the result.</param>
        /// <param name="a_inherited">Return inheriting elements also or just the exact match.</param>
        /// <returns>All descendants of <paramref name="a_source"/> that match the requested type.</returns>
        public static IEnumerable<T> FindChildren<T>(this DependencyObject a_source, bool a_inherited) 
            where T : DependencyObject
        {
            return FindChildrenInterface<T>(a_source, a_inherited, false);
        }

        /// <summary>
        /// Analyzes both visual and logical tree in order to find all elements of a given
        /// type that are descendants of the <paramref name="a_source"/> item.
        /// </summary>
        /// <typeparam name="T">The type of the queried items.</typeparam>
        /// <param name="a_source">The root element that marks the source of the search. If the
        /// source is already of the requested type, it will not be included in the result.</param>
        /// <param name="a_inherited">Return inheriting elements also or just the exact match.</param>
        /// <param name="a_preferVisualTree">Should we use the visual tree only or the logical tree as well</param>
        /// <returns>All descendants of <paramref name="a_source"/> that match the requested type.</returns>
        public static IEnumerable<T> FindChildrenInterface<T>(this DependencyObject a_source, bool a_inherited, bool a_preferVisualTree) 
            where T : class
        {
            if (a_source != null)
            {
                var childs = GetChildObjects(a_source, a_preferVisualTree);
                foreach (DependencyObject child in childs)
                {
                    // Analyze if children match the requested type
                    if (child != null)
                    {
                        if (child is T) 
                            yield return child as T;
                    }

                    // Recurse tree
                    if ( a_inherited )
                    {
                        foreach (T descendant in FindChildrenInterface<T>(child, true, a_preferVisualTree))
                            yield return descendant;
                    }
                }
            }
        }

        /// <summary>
        /// This method is an alternative to WPF's
        /// <see cref="VisualTreeHelper.GetChild"/> method, which also
        /// supports content elements. Keep in mind that for content elements,
        /// this method falls back to the logical tree of the element.
        /// </summary>
        /// <param name="a_parent">The item to be processed.</param>
        /// <param name="a_preferVisualTree">Should we prefer the visual tree only or should we consider the logical tree as well</param>
        /// <returns>The submitted item's child elements, if available.</returns>
        public static IEnumerable<DependencyObject> GetChildObjects(this DependencyObject a_parent, bool a_preferVisualTree)
        {
            if (a_parent == null) yield break;

            if (!a_preferVisualTree && (a_parent is ContentElement || a_parent is FrameworkElement))
            {
                //use the logical tree for content / framework elements
                foreach (object obj in LogicalTreeHelper.GetChildren(a_parent))
                {
                    var depObj = obj as DependencyObject;
                    if (depObj != null) yield return (DependencyObject)obj;
                }
            }
            else
            {
                //use the visual tree per default
                int count = VisualTreeHelper.GetChildrenCount(a_parent);
                for (int i = 0; i < count; i++)
                {
                    yield return VisualTreeHelper.GetChild(a_parent, i);
                }
            }
        }

        #endregion

        #region find from point

        /// <summary>
        /// Tries to locate a given item within the visual tree,
        /// starting with the dependency object at a given position. 
        /// </summary>
        /// <typeparam name="T">The type of the element to be found
        /// on the visual tree of the element at the given location.</typeparam>
        /// <param name="reference">The main element which is used to perform
        /// hit testing.</param>
        /// <param name="point">The position to be evaluated on the origin.</param>
        public static T TryFindFromPoint<T>(UIElement reference, Point point)
            where T : DependencyObject
        {
            DependencyObject element = reference.InputHitTest(point) as DependencyObject;

            if (element == null) return null;
            else if (element is T) return (T)element;
            else return TryFindParent<T>(element);
        }

        #endregion
    }
}
