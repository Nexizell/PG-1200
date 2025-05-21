using UnityEngine;

namespace FyberPlugin
{
	internal class PluginBridgeComponent : IPluginBridge
	{
		public void StartSDK(string json)
		{
			Debug.Log("Version - " + Application.unityVersion);
			Utils.printWarningMessage();
		}

		public void Cache(string action)
		{
			Utils.printWarningMessage();
		}

		public void Request(string json)
		{
			Utils.printWarningMessage();
		}

		public void StartAd(string json)
		{
			Utils.printWarningMessage();
		}

		public bool Banner(string json)
		{
			Utils.printWarningMessage();
			return false;
		}

		public void Report(string json)
		{
			Utils.printWarningMessage();
		}

		public string Settings(string json)
		{
			Utils.printWarningMessage();
			return "no yet implemented";
		}

		public void EnableLogging(bool shouldLog)
		{
			Utils.printWarningMessage();
		}

		public void GameObjectStarted()
		{
			Utils.printWarningMessage();
		}

		public void ApplicationQuit()
		{
			Utils.printWarningMessage();
		}

		public void ShowTestSuite()
		{
			Utils.printWarningMessage();
		}

		public void AnalyzeTestSuite()
		{
			Utils.printWarningMessage();
		}
	}
}
