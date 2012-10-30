using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LiorTech.PowerTools.Utilities
{
    /// <summary>
    /// Wrapper around IComparer for a function (basically .Net Framework 4.5 already implements this with Comparer.Create function).
    /// </summary>
    /// <typeparam name="T">Compare type</typeparam>
    public class FunctionComparer<T> : IComparer<T>
    {
        public FunctionComparer(Func<T, T, int> a_func)
        {
            m_func = a_func;
        }

        public static FunctionComparer<T> Create(Func<T, T, int> a_func)
        {
            return new FunctionComparer<T>(a_func);
        }

        readonly Func<T, T, int> m_func;

        #region Implementation of IComparer<in T>

        public int Compare( T a_x, T a_y )
        {
            return m_func(a_x, a_y);
        }

        #endregion
    }
}
