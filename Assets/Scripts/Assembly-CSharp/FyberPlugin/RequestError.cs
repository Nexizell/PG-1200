namespace FyberPlugin
{
	public sealed class RequestError
	{
		private static readonly RequestError Empty = new RequestError
		{
			Description = string.Empty
		};

		public static RequestError CONNECTION_ERROR
		{
			get
			{
				return Empty;
			}
		}

		public static RequestError DEVICE_NOT_SUPPORTED
		{
			get
			{
				return Empty;
			}
		}

		public static RequestError ERROR_REQUESTING_ADS
		{
			get
			{
				return Empty;
			}
		}

		public static RequestError NULL_CONTEXT_REFERENCE
		{
			get
			{
				return Empty;
			}
		}

		public static RequestError SDK_NOT_STARTED
		{
			get
			{
				return Empty;
			}
		}

		public static RequestError SECURITY_TOKEN_NOT_PROVIDED
		{
			get
			{
				return Empty;
			}
		}

		public static RequestError UNABLE_TO_REQUEST_ADS
		{
			get
			{
				return Empty;
			}
		}

		public static RequestError UNKNOWN_ERROR
		{
			get
			{
				return Empty;
			}
		}

		public string Description { get; set; }
	}
}
