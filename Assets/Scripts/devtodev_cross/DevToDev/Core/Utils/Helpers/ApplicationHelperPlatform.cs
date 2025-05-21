using System;
using System.Reflection;
using UnityEngine;

namespace DevToDev.Core.Utils.Helpers
{
	public class ApplicationHelperPlatform
	{
		public static string GetAppVersion()
		{
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
				AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[2]
				{
					@static.Call<string>("getPackageName", new object[0]),
					0
				});
				return androidJavaObject2.Get<string>("versionName");
			}
			catch (Exception)
			{
			}
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				Type type = Type.GetType("DevToDev.MacOSHelper, Assembly-CSharp");
				return (string)type.GetMethod("dtd_e", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
			}
			return null;
		}

		public static string GetAppCodeVersion()
		{
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
				AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getPackageInfo", new object[2]
				{
					@static.Call<string>("getPackageName", new object[0]),
					0
				});
				return androidJavaObject2.Get<int>("versionCode").ToString();
			}
			catch (Exception)
			{
			}
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				Type type = Type.GetType("DevToDev.MacOSHelper, Assembly-CSharp");
				return (string)type.GetMethod("dtd_f", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
			}
			return null;
		}

		public static string GetAppBundle()
		{
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				return @static.Call<string>("getPackageName", new object[0]);
			}
			catch (Exception)
			{
			}
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				Type type = Type.GetType("DevToDev.MacOSHelper, Assembly-CSharp");
				return (string)type.GetMethod("dtd_d", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
			}
			return null;
		}

		public static object GetInstallSource()
		{
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getPackageManager", new object[0]);
				return androidJavaObject.Call<string>("getInstallerPackageName", new object[1] { @static.Call<string>("getPackageName", new object[0]) });
			}
			catch (Exception)
			{
			}
			return null;
		}
	}
}
