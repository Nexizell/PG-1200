using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Logic;

namespace DevToDev.Data.Metrics.Simple
{
	internal class GetServerNodeEvent : SimpleEvent
	{
		private static readonly string SDK_VERSION = "sdkVersion";

		private static readonly string APP_VERSION = "appVersion";

		private static readonly string CONFIG_VERSION = "configVersion";

		public GetServerNodeEvent(ObjectInfo info)
			: base(info)
		{
		}

		public GetServerNodeEvent()
			: base(EventType.GetServerNode)
		{
			parameters.Remove(Event.TIMESTAMP);
			parameters.Add(SDK_VERSION, Analytics.SDKVersion);
			parameters.Add(APP_VERSION, ApplicationHelper.GetAppVersion());
			string configVersion = SDKClient.Instance.NetworkStorage.ConfigVersion;
			if (configVersion != null)
			{
				parameters.Add(CONFIG_VERSION, configVersion);
			}
		}
	}
}
