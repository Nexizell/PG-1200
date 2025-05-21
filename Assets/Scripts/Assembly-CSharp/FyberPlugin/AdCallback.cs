namespace FyberPlugin
{
	public interface AdCallback
	{
		void OnAdFinished(AdResult result);

		void OnAdStarted(Ad ad);
	}
}
