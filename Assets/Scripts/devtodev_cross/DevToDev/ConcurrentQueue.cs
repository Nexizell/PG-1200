using System.Collections.Generic;

namespace DevToDev
{
	internal class ConcurrentQueue<T>
	{
		private readonly object mutex = new object();

		private List<T> queue;

		public int Count
		{
			get
			{
				lock (mutex)
				{
					return queue.Count;
				}
			}
		}

		public ConcurrentQueue()
		{
			queue = new List<T>();
		}

		public void Enqueue(T obj)
		{
			lock (mutex)
			{
				queue.Add(obj);
			}
		}

		public T Dequeue()
		{
			lock (mutex)
			{
				if (queue.Count > 0)
				{
					T val = queue[0];
					queue.Remove(val);
					return val;
				}
				return default(T);
			}
		}

		public void Clear()
		{
			lock (mutex)
			{
				queue.Clear();
			}
		}
	}
}
