using System;
using System.Collections.Generic;

namespace DevToDev.Core.Utils.Extensions
{
	internal static class DictionaryExtensions
	{
		public static void AddRange<T, S>(this Dictionary<T, S> source, Dictionary<T, S> collection)
		{
			if (collection == null)
			{
				throw new ArgumentNullException("Collection is null");
			}
			foreach (KeyValuePair<T, S> item in collection)
			{
				if (source.ContainsKey(item.Key))
				{
					source.Remove(item.Key);
				}
				source.Add(item.Key, item.Value);
			}
		}

		public static void AddWithReplace<T, S>(this Dictionary<T, S> source, T key, S value)
		{
			if (source.ContainsKey(key))
			{
				source.Remove(key);
			}
			source.Add(key, value);
		}

		public static void AddWithoutReplace<T, S>(this Dictionary<T, S> source, T key, S value)
		{
			if (!source.ContainsKey(key))
			{
				source.Add(key, value);
			}
		}
	}
}
