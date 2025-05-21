using System;
using System.Globalization;
using I2.Loc;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	public sealed class CheckpointsRewardedVideoController : MonoBehaviour
	{
		[SerializeField]
		protected internal GameObject _checkpointsRewardedVideoParent;

		private ReusableRewardedVideo _checkpointsRewardedVideo;

		private int _reward = FreeCheckpointsPointMemento.DefaultReward;

		private const string LastTimeShownKey = "Ads.FreeCheckpointsLastTimeShown";

		private const string PendingFreeCheckpointsKey = "Ads.PendingFreeCheckpoints";

		private static CheckpointsRewardedVideoController s_instance;

		internal static CheckpointsRewardedVideoController Instance
		{
			get
			{
				return s_instance;
			}
		}

		private GameObject CheckpointsRewardedVideoParent
		{
			get
			{
				if (_checkpointsRewardedVideoParent == null)
				{
					_checkpointsRewardedVideoParent = base.gameObject;
				}
				return _checkpointsRewardedVideoParent;
			}
		}

		private ReusableRewardedVideo CheckpointsRewardedVideo
		{
			get
			{
				if (_checkpointsRewardedVideo == null)
				{
					ReusableRewardedVideo reusableRewardedVideo = Resources.Load<ReusableRewardedVideo>("CheckpointsRewardedVideo");
					if (reusableRewardedVideo == null)
					{
						Debug.LogError("checkpointsRewardedVideoPrefab is null.");
						return null;
					}
					_checkpointsRewardedVideo = UnityEngine.Object.Instantiate(reusableRewardedVideo, CheckpointsRewardedVideoParent.transform);
					if (_checkpointsRewardedVideo == null)
					{
						Debug.LogError("_checkpointsRewardedVideo is null.");
						return null;
					}
					_checkpointsRewardedVideo.gameObject.SetLayerRecursively(CheckpointsRewardedVideoParent.layer);
					_checkpointsRewardedVideo.transform.localPosition = Vector3.zero;
					_checkpointsRewardedVideo.transform.localScale = Vector3.one;
					_checkpointsRewardedVideo.AdWatchedSuccessfully += OnAdWatchedSuccessfully;
					_checkpointsRewardedVideo.EnterWatching += OnEnterWatching;
					_checkpointsRewardedVideo.ExitWatching += OnExitWatching;
				}
				return _checkpointsRewardedVideo;
			}
		}

		internal bool AreFreeCheckpointsAvailable()
		{
			if (Tools.IsWsa && !Tools.IsEditor)
			{
				return false;
			}
			string reasonToDismissVideoFreeCheckpoints = GetReasonToDismissVideoFreeCheckpoints();
			if (!string.IsNullOrEmpty(reasonToDismissVideoFreeCheckpoints))
			{
				Debug.LogFormat("[Rilisoft] Dismissing rewarded video `Free Checkpoints`: {0}", reasonToDismissVideoFreeCheckpoints);
				return false;
			}
			int @int = Storager.getInt("FreeCheckpointsKey");
			if (@int > 0)
			{
				Debug.LogFormat("[Rilisoft] Dismissing rewarded video `Free Checkpoints`; freeCheckpointCount: {0}", @int);
				return false;
			}
			return true;
		}

		internal bool TryShowFreCheckpointsInterface(Action callback)
		{
			Debug.Log("OnTicketsRewardedVideoButton");
			DateTime? serverTime = FriendsController.GetServerTime();
			if (!serverTime.HasValue)
			{
				Debug.LogWarning("Server time not received.");
				return false;
			}
			AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
			if (lastLoadedConfig == null)
			{
				Debug.LogWarning("config == null");
				return false;
			}
			if (lastLoadedConfig.Exception != null)
			{
				Debug.LogWarning("config.Exception != null");
				return false;
			}
			if (!string.IsNullOrEmpty(AdsConfigManager.GetVideoDisabledReason(lastLoadedConfig)))
			{
				Debug.LogWarning("!string.IsNullOrEmpty(videoDisabledReason)");
				return false;
			}
			FreeCheckpointsPointMemento freeCheckpoints = lastLoadedConfig.AdPointsConfig.FreeCheckpoints;
			if (freeCheckpoints == null)
			{
				Debug.LogWarning("pointConfig == null");
				return false;
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
			_reward = freeCheckpoints.GetFinalReward(playerCategory);
			string rewardCountCaption = LocalizationStore.Get(ScriptLocalization.Key_0291) + " " + _reward.ToString(CultureInfo.InvariantCulture);
			CheckpointsRewardedVideo.SetRewardCountCaption(rewardCountCaption);
			string text = LocalizationStore.Get("Key_3172");
			try
			{
				CheckpointsRewardedVideo.SetReadyToWatchTitle(string.Format(CultureInfo.InvariantCulture, text, _reward));
			}
			catch
			{
				CheckpointsRewardedVideo.SetReadyToWatchTitle(text);
			}
			string text2 = LocalizationStore.Get("Key_3205");
			try
			{
				CheckpointsRewardedVideo.SetClaimRewardCountCaption(string.Format(CultureInfo.InvariantCulture, text2, _reward));
			}
			catch
			{
				CheckpointsRewardedVideo.SetClaimRewardCountCaption(text2);
			}
			CheckpointsRewardedVideo.SetReadyToWatchState(serverTime.Value, false, false);
			EventHandler<FinishedEventArgs> onEnterIdle = null;
			onEnterIdle = delegate
			{
				callback();
				//CheckpointsRewardedVideo.EnterIdle -= onEnterIdle;
			};
//			CheckpointsRewardedVideo.EnterIdle += onEnterIdle;
			return true;
		}

		private void Awake()
		{
			s_instance = this;
			int num = LoadPendingFreeCheckpoints();
			if (num > 0)
			{
				IncrementCheckpointsCountAndResetPending(num);
			}
		}

		private void OnDestroy()
		{
			if (_checkpointsRewardedVideo != null)
			{
				_checkpointsRewardedVideo.AdWatchedSuccessfully -= OnAdWatchedSuccessfully;
				_checkpointsRewardedVideo.EnterWatching -= OnEnterWatching;
				_checkpointsRewardedVideo.ExitWatching -= OnExitWatching;
			}
		}

		private void OnEnterWatching(object sender, EventArgs e)
		{
			if (MenuBackgroundMusic.sharedMusic != null)
			{
				MenuBackgroundMusic.sharedMusic.Stop();
			}
			SavePendingFreeCheckpoints(_reward);
			SaveShownTimestamp(CheckpointsRewardedVideo.StartTime);
		}

		private void OnExitWatching(object sender, EventArgs e)
		{
			ResetPendingFreeCheckpoints();
			if (MenuBackgroundMusic.sharedMusic != null)
			{
				MenuBackgroundMusic.sharedMusic.Play();
			}
		}

		private void OnAdWatchedSuccessfully(object sender, EventArgs e)
		{
			if (Application.isEditor)
			{
				Debug.LogFormat("<color=magenta>{0}.OnAdWatchedSuccessfully()</color>", GetType().Name);
			}
			IncrementCheckpointsCountAndResetPending(_reward);
		}

		private string GetReasonToDismissVideoFreeCheckpoints()
		{
			AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
			if (lastLoadedConfig == null)
			{
				return "Ads config is `null`.";
			}
			if (lastLoadedConfig.Exception != null)
			{
				return lastLoadedConfig.Exception.Message;
			}
			string videoDisabledReason = AdsConfigManager.GetVideoDisabledReason(lastLoadedConfig);
			if (!string.IsNullOrEmpty(videoDisabledReason))
			{
				return videoDisabledReason;
			}
			FreeCheckpointsPointMemento freeCheckpoints = lastLoadedConfig.AdPointsConfig.FreeCheckpoints;
			if (freeCheckpoints == null)
			{
				return string.Format("`{0}` config is `null`", new object[1] { freeCheckpoints.Id });
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
			string disabledReason = freeCheckpoints.GetDisabledReason(playerCategory);
			if (!string.IsNullOrEmpty(disabledReason))
			{
				return disabledReason;
			}
			if (FreeAwardController.Instance != null && !FreeAwardController.Instance.AdvertCountLessThanLimit())
			{
				return "`AdvertCountLessThanLimit == false`";
			}
			DateTime? serverTime = FriendsController.GetServerTime();
			if (!serverTime.HasValue)
			{
				return "Server time not received.";
			}
			DateTime? timeOfLastShow = GetTimeOfLastShow();
			if (timeOfLastShow.HasValue && serverTime.Value.Date <= timeOfLastShow.Value.Date)
			{
				return string.Format(CultureInfo.InvariantCulture, "`{0} <= {1}`", serverTime.Value.Date, timeOfLastShow.Value.Date);
			}
			return string.Empty;
		}

		private static DateTime? GetTimeOfLastShow()
		{
			string @string = Storager.getString("Ads.FreeCheckpointsLastTimeShown");
			if (string.IsNullOrEmpty(@string))
			{
				return null;
			}
			DateTime result;
			if (!DateTime.TryParse(@string, out result))
			{
				return null;
			}
			return result;
		}

		private static void SaveShownTimestamp(DateTime now)
		{
			Storager.setString("Ads.FreeCheckpointsLastTimeShown", now.ToString("s"));
			if (FreeAwardController.Instance != null)
			{
				FreeAwardController.Instance.AddAdvertTime(now);
			}
		}

		private static void IncrementCheckpointsCountAndResetPending(int reward)
		{
			int val = Storager.getInt("FreeCheckpointsKey") + reward;
			Storager.setInt("FreeCheckpointsKey", val);
			ResetPendingFreeCheckpoints();
			AnalyticsStuff.DeathEscapeAds(false);
		}

		private static int LoadPendingFreeCheckpoints()
		{
			if (!Storager.hasKey("Ads.PendingFreeCheckpoints"))
			{
				return 0;
			}
			return Storager.getInt("Ads.PendingFreeCheckpoints");
		}

		private static void SavePendingFreeCheckpoints(int reward)
		{
			Storager.setInt("Ads.PendingFreeCheckpoints", reward);
		}

		private static void ResetPendingFreeCheckpoints()
		{
			Storager.setInt("Ads.PendingFreeCheckpoints", 0);
		}
	}
}
