using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Core.Utils.Helpers;

namespace DevToDev.Data.Metrics.Aggregated.Progression
{
	internal class ProgressionEvent : AggregatedEvent
	{
		protected static readonly string ID = "id";

		private Dictionary<string, ProgressionEventParams> events;

		internal Dictionary<string, List<ProgressionEventParams>> CompletedEvents { get; set; }

		private Dictionary<string, ProgressionEventParams> Events
		{
			get
			{
				if (events == null)
				{
					events = new Dictionary<string, ProgressionEventParams>();
				}
				return events;
			}
		}

		public ProgressionEvent(ProgressionEvent metric)
			: base(EventType.ProgressionEvent)
		{
			CompletedEvents = new Dictionary<string, List<ProgressionEventParams>>();
			foreach (KeyValuePair<string, ProgressionEventParams> @event in metric.Events)
			{
				Events.Add(@event.Key, @event.Value.Clone());
			}
			foreach (KeyValuePair<string, List<ProgressionEventParams>> completedEvent in metric.CompletedEvents)
			{
				CompletedEvents.Add(completedEvent.Key, new List<ProgressionEventParams>());
				foreach (ProgressionEventParams item in completedEvent.Value)
				{
					CompletedEvents[completedEvent.Key].Add(item.Clone());
				}
			}
			SelfAggregate();
		}

		public ProgressionEvent(ProgressionEventParams eventData)
			: base(EventType.ProgressionEvent)
		{
			CompletedEvents = new Dictionary<string, List<ProgressionEventParams>>();
			Events.Add(eventData.GetEventName(), eventData);
			if (eventData.GetFinishTime() == 0)
			{
				Log.D("Progression event {0} is started.", eventData.GetEventName());
			}
			SelfAggregate();
		}

		public ProgressionEvent(ObjectInfo info)
			: base(info)
		{
			try
			{
				CompletedEvents = info.GetValue("CompletedEvents", typeof(Dictionary<string, List<ProgressionEventParams>>)) as Dictionary<string, List<ProgressionEventParams>>;
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
				info.AddValue("CompletedEvents", CompletedEvents);
			}
			catch (Exception ex)
			{
				Log.D("Error in sealization: " + ex.Message + "\n" + ex.StackTrace);
			}
		}

		private void AddOrMergeParams(string paramName, ProgressionEventParams destinationParam, ProgressionEventParams sourceParam)
		{
			if (destinationParam.GetFinishTime() == 0 && sourceParam.GetFinishTime() != 0)
			{
				destinationParam.Merge(sourceParam);
				Events.Remove(paramName);
				AddFinishedEvent(paramName, destinationParam);
				return;
			}
			if (destinationParam.GetFinishTime() == 0)
			{
				Events[paramName] = sourceParam;
				return;
			}
			Events.Remove(paramName);
			if (sourceParam.GetFinishTime() != 0)
			{
				AddFinishedEvent(paramName, sourceParam);
			}
			else
			{
				Events[paramName] = sourceParam;
			}
		}

		public override void AddEvent(AggregatedEvent metric)
		{
			ProgressionEvent progressionEvent = metric as ProgressionEvent;
			foreach (KeyValuePair<string, List<ProgressionEventParams>> completedEvent in progressionEvent.CompletedEvents)
			{
				CompletedEvents.Add(completedEvent.Key, completedEvent.Value);
			}
			foreach (KeyValuePair<string, ProgressionEventParams> @event in progressionEvent.Events)
			{
				if (Events.ContainsKey(@event.Key))
				{
					ProgressionEventParams progressionEventParams = Events[@event.Key];
					if (!object.Equals(progressionEventParams.GetType(), @event.Value.GetType()))
					{
						Events[@event.Key] = @event.Value;
					}
					else
					{
						AddOrMergeParams(@event.Key, progressionEventParams, @event.Value);
					}
				}
				else if (@event.Value.GetFinishTime() == 0)
				{
					Dictionary<string, ProgressionEventParams> dictionary = new Dictionary<string, ProgressionEventParams>();
					foreach (KeyValuePair<string, ProgressionEventParams> event2 in Events)
					{
						if (!object.Equals(event2.Value.GetType(), @event.Value.GetType()))
						{
							dictionary.Add(event2.Key, event2.Value);
						}
					}
					events = dictionary;
					Events.Add(@event.Key, @event.Value);
				}
				else
				{
					AddFinishedEvent(@event.Key, @event.Value);
				}
			}
		}

		private void AddFinishedEvent(string name, ProgressionEventParams eventParam)
		{
			if (eventParam.IsCorrect())
			{
				if (!CompletedEvents.ContainsKey(name))
				{
					CompletedEvents.Add(name, new List<ProgressionEventParams>());
				}
				CompletedEvents[name].Add(eventParam);
				Log.R("Progression event {0} is finished.", name);
				Log.R("Metric {0} added to storage", base.MetricName);
			}
			else
			{
				Log.R("Progression event {0} could not be finished. There is no active progression event {0}.", name);
			}
		}

		public override JSONNode GetAdditionalDataJson()
		{
			if (!IsReadyToSend())
			{
				return null;
			}
			JSONArray jSONArray = new JSONArray();
			foreach (KeyValuePair<string, List<ProgressionEventParams>> completedEvent in CompletedEvents)
			{
				if (completedEvent.Key == null || completedEvent.Value == null)
				{
					continue;
				}
				foreach (ProgressionEventParams item in completedEvent.Value)
				{
					JSONNode jSONNode = item.ToJson();
					jSONNode.Add(ID, Uri.EscapeDataString(completedEvent.Key));
					jSONNode.Add(Event.TIMESTAMP, new JSONData(DeviceHelper.Instance.GetUnixTime() / 1000));
					jSONArray.Add(jSONNode);
				}
			}
			return jSONArray;
		}

		public List<string> GetPendingEvents()
		{
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, ProgressionEventParams> @event in Events)
			{
				if (@event.Value.GetFinishTime() == 0)
				{
					list.Add(@event.Value.EventName);
				}
			}
			if (list.Count > 0)
			{
				return list;
			}
			return null;
		}

		public override bool IsNeedToClear()
		{
			return Events.Count == 0;
		}

		public override bool IsReadyToSend()
		{
			return CompletedEvents.Count > 0;
		}

		protected override void SelfAggregate()
		{
		}

		public override void RemoveSentMetrics()
		{
			CompletedEvents.Clear();
		}

		public void ClearUnfinishedEvents()
		{
			Events.Clear();
		}
	}
}
