using System.Threading.Tasks;
using UnityEngine;

namespace Rilisoft
{
	internal abstract class AmazonCloudSlotSynchronizer : CloudSlotSynchronizer
	{
		public override string CurrentResult
		{
			get
			{
				return CurrentResultCore;
			}
		}

		public override bool Pulled
		{
			get
			{
				return PulledCore;
			}
		}

		protected string CurrentResultCore { get; set; }

		protected bool PulledCore { get; set; }

		public AmazonCloudSlotSynchronizer()
		{
		}

		public override Task<CloudPullResult> Pull(bool silent = true)
		{
			Task<CloudPullResult> result = Task.FromResult<CloudPullResult>(CloudPullResult.Failed);
			string text = string.Concat(GetType(), ".Pull()");
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				Debug.LogWarningFormat("Skipping `{0}` on {1}", text, BuildSettings.BuildTargetPlatform);
				return result;
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				Debug.LogWarningFormat("Skipping `{0}` on {1}", text, Defs.AndroidEdition);
				return result;
			}
			if (Application.isEditor)
			{
				Debug.LogWarningFormat("Skipping `{0}` in Editor", text);
				return result;
			}
			if (!AGSClient.IsServiceReady())
			{
				Debug.LogWarningFormat("Skipping `{0}`: Amazon service is not ready", text);
				return result;
			}
			if (GameCircleSocial.Instance == null)
			{
				Debug.LogWarningFormat("Skipping `{0}`: GameCircleSocial.Instance == null", text);
				return result;
			}
			if (GameCircleSocial.Instance.localUser == null)
			{
				Debug.LogWarningFormat("Skipping `{0}`: GameCircleSocial.Instance.localUser == null", text);
				return result;
			}
			if (!IsAuthenticated() && !silent)
			{
				AGSClient.ShowSignInPage();
			}
			if (!IsAuthenticated())
			{
				Debug.LogWarningFormat("Skipping `{0}`: Not authenticated, silent: {1}", text, silent);
				return Task.FromResult<CloudPullResult>(CloudPullResult.LoginFailed);
			}
			ScopeLogger scopeLogger = new ScopeLogger(text, Defs.IsDeveloperBuild);
			try
			{
				return result;
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		public override void Push(string data)
		{
			string text = string.Concat(GetType(), ".Push()");
			if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
			{
				Debug.LogWarningFormat("Skipping `{0}` on {1}", text, BuildSettings.BuildTargetPlatform);
				return;
			}
			if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
			{
				Debug.LogWarningFormat("Skipping `{0}` on {1}", text, Defs.AndroidEdition);
				return;
			}
			if (Application.isEditor)
			{
				Debug.LogWarningFormat("Skipping `{0}` in Editor", text);
				return;
			}
			if (!AGSClient.IsServiceReady())
			{
				Debug.LogWarningFormat("Skipping `{0}`: Amazon service is not ready", text);
				return;
			}
			ScopeLogger scopeLogger = new ScopeLogger(text, Defs.IsDeveloperBuild);
			try
			{
			}
			finally
			{
				scopeLogger.Dispose();
			}
		}

		protected abstract Task<CloudPullResult> PullCore(AGSGameDataMap dataMap);

		protected abstract void PushCore(AGSGameDataMap dataMap, string data);

		protected bool IsAuthenticated()
		{
			if (GameCircleSocial.Instance == null)
			{
				return false;
			}
			if (GameCircleSocial.Instance.localUser == null)
			{
				return false;
			}
			return GameCircleSocial.Instance.localUser.authenticated;
		}
	}
}
