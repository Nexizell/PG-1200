namespace System.Threading.Tasks
{
	public class TaskCompletionSource<T>
	{
		public Task<T> Task { get; private set; }

		public TaskCompletionSource()
		{
			Task = new Task<T>();
		}

		public bool TrySetResult(T result)
		{
			return Task.TrySetResult(result);
		}

		public bool TrySetException(AggregateException exception)
		{
			return Task.TrySetException(exception);
		}

		public bool TrySetException(Exception exception)
		{
			AggregateException ex = exception as AggregateException;
			if (ex != null)
			{
				return Task.TrySetException(ex);
			}
			return Task.TrySetException(new AggregateException(new Exception[1] { exception }).Flatten());
		}

		public bool TrySetCanceled()
		{
			return Task.TrySetCanceled();
		}

		public void SetResult(T result)
		{
			if (!TrySetResult(result))
			{
				throw new InvalidOperationException("Cannot set the result of a completed task.");
			}
		}

		public void SetException(AggregateException exception)
		{
			if (!TrySetException(exception))
			{
				throw new InvalidOperationException("Cannot set the exception of a completed task.");
			}
		}

		public void SetException(Exception exception)
		{
			if (!TrySetException(exception))
			{
				throw new InvalidOperationException("Cannot set the exception of a completed task.");
			}
		}

		public void SetCanceled()
		{
			if (!TrySetCanceled())
			{
				throw new InvalidOperationException("Cannot cancel a completed task.");
			}
		}
	}
}
