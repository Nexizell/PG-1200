namespace com.amazon.mas.cpt.ads
{
	public class AmazonLogger
	{
		private readonly string tag;

		public AmazonLogger(string tag)
		{
			this.tag = tag;
		}

		public void Debug(string msg)
		{
		}

		public string getTag()
		{
			return tag;
		}
	}
}
