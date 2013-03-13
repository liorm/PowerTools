using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace LiorTech.PowerTools
{
    /// <summary>
    /// Provide a storage for a variable. It fires observable events when changed.
    /// </summary>
    /// <typeparam name="T">The contained type</typeparam>
    public class ObservableVariable<T> : IObservable<T>, IDisposable
    {
        /// <summary>
        /// Construct the object using an initial value
        /// </summary>
        /// <param name="a_initialValue">Initial value for the variable</param>
        public ObservableVariable(T a_initialValue = default(T))
        {
            m_value = a_initialValue;
            m_subject = null;
        }

        public static implicit operator T(ObservableVariable<T> a_variable)
        {
            return a_variable.m_value;
        }

        /// <summary>
        /// The current value.
        /// </summary>
        public T Value
        {
            get { return m_value; }
            set
            {
                if ( Object.Equals(m_value, value) )
                    return;

                // Set value.
                m_value = value;

                // Fire event!
                if ( m_subject != null )
                    m_subject.OnNext(m_value);
            }
        }
        private T m_value;

        #region Implementation of IObservable<out T>

        private ReplaySubject<T> m_subject;

        public IDisposable Subscribe( IObserver<T> a_observer )
        {
            if ( m_subject == null )
                m_subject = new ReplaySubject<T>(1);

            return m_subject.Subscribe(a_observer);
        }

        #endregion

        #region Implementation of IDisposable

        public void Dispose()
        {
            if ( m_subject != null )
                m_subject.Dispose();
        }

        #endregion
    }
}
