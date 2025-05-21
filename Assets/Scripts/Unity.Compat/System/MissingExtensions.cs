using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace System
{
	public static class MissingExtensions
	{
		public static bool HasFlag(this Enum enumValue, Enum flag)
		{
			long num = Convert.ToInt64(enumValue);
			long num2 = Convert.ToInt64(flag);
			return (num & num2) == num2;
		}

		public static T GetCustomAttribute<T>(this PropertyInfo prop, bool inherit) where T : Attribute
		{
			return (T)prop.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
		}

		public static T GetCustomAttribute<T>(this PropertyInfo prop) where T : Attribute
		{
			return prop.GetCustomAttribute<T>(true);
		}

		public static T GetCustomAttribute<T>(this MemberInfo member, bool inherit) where T : Attribute
		{
			return (T)member.GetCustomAttributes(typeof(T), inherit).FirstOrDefault();
		}

		public static T GetCustomAttribute<T>(this MemberInfo member) where T : Attribute
		{
			return member.GetCustomAttribute<T>(true);
		}

		public static IEnumerable<TResult> Zip<T1, T2, TResult>(this IEnumerable<T1> list1, IEnumerable<T2> list2, Func<T1, T2, TResult> zipper)
		{
			IEnumerator<T1> e1 = list1.GetEnumerator();
			IEnumerator<T2> e2 = list2.GetEnumerator();
			while (e1.MoveNext() && e2.MoveNext())
			{
				yield return zipper(e1.Current, e2.Current);
			}
		}
	}
}
