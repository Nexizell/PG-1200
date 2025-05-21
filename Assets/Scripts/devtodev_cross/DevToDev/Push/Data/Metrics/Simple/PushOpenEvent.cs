using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils;
using DevToDev.Data.Metrics;
using DevToDev.Data.Metrics.Simple;

namespace DevToDev.Push.Data.Metrics.Simple
{
	internal class PushOpenEvent : SimpleEvent
	{
		public static readonly string PUSH_ID_KEY = "pushId";

		public PushOpenEvent()
		{
		}

		public PushOpenEvent(int pushId)
			: base(EventType.PushOpen)
		{
			parameters.Add(PUSH_ID_KEY, pushId);
		}

		public PushOpenEvent(ObjectInfo info)
			: base(info)
		{
		}

		public static PushOpenEvent CreateFromJSON(JSONNode elem)
		{
			if (elem[PUSH_ID_KEY] == null)
			{
				return null;
			}
			PushOpenEvent pushOpenEvent = new PushOpenEvent(elem[PUSH_ID_KEY].AsInt);
			if (elem[Event.TIMESTAMP] != null)
			{
				pushOpenEvent.parameters.Remove(Event.TIMESTAMP);
				pushOpenEvent.parameters.Add(Event.TIMESTAMP, elem[Event.TIMESTAMP].AsInt);
			}
			return pushOpenEvent;
		}
	}
}
