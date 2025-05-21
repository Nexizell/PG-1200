namespace DevToDev
{
	public sealed class ReferralProperty
	{
		private string name;

		private static readonly ReferralProperty _publisher = new ReferralProperty("publisher");

		private static readonly ReferralProperty _subpublisher = new ReferralProperty("subpublisher");

		private static readonly ReferralProperty _subad = new ReferralProperty("subad");

		private static readonly ReferralProperty _subcampaign = new ReferralProperty("subcampaign");

		private static readonly ReferralProperty _subkeyword = new ReferralProperty("subkeyword");

		public static ReferralProperty Source
		{
			get
			{
				return _publisher;
			}
		}

		public static ReferralProperty Medium
		{
			get
			{
				return _subpublisher;
			}
		}

		public static ReferralProperty Content
		{
			get
			{
				return _subad;
			}
		}

		public static ReferralProperty Campaign
		{
			get
			{
				return _subcampaign;
			}
		}

		public static ReferralProperty Term
		{
			get
			{
				return _subkeyword;
			}
		}

		private ReferralProperty(string name)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return name;
		}

		public static ReferralProperty Custom(string propertyName)
		{
			return new ReferralProperty(propertyName);
		}
	}
}
