using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;

namespace DevToDev.Data.Metrics.Simple
{
	internal class GameSessionEvent : SimpleEvent
	{
		private static readonly string LENGTH = "length";

		public GameSessionEvent()
		{
		}

		public GameSessionEvent(ObjectInfo info)
			: base(info)
		{
		}

		public GameSessionEvent(long startTime, long endTime)
			: base(EventType.GameSession)
		{
			parameters.Add(LENGTH, endTime - startTime);
			if (parameters.ContainsKey(Event.TIMESTAMP))
			{
				parameters.Remove(Event.TIMESTAMP);
			}
			parameters.Add(Event.TIMESTAMP, startTime);
		}
	}
}
