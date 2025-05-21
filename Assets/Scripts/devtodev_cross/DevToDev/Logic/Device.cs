using System;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Logic
{
	internal class Device : ISaveable
	{
		private List<string> deviceIds;

		public string DeviceId
		{
			get
			{
				if (deviceIds.Count > 0)
				{
					return deviceIds[deviceIds.Count - 1];
				}
				return null;
			}
		}

		public string DevicePrev
		{
			get
			{
				if (deviceIds.Count > 1)
				{
					return deviceIds[deviceIds.Count - 2];
				}
				return null;
			}
		}

		public Device()
		{
			deviceIds = new List<string>();
			deviceIds.Add(DeviceHelper.Instance.GetDeviceId());
		}

		public Device(ObjectInfo info)
		{
			try
			{
				deviceIds = info.GetValue("deviceIds", typeof(List<string>)) as List<string>;
			}
			catch (Exception ex)
			{
				Log.D("Error in desealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			try
			{
				info.AddValue("deviceIds", deviceIds);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public bool CheckDeviceChanges()
		{
			if (!DeviceId.Equals(DeviceHelper.Instance.GetDeviceId()))
			{
				deviceIds.Add(DeviceHelper.Instance.GetDeviceId());
				return true;
			}
			return false;
		}

		public void LoadNativeData(JSONNode data)
		{
			List<string> list = deviceIds;
			deviceIds = new List<string>();
			if (data["PreviousId"] != null)
			{
				string value = data["PreviousId"].Value;
				Log.D("Adding PreviousId: " + value);
				if (!string.IsNullOrEmpty(value))
				{
					deviceIds.Add(value);
				}
			}
			if (data["AdvertismentId"] != null)
			{
				string value2 = data["AdvertismentId"].Value;
				Log.D("Adding AdvertismentId: " + value2);
				if (!string.IsNullOrEmpty(value2))
				{
					deviceIds.Add(value2);
				}
			}
			if (list.Count > 0)
			{
				deviceIds.Add(list[0]);
			}
		}
	}
}
