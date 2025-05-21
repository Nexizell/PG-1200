namespace FyberPlugin
{
	public sealed class AdResult
	{
		public AdFormat AdFormat
		{
			get
			{
				return AdFormat.OFFER_WALL;
			}
		}

		public string Message
		{
			get
			{
				return string.Empty;
			}
		}

		public AdStatus Status
		{
			get
			{
				return AdStatus.OK;
			}
		}
	}
}
