using System;
using System.Collections;
using System.Collections.Generic;

namespace Rilisoft
{
	public class ObservableList<T> : List<T>, IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
	{
		public event Action<int, T> OnItemInserted;

		public event Action<int, T> OnItemRemoved;

		public ObservableList()
		{
		}

		public ObservableList(IEnumerable<T> collection)
		{
			base.AddRange(collection);
		}

		public void Add(T item, bool silent = false)
		{
			base.Add(item);
			if (this.OnItemInserted != null && !silent)
			{
				this.OnItemInserted(base.Count - 1, item);
			}
		}

		public void AddRange(IEnumerable<T> collection, bool silent = false)
		{
			base.AddRange(collection);
			if (this.OnItemInserted == null || silent)
			{
				return;
			}
			int num = 0;
			foreach (T item in collection)
			{
				this.OnItemInserted(num, item);
				num++;
			}
		}

		public void Insert(int index, T item, bool silent = false)
		{
			base.Insert(index, item);
			if (this.OnItemInserted != null && !silent)
			{
				this.OnItemInserted(index, item);
			}
		}

		public void RemoveAt(int index, bool silent = false)
		{
			T arg = base[index];
			base.RemoveAt(index);
			if (this.OnItemRemoved != null && !silent)
			{
				this.OnItemRemoved(index, arg);
			}
		}

		public void Remove(T item, bool silent = false)
		{
			int arg = IndexOf(item);
			base.Remove(item);
			if (this.OnItemRemoved != null && !silent)
			{
				this.OnItemRemoved(arg, item);
			}
		}

		public void RemoveRange(int idx, int count, bool silent = false)
		{
			if (this.OnItemRemoved != null && !silent)
			{
				List<T> list = new List<T>();
				for (int i = 0; i <= count; i++)
				{
					list.Add(base[idx + i]);
				}
				base.RemoveRange(idx, count);
				int num = 0;
				{
					foreach (T item in list)
					{
						this.OnItemRemoved(idx + num, item);
						num++;
					}
					return;
				}
			}
			base.RemoveRange(idx, count);
		}
	}
}
