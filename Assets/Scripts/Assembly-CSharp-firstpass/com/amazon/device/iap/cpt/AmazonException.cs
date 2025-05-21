using System;

namespace com.amazon.device.iap.cpt
{
	public class AmazonException : ApplicationException
	{
		public AmazonException()
		{
		}

		public AmazonException(string message)
			: base(message)
		{
		}

		public AmazonException(string message, Exception inner)
			: base(message, inner)
		{
		}
	}
}
