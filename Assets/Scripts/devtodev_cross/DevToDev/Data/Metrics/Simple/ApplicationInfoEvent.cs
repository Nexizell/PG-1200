using DevToDev.Core.Data.Consts;
using DevToDev.Core.Serialization;
using DevToDev.Core.Utils.Helpers;
using DevToDev.Logic;

namespace DevToDev.Data.Metrics.Simple
{
	internal class ApplicationInfoEvent : SimpleEvent
	{
		private static readonly string SDK_VERSION = "sdkVersion";

		private static readonly string APP_VERSION = "appVersion";

		private static readonly string CODE_VERSION = "codeVersion";

		private static readonly string BUNDLE_ID = "bundleId";

		private static readonly string ENGINE = "engine";

		private static readonly string INSTALL_SOURCE = "installationSource";

		private static readonly string UNITY = "Unity";

		public ApplicationInfoEvent()
			: base(EventType.ApplicationInfo)
		{
			parameters.Remove(Event.TIMESTAMP);
			addParameterIfNotNull(SDK_VERSION, Analytics.SDKVersion);
			addParameterIfNotNull(ENGINE, UNITY);
			string appVersion = ApplicationHelper.GetAppVersion();
			if (appVersion != null && !appVersion.Contains("Unity Player"))
			{
				addParameterIfNotNull(APP_VERSION, appVersion);
			}
			else
			{
				addParameterIfNotNull(APP_VERSION, SDKClient.Instance.ApplicationVersion);
			}
			string appCodeVersion = ApplicationHelper.GetAppCodeVersion();
			int result = 0;
			if (int.TryParse(appCodeVersion, out result))
			{
				addParameterIfNotNull(CODE_VERSION, result);
			}
			addParameterIfNotNull(BUNDLE_ID, ApplicationHelper.GetAppBundle());
			addParameterIfNotNull(INSTALL_SOURCE, ApplicationHelper.GetInstallSource());
		}

		public ApplicationInfoEvent(ObjectInfo info)
			: base(info)
		{
		}
	}
}
