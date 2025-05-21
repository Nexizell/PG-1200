using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

namespace DevToDev.Core.Utils.Helpers
{
	public class DeviceHelperPlatform
	{
		public string GetDeviceOSName()
		{
			return "Android";
		}

		public string GetDeviceOSVersion()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build$VERSION"))
			{
				return androidJavaClass.GetStatic<string>("RELEASE");
			}
		}

		public string GetDeviceId(string RFC4122Id)
		{
			string text = GetAdvertismentId();
			if (string.IsNullOrEmpty(text))
			{
				text = RFC4122Id;
			}
			if (Application.platform == RuntimePlatform.OSXPlayer)
			{
				Type type = Type.GetType("DevToDev.MacOSHelper, Assembly-CSharp");
				return (string)type.GetMethod("dtd_g", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
			}
			return text;
		}

		public string GetAdvertismentId()
		{
			string text = null;
			bool flag = false;
			try
			{
				using (AndroidJavaClass androidJavaClass2 = new AndroidJavaClass("com.google.android.gms.ads.identifier.AdvertisingIdClient"))
				{
					AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
					AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
					AndroidJavaObject androidJavaObject = androidJavaClass2.CallStatic<AndroidJavaObject>("getAdvertisingIdInfo", new object[1] { @static });
					text = androidJavaObject.Call<string>("getId", new object[0]).ToString();
					if (!androidJavaObject.Call<bool>("isLimitAdTrackingEnabled", new object[0]))
					{
						return text;
					}
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		public string GetHardwareToken()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.core.utils.DeviceUtils");
			return androidJavaClass.CallStatic<string>("getHardwareToken", new object[0]);
		}

		public string GetAndroidId()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.core.utils.DeviceUtils");
			return androidJavaClass.CallStatic<string>("getAndroidID", new object[0]);
		}

		public int[] GetScreenResolution()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.core.utils.DeviceUtils");
			int num = androidJavaClass.CallStatic<int>("getScreenResolutionMax", new object[0]);
			int num2 = androidJavaClass.CallStatic<int>("getScreenResolutionMin", new object[0]);
			return new int[2] { num, num2 };
		}

		public string GetIDFA()
		{
			return null;
		}

		public string GetIDFV()
		{
			return null;
		}

		public string GetUserAgentSring()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.core.utils.DeviceUtils");
			return androidJavaClass.CallStatic<string>("getUserAgent", new object[0]);
		}

		public int IsRooted()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.core.utils.DeviceUtils");
			if (!androidJavaClass.CallStatic<bool>("isRoot", new object[0]))
			{
				return 0;
			}
			return 1;
		}

		public string GetMobileOperator()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.devtodev.core.utils.DeviceUtils");
			return androidJavaClass.CallStatic<int>("mobileOperator", new object[0]).ToString();
		}

		public string GetDeviceManufacturer()
		{
			AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build");
			return androidJavaClass.GetStatic<string>("MANUFACTURER");
		}

		public int GetTimeZone()
		{
			return (int)TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalSeconds;
		}

		public string GetDeviceModel()
		{
			using (AndroidJavaClass androidJavaClass = new AndroidJavaClass("android.os.Build"))
			{
				return androidJavaClass.GetStatic<string>("MODEL");
			}
		}

		public float GetDeviceScaleFactor()
		{
			return 1f;
		}

		public string GetLocale()
		{
			try
			{
				AndroidJavaClass androidJavaClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
				AndroidJavaObject @static = androidJavaClass.GetStatic<AndroidJavaObject>("currentActivity");
				AndroidJavaObject androidJavaObject = @static.Call<AndroidJavaObject>("getResources", new object[0]);
				AndroidJavaObject androidJavaObject2 = androidJavaObject.Call<AndroidJavaObject>("getConfiguration", new object[0]);
				AndroidJavaObject androidJavaObject3 = androidJavaObject2.Get<AndroidJavaObject>("locale");
				if (androidJavaObject3 != null)
				{
					return androidJavaObject3.Call<string>("toString", new object[0]);
				}
			}
			catch (Exception)
			{
			}
			return null;
		}

		public string GetIpAddress()
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			string text = null;
			try
			{
				text = Dns.GetHostName();
				IPHostEntry hostEntry = Dns.GetHostEntry(text);
				IPAddress[] addressList = hostEntry.AddressList;
				return addressList[addressList.Length - 1].ToString();
			}
			catch (Exception)
			{
				try
				{
					return "0.0.0.0";
				}
				catch (Exception)
				{
					return "0.0.0.0";
				}
			}
		}

		public string GetODIN(string RFC4122Id)
		{
			string result = null;
			byte[] bytes = Encoding.UTF8.GetBytes(RFC4122Id);
			if (bytes != null)
			{
				SHA1Managed sHA1Managed = new SHA1Managed();
				bytes = sHA1Managed.ComputeHash(bytes);
				result = BitConverter.ToString(bytes).Replace("-", "").ToLower();
			}
			return result;
		}

		public string GetMac()
		{
			string text = null;
			try
			{
				NetworkInterface[] allNetworkInterfaces = NetworkInterface.GetAllNetworkInterfaces();
				NetworkInterface[] array = allNetworkInterfaces;
				int num = 0;
				if (num < array.Length)
				{
					NetworkInterface networkInterface = array[num];
					PhysicalAddress physicalAddress = networkInterface.GetPhysicalAddress();
					byte[] addressBytes = physicalAddress.GetAddressBytes();
					for (int i = 0; i < addressBytes.Length; i++)
					{
						text = string.Concat(text + string.Format("{0}", addressBytes[i].ToString("X2")));
						if (i != addressBytes.Length - 1)
						{
							text = string.Concat(text + ":");
						}
					}
				}
			}
			catch (Exception)
			{
				text = "02:00:00:00:00:00";
			}
			return text;
		}
	}
}
