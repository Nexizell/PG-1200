using System.Collections.Generic;

namespace System.Threading
{
	public sealed class CancellationTokenSource
	{
		private object mutex = new object();

		private Action actions;

		private bool isCancellationRequested;

		internal bool IsCancellationRequested
		{
			get
			{
				lock (mutex)
				{
					return isCancellationRequested;
				}
			}
		}

		public CancellationToken Token
		{
			get
			{
				return new CancellationToken(this);
			}
		}

		internal CancellationTokenRegistration Register(Action action)
		{
			lock (mutex)
			{
				actions = (Action)Delegate.Combine(actions, action);
				return new CancellationTokenRegistration(this, action);
			}
		}

		internal void Unregister(Action action)
		{
			lock (mutex)
			{
				actions = (Action)Delegate.Remove(actions, action);
			}
		}

		public void Cancel()
		{
			Cancel(false);
		}

		public void Cancel(bool throwOnFirstException)
		{
			lock (mutex)
			{
				isCancellationRequested = true;
				if (actions == null)
				{
					return;
				}
				try
				{
					if (throwOnFirstException)
					{
						actions();
						return;
					}
					Delegate[] invocationList = actions.GetInvocationList();
					foreach (Delegate @delegate in invocationList)
					{
						List<Exception> list = new List<Exception>();
						try
						{
							((Action)@delegate)();
						}
						catch (Exception item)
						{
							list.Add(item);
						}
						if (list.Count > 0)
						{
							throw new AggregateException(list);
						}
					}
				}
				finally
				{
					actions = null;
				}
			}
		}
	}
}
