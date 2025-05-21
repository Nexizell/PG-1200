using System;
using UnityEngine;

namespace DevToDev.Push
{
	public class PushClientPlatform
	{
		public void Initialize(Action<string> received, Action<string> failed, Action<string> pushReceived)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.push.DevToDevPushManager");
			androidJavaClass.CallStatic("init");
		}

		public string GetNativeEvents()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.push.DevToDevPushManager");
			return androidJavaClass.CallStatic<string>("getPushData", new object[0]);
		}

		public void ClearNativeEvents()
		{
		}

		public void SetCustomSmallIcon(string pathToResource)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.push.DevToDevPushManager");
			androidJavaClass.CallStatic("setCustomSmallIcon", pathToResource);
		}

		public void SetCustomLargeIcon(string pathToResource)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.push.DevToDevPushManager");
			androidJavaClass.CallStatic("setCustomLargeIcon", pathToResource);
		}
	}
}
