namespace DevToDev.Core.Utils.Helpers
{
	internal static class ApplicationHelper
	{
		public static string GetAppVersion()
		{
			return ApplicationHelperPlatform.GetAppVersion();
		}

		public static string GetAppCodeVersion()
		{
			return ApplicationHelperPlatform.GetAppCodeVersion();
		}

		public static string GetAppBundle()
		{
			return ApplicationHelperPlatform.GetAppBundle();
		}

		public static object GetInstallSource()
		{
			return ApplicationHelperPlatform.GetInstallSource();
		}
	}
}
