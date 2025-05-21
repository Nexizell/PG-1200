namespace DevToDev.Core.Utils
{
	internal class Connectivity
	{
		public static bool isConnected()
		{
			return ConnectivityPlatform.isConnected();
		}

		public static bool isConnectedWifi()
		{
			return ConnectivityPlatform.isConnectedWifi();
		}

		public static bool isConnectedMobile()
		{
			return !isConnectedWifi();
		}

		public static bool isConnectionFast()
		{
			return isConnectedWifi();
		}
	}
}
