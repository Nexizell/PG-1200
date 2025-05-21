using System;
using UnityEngine;

namespace DevToDev.Core.Utils
{
	public class ConnectivityPlatform
	{
		public static bool isConnected()
		{
			bool result = false;
			try
			{
				if (Application.internetReachability != 0)
				{
					result = true;
				}
			}
			catch (Exception)
			{
			}
			return result;
		}

		public static bool isConnectedWifi()
		{
			bool result = false;
			try
			{
				if (Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
				{
					result = true;
				}
			}
			catch (Exception)
			{
			}
			return result;
		}
	}
}
