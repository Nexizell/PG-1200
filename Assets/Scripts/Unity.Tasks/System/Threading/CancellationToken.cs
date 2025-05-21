namespace System.Threading
{
	public struct CancellationToken
	{
		private CancellationTokenSource source;

		public static CancellationToken None
		{
			get
			{
				return default(CancellationToken);
			}
		}

		public bool IsCancellationRequested
		{
			get
			{
				if (source != null)
				{
					return source.IsCancellationRequested;
				}
				return false;
			}
		}

		internal CancellationToken(CancellationTokenSource source)
		{
			this.source = source;
		}

		public CancellationTokenRegistration Register(Action callback)
		{
			if (source != null)
			{
				return source.Register(callback);
			}
			return default(CancellationTokenRegistration);
		}

		public void ThrowIfCancellationRequested()
		{
			if (IsCancellationRequested)
			{
				throw new OperationCanceledException();
			}
		}
	}
}
