using System.Collections.Generic;

namespace System.Runtime.CompilerServices
{
	public class ConditionalWeakTable<TKey, TValue> where TKey : class where TValue : class
	{
		private class Reference
		{
			private int hashCode;

			public WeakReference WeakReference { get; private set; }

			public TKey Value
			{
				get
				{
					return (TKey)WeakReference.Target;
				}
			}

			public Reference(TKey obj)
			{
				hashCode = obj.GetHashCode();
				WeakReference = new WeakReference(obj);
			}

			public override int GetHashCode()
			{
				return hashCode;
			}

			public override bool Equals(object obj)
			{
				Reference reference = obj as Reference;
				if (reference == null)
				{
					return false;
				}
				if (reference.GetHashCode() != GetHashCode())
				{
					return false;
				}
				return reference.WeakReference.Target == WeakReference.Target;
			}
		}

		public delegate TValue CreateValueCallback(TKey key);

		private IDictionary<Reference, TValue> data;

		public ConditionalWeakTable()
		{
			data = new Dictionary<Reference, TValue>();
		}

		private void CleanUp()
		{
			foreach (Reference item in new HashSet<Reference>(data.Keys))
			{
				if (!item.WeakReference.IsAlive)
				{
					data.Remove(item);
				}
			}
		}

		public TValue GetValue(TKey key, CreateValueCallback createValueCallback)
		{
			CleanUp();
			Reference key2 = new Reference(key);
			TValue value;
			if (data.TryGetValue(key2, out value))
			{
				return value;
			}
			return data[key2] = createValueCallback(key);
		}
	}
}
