using System;
using System.Reflection;
using UnityEngine;

namespace DevToDev.Core.Utils
{
	public class InitializationPlatform
	{
		public static void StartSessionTracker(string gameObjectName)
		{
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				Type type = Type.GetType("DevToDev.MacOSHelper, Assembly-CSharp");
				type.GetMethod("dtd_z", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
			}
		}

		public static string GetReferral()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.InstallReceiver");
			return androidJavaClass.CallStatic<string>("getReferral", new object[0]);
		}
	}
}
