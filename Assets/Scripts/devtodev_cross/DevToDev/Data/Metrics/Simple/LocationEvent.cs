using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;

namespace DevToDev.Data.Metrics.Simple
{
	internal class LocationEvent : SimpleEvent
	{
		private static readonly string LATITUDE = "lt";

		private static readonly string LONGITUDE = "lg";

		public LocationEvent()
		{
		}

		public LocationEvent(ObjectInfo info)
			: base(info)
		{
		}

		public LocationEvent(float latitude, float longitude)
			: base(EventType.Location)
		{
			parameters.Add(LATITUDE, latitude);
			parameters.Add(LONGITUDE, longitude);
		}
	}
}
