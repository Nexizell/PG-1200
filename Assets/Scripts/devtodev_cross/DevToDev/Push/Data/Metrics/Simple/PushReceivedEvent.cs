using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Simple;

namespace DevToDev.Push.Data.Metrics.Simple
{
	internal class PushReceivedEvent : SimpleEvent
	{
		public static readonly string PUSH_ID_KEY = "pushId";

		public PushReceivedEvent()
		{
		}

		public PushReceivedEvent(int pushId)
			: base(EventType.PushReceived)
		{
			parameters.Add(PUSH_ID_KEY, pushId);
		}

		public PushReceivedEvent(ObjectInfo info)
			: base(info)
		{
		}

		public static PushReceivedEvent CreateFromJSON(JSONNode elem)
		{
			if (elem[PUSH_ID_KEY] == null)
			{
				return null;
			}
			PushReceivedEvent pushReceivedEvent = new PushReceivedEvent(elem[PUSH_ID_KEY].AsInt);
			if (elem[Event.TIMESTAMP] != null)
			{
				pushReceivedEvent.parameters.Remove(Event.TIMESTAMP);
				pushReceivedEvent.parameters.Add(Event.TIMESTAMP, elem[Event.TIMESTAMP].AsInt);
			}
			return pushReceivedEvent;
		}
	}
}
