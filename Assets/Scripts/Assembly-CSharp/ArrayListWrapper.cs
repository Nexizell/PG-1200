using System;
using System.Collections;
using Rilisoft;

public class ArrayListWrapper
{
	private ArrayList _list = new ArrayList();

	public int Count
	{
		get
		{
			using (new ArrayListChecker(_list, "_list"))
			{
				return _list.Count;
			}
		}
	}

	public object this[int index]
	{
		get
		{
			using (new ArrayListChecker(_list, "_list"))
			{
				return _list[index];
			}
		}
		set
		{
			using (new ArrayListChecker(_list, "_list"))
			{
				_list[index] = value;
			}
		}
	}

	public void AddRange(ICollection c)
	{
		using (new ArrayListChecker(_list, "_list"))
		{
			_list.AddRange(c);
		}
	}

	public int Add(object item)
	{
		using (new ArrayListChecker(_list, "_list"))
		{
			return _list.Add(item);
		}
	}

	public bool Contains(object item)
	{
		using (new ArrayListChecker(_list, "_list"))
		{
			return _list.Contains(item);
		}
	}

	public Array ToArray(Type type)
	{
		using (new ArrayListChecker(_list, "_list"))
		{
			return _list.ToArray(type);
		}
	}

	public void RemoveAt(int index)
	{
		using (new ArrayListChecker(_list, "_list"))
		{
			_list.RemoveAt(index);
		}
	}

	public void Insert(int index, object obj)
	{
		using (new ArrayListChecker(_list, "_list"))
		{
			_list.Insert(index, obj);
		}
	}
}
