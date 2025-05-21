using System.Collections.Generic;
using System.Linq;
using Unity.Tasks.Internal;

namespace System.Threading.Tasks
{
	public abstract class Task
	{
		private static readonly ThreadLocal<int> executionDepth = new ThreadLocal<int>(() => 0);

		private static readonly Action<Action> immediateExecutor = delegate(Action a)
		{
			bool num = AppDomain.CurrentDomain.FriendlyName.Equals("IL2CPP Root Domain");
			int num2 = 10;
			if (num)
			{
				num2 = 200;
			}
			executionDepth.Value++;
			try
			{
				if (executionDepth.Value <= num2)
				{
					a();
				}
				else
				{
					Factory.Scheduler.Post(a);
				}
			}
			finally
			{
				executionDepth.Value--;
			}
		};

		internal readonly object mutex = new object();

		internal IList<Action<Task>> continuations = new List<Action<Task>>();

		internal AggregateException exception;

		internal bool isCanceled;

		internal bool isCompleted;

		public static TaskFactory Factory
		{
			get
			{
				return new TaskFactory();
			}
		}

		public AggregateException Exception
		{
			get
			{
				lock (mutex)
				{
					return exception;
				}
			}
		}

		public bool IsCanceled
		{
			get
			{
				lock (mutex)
				{
					return isCanceled;
				}
			}
		}

		public bool IsCompleted
		{
			get
			{
				lock (mutex)
				{
					return isCompleted;
				}
			}
		}

		public bool IsFaulted
		{
			get
			{
				return Exception != null;
			}
		}

		internal Task()
		{
		}

		public void Wait()
		{
			lock (mutex)
			{
				if (!IsCompleted)
				{
					Monitor.Wait(mutex);
				}
				if (IsFaulted)
				{
					throw Exception;
				}
			}
		}

		public Task<T> ContinueWith<T>(Func<Task, T> continuation)
		{
			return ContinueWith(continuation, CancellationToken.None);
		}

		public Task<T> ContinueWith<T>(Func<Task, T> continuation, CancellationToken cancellationToken)
		{
			bool flag = false;
			TaskCompletionSource<T> tcs = new TaskCompletionSource<T>();
			CancellationTokenRegistration cancellation = cancellationToken.Register(delegate
			{
				tcs.TrySetCanceled();
			});
			Action<Task> action = delegate(Task t)
			{
				immediateExecutor(delegate
				{
					try
					{
						tcs.TrySetResult(continuation(t));
						cancellation.Dispose();
					}
					catch (Exception ex)
					{
						tcs.TrySetException(ex);
						cancellation.Dispose();
					}
				});
			};
			lock (mutex)
			{
				flag = IsCompleted;
				if (!flag)
				{
					continuations.Add(action);
				}
			}
			if (flag)
			{
				action(this);
			}
			return tcs.Task;
		}

		public Task ContinueWith(Action<Task> continuation)
		{
			return ContinueWith(continuation, CancellationToken.None);
		}

		public Task ContinueWith(Action<Task> continuation, CancellationToken cancellationToken)
		{
			return ContinueWith(delegate(Task t)
			{
				continuation(t);
				return 0;
			}, cancellationToken);
		}

		public static Task WhenAll(params Task[] tasks)
		{
			return WhenAll((IEnumerable<Task>)tasks);
		}

		public static Task WhenAll(IEnumerable<Task> tasks)
		{
			Task[] taskArr = tasks.ToArray();
			if (taskArr.Length == 0)
			{
				return FromResult(0);
			}
			TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
			Factory.ContinueWhenAll(taskArr, delegate
			{
				AggregateException[] array = (from t in taskArr
					where t.IsFaulted
					select t.Exception).ToArray();
				if (array.Length != 0)
				{
					tcs.SetException(new AggregateException(array));
				}
				else if (taskArr.Any((Task t) => t.IsCanceled))
				{
					tcs.SetCanceled();
				}
				else
				{
					tcs.SetResult(0);
				}
			});
			return tcs.Task;
		}

		internal static Task<Task> WhenAny(params Task[] tasks)
		{
			return WhenAny((IEnumerable<Task>)tasks);
		}

		internal static Task<Task> WhenAny(IEnumerable<Task> tasks)
		{
			TaskCompletionSource<Task> tcs = new TaskCompletionSource<Task>();
			foreach (Task task in tasks)
			{
				task.ContinueWith((Task t) => tcs.TrySetResult(t));
			}
			return tcs.Task;
		}

		public static Task<T[]> WhenAll<T>(IEnumerable<Task<T>> tasks)
		{
			return WhenAll(tasks.Cast<Task>()).OnSuccess((Task _) => tasks.Select((Task<T> t) => t.Result).ToArray());
		}

		public static Task<T> FromResult<T>(T result)
		{
			TaskCompletionSource<T> taskCompletionSource = new TaskCompletionSource<T>();
			taskCompletionSource.SetResult(result);
			return taskCompletionSource.Task;
		}

		public static Task<T> Run<T>(Func<T> toRun)
		{
			return Factory.StartNew(toRun);
		}

		public static Task Run(Action toRun)
		{
			return Factory.StartNew(delegate
			{
				toRun();
				return 0;
			});
		}

		public static Task Delay(TimeSpan timespan)
		{
			TaskCompletionSource<int> tcs = new TaskCompletionSource<int>();
			new Timer(delegate
			{
				tcs.TrySetResult(0);
			}).Change(timespan, TimeSpan.FromMilliseconds(-1.0));
			return tcs.Task;
		}
	}
	public sealed class Task<T> : Task
	{
		private T result;

		public T Result
		{
			get
			{
				Wait();
				return result;
			}
		}

		internal Task()
		{
		}

		public Task ContinueWith(Action<Task<T>> continuation)
		{
			return ContinueWith(delegate(Task t)
			{
				continuation((Task<T>)t);
			});
		}

		public Task<TResult> ContinueWith<TResult>(Func<Task<T>, TResult> continuation)
		{
			return ContinueWith((Task t) => continuation((Task<T>)t));
		}

		private void RunContinuations()
		{
			lock (mutex)
			{
				foreach (Action<Task> continuation in continuations)
				{
					continuation(this);
				}
				continuations = null;
			}
		}

		internal bool TrySetResult(T result)
		{
			lock (mutex)
			{
				if (isCompleted)
				{
					return false;
				}
				isCompleted = true;
				this.result = result;
				Monitor.PulseAll(mutex);
				RunContinuations();
				return true;
			}
		}

		internal bool TrySetCanceled()
		{
			lock (mutex)
			{
				if (isCompleted)
				{
					return false;
				}
				isCompleted = true;
				isCanceled = true;
				Monitor.PulseAll(mutex);
				RunContinuations();
				return true;
			}
		}

		internal bool TrySetException(AggregateException exception)
		{
			lock (mutex)
			{
				if (isCompleted)
				{
					return false;
				}
				isCompleted = true;
				base.exception = exception;
				Monitor.PulseAll(mutex);
				RunContinuations();
				return true;
			}
		}
	}
}
