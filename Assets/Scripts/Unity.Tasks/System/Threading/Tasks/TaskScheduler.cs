namespace System.Threading.Tasks
{
	public class TaskScheduler
	{
		private static SynchronizationContext defaultContext = new SynchronizationContext();

		private SynchronizationContext context;

		public TaskScheduler(SynchronizationContext context)
		{
			this.context = context ?? defaultContext;
		}

		public void Post(Action action)
		{
			context.Post(delegate
			{
				action();
			}, null);
		}

		public static TaskScheduler FromCurrentSynchronizationContext()
		{
			return new TaskScheduler(SynchronizationContext.Current);
		}
	}
}
