using System;
using System.Globalization;
using I2.Loc;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	public sealed class TicketsRewardedVideoController : MonoBehaviour
	{
		[SerializeField]
		protected internal GameObject _ticketsRewardedVideoParent;

		private ReusableRewardedVideo _ticketsRewardedVideo;

		private int _reward = TicketsPointMemento.DefaultReward;

		internal const string LastTimeShownKey = "Ads.FreeTicketsLastTimeShown";

		private const string PendingFreeTicketsKey = "Ads.PendingFreeTickets";

		private static TicketsRewardedVideoController s_instance;

		internal static TicketsRewardedVideoController Instance
		{
			get
			{
				return s_instance;
			}
		}

		private int Reward
		{
			get
			{
				return _reward;
			}
		}

		private ReusableRewardedVideo TicketsRewardedVideo
		{
			get
			{
				if (_ticketsRewardedVideo == null)
				{
					ReusableRewardedVideo reusableRewardedVideo = Resources.Load<ReusableRewardedVideo>("TicketsRewardedVideo");
					if (reusableRewardedVideo == null)
					{
						Debug.LogError("ticketsRewardedVideoPrefab is null.");
						return null;
					}
					_ticketsRewardedVideo = UnityEngine.Object.Instantiate(reusableRewardedVideo, _ticketsRewardedVideoParent.transform);
					if (_ticketsRewardedVideo == null)
					{
						Debug.LogError("_ticketsRewardedVideo is null.");
						return null;
					}
					_ticketsRewardedVideo.gameObject.SetLayerRecursively(_ticketsRewardedVideoParent.layer);
					_ticketsRewardedVideo.transform.localPosition = Vector3.zero;
					_ticketsRewardedVideo.transform.localScale = Vector3.one;
					_ticketsRewardedVideo.AdWatchedSuccessfully += OnAdWatchedSuccessfully;
					_ticketsRewardedVideo.EnterWatching += OnEnterWatching;
					_ticketsRewardedVideo.ExitWatching += OnExitWatching;
					_ticketsRewardedVideo.ExitReward += OnExitReward;
//					_ticketsRewardedVideo.EnterIdle += OnEnterIdle;
				}
				return _ticketsRewardedVideo;
			}
		}

		internal event EventHandler ButtonAvailabilityChanged;

		internal bool IsButtonAvailable()
		{
			if (Tools.IsWsa && !Tools.IsEditor)
			{
				return false;
			}
			string reasonToDismissVideoFreeTickets = GetReasonToDismissVideoFreeTickets();
			if (!string.IsNullOrEmpty(reasonToDismissVideoFreeTickets))
			{
				Debug.LogFormat("[Rilisoft] Dismissing rewarded video `Free Tickets`: {0}", reasonToDismissVideoFreeTickets);
				return false;
			}
			return true;
		}

		private string GetReasonToDismissVideoFreeTickets()
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
			TicketsPointMemento freeTickets = lastLoadedConfig.AdPointsConfig.FreeTickets;
			if (freeTickets == null)
			{
				return string.Format("`{0}` config is `null`", new object[1] { freeTickets.Id });
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
			string disabledReason = freeTickets.GetDisabledReason(playerCategory);
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
			string @string = Storager.getString("Ads.FreeTicketsLastTimeShown");
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

		public int GetCurrentReward()
		{
			AdsConfigMemento currentConfig = GetCurrentConfig();
			if (currentConfig == null)
			{
				throw new InvalidOperationException("config == null");
			}
			TicketsPointMemento pointConfig = GetPointConfig(currentConfig);
			if (pointConfig == null)
			{
				throw new InvalidOperationException("pointConfig == null");
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(currentConfig);
			return pointConfig.GetFinalReward(playerCategory);
		}

		public void OnTicketsRewardedVideoButton()
		{
			if (TicketsRewardedVideo == null)
			{
				return;
			}
			AdsConfigMemento currentConfig = GetCurrentConfig();
			if (currentConfig == null)
			{
				RaiseButtonAvailabilityChanged();
				return;
			}
			if (!string.IsNullOrEmpty(AdsConfigManager.GetVideoDisabledReason(currentConfig)))
			{
				Debug.LogWarning("!string.IsNullOrEmpty(videoDisabledReason)");
				RaiseButtonAvailabilityChanged();
				return;
			}
			TicketsPointMemento pointConfig = GetPointConfig(currentConfig);
			if (pointConfig == null)
			{
				RaiseButtonAvailabilityChanged();
				return;
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(currentConfig);
			if (!string.IsNullOrEmpty(pointConfig.GetDisabledReason(playerCategory)))
			{
				RaiseButtonAvailabilityChanged();
				return;
			}
			if (FreeAwardController.Instance != null && !FreeAwardController.Instance.AdvertCountLessThanLimit())
			{
				RaiseButtonAvailabilityChanged();
				return;
			}
			DateTime? serverTime = FriendsController.GetServerTime();
			if (!serverTime.HasValue)
			{
				RaiseButtonAvailabilityChanged();
				return;
			}
			DateTime? timeOfLastShow = GetTimeOfLastShow();
			if (timeOfLastShow.HasValue && serverTime.Value.Date <= timeOfLastShow.Value.Date)
			{
				RaiseButtonAvailabilityChanged();
				return;
			}
			_reward = pointConfig.GetFinalReward(playerCategory);
			string rewardCountCaption = LocalizationStore.Get(ScriptLocalization.Key_0291) + " " + Reward.ToString(CultureInfo.InvariantCulture);
			TicketsRewardedVideo.SetRewardCountCaption(rewardCountCaption);
			TicketsRewardedVideo.SetClaimRewardCountCaption(Reward.ToString());
			bool finalSimplifiedInterface = pointConfig.GetFinalSimplifiedInterface(playerCategory);
			TicketsRewardedVideo.SetReadyToWatchState(serverTime.Value, finalSimplifiedInterface, false);
		}

		private void OnEnterWatching(object sender, EventArgs e)
		{
			if (MenuBackgroundMusic.sharedMusic != null)
			{
				MenuBackgroundMusic.sharedMusic.Stop();
			}
			SavePendingFreeTickets(Reward);
			SaveShownTimestamp(TicketsRewardedVideo.StartTime);
		}

		private void OnEnterIdle(object sender, EventArgs e)
		{
			RaiseButtonAvailabilityChanged();
		}

		private void OnExitWatching(object sender, EventArgs e)
		{
			ResetPendingFreeTickets();
			if (MenuBackgroundMusic.sharedMusic != null)
			{
				MenuBackgroundMusic.sharedMusic.Play();
			}
		}

		private void OnExitReward(object sender, EventArgs e)
		{
			IncrementTicketsCountAndResetPending(Reward);
			RaiseButtonAvailabilityChanged();
		}

		private void OnAdWatchedSuccessfully(object sender, EventArgs e)
		{
		}

		private void RaiseButtonAvailabilityChanged()
		{
			EventHandler buttonAvailabilityChanged = this.ButtonAvailabilityChanged;
			if (buttonAvailabilityChanged != null)
			{
				buttonAvailabilityChanged(this, EventArgs.Empty);
			}
		}

		private void Awake()
		{
			s_instance = this;
			int num = LoadPendingFreeTickets();
			if (num > 0)
			{
				IncrementTicketsCountAndResetPending(num);
			}
		}

		private void OnDestroy()
		{
			if (_ticketsRewardedVideo != null)
			{
				_ticketsRewardedVideo.AdWatchedSuccessfully -= OnAdWatchedSuccessfully;
				_ticketsRewardedVideo.EnterWatching -= OnEnterWatching;
				_ticketsRewardedVideo.ExitWatching -= OnExitWatching;
				_ticketsRewardedVideo.ExitReward -= OnExitReward;
//				_ticketsRewardedVideo.EnterIdle -= OnEnterIdle;
			}
		}

		private static void SaveShownTimestamp(DateTime now)
		{
			if (Application.isEditor)
			{
				Debug.Log("TicketsRewardedVideoController.SaveShownTimestamp()");
			}
			Storager.setString("Ads.FreeTicketsLastTimeShown", now.ToString("s"));
			if (FreeAwardController.Instance != null)
			{
				FreeAwardController.Instance.AddAdvertTime(now);
			}
		}

		private static void IncrementTicketsCountAndResetPending(int reward)
		{
			if (Application.isEditor)
			{
				Debug.Log("TicketsRewardedVideoController.IncrementTicketsCountAndResetPending()");
			}
			BankController.AddTickets(reward);
			ResetPendingFreeTickets();
		}

		private static int LoadPendingFreeTickets()
		{
			if (Application.isEditor)
			{
				Debug.Log("TicketsRewardedVideoController.LoadPendingFreeTickets()");
			}
			if (!Storager.hasKey("Ads.PendingFreeTickets"))
			{
				return 0;
			}
			return Storager.getInt("Ads.PendingFreeTickets");
		}

		private static void SavePendingFreeTickets(int reward)
		{
			if (Application.isEditor)
			{
				Debug.Log("TicketsRewardedVideoController.SavePendingFreeTickets()");
			}
			Storager.setInt("Ads.PendingFreeTickets", reward);
		}

		private static void ResetPendingFreeTickets()
		{
			if (Application.isEditor)
			{
				Debug.Log("TicketsRewardedVideoController.ResetPendingFreeTickets()");
			}
			Storager.setInt("Ads.PendingFreeTickets", 0);
		}

		private AdsConfigMemento GetCurrentConfig()
		{
			AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
			if (lastLoadedConfig == null)
			{
				Debug.LogWarning("config == null");
				return null;
			}
			if (lastLoadedConfig.Exception != null)
			{
				Debug.LogWarning("config.Exception != null");
				return null;
			}
			return lastLoadedConfig;
		}

		private TicketsPointMemento GetPointConfig(AdsConfigMemento config)
		{
			TicketsPointMemento freeTickets = config.AdPointsConfig.FreeTickets;
			if (freeTickets == null)
			{
				Debug.LogWarning("pointConfig == null");
				return null;
			}
			return freeTickets;
		}
	}
}
