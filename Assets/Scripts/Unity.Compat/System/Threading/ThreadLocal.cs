using System.Collections.Generic;

namespace System.Threading
{
	public class ThreadLocal<T> : IDisposable
	{
		private static long lastId = -1L;

		[ThreadStatic]
		private static IDictionary<long, T> threadLocalData;

		private static IList<WeakReference> allDataDictionaries = new List<WeakReference>();

		private bool disposed;

		private readonly long id;

		private readonly Func<T> valueFactory;

		private static IDictionary<long, T> ThreadLocalData
		{
			get
			{
				if (threadLocalData == null)
				{
					threadLocalData = new Dictionary<long, T>();
					lock (allDataDictionaries)
					{
						allDataDictionaries.Add(new WeakReference(threadLocalData));
					}
				}
				return threadLocalData;
			}
		}

		public T Value
		{
			get
			{
				CheckDisposed();
				T value;
				if (ThreadLocalData.TryGetValue(id, out value))
				{
					return value;
				}
				return ThreadLocalData[id] = valueFactory();
			}
			set
			{
				CheckDisposed();
				ThreadLocalData[id] = value;
			}
		}

		public ThreadLocal()
			: this((Func<T>)(() => default(T)))
		{
		}

		public ThreadLocal(Func<T> valueFactory)
		{
			this.valueFactory = valueFactory;
			id = Interlocked.Increment(ref lastId);
		}

		~ThreadLocal()
		{
			if (!disposed)
			{
				Dispose();
			}
		}

		private void CheckDisposed()
		{
			if (disposed)
			{
				throw new ObjectDisposedException("ThreadLocal has been disposed.");
			}
		}

		public void Dispose()
		{
			lock (allDataDictionaries)
			{
				for (int i = 0; i < allDataDictionaries.Count; i++)
				{
					IDictionary<object, T> dictionary = allDataDictionaries[i].Target as IDictionary<object, T>;
					if (dictionary == null)
					{
						allDataDictionaries.RemoveAt(i);
						i--;
					}
					else
					{
						dictionary.Remove(id);
					}
				}
			}
			disposed = true;
		}
	}
}
