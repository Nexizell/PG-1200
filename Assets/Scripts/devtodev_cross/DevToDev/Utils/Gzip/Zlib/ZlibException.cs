using System;

namespace DevToDev.Utils.Gzip.Zlib
{
	internal class ZlibException : Exception
	{
		public ZlibException()
		{
		}

		public ZlibException(string s)
			: base(s)
		{
		}
	}
}
