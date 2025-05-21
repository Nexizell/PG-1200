using DevToDev.Core.Utils;
using DevToDev.Logic;
using DevToDev.Push;
using UnityEngine;

namespace DevToDev
{
	public static class PushManager
	{
		private static PushClient pushClient;

		internal static PushClient PushClient
		{
			get
			{
				if (pushClient == null)
				{
					pushClient = new PushClient();
				}
				return pushClient;
			}
		}

		public static OnPushTokenReceivedHandler PushTokenReceived
		{
			get
			{
				return PushClient.OnPushTokenReceived;
			}
			set
			{
				PushClient.OnPushTokenReceived = value;
			}
		}

		public static OnPushTokenFailedHandler PushTokenFailed
		{
			get
			{
				return PushClient.OnPushTokenFailed;
			}
			set
			{
				PushClient.OnPushTokenFailed = value;
			}
		}

		public static OnPushReceivedHandler PushReceived
		{
			get
			{
				return PushClient.OnPushReceived;
			}
			set
			{
				PushClient.OnPushReceived = value;
			}
		}

		public static void Initialize()
		{
			if (!SDKClient.Instance.IsInitialized)
			{
				SDKClient.Instance.Execute(delegate
				{
					Initialize();
				});
			}
			else if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer || UnityPlayerPlatform.isUnityWSAPlatform())
			{
				PushClient.Initialize();
			}
		}

		public static void Destroy()
		{
			if (pushClient != null)
			{
				pushClient = null;
			}
		}

		public static void SetCustomSmallIcon(string pathToResource)
		{
			PushClient.SetCustomSmallIcon(pathToResource);
		}

		public static void SetCustomLargeIcon(string pathToResource)
		{
			PushClient.SetCustomLargeIcon(pathToResource);
		}
	}
}
