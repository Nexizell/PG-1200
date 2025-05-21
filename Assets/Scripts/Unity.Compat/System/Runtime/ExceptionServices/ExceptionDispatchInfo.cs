namespace System.Runtime.ExceptionServices
{
	public class ExceptionDispatchInfo
	{
		public Exception SourceException { get; private set; }

		public static ExceptionDispatchInfo Capture(Exception ex)
		{
			return new ExceptionDispatchInfo(ex);
		}

		private ExceptionDispatchInfo(Exception ex)
		{
			SourceException = ex;
		}

		public void Throw()
		{
			throw SourceException;
		}
	}
}
