using System;
using UnityEngine;

namespace DevToDev.Core.Utils.Helpers
{
	internal class DeviceHelper
	{
		private static DeviceHelper instance;

		private DeviceHelperPlatform deviceHelperPlatform;

		public static DeviceHelper Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new DeviceHelper();
				}
				return instance;
			}
		}

		public DeviceHelper()
		{
			deviceHelperPlatform = new DeviceHelperPlatform();
		}

		public long GetUnixTime()
		{
			DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).ToLocalTime();
			return (DateTime.Now.Ticks - dateTime.Ticks) / 10000;
		}

		public long GetSecondsFromTime(DateTime time)
		{
			DateTime dateTime = new DateTime(1970, 1, 1);
			return (time.Ticks - dateTime.Ticks) / 10000000;
		}

		public string GetDeviceId()
		{
			return deviceHelperPlatform.GetDeviceId(RFC4122.Get());
		}

		public string GetAdvertismentId()
		{
			return deviceHelperPlatform.GetAdvertismentId();
		}

		public string GetHardwareToken()
		{
			return deviceHelperPlatform.GetHardwareToken();
		}

		public string GetIDFA()
		{
			return deviceHelperPlatform.GetIDFA();
		}

		public string GetIDFV()
		{
			return deviceHelperPlatform.GetIDFV();
		}

		public string GetUserAgentSring()
		{
			return deviceHelperPlatform.GetUserAgentSring();
		}

		public int IsRooted()
		{
			return deviceHelperPlatform.IsRooted();
		}

		public string GetMobileOperator()
		{
			return deviceHelperPlatform.GetMobileOperator();
		}

		public string GetDeviceManufacturer()
		{
			return deviceHelperPlatform.GetDeviceManufacturer();
		}

		public string GetD2DUid()
		{
			return RFC4122.Get();
		}

		public string GetScreenResolutionString()
		{
			int[] screenResolution = deviceHelperPlatform.GetScreenResolution();
			return screenResolution[0] + "x" + screenResolution[1];
		}

		public int[] GetScreenResolution()
		{
			return deviceHelperPlatform.GetScreenResolution();
		}

		public float GetScreenInches()
		{
			int[] screenResolution = GetScreenResolution();
			float num = GetScreenDPI();
			return (float)Math.Sqrt(Math.Pow((float)screenResolution[1] / num, 2.0) + Math.Pow((float)screenResolution[0] / num, 2.0));
		}

		public int GetScreenDPI()
		{
			return (int)Screen.dpi;
		}

		public string GetDeviceOSName()
		{
			return deviceHelperPlatform.GetDeviceOSName();
		}

		public string GetOSVersion()
		{
			return deviceHelperPlatform.GetDeviceOSVersion();
		}

		public string GetDeviceModel()
		{
			return deviceHelperPlatform.GetDeviceModel();
		}

		public string GetMac()
		{
			return deviceHelperPlatform.GetMac();
		}

		public int GetDeviceOrientation()
		{
			int result = 0;
			if (Screen.orientation == ScreenOrientation.Portrait || Screen.orientation == ScreenOrientation.PortraitUpsideDown)
			{
				result = 1;
			}
			if (Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeLeft || Screen.orientation == ScreenOrientation.LandscapeRight)
			{
				result = 2;
			}
			return result;
		}

		public string GetLocale()
		{
			return deviceHelperPlatform.GetLocale();
		}

		public string GetInternetProtocolAddress()
		{
			return deviceHelperPlatform.GetIpAddress();
		}

		public string GetODIN()
		{
			return deviceHelperPlatform.GetODIN(RFC4122.Get());
		}

		public float GetDeviceScaleFactor()
		{
			return deviceHelperPlatform.GetDeviceScaleFactor();
		}

		public int GetTimeZoneOffset()
		{
			return deviceHelperPlatform.GetTimeZone();
		}

		public string GetAndroidId()
		{
			return deviceHelperPlatform.GetAndroidId();
		}
	}
}
