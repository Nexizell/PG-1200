using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;

namespace DevToDev.Data.Metrics.Simple
{
	internal class GenderEvent : SimpleEvent
	{
		private static readonly string GENDER = "gender";

		public GenderEvent()
		{
		}

		public GenderEvent(ObjectInfo info)
			: base(info)
		{
		}

		public GenderEvent(Gender gender)
			: base(EventType.Gender)
		{
			parameters.Remove(Event.TIMESTAMP);
			parameters.Add(GENDER, (int)gender);
		}
	}
}
