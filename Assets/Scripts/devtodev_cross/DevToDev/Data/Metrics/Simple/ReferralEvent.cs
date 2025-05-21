using System;
using System.Collections.Generic;
using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;

namespace DevToDev.Data.Metrics.Simple
{
	internal class ReferralEvent : SimpleEvent
	{
		public ReferralEvent()
		{
		}

		public ReferralEvent(ObjectInfo info)
			: base(info)
		{
		}

		public ReferralEvent(IDictionary<ReferralProperty, string> referralData)
			: base(EventType.Referral)
		{
			foreach (KeyValuePair<ReferralProperty, string> referralDatum in referralData)
			{
				addParameterIfNotNull(Uri.EscapeDataString(referralDatum.Key.ToString()), referralDatum.Value);
			}
		}
	}
}
