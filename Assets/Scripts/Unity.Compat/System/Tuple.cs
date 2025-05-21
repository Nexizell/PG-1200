namespace System
{
	public static class Tuple
	{
		public static Tuple<T1, T2> Create<T1, T2>(T1 t1, T2 t2)
		{
			return new Tuple<T1, T2>(t1, t2);
		}
	}
	public class Tuple<T1, T2>
	{
		public T1 Item1 { get; private set; }

		public T2 Item2 { get; private set; }

		public Tuple(T1 item1, T2 item2)
		{
			Item1 = item1;
			Item2 = item2;
		}

		public override bool Equals(object obj)
		{
			Tuple<T1, T2> tuple = obj as Tuple<T1, T2>;
			if (tuple == null)
			{
				return false;
			}
			if (object.Equals(Item1, tuple.Item1))
			{
				return object.Equals(Item2, tuple.Item2);
			}
			return false;
		}

		public override int GetHashCode()
		{
			int num = ((Item1 != null) ? Item1.GetHashCode() : 0);
			int num2 = ((Item2 != null) ? Item2.GetHashCode() : 0);
			return num ^ num2;
		}
	}
}
