namespace DevToDev
{
	public sealed class SocialNetwork
	{
		private string name;

		private static readonly SocialNetwork _Vk = new SocialNetwork("vk");

		private static readonly SocialNetwork _Twitter = new SocialNetwork("tw");

		private static readonly SocialNetwork _Facebook = new SocialNetwork("fb");

		private static readonly SocialNetwork _GooglePlus = new SocialNetwork("gp");

		private static readonly SocialNetwork _WhatsApp = new SocialNetwork("wp");

		private static readonly SocialNetwork _Viber = new SocialNetwork("vb");

		private static readonly SocialNetwork _Evernote = new SocialNetwork("en");

		private static readonly SocialNetwork _GoogleMail = new SocialNetwork("gm");

		private static readonly SocialNetwork _LinkedIn = new SocialNetwork("in");

		private static readonly SocialNetwork _Pinterest = new SocialNetwork("pi");

		private static readonly SocialNetwork _Qzone = new SocialNetwork("qq");

		private static readonly SocialNetwork _Reddit = new SocialNetwork("rt");

		private static readonly SocialNetwork _Renren = new SocialNetwork("rr");

		private static readonly SocialNetwork _Tumblr = new SocialNetwork("tb");

		public static SocialNetwork Vk
		{
			get
			{
				return _Vk;
			}
		}

		public static SocialNetwork Twitter
		{
			get
			{
				return _Twitter;
			}
		}

		public static SocialNetwork Facebook
		{
			get
			{
				return _Facebook;
			}
		}

		public static SocialNetwork GooglePlus
		{
			get
			{
				return _GooglePlus;
			}
		}

		public static SocialNetwork WhatsApp
		{
			get
			{
				return _WhatsApp;
			}
		}

		public static SocialNetwork Viber
		{
			get
			{
				return _Viber;
			}
		}

		public static SocialNetwork Evernote
		{
			get
			{
				return _Evernote;
			}
		}

		public static SocialNetwork GoogleMail
		{
			get
			{
				return _GoogleMail;
			}
		}

		public static SocialNetwork LinkedIn
		{
			get
			{
				return _LinkedIn;
			}
		}

		public static SocialNetwork Pinterest
		{
			get
			{
				return _Pinterest;
			}
		}

		public static SocialNetwork Qzone
		{
			get
			{
				return _Qzone;
			}
		}

		public static SocialNetwork Reddit
		{
			get
			{
				return _Reddit;
			}
		}

		public static SocialNetwork Renren
		{
			get
			{
				return _Renren;
			}
		}

		public static SocialNetwork Tumblr
		{
			get
			{
				return _Tumblr;
			}
		}

		private SocialNetwork(string name)
		{
			this.name = name;
		}

		public override string ToString()
		{
			return name;
		}

		public static SocialNetwork Custom(string network)
		{
			return new SocialNetwork(network);
		}
	}
}
