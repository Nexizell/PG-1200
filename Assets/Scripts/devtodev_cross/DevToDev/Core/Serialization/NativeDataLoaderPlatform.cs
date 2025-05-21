using System;
using System.Reflection;
using UnityEngine;

namespace DevToDev.Core.Serialization
{
	public class NativeDataLoaderPlatform
	{
		public static string Load(string applicationKey)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.nativedata.NativeDataLoader");
			return androidJavaClass.CallStatic<string>("GetNativeData", new object[1] { applicationKey });
		}

		public static void Clear(string applicationKey)
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.nativedata.NativeDataLoader");
			androidJavaClass.CallStatic("RemoveNativeData", applicationKey);
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				Type type = Type.GetType("DevToDev.MacOSHelper, Assembly-CSharp");
				type.GetMethod("dtd_j", BindingFlags.Static | BindingFlags.Public).Invoke(null, new object[1] { applicationKey });
			}
		}
	}
}
