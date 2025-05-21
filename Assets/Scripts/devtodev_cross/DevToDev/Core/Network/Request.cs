namespace DevToDev.Core.Network
{
	internal class Request
	{
		private readonly string url;

		private readonly byte[] postData;

		public string Url
		{
			get
			{
				return url;
			}
		}

		public byte[] PostData
		{
			get
			{
				return postData;
			}
		}

		public string Method { get; set; }

		public Request(string url)
			: this(url, null)
		{
			Method = HttpMethod.GET;
		}

		public Request(string url, byte[] postData)
		{
			this.url = url;
			this.postData = postData;
			if (PostData != null)
			{
				Method = HttpMethod.POST;
			}
		}
	}
}
