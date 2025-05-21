using System.Threading;

namespace System
{
	public class Progress<T> : IProgress<T> where T : EventArgs
	{
		private SynchronizationContext synchronizationContext;

		private SendOrPostCallback synchronizationCallback;

		private Action<T> eventHandler;

		public event EventHandler<T> ProgressChanged;

		public Progress()
		{
			synchronizationContext = SynchronizationContext.Current ?? ProgressSynchronizationContext.SharedContext;
			synchronizationCallback = NotifyDelegates;
		}

		public Progress(Action<T> handler)
			: this()
		{
			eventHandler = handler;
		}

		void IProgress<T>.Report(T value)
		{
			OnReport(value);
		}

		protected virtual void OnReport(T value)
		{
			synchronizationContext.Post(synchronizationCallback, value);
		}

		private void NotifyDelegates(object newValue)
		{
			T val = (T)newValue;
			Action<T> action = eventHandler;
			EventHandler<T> progressChanged = this.ProgressChanged;
			if (action != null)
			{
				action(val);
			}
			if (progressChanged != null)
			{
				progressChanged(this, val);
			}
		}
	}
}
