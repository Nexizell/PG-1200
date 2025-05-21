using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev.Data.Metrics.Aggregated.CustomEvent
{
	internal sealed class CustomEvent : AggregatedEvent
	{
		private static readonly string NAME = "name";

		private static readonly string ENTRIES = "entries";

		private static readonly string START_EVENT = "t1";

		private static readonly string END_EVENT = "t2";

		private static readonly string DURATION = "d";

		private static readonly string PARAMS_ENUMERATION = "p";

		private static readonly string DOUBLE = "double";

		private static readonly string STRING = "string";

		private static readonly string DATE = "date";

		private CustomEventData customEventData;

		public Dictionary<string, List<CustomEventData>> CustomEventsData { get; set; }

		public Dictionary<string, CustomEventData> NotFinishedEvents { get; set; }

		public CustomEvent()
		{
		}

		public CustomEvent(ObjectInfo info)
			: base(info)
		{
			try
			{
				CustomEventsData = info.GetValue("CustomEventsData", typeof(Dictionary<string, List<CustomEventData>>)) as Dictionary<string, List<CustomEventData>>;
				NotFinishedEvents = info.GetValue("NotFinishedEvents", typeof(Dictionary<string, CustomEventData>)) as Dictionary<string, CustomEventData>;
				customEventData = info.GetValue("customEventData", typeof(CustomEventData)) as CustomEventData;
			}
			catch (Exception ex)
			{
				Log.D("Error in desealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public override void GetObjectData(ObjectInfo info)
		{
			base.GetObjectData(info);
			try
			{
				info.AddValue("CustomEventsData", CustomEventsData);
				info.AddValue("NotFinishedEvents", NotFinishedEvents);
				info.AddValue("customEventData", customEventData);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		public CustomEvent(CustomEventData customEventData)
			: base(EventType.CustomEvent)
		{
			this.customEventData = customEventData;
			SelfAggregate();
		}

		public override JSONNode GetAdditionalDataJson()
		{
			JSONArray jSONArray = new JSONArray();
			foreach (KeyValuePair<string, List<CustomEventData>> customEventsDatum in CustomEventsData)
			{
				if (customEventsDatum.Key == null || customEventsDatum.Value == null)
				{
					continue;
				}
				JSONClass jSONClass = new JSONClass();
				jSONArray.Add(jSONClass);
				jSONClass.Add(NAME, Uri.EscapeDataString(customEventsDatum.Key));
				JSONArray jSONArray2 = new JSONArray();
				List<CustomEventData> value = customEventsDatum.Value;
				jSONClass.Add(ENTRIES, jSONArray2);
				foreach (CustomEventData item in value)
				{
					if (item == null)
					{
						continue;
					}
					JSONClass jSONClass2 = new JSONClass();
					jSONArray2.Add(jSONClass2);
					AddPendingToJSON(jSONClass2, item.PendingData);
					jSONClass2.Add(START_EVENT, new JSONData(item.StartTime));
					if (!item.HasParams || item.Params == null)
					{
						continue;
					}
					JSONClass jSONClass3 = new JSONClass();
					jSONClass2.Add(PARAMS_ENUMERATION, jSONClass3);
					JSONClass jSONClass4 = new JSONClass();
					jSONClass3.Add(START_EVENT, jSONClass4);
					if (item.Params.HasNumeric)
					{
						JSONClass jSONClass5 = new JSONClass();
						jSONClass4.Add(DOUBLE, jSONClass5);
						foreach (KeyValuePair<string, double> doubleParam in item.Params.DoubleParams)
						{
							jSONClass5.Add(Uri.EscapeDataString(doubleParam.Key), new JSONData(doubleParam.Value));
						}
						foreach (KeyValuePair<string, long> longParam in item.Params.LongParams)
						{
							jSONClass5.Add(Uri.EscapeDataString(longParam.Key), new JSONData(longParam.Value));
						}
						foreach (KeyValuePair<string, int> intParam in item.Params.IntParams)
						{
							jSONClass5.Add(Uri.EscapeDataString(intParam.Key), new JSONData(intParam.Value));
						}
					}
					if (!item.Params.HasStrings)
					{
						continue;
					}
					JSONClass jSONClass6 = new JSONClass();
					jSONClass4.Add(STRING, jSONClass6);
					foreach (KeyValuePair<string, string> stringParam in item.Params.StringParams)
					{
						jSONClass6.Add(Uri.EscapeDataString(stringParam.Key), new JSONData(Uri.EscapeDataString(stringParam.Value)));
					}
				}
			}
			return jSONArray;
		}

		public override void AddEvent(AggregatedEvent metric)
		{
			foreach (KeyValuePair<string, List<CustomEventData>> customEventsDatum in (metric as CustomEvent).CustomEventsData)
			{
				if (!CustomEventsData.ContainsKey(customEventsDatum.Key))
				{
					continue;
				}
				List<CustomEventData> list = CustomEventsData[customEventsDatum.Key];
				foreach (CustomEventData a in customEventsDatum.Value)
				{
					Predicate<CustomEventData> match = (CustomEventData x) => x.CreationTimestamp == a.CreationTimestamp;
					CustomEventData customEventData = list.Find(match);
					if (customEventData != null)
					{
						list.Remove(customEventData);
					}
				}
			}
			if (metric != null)
			{
				AddSingleData(metric as CustomEvent);
			}
		}

		protected override void SelfAggregate()
		{
			CustomEventsData = new Dictionary<string, List<CustomEventData>>();
			NotFinishedEvents = new Dictionary<string, CustomEventData>();
			if (customEventData != null)
			{
				if (customEventData.Type != 0)
				{
					NotFinishedEvents.Add(customEventData.EventId, customEventData);
					return;
				}
				CustomEventsData.Add(customEventData.EventName, new List<CustomEventData> { customEventData });
			}
		}

		public override void AddPendingEvents(List<string> events)
		{
			foreach (KeyValuePair<string, List<CustomEventData>> customEventsDatum in CustomEventsData)
			{
				foreach (CustomEventData item in customEventsDatum.Value)
				{
					item.PendingData = events;
				}
			}
		}

		public override bool IsReadyToSend()
		{
			return CustomEventsData.Count > 0;
		}

		public override bool IsNeedToClear()
		{
			return NotFinishedEvents.Count == 0;
		}

		private void AddSingleData(CustomEvent ce)
		{
			Dictionary<string, List<CustomEventData>> customEventsData = ce.CustomEventsData;
			foreach (KeyValuePair<string, List<CustomEventData>> item in customEventsData)
			{
				if (CustomEventsData.ContainsKey(item.Key))
				{
					CustomEventsData[item.Key].AddRange(item.Value);
				}
				else
				{
					CustomEventsData.Add(item.Key, item.Value);
				}
			}
		}

		private void FinishEvent(CustomEventData complete)
		{
			if (CustomEventsData.ContainsKey(complete.EventName))
			{
				CustomEventsData[complete.EventName].Add(complete);
			}
			else
			{
				CustomEventsData.Add(complete.EventName, new List<CustomEventData> { complete });
			}
			NotFinishedEvents.Remove(complete.EventId);
		}

		public override bool IsEqualToMetric(Event other)
		{
			if (!IsMetricTypeEqual(other))
			{
				return false;
			}
			CustomEvent customEvent = other as CustomEvent;
			if (customEventData == null || customEvent.customEventData == null)
			{
				return false;
			}
			return customEventData.EventName.Equals(customEvent.customEventData.EventName);
		}

		public override void RemoveSentMetrics()
		{
		}
	}
}
