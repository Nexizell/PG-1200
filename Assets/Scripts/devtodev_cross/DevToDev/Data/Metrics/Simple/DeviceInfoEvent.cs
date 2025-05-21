using System;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Data.Metrics.Simple
{
	internal class DeviceInfoEvent : SimpleEvent
	{
		private static readonly string DEVICE_OS = "deviceOS";

		private static readonly string ANDROID_ID = "androidId";

		private static readonly string INCH = "inch";

		private static readonly string MANUFACTURER = "manufacturer";

		private static readonly string SERIAL_ID = "serialId";

		private static readonly string D2D_UDID = "d2dUdid";

		private static readonly string ADVERTISING_ID = "advertisingId";

		private static readonly string DEVICE_VERSION = "deviceVersion";

		private static readonly string SCREEN_RESOLUTION = "screenResolution";

		private static readonly string SCREEN_DPI = "screenDpi";

		private static readonly string MAC_WIFI = "macWifi";

		private static readonly string ODIN = "odin";

		private static readonly string MODEL = "model";

		private static readonly string IDFV = "idfv";

		private static readonly string IDFA = "idfa";

		private static readonly string TIME_ZIONE_OFFSET = "timeZoneOffset";

		public DeviceInfoEvent()
			: base(EventType.DeviceInfo)
		{
			try
			{
				DeviceHelper instance = DeviceHelper.Instance;
				parameters.Remove(Event.TIMESTAMP);
				addParameterIfNotNull(DEVICE_OS, instance.GetDeviceOSName());
				if (instance.GetScreenDPI() > 0)
				{
					addParameterIfNotNull(INCH, instance.GetScreenInches());
					addParameterIfNotNull(SCREEN_DPI, instance.GetScreenDPI());
				}
				addParameterIfNotNull(MANUFACTURER, instance.GetDeviceManufacturer());
				addParameterIfNotNull(D2D_UDID, instance.GetD2DUid());
				addParameterIfNotNull(ADVERTISING_ID, instance.GetAdvertismentId());
				addParameterIfNotNull(MODEL, instance.GetDeviceModel());
				addParameterIfNotNull(DEVICE_VERSION, instance.GetOSVersion());
				addParameterIfNotNull(SCREEN_RESOLUTION, instance.GetScreenResolutionString());
				if (!UnityPlayerPlatform.isUnityWebPlatform())
				{
					addParameterIfNotNull(MAC_WIFI, instance.GetMac());
				}
				addParameterIfNotNull(ODIN, instance.GetODIN());
				string hardwareToken = instance.GetHardwareToken();
				if (hardwareToken != null)
				{
					addParameterIfNotNull(SERIAL_ID, hardwareToken);
				}
				string androidId = instance.GetAndroidId();
				if (androidId != null)
				{
					addParameterIfNotNull(ANDROID_ID, androidId);
				}
				addParameterIfNotNull(IDFA, instance.GetIDFA());
				addParameterIfNotNull(IDFV, instance.GetIDFV());
				addParameterIfNotNull(TIME_ZIONE_OFFSET, instance.GetTimeZoneOffset());
			}
			catch (Exception ex)
			{
				Log.E(ex.Message + "\r\n" + ex.StackTrace);
			}
		}

		public DeviceInfoEvent(ObjectInfo info)
			: base(info)
		{
		}
	}
}
