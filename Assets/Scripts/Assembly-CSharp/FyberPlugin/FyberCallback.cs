using System;

namespace FyberPlugin
{
	public class FyberCallback
	{
		public static event Action<Ad> AdAvailable;

		public static event Action<AdResult> AdFinished;

		public static event Action<AdFormat> AdNotAvailable;

		public static event Action<Ad> AdStarted;

		public static event Action<string> NativeError;

		public static event Action<RequestError> RequestFail;
	}
}
