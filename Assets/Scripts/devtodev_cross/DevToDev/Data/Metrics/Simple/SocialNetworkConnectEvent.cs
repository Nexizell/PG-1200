using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;

namespace DevToDev.Data.Metrics.Simple
{
	internal class SocialNetworkConnectEvent : SimpleEvent
	{
		private static readonly string SOCIAL_NETWORK = "socialNetwork";

		public SocialNetworkConnectEvent()
		{
		}

		public SocialNetworkConnectEvent(ObjectInfo info)
			: base(info)
		{
		}

		public SocialNetworkConnectEvent(SocialNetwork network)
			: base(EventType.SocialNetworkConnect)
		{
			parameters.Add(SOCIAL_NETWORK, network.ToString());
		}

		public override bool IsEqualToMetric(Event other)
		{
			return IsParameterEqual(other, SOCIAL_NETWORK);
		}
	}
}
