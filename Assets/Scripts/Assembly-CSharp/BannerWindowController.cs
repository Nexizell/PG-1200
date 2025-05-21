using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Unity.Linq;
using UnityEngine;

public sealed class BannerWindowController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003COnApplicationPause_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool pause;

		public BannerWindowController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003COnApplicationPause_003Ed__24(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (!pause)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				break;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = null;
				_003C_003E1__state = 3;
				return true;
			case 3:
				_003C_003E1__state = -1;
				if (StarterPackController.Get != null)
				{
					StarterPackController.Get.UpdateCountShownWindowByTimeCondition();
				}
				PromoActionsManager.UpdateDaysOfValorShownCondition();
				_003C_003E4__this._isBlockShowForNewPlayer = !_003C_003E4__this.IsBannersCanShowAfterNewInstall();
				break;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public BannerWindow[] bannerWindows;

	[NonSerialized]
	public AdvertisementController advertiseController;

	private readonly int BannerWindowCount = Enum.GetNames(typeof(BannerWindowType)).Length;

	private const float StartBannerShowDelay = 3f;

	private float eventX3ShowTimeoutHours = 12f;

	private Queue<BannerWindow> _bannerQueue;

	private BannerWindow _currentBanner;

	private bool[] _bannerShowed;

	private bool[] _needShowBanner;

	private bool _someBannerShown;

	private float _lastCheckTime;

	private float _whenStart;

	private bool _isBlockShowForNewPlayer;

	public int viewedBannersCountInConnectScene;

	private static long _lastTimeX3WindowShown = -1L;

	public static BannerWindowController SharedController { get; private set; }

	public static long lastTimeX3WindowShown
	{
		get
		{
			if (_lastTimeX3WindowShown <= 0)
			{
				if (PlayerPrefs.HasKey(Defs.EventX3WindowShownLastTime))
				{
					long.TryParse(PlayerPrefs.GetString(Defs.EventX3WindowShownLastTime), out _lastTimeX3WindowShown);
				}
				else
				{
					PlayerPrefs.SetString(Defs.EventX3WindowShownLastTime, PromoActionsManager.CurrentUnixTime.ToString());
					_lastTimeX3WindowShown = PromoActionsManager.CurrentUnixTime;
				}
			}
			return _lastTimeX3WindowShown;
		}
		set
		{
			PlayerPrefs.SetString(Defs.EventX3WindowShownLastTime, value.ToString());
			_lastTimeX3WindowShown = value;
		}
	}

	internal bool IsAnyBannerShown
	{
		get
		{
			return _currentBanner != null;
		}
	}

	public BannerWindowController()
	{
		_bannerShowed = new bool[BannerWindowCount];
		_needShowBanner = new bool[BannerWindowCount];
	}

	private void Awake()
	{
		SharedController = this;
	}

	private void Start()
	{
		_currentBanner = null;
		_bannerQueue = new Queue<BannerWindow>();
		_isBlockShowForNewPlayer = !IsBannersCanShowAfterNewInstall();
	}

	public void ResetScene()
	{
		_whenStart = Time.realtimeSinceStartup + 3f;
		_someBannerShown = false;
	}

	public void AddBannersTimeout(float seconds)
	{
		_lastCheckTime = Time.realtimeSinceStartup + seconds;
	}

	private void OnDestroy()
	{
		SharedController = null;
		_bannerQueue = null;
		advertiseController = null;
	}

	private IEnumerator OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			yield return null;
			yield return null;
			yield return null;
			if (StarterPackController.Get != null)
			{
				StarterPackController.Get.UpdateCountShownWindowByTimeCondition();
			}
			PromoActionsManager.UpdateDaysOfValorShownCondition();
			_isBlockShowForNewPlayer = !IsBannersCanShowAfterNewInstall();
		}
	}

	public void RegisterWindow(BannerWindow window, BannerWindowType windowType)
	{
		if (bannerWindows.Length < (int)(windowType + 1))
		{
			List<BannerWindow> list = bannerWindows.ToList();
			while (list.Count() < (int)(windowType + 1))
			{
				list.Add(null);
			}
			bannerWindows = list.ToArray();
		}
		bannerWindows[(int)windowType] = window;
		int layer = LayerMask.NameToLayer("Banners");
		window.gameObject.Descendants().ForEach(delegate(GameObject go)
		{
			go.layer = layer;
		});
	}

	private BannerWindow ShowBannerWindow(BannerWindowType windowType)
	{
		if (bannerWindows.Length < 0 || (int)windowType > bannerWindows.Length - 1)
		{
			return null;
		}
		if (bannerWindows[(int)windowType] == null)
		{
			return null;
		}
		if (bannerWindows[(int)windowType].gameObject.activeSelf)
		{
			return null;
		}
		BannerWindow bannerWindow = bannerWindows[(int)windowType];
		if (_currentBanner == null)
		{
			_currentBanner = bannerWindow;
			_currentBanner.type = windowType;
			if (ConnectScene.isEnable)
			{
				viewedBannersCountInConnectScene++;
			}
			bannerWindow.Show();
		}
		else
		{
			_bannerQueue.Enqueue(bannerWindow);
		}
		return bannerWindow;
	}

	public void HideBannerWindowNoShowNext()
	{
		if (_currentBanner != null)
		{
			_currentBanner.Hide();
			_currentBanner = null;
		}
	}

	public void ClearBannerStates()
	{
		_bannerShowed = new bool[BannerWindowCount];
		_needShowBanner = new bool[BannerWindowCount];
	}

	public void HideBannerWindow()
	{
		BuySmileBannerController.openedFromPromoActions = false;
		HideBannerWindowNoShowNext();
		if (_bannerQueue.Count > 0)
		{
			(_currentBanner = _bannerQueue.Dequeue()).Show();
		}
	}

	private void ShowAdmobBanner()
	{
		if (!(AdmobPerelivWindow.admobTexture == null) && !string.IsNullOrEmpty(AdmobPerelivWindow.admobUrl))
		{
			ShowBannerWindow(BannerWindowType.Admob);
		}
	}

	public void AdmobBannerExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		HideBannerWindow();
		_bannerShowed[0] = false;
		_needShowBanner[0] = false;
		ResetStateBannerShowed(BannerWindowType.Admob);
	}

	public void AdmobBannerApplyClick()
	{
		Application.OpenURL(AdmobPerelivWindow.admobUrl);
	}

	private void ShowAdvertisementBanner(AdvertisementController advertisementController)
	{
		if (!(advertisementController.AdvertisementTexture == null))
		{
			advertiseController = advertisementController;
			BannerWindow bannerWindow = ShowBannerWindow(BannerWindowType.Advertisement);
			if (!(bannerWindow == null) && AdsConfigManager.Instance.LastLoadedConfig != null)
			{
				string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
				PerelivSettings perelivSettings = PerelivConfigManager.Instance.GetPerelivSettings(playerCategory);
				bannerWindow.SetEnableExitButton(perelivSettings.ButtonClose);
				bannerWindow.SetBackgroundImage(advertisementController.AdvertisementTexture);
			}
		}
	}

	public void AdvertBannerExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		advertiseController.Close();
		UpdateAdvertShownCount();
		HideBannerWindow();
	}

	public void NewVersionBannerExitClick()
	{
		ButtonClickSound.Instance.PlayClick();
		UpdateNewVersionShownCount();
		HideBannerWindow();
	}

	private static void UpdateNewVersionShownCount()
	{
		PlayerPrefs.SetInt(Defs.UpdateAvailableShownTimesSN, PlayerPrefs.GetInt(Defs.UpdateAvailableShownTimesSN, 3) - 1);
		PlayerPrefs.SetString(Defs.LastTimeUpdateAvailableShownSN, DateTimeOffset.Now.ToString("s"));
		PlayerPrefs.Save();
	}

	private static void ClearNewVersionShownCount()
	{
		PlayerPrefs.SetInt(Defs.UpdateAvailableShownTimesSN, 0);
		PlayerPrefs.SetString(Defs.LastTimeUpdateAvailableShownSN, DateTimeOffset.Now.ToString("s"));
		PlayerPrefs.Save();
	}

	public void AdvertBannerApplyClick()
	{
		ButtonClickSound.Instance.PlayClick();
		advertiseController.Close();
		UpdateAdvertShownCount();
		if (AdsConfigManager.Instance.LastLoadedConfig != null)
		{
			string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
			Application.OpenURL(PerelivConfigManager.Instance.GetPerelivSettings(playerCategory).RedirectUrl);
		}
		HideBannerWindow();
	}

	public void NewVersionBannerApplyClick()
	{
		ButtonClickSound.Instance.PlayClick();
		ClearNewVersionShownCount();
		Application.OpenURL(MainMenu.RateUsURL);
		HideBannerWindow();
	}

	public void EverydayRewardApplyClick()
	{
		ButtonClickSound.TryPlayClick();
		TakeEverydayRewardForPlayer();
		HideBannerWindow();
	}

	private void TakeEverydayRewardForPlayer()
	{
		NotificationController.isGetEveryDayMoney = false;
		if (MainMenu.sharedMenu != null)
		{
			MainMenu.sharedMenu.isShowAvard = false;
		}
		BankController.GiveInitialNumOfCoins();
		int @int = Storager.getInt("Coins");
		Storager.setInt("Coins", @int + 3);
		AnalyticsFacade.CurrencyAccrual(3, "Coins");
		CoinsMessage.FireCoinsAddedEvent();
		AudioClip audioClip = Resources.Load<AudioClip>("coin_get");
		if (audioClip != null && Defs.isSoundFX)
		{
			NGUITools.PlaySound(audioClip);
		}
	}

	public void SorryBannerExitButtonClick()
	{
		MainMenuController.sharedController.stubLoading.SetActive(false);
		HideBannerWindow();
	}

	public void EventX3ExitClick()
	{
		ButtonClickSound.TryPlayClick();
		lastTimeX3WindowShown = PromoActionsManager.CurrentUnixTime;
		HideBannerWindow();
	}

	public void EventX3ApplyClick()
	{
		EventX3ExitClick();
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.ShowBankWindow();
		}
		else if (ConnectScene.sharedController != null)
		{
			ConnectScene.sharedController.ShowBankWindow();
		}
	}

	private void UpdateAdvertShownCount()
	{
		if (AdsConfigManager.Instance.LastLoadedConfig != null)
		{
			string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
			if (!PerelivConfigManager.Instance.GetPerelivSettings(playerCategory).ShowAlways)
			{
				PlayerPrefs.SetInt(Defs.AdvertWindowShownCount, PlayerPrefs.GetInt(Defs.AdvertWindowShownCount, 3) - 1);
				PlayerPrefs.SetString(Defs.AdvertWindowShownLastTime, PromoActionsManager.CurrentUnixTime.ToString());
				PlayerPrefs.Save();
			}
		}
	}

	private bool IsBannersCanShowAfterNewInstall()
	{
		if (string.IsNullOrEmpty(Defs.StartTimeShowBannersString))
		{
			return true;
		}
		DateTime result;
		if (!DateTime.TryParse(Defs.StartTimeShowBannersString, out result))
		{
			return true;
		}
		if (!((DateTime.UtcNow - result).TotalMinutes >= 1440.0))
		{
			return Defs.countReturnInConnectScene >= 4;
		}
		return true;
	}

	private void Update()
	{
		if (ReviewHUDWindow.isShow || Time.realtimeSinceStartup < _whenStart || !(Time.realtimeSinceStartup - _lastCheckTime >= 1f))
		{
			return;
		}
		CheckBannersShowConditions();
		for (int i = 0; i < _needShowBanner.Length; i++)
		{
			if ((_someBannerShown && i != 0) || !_needShowBanner[i] || ActivityIndicator.IsActiveIndicator)
			{
				continue;
			}
			if (MainMenuController.IsShowRentExpiredPoint() || (MainMenuController.sharedController != null && (MainMenuController.sharedController.FreePanelIsActive || MainMenuController.sharedController.InMiniGamesScreen)) || (BankController.Instance != null && BankController.Instance.InterfaceEnabled) || (MainMenuController.sharedController != null && !MainMenuController.sharedController.mainPanel.activeInHierarchy && !ConnectScene.isEnable) || !FreeAwardController.FreeAwardChestIsInIdleState || MainMenuController.SavedShwonLobbyLevelIsLessThanActual() || (ExpController.Instance != null && ExpController.Instance.IsLevelUpShown))
			{
				break;
			}
			if ((i != 3 || !ConnectScene.isEnable) && (i != 2 || (ConnectScene.isReturnFromGame && viewedBannersCountInConnectScene <= 0)) && (!ConnectScene.isReturnFromGame || !_needShowBanner[4] || i == 4))
			{
				_needShowBanner[i] = false;
				switch (i)
				{
				case 0:
					ShowAdmobBanner();
					break;
				case 1:
					ShowAdvertisementBanner(advertiseController);
					break;
				default:
					ShowBannerWindow((BannerWindowType)i);
					break;
				}
				_someBannerShown = true;
				break;
			}
		}
		_lastCheckTime = Time.realtimeSinceStartup;
	}

	private void CheckDownloadAdvertisement()
	{
		if (!(ExperienceController.sharedController == null) && AdsConfigManager.Instance.LastLoadedConfig != null)
		{
			string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
			PerelivSettings perelivSettings = PerelivConfigManager.Instance.GetPerelivSettings(playerCategory);
			int currentLevel = ExperienceController.sharedController.currentLevel;
			bool flag = perelivSettings.MinLevel == -1 || currentLevel >= perelivSettings.MinLevel;
			bool flag2 = perelivSettings.MaxLevel == -1 || currentLevel <= perelivSettings.MaxLevel;
			bool flag3 = perelivSettings.ShowAlways || PlayerPrefs.GetInt(Defs.AdvertWindowShownCount, 3) > 0;
			if (perelivSettings.Enabled && advertiseController.CurrentState == AdvertisementController.State.Idle && flag && flag2 && flag3)
			{
				advertiseController.Run();
			}
		}
	}

	private bool IsAdvertisementDownloading()
	{
		if (advertiseController == null)
		{
			return false;
		}
		AdvertisementController.State currentState = advertiseController.CurrentState;
		if (currentState != 0 && currentState != AdvertisementController.State.Complete)
		{
			return currentState != AdvertisementController.State.Error;
		}
		return false;
	}

	private void CheckBannersShowConditions()
	{
		if (PromoActionsManager.sharedManager == null || (LobbyCraftController.Instance != null && LobbyCraftController.Instance.InterfaceEnabled))
		{
			return;
		}
		if (AdmobPerelivWindow.admobTexture != null && !string.IsNullOrEmpty(AdmobPerelivWindow.admobUrl))
		{
			if (Nest.Instance != null && Nest.Instance.BannerIsVisible)
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log("Skipping fake interstitial while Nest Banner is active.");
				}
			}
			else if (!_bannerShowed[0])
			{
				_bannerShowed[0] = true;
				_needShowBanner[0] = true;
			}
		}
		CheckDownloadAdvertisement();
		if (IsAdvertisementDownloading() || AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			return;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
		if (PerelivConfigManager.Instance.GetPerelivSettings(playerCategory).Enabled && advertiseController.CurrentState == AdvertisementController.State.Complete)
		{
			if (ConnectScene.sharedController != null)
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log("Skipping pereliv in Connect Scene.");
				}
			}
			else if (Nest.Instance != null && Nest.Instance.BannerIsVisible)
			{
				if (Application.isEditor)
				{
					UnityEngine.Debug.Log("Skipping pereliv while Nest Banner is active.");
				}
			}
			else if (!_bannerShowed[1])
			{
				_bannerShowed[1] = true;
				_needShowBanner[1] = true;
			}
		}
		if (PlayerPrefs.GetInt("leave_from_duel_penalty") != 0 && (Nest.Instance == null || !Nest.Instance.BannerIsVisible) && !_bannerShowed[4])
		{
			_bannerShowed[4] = true;
			_needShowBanner[4] = true;
		}
		if (Nest.Instance == null || !Nest.Instance.BannerIsVisible)
		{
			if (!ConnectScene.isReturnFromGame && !ConnectScene.isEnable && TournamentAvailableBannerWindow.CanShow && !_bannerShowed[6])
			{
				_bannerShowed[6] = true;
				_needShowBanner[6] = true;
			}
			if (!ConnectScene.isReturnFromGame && !ConnectScene.isEnable && TournamentLooserBannerWindow.CanShow && !_bannerShowed[8])
			{
				_bannerShowed[8] = true;
				_needShowBanner[8] = true;
			}
			if (!ConnectScene.isReturnFromGame && !ConnectScene.isEnable && TournamentWinnerBannerWindow.CanShow && !_bannerShowed[7])
			{
				_bannerShowed[7] = true;
				_needShowBanner[7] = true;
			}
		}
		if (!_isBlockShowForNewPlayer)
		{
			if (PromoActionsManager.CurrentUnixTime - lastTimeX3WindowShown > (long)TimeSpan.FromHours(eventX3ShowTimeoutHours).TotalSeconds && ConnectScene.isReturnFromGame && TrainingController.TrainingCompleted && PromoActionsManager.sharedManager.IsEventX3Active && (PromoActionsManager.CurrentUnixTime - lastTimeX3WindowShown > (long)TimeSpan.FromHours(eventX3ShowTimeoutHours).TotalSeconds || PromoActionsManager.CurrentUnixTime - lastTimeX3WindowShown < 0) && !_bannerShowed[2])
			{
				_bannerShowed[2] = true;
				_needShowBanner[2] = true;
			}
			if (GlobalGameController.NewVersionAvailable && PlayerPrefs.GetInt(Defs.UpdateAvailableShownTimesSN, 3) > 0 && !_bannerShowed[3])
			{
				_bannerShowed[3] = true;
				_needShowBanner[3] = true;
			}
		}
	}

	public void ResetStateBannerShowed(BannerWindowType windowType)
	{
		if (bannerWindows.Length >= 0 && (int)windowType <= bannerWindows.Length - 1)
		{
			_bannerShowed[(int)windowType] = false;
			_someBannerShown = false;
		}
	}

	public bool IsBannerShow(BannerWindowType bannerType)
	{
		if (_currentBanner == null)
		{
			return false;
		}
		return _currentBanner.type == bannerType;
	}

	public void ForceShowBanner(BannerWindowType windowType)
	{
		if (_currentBanner == null)
		{
			ShowBannerWindow(windowType);
		}
		else if (_currentBanner.type != windowType)
		{
			HideBannerWindow();
			ShowBannerWindow(windowType);
		}
	}

	internal void SubmitCurrentBanner()
	{
		if (!(_currentBanner == null))
		{
			_currentBanner.Submit();
		}
	}
}
