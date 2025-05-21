namespace FyberPlugin
{
	public sealed class Ad
	{
		public AdFormat AdFormat
		{
			get
			{
				return AdFormat.OFFER_WALL;
			}
		}

		public string PlacementId
		{
			get
			{
				return string.Empty;
			}
		}

		public void Start()
		{
		}

		public Ad WithCallback(AdCallback callback)
		{
			return null;
		}
	}
}
