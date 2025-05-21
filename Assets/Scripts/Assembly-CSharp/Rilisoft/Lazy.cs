using System;

namespace Rilisoft
{
	internal class Lazy<T>
	{
		internal class Boxed
		{
			internal T m_value;

			internal Boxed(T value)
			{
				m_value = value;
			}
		}

		private static Func<T> ALREADY_INVOKED_SENTINEL = () => default(T);

		private object m_boxed;

		private Func<T> m_valueFactory;

		private volatile object m_threadSafeObj;

		public T Value
		{
			get
			{
				Boxed boxed = null;
				if (m_boxed != null)
				{
					boxed = m_boxed as Boxed;
					if (boxed != null)
					{
						return boxed.m_value;
					}
					throw m_boxed as Exception;
				}
				return LazyInitValue();
			}
		}

		public Lazy(Func<T> valueFactory)
		{
			m_threadSafeObj = new object();
			m_valueFactory = valueFactory;
		}

		private T LazyInitValue()
		{
			Boxed boxed = null;
			object threadSafeObj = m_threadSafeObj;
			Func<T> aLREADY_INVOKED_SENTINEL = ALREADY_INVOKED_SENTINEL;
			if (m_boxed == null)
			{
				boxed = (Boxed)(m_boxed = CreateValue());
				m_threadSafeObj = ALREADY_INVOKED_SENTINEL;
			}
			else
			{
				boxed = m_boxed as Boxed;
				if (boxed == null)
				{
					throw m_boxed as Exception;
				}
			}
			return boxed.m_value;
		}

		private Boxed CreateValue()
		{
			Boxed boxed = null;
			try
			{
				if (m_valueFactory == ALREADY_INVOKED_SENTINEL)
				{
					throw new InvalidOperationException();
				}
				Func<T> valueFactory = m_valueFactory;
				m_valueFactory = ALREADY_INVOKED_SENTINEL;
				return new Boxed(valueFactory());
			}
			catch (Exception boxed2)
			{
				m_boxed = boxed2;
				throw;
			}
		}
	}
}
