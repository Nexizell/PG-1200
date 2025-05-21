namespace System.Threading
{
	public struct CancellationTokenRegistration : IDisposable
	{
		private Action action;

		private CancellationTokenSource source;

		internal CancellationTokenRegistration(CancellationTokenSource source, Action action)
		{
			this.source = source;
			this.action = action;
		}

		public void Dispose()
		{
			if (source != null && action != null)
			{
				source.Unregister(action);
				action = null;
				source = null;
			}
		}
	}
}
