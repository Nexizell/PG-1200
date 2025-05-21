using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;

namespace DevToDev.Data.Metrics.Simple
{
	internal class SocialNetworkPostEvent : SimpleEvent
	{
		private static readonly string SOCIAL_NETWORK = "socialNetwork";

		private static readonly string POST_REASON = "postReason";

		public SocialNetworkPostEvent()
		{
		}

		public SocialNetworkPostEvent(ObjectInfo info)
			: base(info)
		{
		}

		public SocialNetworkPostEvent(SocialNetwork network, string reason)
			: base(EventType.SocialNetworkPost)
		{
			parameters.Add(SOCIAL_NETWORK, network.ToString());
			parameters.Add(POST_REASON, reason);
		}

		public override bool IsEqualToMetric(Event other)
		{
			return IsParameterEqual(other, SOCIAL_NETWORK);
		}
	}
}
