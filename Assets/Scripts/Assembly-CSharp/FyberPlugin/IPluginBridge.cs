namespace FyberPlugin
{
	public interface IPluginBridge
	{
		void Cache(string action);

		void EnableLogging(bool shouldLog);

		void Report(string json);

		void Request(string json);

		string Settings(string json);

		void StartAd(string json);

		void StartSDK(string json);
	}
}
