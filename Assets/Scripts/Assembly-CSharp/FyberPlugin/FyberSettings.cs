using UnityEngine;

namespace FyberPlugin
{
	public class FyberSettings : ScriptableObject
	{
		private const string fyberSettingsAssetName = "FyberSettings";

		private const string fyberSettingsPath = "Fyber/Resources";

		private const string fyberSettingsAssetExtension = ".asset";

		private static FyberSettings instance;

		[HideInInspector]
		[SerializeField]
		protected internal string bundlesJson;

		[SerializeField]
		[HideInInspector]
		protected internal string configJson;

		[SerializeField]
		[HideInInspector]
		protected internal int bundlesCount;

		public static FyberSettings Instance
		{
			get
			{
				return GetInstance();
			}
		}

		private void OnEnable()
		{
			GetInstance();
		}

		private static FyberSettings GetInstance()
		{
			if (instance == null)
			{
				PluginBridge.bridge = new PluginBridgeComponent();
				instance = Resources.Load("FyberSettings") as FyberSettings;
				if (instance == null)
				{
					instance = ScriptableObject.CreateInstance<FyberSettings>();
				}
			}
			return instance;
		}

		internal string BundlesInfoJson()
		{
			return bundlesJson;
		}

		internal string BundlesConfigJson()
		{
			return configJson;
		}

		internal int BundlesCount()
		{
			return bundlesCount;
		}
	}
}
