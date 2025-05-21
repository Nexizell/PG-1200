using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Data.Metrics
{
	internal abstract class Event : ISaveable
	{
		public static readonly string TIMESTAMP = "timestamp";

		public static readonly string IN_PROGRESS = "inProgress";

		protected EventType metricType;

		protected string metricName;

		protected string metricCode;

		protected Dictionary<string, object> parameters;

		public EventType MetricType
		{
			get
			{
				return metricType;
			}
			set
			{
				metricType = value;
			}
		}

		public string MetricName
		{
			get
			{
				return metricName;
			}
			set
			{
				metricName = value;
			}
		}

		public string MetricCode
		{
			get
			{
				return metricCode;
			}
			set
			{
				metricCode = value;
			}
		}

		public Dictionary<string, object> Parameters
		{
			get
			{
				return parameters;
			}
			set
			{
				parameters = value;
			}
		}

		public Event()
		{
		}

		protected Event(EventType type)
		{
			metricType = type;
			metricName = EventConst.EventsInfo[type].Key;
			metricCode = EventConst.EventsInfo[type].Value;
			parameters = new Dictionary<string, object>();
			parameters.Add(TIMESTAMP, DeviceHelper.Instance.GetUnixTime() / 1000);
		}

		public Event(ObjectInfo info)
		{
			try
			{
				metricType = (EventType)(int)info.GetValue("metricType", typeof(int));
				metricName = info.GetValue("metricName", typeof(string)) as string;
				metricCode = info.GetValue("metricCode", typeof(string)) as string;
				parameters = info.GetValue("parameters", typeof(Dictionary<string, object>)) as Dictionary<string, object>;
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
				info.AddValue("metricType", (int)metricType);
				info.AddValue("metricName", metricName);
				info.AddValue("metricCode", metricCode);
				info.AddValue("parameters", parameters);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public abstract JSONNode GetAdditionalDataJson();

		protected bool IsMetricTypeEqual(Event other)
		{
			if (metricCode != other.metricCode)
			{
				return false;
			}
			return other.GetType().Equals(GetType());
		}

		protected bool IsParameterEqual(Event other, string paramName)
		{
			if (!IsMetricTypeEqual(other))
			{
				return false;
			}
			if (!parameters.ContainsKey(paramName))
			{
				return false;
			}
			if (!parameters.ContainsKey(paramName))
			{
				return false;
			}
			return parameters[paramName].Equals(other.parameters[paramName]);
		}

		public virtual void AddPendingEvents(List<string> events)
		{
			if (!parameters.ContainsKey(IN_PROGRESS))
			{
				parameters.Add(IN_PROGRESS, events);
			}
			else
			{
				parameters[IN_PROGRESS] = events;
			}
		}

		public void AddPendingToJSON(JSONNode json)
		{
			JSONArray pendingEventsJson = GetPendingEventsJson();
			if (pendingEventsJson != null && json[IN_PROGRESS] == null)
			{
				json.Add(IN_PROGRESS, pendingEventsJson);
			}
		}

		public JSONArray GetPendingEventsJson()
		{
			if (parameters.ContainsKey(IN_PROGRESS))
			{
				List<string> list = parameters[IN_PROGRESS] as List<string>;
				if (list != null)
				{
					JSONArray jSONArray = new JSONArray();
					{
						foreach (string item in list)
						{
							jSONArray.Add(Uri.EscapeDataString(item));
						}
						return jSONArray;
					}
				}
			}
			return null;
		}

		public virtual bool IsEqualToMetric(Event other)
		{
			return false;
		}
	}
}
