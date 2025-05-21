using System;
using System.Globalization;
using UnityEngine;

namespace Rilisoft
{
	[DisallowMultipleComponent]
	public sealed class GachaRewardedVideoController : MonoBehaviour
	{
		[SerializeField]
		protected internal GameObject gachaRewardedVideoParent;

		[SerializeField]
		protected internal GameObject gachaRewardedVideoButton;

		[SerializeField]
		protected internal GameObject skinCamera;

		private GachaRewardedVideo _gachaRewardedVideo;

		private int _reward = FreeSpinPointMemento.DefaultReward;

		private static GachaRewardedVideoController s_instance;

		private const string LastTimeShownKey = "Ads.FreeSpinLastTimeShown";

		private const string PendingFreeSpinsKey = "Ads.PendingFreeSpin";

		internal int Reward
		{
			get
			{
				return _reward;
			}
		}

		public static GachaRewardedVideoController Instance
		{
			get
			{
				return s_instance;
			}
		}

		private GachaRewardedVideo GachaRewardedVideo
		{
			get
			{
				if (_gachaRewardedVideo == null)
				{
					GachaRewardedVideo gachaRewardedVideo = Resources.Load<GachaRewardedVideo>("GachaRewardedVideo");
					if (gachaRewardedVideo == null)
					{
						Debug.LogWarning("gachaRewardedVideoPrefab is null.");
						return null;
					}
					_gachaRewardedVideo = UnityEngine.Object.Instantiate(gachaRewardedVideo);
					if (_gachaRewardedVideo == null)
					{
						Debug.LogWarning("gachaRewardedVideo is null.");
						return null;
					}
					_gachaRewardedVideo.transform.SetParent(gachaRewardedVideoParent.transform);
					_gachaRewardedVideo.transform.localPosition = Vector3.zero;
					_gachaRewardedVideo.transform.localScale = Vector3.one;
//					_gachaRewardedVideo.EnterIdle += OnEnterIdle;
					_gachaRewardedVideo.ExitIdle += OnExitIdle;
					_gachaRewardedVideo.AdWatchedSuccessfully += OnAdWatchedSuccessfully;
				}
				return _gachaRewardedVideo;
			}
		}

		public void OnGachaRewardedVideoButton()
		{
			if (GachaRewardedVideo == null)
			{
				return;
			}
			AdsConfigMemento lastLoadedConfig = AdsConfigManager.Instance.LastLoadedConfig;
			if (lastLoadedConfig == null)
			{
				Debug.LogWarning("config == null");
				RefreshGui();
				return;
			}
			if (lastLoadedConfig.Exception != null)
			{
				Debug.LogWarning("config.Exception != null");
				RefreshGui();
				return;
			}
			if (!string.IsNullOrEmpty(AdsConfigManager.GetVideoDisabledReason(lastLoadedConfig)))
			{
				Debug.LogWarning("!string.IsNullOrEmpty(videoDisabledReason)");
				RefreshGui();
				return;
			}
			FreeSpinPointMemento freeSpin = lastLoadedConfig.AdPointsConfig.FreeSpin;
			if (freeSpin == null)
			{
				Debug.LogWarning("pointConfig == null");
				RefreshGui();
			}
			else
			{
				string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
				_reward = freeSpin.GetFinalReward(playerCategory);
				GachaRewardedVideo.OnWatchButtonClicked();
			}
		}

		public void RefreshGui(bool forceButton)
		{
			gachaRewardedVideoButton.SetActive(forceButton);
		}

		public void RefreshGui()
		{
			bool forceButton = GachaRewardedVideoButtonIsEnabled();
			RefreshGui(forceButton);
		}

		private void Awake()
		{
			s_instance = this;
			int num = LoadPendingFreeSpins();
			if (num > 0)
			{
				IncrementSpinCountAndResetPending(num);
			}
		}

		private void Start()
		{
			RefreshGui();
		}

		private void OnDestroy()
		{
			if (_gachaRewardedVideo != null)
			{
//				_gachaRewardedVideo.EnterIdle -= OnEnterIdle;
				_gachaRewardedVideo.ExitIdle -= OnExitIdle;
				_gachaRewardedVideo.AdWatchedSuccessfully -= OnAdWatchedSuccessfully;
			}
		}

		private void OnEnable()
		{
			if (FreeAwardController.Instance != null)
			{
				FreeAwardController.Instance.AdvertTimeChanged += OnFreeSpinAvailabilityChanged;
			}
			if (GiftController.Instance != null)
			{
				if (Defs.IsDeveloperBuild)
				{
					Debug.Log("+= OnFreeSpinAvailabilityChanged");
				}
				GiftController.Instance.FreeSpinCountChanged += OnFreeSpinAvailabilityChanged;
			}
			else
			{
				Debug.LogWarning("GiftController.Instance == null");
			}
		}

		private void OnDisable()
		{
			if (FreeAwardController.Instance != null)
			{
				FreeAwardController.Instance.AdvertTimeChanged -= OnFreeSpinAvailabilityChanged;
			}
			if (GiftController.Instance != null)
			{
				if (Defs.IsDeveloperBuild)
				{
					Debug.Log("-= OnFreeSpinAvailabilityChanged");
				}
				GiftController.Instance.FreeSpinCountChanged -= OnFreeSpinAvailabilityChanged;
			}
		}

		private void OnFreeSpinAvailabilityChanged(object sender, EventArgs e)
		{
			if (Defs.IsDeveloperBuild)
			{
				Debug.Log("OnFreeSpinAvailabilityChanged");
			}
			RefreshGui();
		}

		private static string GetReasonToDismissVideoFreeSpin()
		{
			if (GiftController.Instance == null)
			{
				return "GiftController.Instance == null";
			}
			if (GiftController.Instance.CanGetGift)
			{
				return "GiftController.Instance.CanGetGift";
			}
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
			FreeSpinPointMemento freeSpin = lastLoadedConfig.AdPointsConfig.FreeSpin;
			if (freeSpin == null)
			{
				return string.Format("`{0}` config is `null`", new object[1] { freeSpin.Id });
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(lastLoadedConfig);
			string disabledReason = freeSpin.GetDisabledReason(playerCategory);
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
				return "server time not received";
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
			string @string = Storager.getString("Ads.FreeSpinLastTimeShown");
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

		private bool GachaRewardedVideoButtonIsEnabled()
		{
			if (Tools.IsWsa && !Tools.IsEditor)
			{
				return false;
			}
			string reasonToDismissVideoFreeSpin = GetReasonToDismissVideoFreeSpin();
			if (string.IsNullOrEmpty(reasonToDismissVideoFreeSpin))
			{
				return true;
			}
			Debug.LogFormat(Application.isEditor ? "<color=magenta>GachaRewardedVideoButtonIsEnabled(): false. {0}</color>" : "GachaRewardedVideoButtonIsEnabled(): false. {0}", reasonToDismissVideoFreeSpin);
			return false;
		}

		private void OnEnterIdle(object sender, FinishedEventArgs e)
		{
			bool succeeded = e.Succeeded;
			if (Application.isEditor)
			{
				Debug.LogFormat("<color=magenta>OnEnterIdle: {0}</color>", succeeded);
			}
			if (skinCamera != null)
			{
				skinCamera.SetActive(true);
			}
			gachaRewardedVideoButton.SetActive(GachaRewardedVideoButtonIsEnabled());
		}

		private void OnExitIdle(object sender, EventArgs e)
		{
			if (Application.isEditor)
			{
				Debug.Log("<color=magenta>OnExitIdle</color>");
			}
			if (skinCamera != null)
			{
				skinCamera.SetActive(false);
			}
			gachaRewardedVideoButton.SetActive(GachaRewardedVideoButtonIsEnabled());
		}

		private void OnAdWatchedSuccessfully(object sender, EventArgs e)
		{
			if (Application.isEditor)
			{
				Debug.Log("<color=magenta>OnAdWatchedSuccessfully</color>");
			}
			IncrementSpinCountAndResetPending(Reward);
		}

		internal static void SaveShownTimestamp(DateTime now)
		{
			if (Application.isEditor)
			{
				Debug.Log("GachaRewardedVideoController.SaveShownTimestamp()");
			}
			Storager.setString("Ads.FreeSpinLastTimeShown", now.ToString("s"));
			if (FreeAwardController.Instance != null)
			{
				FreeAwardController.Instance.AddAdvertTime(now);
			}
		}

		private static void IncrementSpinCountAndResetPending(int reward)
		{
			if (Application.isEditor)
			{
				Debug.LogFormat("TicketsRewardedVideoController.IncrementTicketsCountAndResetPending({0})", reward);
			}
			GiftController.Instance.IncrementFreeSpins(reward);
			GiftController.Instance.ReCreateSlots();
			ResetPendingFreeSpins();
		}

		private static int LoadPendingFreeSpins()
		{
			if (Application.isEditor)
			{
				Debug.Log("GachaRewardedVideoController.LoadPendingFreeSpins()");
			}
			if (!Storager.hasKey("Ads.PendingFreeSpin"))
			{
				return 0;
			}
			return Storager.getInt("Ads.PendingFreeSpin");
		}

		internal static void SavePendingFreeSpins(int reward)
		{
			if (Application.isEditor)
			{
				Debug.Log("GachaRewardedVideoController.SavePendingFreeSpins()");
			}
			Storager.setInt("Ads.PendingFreeSpin", reward);
		}

		internal static void ResetPendingFreeSpins()
		{
			if (Application.isEditor)
			{
				Debug.Log("GachaRewardedVideoController.ResetPendingFreeSpins()");
			}
			Storager.setInt("Ads.PendingFreeSpin", 0);
		}
	}
}
