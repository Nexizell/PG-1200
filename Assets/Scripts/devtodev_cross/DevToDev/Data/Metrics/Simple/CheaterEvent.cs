using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;

namespace DevToDev.Data.Metrics.Simple
{
	internal class CheaterEvent : SimpleEvent
	{
		private static readonly string CHEATER = "cheater";

		public CheaterEvent()
		{
		}

		public CheaterEvent(bool isCheater)
			: base(EventType.Cheater)
		{
			parameters.Remove(Event.TIMESTAMP);
			parameters.Add(CHEATER, isCheater ? 1 : 0);
		}

		public CheaterEvent(ObjectInfo info)
			: base(info)
		{
		}
	}
}
