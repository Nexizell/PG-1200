using System;

namespace Rilisoft.NullExtensions
{
	public static class NullEx
	{
		public static TResult Map<TInput, TResult>(this TInput o, Func<TInput, TResult> selector) where TInput : class
		{
			if (o == null)
			{
				return default(TResult);
			}
			return selector(o);
		}

		public static TResult Map<TInput, TResult>(this TInput o, Func<TInput, TResult> selector, TResult defaultValue) where TInput : class
		{
			if (o == null)
			{
				return defaultValue;
			}
			return selector(o);
		}

		public static TInput Filter<TInput>(this TInput o, Func<TInput, bool> pred) where TInput : class
		{
			if (o == null)
			{
				return null;
			}
			if (!pred(o))
			{
				return null;
			}
			return o;
		}

		public static TInput Do<TInput>(this TInput o, Action<TInput> action) where TInput : class
		{
			if (o == null)
			{
				return null;
			}
			action(o);
			return o;
		}

		public static TInput Do<TInput>(this TInput o, Action<TInput> action, Action defaultAction) where TInput : class
		{
			if (o == null)
			{
				defaultAction();
				return null;
			}
			action(o);
			return o;
		}
	}
}
