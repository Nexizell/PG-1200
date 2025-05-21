using System;
using System.Collections.Generic;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Data.Metrics.Aggregated.CustomEvent
{
	internal class CustomEventData : ISaveable
	{
		public long CreationTimestamp = DeviceHelper.Instance.GetUnixTime() / 1000;

		public string EventId { get; set; }

		public string EventName { get; set; }

		public long StartTime { get; set; }

		public long EndTime { get; set; }

		public long Duration { get; set; }

		public CustomEventParams Params { get; set; }

		public CustomEventDataType Type { get; set; }

		public List<string> PendingData { get; set; }

		public bool HasParams
		{
			get
			{
				if (Params != null)
				{
					return Params.Count > 0;
				}
				return false;
			}
		}

		private CustomEventData()
		{
			Duration = 0L;
			StartTime = DeviceHelper.Instance.GetUnixTime() / 1000;
			Params = new CustomEventParams();
		}

		public CustomEventData(ObjectInfo info)
		{
			try
			{
				EventId = info.GetValue("EventId", typeof(string)) as string;
				EventName = info.GetValue("EventName", typeof(string)) as string;
				StartTime = (long)info.GetValue("StartTime", typeof(long));
				EndTime = (long)info.GetValue("EndTime", typeof(long));
				Duration = (long)info.GetValue("Duration", typeof(long));
				Params = info.GetValue("ceParams", typeof(CustomEventParams)) as CustomEventParams;
				Type = (CustomEventDataType)(int)info.GetValue("Type", typeof(int));
				PendingData = info.GetValue("pendingData", typeof(List<string>)) as List<string>;
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
				info.AddValue("EventId", EventId);
				info.AddValue("EventName", EventName);
				info.AddValue("StartTime", StartTime);
				info.AddValue("EndTime", EndTime);
				info.AddValue("Duration", Duration);
				info.AddValue("Type", (int)Type);
				info.AddValue("ceParams", Params);
				info.AddValue("pendingData", PendingData);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public static CustomEventData SingleData(string eventName, CustomEventParams eventParams)
		{
			CustomEventData customEventData = new CustomEventData();
			customEventData.Type = CustomEventDataType.Single;
			customEventData.EventName = eventName;
			if (eventParams != null)
			{
				customEventData.Params.CopyFromAnother(eventParams);
			}
			return customEventData;
		}
	}
}
