using System;

namespace Prime31Exceptions
{
	public class P31Windows8Exception : Exception
	{
		public P31Windows8Exception()
			: base("This method is not available on Windows 8")
		{
		}
	}
}
