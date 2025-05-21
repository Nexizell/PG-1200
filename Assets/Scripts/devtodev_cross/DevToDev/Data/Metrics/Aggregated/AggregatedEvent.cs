using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;

namespace DevToDev.Data.Metrics.Aggregated
{
	internal abstract class AggregatedEvent : Event
	{
		public AggregatedEvent()
		{
		}

		protected AggregatedEvent(EventType type)
			: base(type)
		{
		}

		public AggregatedEvent(ObjectInfo info)
			: base(info)
		{
		}

		public abstract void AddEvent(AggregatedEvent metric);

		protected abstract void SelfAggregate();

		public abstract bool IsReadyToSend();

		public abstract bool IsNeedToClear();

		public abstract void RemoveSentMetrics();

		public void AddPendingToJSON(JSONNode json, List<string> pending)
		{
			if (pending == null || pending.Count <= 0)
			{
				return;
			}
			JSONArray jSONArray = new JSONArray();
			foreach (string item in pending)
			{
				jSONArray.Add(Uri.EscapeDataString(item));
			}
			if (json[Event.IN_PROGRESS] == null)
			{
				json.Add(Event.IN_PROGRESS, jSONArray);
			}
		}
	}
}
