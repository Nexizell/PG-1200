using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using FyberPlugin;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class FreeAwardController : MonoBehaviour
{
	public class StateEventArgs : EventArgs
	{
		public State State { get; set; }

		public State OldState { get; set; }
	}

	public abstract class State
	{
	}

	public sealed class IdleState : State
	{
		private static readonly IdleState _instance = new IdleState();

		internal static IdleState Instance
		{
			get
			{
				return _instance;
			}
		}

		private IdleState()
		{
		}
	}

	public sealed class WatchState : State
	{
		private readonly DateTime _nextTimeEnabled;

		public WatchState(DateTime nextTimeEnabled)
		{
			DateTime serverTimeOrFallbackToLocalUtc = FriendsController.GetServerTimeOrFallbackToLocalUtc();
			if (Defs.IsDeveloperBuild && serverTimeOrFallbackToLocalUtc < nextTimeEnabled)
			{
				UnityEngine.Debug.Log("Watching state inactive: need to wait till UTC " + nextTimeEnabled.ToString("T", CultureInfo.InvariantCulture));
			}
			_nextTimeEnabled = nextTimeEnabled;
		}

		public TimeSpan GetEstimatedTimeSpan()
		{
			return _nextTimeEnabled - FriendsController.GetServerTimeOrFallbackToLocalUtc();
		}
	}

	public sealed class WaitingState : State
	{
		private readonly float _startTime;

		public float StartTime
		{
			get
			{
				return _startTime;
			}
		}

		public WaitingState(float startTime)
		{
			_startTime = startTime;
		}

		public WaitingState()
			: this(Time.realtimeSinceStartup)
		{
		}
	}

	public sealed class WatchingState : State
	{
		private readonly float _startTime;

		private readonly Promise<string> _adClosed = new Promise<string>();

		public Future<string> AdClosed
		{
			get
			{
				return _adClosed.Future;
			}
		}

		public float StartTime
		{
			get
			{
				return _startTime;
			}
		}

		public WatchingState()
		{
			_startTime = Time.realtimeSinceStartup;
			string text = DetermineContext();
			Storager.setString("Ads.PendingRewardTimestamp", FriendsController.GetServerTimeOrFallbackToLocalUtc().ToString("s"));
			if (Instance.Provider == AdProvider.GoogleMobileAds)
			{
				UnityEngine.Debug.Log("[Rilisoft] GoogleMobileAds are not supported directly.");
			}
			else if (Instance.Provider == AdProvider.UnityAds)
			{
				UnityEngine.Debug.Log("[Rilisoft] UnityAds are not supported directly.");
			}
			else if (Instance.Provider == AdProvider.Vungle)
			{
				UnityEngine.Debug.Log("[Rilisoft] Vungle is not supported directly.");
			}
			else
			{
				if (Instance.Provider != AdProvider.Fyber)
				{
					return;
				}
				AdvertisementInfo advertisementInfo = new AdvertisementInfo(0, 0);
				if (!FyberVideoLoaded.IsCompleted)
				{
					UnityEngine.Debug.LogWarning("FyberVideoLoaded.IsCompleted: False");
					return;
				}
				Ad ad = FyberVideoLoaded.Result as Ad;
				if (ad == null)
				{
					UnityEngine.Debug.LogWarningFormat("FyberVideoLoaded.Result: {0}", FyberVideoLoaded.Result);
					return;
				}
				Action<AdResult> adFinished = null;
				adFinished = delegate(AdResult adResult)
				{
					FyberCallback.AdFinished -= adFinished;
					LogImpressionDetails(advertisementInfo);
					if (adResult.Message == "CLOSE_FINISHED")
					{
						AnalyticsFacade.SendCustomEventToFacebook("rewarded_ads_watched_count", null);
					}
					_adClosed.SetResult(adResult.Message);
				};
				FyberCallback.AdFinished += adFinished;
				ad.Start();
				FyberVideoLoaded = null;
				Dictionary<string, string> eventParams = new Dictionary<string, string>
				{
					{ "af_content_type", "Rewarded video" },
					{
						"af_content_id",
						string.Format("Rewarded video ({0})", new object[1] { text })
					}
				};
				AnalyticsFacade.SendCustomEventToAppsFlyer("af_content_view", eventParams);
			}
		}

		private static void LogImpressionDetails(AdvertisementInfo advertisementInfo)
		{
			if (advertisementInfo == null)
			{
				advertisementInfo = AdvertisementInfo.Default;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Round {0}", new object[1] { advertisementInfo.Round + 1 });
			stringBuilder.AppendFormat(", Slot {0} ({1})", new object[2]
			{
				advertisementInfo.Slot + 1,
				AnalyticsHelper.GetAdProviderName(Instance.GetProviderByIndex(advertisementInfo.Slot))
			});
			if (InterstitialManager.Instance.Provider == AdProvider.GoogleMobileAds)
			{
				stringBuilder.AppendFormat(", Unit {0}", new object[1] { advertisementInfo.Unit + 1 });
			}
			if (string.IsNullOrEmpty(advertisementInfo.Details))
			{
				stringBuilder.Append(" - Impression");
				return;
			}
			stringBuilder.AppendFormat(" - Impression: {0}", new object[1] { advertisementInfo.Details });
		}

		internal void SimulateCallbackInEditor(string result)
		{
			if (Application.isEditor)
			{
				_adClosed.SetResult(result ?? string.Empty);
			}
		}

		private static string DetermineContext()
		{
			if (BankController.Instance != null && BankController.Instance.InterfaceEnabled)
			{
				if (Defs.isMulti)
				{
					return "Bank (Multiplayer)";
				}
				if (GameConnect.isCompany)
				{
					return "Bank (Campaign)";
				}
				if (GameConnect.isSurvival)
				{
					return "Bank (Survival)";
				}
				return "Bank";
			}
			return "At Lobby";
		}
	}

	public sealed class ConnectionState : State
	{
		private readonly float _startTime;

		public float StartTime
		{
			get
			{
				return _startTime;
			}
		}

		public ConnectionState()
		{
			_startTime = Time.realtimeSinceStartup;
		}
	}

	public sealed class AwardState : State
	{
		private static readonly AwardState _instance = new AwardState();

		internal static AwardState Instance
		{
			get
			{
				return _instance;
			}
		}

		private AwardState()
		{
		}
	}

	public sealed class CloseState : State
	{
		private static readonly CloseState _instance = new CloseState();

		internal static CloseState Instance
		{
			get
			{
				return _instance;
			}
		}

		private CloseState()
		{
		}
	}

	[CompilerGenerated]
	internal sealed class _003CInitFyber_003Ed__58 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private string _003CuserId_003E5__1;

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
		public _003CInitFyber_003Ed__58(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			string value;
			string value2;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_003f;
			case 1:
				_003C_003E1__state = -1;
				goto IL_003f;
			case 2:
				_003C_003E1__state = -1;
				if (_initializedOnce)
				{
					break;
				}
				SetCookieAcceptPolicy();
				FyberLogger.EnableLogging(true);
				_003CuserId_003E5__1 = FriendsController.sharedController.id;
				if (!Application.isEditor)
				{
					AppsFlyer.setCustomerUserID(_003CuserId_003E5__1);
				}
				if (!TrainingController.TrainingCompleted || Initializer.Instance != null)
				{
					UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=olive>{0}</color>" : "{0}", "FreeAwardController: Postponing Fyber initialization till training is completed...");
					goto IL_00e7;
				}
				goto IL_00fb;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_00e7;
				}
				IL_00e7:
				if (!TrainingController.TrainingCompleted || Initializer.Instance != null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_00fb;
				IL_00fb:
				value = ((_003CuserId_003E5__1.Length > 4) ? _003CuserId_003E5__1.Substring(_003CuserId_003E5__1.Length - 4, 4) : _003CuserId_003E5__1);
				value2 = Storager.getInt("PayingUser").ToString(CultureInfo.InvariantCulture);
				new Dictionary<string, string>
				{
					{
						"pub0",
						SystemInfo.deviceModel
					},
					{ "pub1", value },
					{ "pub2", _003CuserId_003E5__1 },
					{ "pub3", value2 }
				};
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("{0}", "FreeAwardController: Initializing Fyber...");
				}
				User.SetAppVersion(GlobalGameController.AppVersion);
				User.SetDevice(SystemInfo.deviceModel);
				User.PutCustomValue("pg3d_paying", value2);
				User.PutCustomValue("RandomKey", _003CuserId_003E5__1);
				_initializedOnce = true;
				_003CuserId_003E5__1 = null;
				break;
				IL_003f:
				if (!FriendsController.isReadABTestAdvertConfig)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
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

	[CompilerGenerated]
	internal sealed class _003CStart_003Ed__60 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

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
		public _003CStart_003Ed__60(int _003C_003E1__state)
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
				goto IL_0043;
			case 1:
				_003C_003E1__state = -1;
				goto IL_0043;
			case 2:
				_003C_003E1__state = -1;
				goto IL_0069;
			case 3:
				_003C_003E1__state = -1;
				goto IL_00a3;
			case 4:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_0043:
				if (FriendsController.sharedController == null)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_0069;
				IL_0069:
				if (string.IsNullOrEmpty(FriendsController.sharedController.id))
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 2;
					return true;
				}
				CoroutineRunner.Instance.StartCoroutine(InitFyber());
				goto IL_00a3;
				IL_00a3:
				if (!_initializedOnce)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				break;
			}
			if (FriendsController.ServerTime < 0)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 4;
				return true;
			}
			try
			{
				AddEmptyEntryForAdvertTime(StarterPackModel.GetCurrentTimeByUnixTime((int)FriendsController.ServerTime));
			}
			finally
			{
				RemoveOldEntriesForAdvertTimes();
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

	public Camera renderCamera;

	public FreeAwardView view;

	private static FreeAwardController _instance;

	private int _adProviderIndex;

	private State _currentState = IdleState.Instance;

	private IDisposable _backSubscription;

	private static bool _initializedOnce;

	public static string appId = "00000";

	public static string securityToken = "00000000000000000000000000000000";

	private KeyValuePair<string, int> _advertCountCache = new KeyValuePair<string, int>(string.Empty, 0);

	private const string AdvertTimeDuringCurrentPeriodKey = "Ads.FreeCurrencyDuringCurrentPeriod";

	private bool? _simplifiedInterfaceCache;

	public const string PendingFreeAwardKey = "Ads.PendingRewardTimestamp";

	public static bool FreeAwardChestIsInIdleState
	{
		get
		{
			if (!(Instance == null))
			{
				return Instance.IsInState<IdleState>();
			}
			return true;
		}
	}

	public static FreeAwardController Instance
	{
		get
		{
			return _instance;
		}
	}

	public AdProvider Provider
	{
		get
		{
			return GetProviderByIndex(_adProviderIndex);
		}
	}

	private State CurrentState
	{
		get
		{
			return _currentState;
		}
		set
		{
			if (value != null)
			{
				SetCacheDirty();
				if (_backSubscription != null)
				{
					_backSubscription.Dispose();
					_backSubscription = null;
				}
				if (!(value is IdleState))
				{
					_backSubscription = BackSystem.Instance.Register(HandleClose, "Rewarded Video");
				}
				if (view != null)
				{
					view.CurrentState = value;
				}
				State currentState = _currentState;
				_currentState = value;
				EventHandler<StateEventArgs> stateChanged = this.StateChanged;
				if (stateChanged != null)
				{
					stateChanged(this, new StateEventArgs
					{
						State = value,
						OldState = currentState
					});
				}
			}
		}
	}

	internal static Future<object> FyberVideoLoaded { get; set; }

	public string CurrencyForAward
	{
		get
		{
			if (AdsConfigManager.Instance.LastLoadedConfig == null)
			{
				return ChestInLobbyPointMemento.DefaultCurrency;
			}
			ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
			if (chestInLobby == null)
			{
				return ChestInLobbyPointMemento.DefaultCurrency;
			}
			string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
			return chestInLobby.GetFinalAwardCurrency(playerCategory);
		}
	}

	internal bool SimplifiedInterface
	{
		get
		{
			if (!_simplifiedInterfaceCache.HasValue)
			{
				if (AdsConfigManager.Instance.LastLoadedConfig == null)
				{
					return ChestInLobbyPointMemento.DefaultSimplifiedInterface;
				}
				ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
				if (chestInLobby == null)
				{
					return ChestInLobbyPointMemento.DefaultSimplifiedInterface;
				}
				string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
				bool finalSimplifiedInterface = chestInLobby.GetFinalSimplifiedInterface(playerCategory);
				_simplifiedInterfaceCache = finalSimplifiedInterface;
			}
			return _simplifiedInterfaceCache.Value;
		}
	}

	internal event EventHandler AdvertTimeChanged;

	public event EventHandler<StateEventArgs> StateChanged;

	public AdProvider GetProviderByIndex(int index)
	{
		return AdProvider.Fyber;
	}

	internal int SwitchAdProvider()
	{
		int adProviderIndex = _adProviderIndex;
		AdProvider provider = Provider;
		_adProviderIndex++;
		if (provider == AdProvider.GoogleMobileAds)
		{
			MobileAdManager.Instance.DestroyVideoInterstitial();
		}
		if (Provider == AdProvider.GoogleMobileAds)
		{
			MobileAdManager.Instance.SwitchVideoIdGroup();
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(string.Format("Switching provider from {0} ({1}) to {2} ({3})", adProviderIndex, provider, _adProviderIndex, Provider));
		}
		return _adProviderIndex;
	}

	private void ResetAdProvider()
	{
		int adProviderIndex = _adProviderIndex;
		AdProvider provider = Provider;
		_adProviderIndex = 0;
		AdProvider provider2 = Provider;
		if (provider == AdProvider.GoogleMobileAds && provider != provider2)
		{
			MobileAdManager.Instance.DestroyVideoInterstitial();
		}
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(string.Format("Resetting AdProvider from {0} ({1}) to {2} ({3})", adProviderIndex, provider, _adProviderIndex, Provider));
		}
		MobileAdManager.Instance.ResetVideoAdUnitId();
	}

	public T TryGetState<T>() where T : State
	{
		return CurrentState as T;
	}

	public bool IsInState<T>() where T : State
	{
		return CurrentState is T;
	}

	internal void SetWatchState(DateTime nextTimeEnabled)
	{
		ResetAdProvider();
		WatchState watchState = (WatchState)(CurrentState = new WatchState(nextTimeEnabled));
		if (SimplifiedInterface && watchState.GetEstimatedTimeSpan() <= TimeSpan.FromMinutes(0.0))
		{
			HandleWatch();
		}
	}

	private void LoadVideo(string callerName = null)
	{
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		if (Instance.Provider == AdProvider.Fyber)
		{
			FyberVideoLoaded = LoadFyberVideo(callerName);
		}
	}

	public void HandleClose()
	{
		ButtonClickSound.TryPlayClick();
		if (IsInState<CloseState>())
		{
			HideButtonsShowAward();
		}
		if (!IsInState<AwardState>())
		{
			if (Provider == AdProvider.GoogleMobileAds)
			{
				MobileAdManager.Instance.DestroyVideoInterstitial();
			}
			CurrentState = IdleState.Instance;
		}
		else
		{
			HandleGetAward();
		}
	}

	public void HandleWatch()
	{
		LoadVideo("HandleWatch");
		CurrentState = new WaitingState();
	}

	public void HandleDeveloperSkip()
	{
		CurrentState = new WatchingState();
	}

	public int GiveAwardAndIncrementCount(DateTime now)
	{
		int num = AddAdvertTime(now);
		int index = num - 1;
		if (CurrencyForAward == "GemsCurrency")
		{
			BankController.AddGems(CountMoneyForAward(index));
		}
		else
		{
			BankController.AddCoins(CountMoneyForAward(index));
		}
		Storager.setString("Ads.PendingRewardTimestamp", string.Empty);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		return num;
	}

	public void HandleGetAward()
	{
		DateTime serverTimeOrFallbackToLocalUtc = FriendsController.GetServerTimeOrFallbackToLocalUtc();
		int num = GiveAwardAndIncrementCount(serverTimeOrFallbackToLocalUtc);
		AnalyticsStuff.LogDailyVideoRewarded(num);
		if (AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			CurrentState = CloseState.Instance;
			return;
		}
		ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
		if (chestInLobby == null)
		{
			CurrentState = CloseState.Instance;
			return;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
		List<double> finalRewardedVideoDelayMinutes = chestInLobby.GetFinalRewardedVideoDelayMinutes(playerCategory);
		if (num < finalRewardedVideoDelayMinutes.Count)
		{
			ResetAdProvider();
			DateTime dateTime = serverTimeOrFallbackToLocalUtc;
			TimeSpan timeSpan = TimeSpan.FromMinutes(finalRewardedVideoDelayMinutes[num]);
			DateTime dateTime2 = dateTime + timeSpan;
			bool flag = dateTime.Date < dateTime2.Date;
			CurrentState = (flag ? ((State)CloseState.Instance) : ((State)new WatchState(dateTime2)));
			if (Defs.IsDeveloperBuild)
			{
				string text = Json.Serialize(finalRewardedVideoDelayMinutes);
				UnityEngine.Debug.LogFormat("HandleGetAward(): `utcNow`: {0:s}, `delay`: {1:f2}, `nextTimeEnabled`: {2:s}, `CurrentState`: {3}, `delays`: {4}, `newCount`: {5}", dateTime, timeSpan.TotalMinutes, dateTime2, CurrentState, text, num);
			}
		}
		else
		{
			CurrentState = CloseState.Instance;
		}
	}

	internal static Future<object> LoadFyberVideo(string callerName = null)
	{
		if (callerName == null)
		{
			callerName = string.Empty;
		}
		Promise<object> promise = new Promise<object>();
		Action<Ad> onAdAvailable = null;
		Action<AdFormat> onAdNotAvailable = null;
		Action<RequestError> onRequestFail = null;
		onAdAvailable = delegate(Ad ad)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("LoadFyberVideo > AdAvailable: {{ format: {0}, placementId: '{1}' }}", ad.AdFormat, ad.PlacementId);
			}
			promise.SetResult(ad);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		};
		onAdNotAvailable = delegate(AdFormat adFormat)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("LoadFyberVideo > AdNotAvailable: {{ format: {0} }}", adFormat);
			}
			promise.SetResult(adFormat);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		};
		onRequestFail = delegate(RequestError requestError)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogFormat("LoadFyberVideo > RequestFail: {{ requestError: {0} }}", requestError.Description);
			}
			promise.SetResult(requestError);
			FyberCallback.AdAvailable -= onAdAvailable;
			FyberCallback.AdNotAvailable -= onAdNotAvailable;
			FyberCallback.RequestFail -= onRequestFail;
		};
		FyberCallback.AdAvailable += onAdAvailable;
		FyberCallback.AdNotAvailable += onAdNotAvailable;
		FyberCallback.RequestFail += onRequestFail;
		RequestFyberRewardedVideo(0);
		return promise.Future;
	}

	private static void RequestFyberRewardedVideo(int roundIndex)
	{
	}

	private void HideButtonsShowAward()
	{
		BankController instance = BankController.Instance;
		if (instance != null && instance.InterfaceEnabled)
		{
			instance.CurrentBankView.freeAwardButton.gameObject.SetActiveSafeSelf(false);
		}
	}

	internal bool AdvertCountLessThanLimit()
	{
		DateTime? serverTime = FriendsController.GetServerTime();
		if (!serverTime.HasValue)
		{
			return false;
		}
		if (AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			return false;
		}
		ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
		if (chestInLobby == null)
		{
			return false;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
		List<double> finalRewardedVideoDelayMinutes = chestInLobby.GetFinalRewardedVideoDelayMinutes(playerCategory);
		int count = finalRewardedVideoDelayMinutes.Count;
		int advertCountDuringCurrentPeriod;
		using (new ProfilerSample("AdvertCountLessThanLimit()->GetAdvertCountDuringCurrentPeriod()"))
		{
			advertCountDuringCurrentPeriod = GetAdvertCountDuringCurrentPeriod(serverTime.Value);
		}
		if (advertCountDuringCurrentPeriod >= count)
		{
			return false;
		}
		TimeSpan timeSpan = TimeSpan.FromMinutes(finalRewardedVideoDelayMinutes[advertCountDuringCurrentPeriod]);
		return !((serverTime.Value + timeSpan).Date > serverTime.Value.Date);
	}

	internal bool TimeTamperingDetected()
	{
		if (!Storager.hasKey("Ads.FreeCurrencyDuringCurrentPeriod"))
		{
			return false;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString("Ads.FreeCurrencyDuringCurrentPeriod")) as Dictionary<string, object>;
		if (dictionary == null)
		{
			return false;
		}
		string strB = dictionary.Keys.Min();
		return FriendsController.GetServerTimeOrFallbackToLocalUtc().ToString("yyyy-MM-dd").CompareTo(strB) < 0;
	}

	private static void RemoveOldEntriesForAdvertTimes()
	{
		if (!Storager.hasKey("Ads.FreeCurrencyDuringCurrentPeriod"))
		{
			return;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString("Ads.FreeCurrencyDuringCurrentPeriod")) as Dictionary<string, object>;
		if (dictionary != null && dictionary.Keys.Count >= 2)
		{
			string maxKey = dictionary.Keys.Max();
			string[] array = dictionary.Keys.Where((string k) => !k.Equals(maxKey, StringComparison.Ordinal)).ToArray();
			foreach (string key in array)
			{
				dictionary.Remove(key);
			}
			SetAdvertTime(dictionary);
		}
	}

	private static void AddEmptyEntryForAdvertTime(DateTime date)
	{
		string dateKey = date.ToString("yyyy-MM-dd");
		Action action = delegate
		{
			SetAdvertTime(new Dictionary<string, object> { 
			{
				dateKey,
				new string[0]
			} });
		};
		if (!Storager.hasKey("Ads.FreeCurrencyDuringCurrentPeriod"))
		{
			action();
			return;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(Storager.getString("Ads.FreeCurrencyDuringCurrentPeriod")) as Dictionary<string, object>;
		if (dictionary == null)
		{
			action();
		}
		else if (!dictionary.ContainsKey(dateKey))
		{
			dictionary.Add(dateKey, new string[0]);
			SetAdvertTime(dictionary);
		}
	}

	private void Awake()
	{
		if (_instance == null)
		{
			_instance = this;
		}
		CurrentState = IdleState.Instance;
	}

	private void OnDestroy()
	{
		_instance = null;
	}

	public static IEnumerator InitFyber()
	{
		while (!FriendsController.isReadABTestAdvertConfig)
		{
			yield return null;
		}
		yield return null;
		if (_initializedOnce)
		{
			yield break;
		}
		SetCookieAcceptPolicy();
		FyberLogger.EnableLogging(true);
		string userId = FriendsController.sharedController.id;
		if (!Application.isEditor)
		{
			AppsFlyer.setCustomerUserID(userId);
		}
		if (!TrainingController.TrainingCompleted || Initializer.Instance != null)
		{
			UnityEngine.Debug.LogFormat(Application.isEditor ? "<color=olive>{0}</color>" : "{0}", "FreeAwardController: Postponing Fyber initialization till training is completed...");
			while (!TrainingController.TrainingCompleted || Initializer.Instance != null)
			{
				yield return null;
			}
		}
		string value = ((userId.Length > 4) ? userId.Substring(userId.Length - 4, 4) : userId);
		string value2 = Storager.getInt("PayingUser").ToString(CultureInfo.InvariantCulture);
		new Dictionary<string, string>
		{
			{
				"pub0",
				SystemInfo.deviceModel
			},
			{ "pub1", value },
			{ "pub2", userId },
			{ "pub3", value2 }
		};
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogFormat("{0}", "FreeAwardController: Initializing Fyber...");
		}
		User.SetAppVersion(GlobalGameController.AppVersion);
		User.SetDevice(SystemInfo.deviceModel);
		User.PutCustomValue("pg3d_paying", value2);
		User.PutCustomValue("RandomKey", userId);
		_initializedOnce = true;
	}

	private static void OnNativeFyberError(string message)
	{
		UnityEngine.Debug.LogError("[Rilisoft] OnNativeFyberError: " + message);
	}

	private IEnumerator Start()
	{
		while (FriendsController.sharedController == null)
		{
			yield return null;
		}
		while (string.IsNullOrEmpty(FriendsController.sharedController.id))
		{
			yield return null;
		}
		CoroutineRunner.Instance.StartCoroutine(InitFyber());
		while (!_initializedOnce)
		{
			yield return null;
		}
		while (FriendsController.ServerTime < 0)
		{
			yield return null;
		}
		try
		{
			AddEmptyEntryForAdvertTime(StarterPackModel.GetCurrentTimeByUnixTime((int)FriendsController.ServerTime));
		}
		finally
		{
			RemoveOldEntriesForAdvertTimes();
		}
	}

	private void Update()
	{
		double num = 3.0;
		if (AdsConfigManager.Instance.LastLoadedConfig != null && AdsConfigManager.Instance.LastLoadedConfig.VideoConfig != null)
		{
			num = AdsConfigManager.Instance.LastLoadedConfig.VideoConfig.TimeoutWaitInSeconds;
		}
		WaitingState waitingState = TryGetState<WaitingState>();
		if (waitingState != null)
		{
			if (Application.isEditor || Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
			{
				if (!((double)(Time.realtimeSinceStartup - waitingState.StartTime) > num))
				{
					return;
				}
				if (Provider == AdProvider.GoogleMobileAds)
				{
					if (MobileAdManager.Instance.SwitchVideoAdUnitId())
					{
						SwitchAdProvider();
					}
				}
				else
				{
					SwitchAdProvider();
				}
				CurrentState = new ConnectionState();
			}
			else if (Provider == AdProvider.GoogleMobileAds)
			{
				if (MobileAdManager.Instance.VideoInterstitialState == MobileAdManager.State.Loaded)
				{
					CurrentState = new WatchingState();
				}
				else if (!string.IsNullOrEmpty(MobileAdManager.Instance.VideoAdFailedToLoadMessage))
				{
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.Log(string.Format("Admob loading failed after {0:F3}s of {1}. Keep waiting.", new object[2]
						{
							Time.realtimeSinceStartup - waitingState.StartTime,
							num
						}));
					}
					if (MobileAdManager.Instance.SwitchVideoAdUnitId())
					{
						int num2 = SwitchAdProvider();
						if (PromoActionsManager.MobileAdvert.AdProviders.Count > 0 && num2 >= PromoActionsManager.MobileAdvert.CountRoundReplaceProviders * PromoActionsManager.MobileAdvert.AdProviders.Count)
						{
							UnityEngine.Debug.Log(string.Format("Reporting connection issues after {0} switches.  Providers count {1}, rounds count {2}", new object[3]
							{
								num2,
								PromoActionsManager.MobileAdvert.AdProviders.Count,
								PromoActionsManager.MobileAdvert.CountRoundReplaceProviders
							}));
							CurrentState = new ConnectionState();
							return;
						}
					}
					LoadVideo("Update");
					CurrentState = new WaitingState(waitingState.StartTime);
				}
				else if ((double)(Time.realtimeSinceStartup - waitingState.StartTime) > num)
				{
					if (MobileAdManager.Instance.SwitchVideoAdUnitId())
					{
						SwitchAdProvider();
					}
					CurrentState = new ConnectionState();
				}
			}
			else if (Provider == AdProvider.Fyber)
			{
				if (FyberVideoLoaded != null && FyberVideoLoaded.IsCompleted)
				{
					if (FyberVideoLoaded.Result is Ad)
					{
						CurrentState = new WatchingState();
						return;
					}
					RequestError requestError = FyberVideoLoaded.Result as RequestError;
					if (Defs.IsDeveloperBuild)
					{
						UnityEngine.Debug.LogFormat("Fyber loading failed: {0}. Keep waiting.", (requestError != null) ? requestError.Description : ((FyberVideoLoaded.Result is AdFormat) ? "Not available" : "?"));
					}
					int num3 = SwitchAdProvider();
					if (PromoActionsManager.MobileAdvert.AdProviders.Count > 0 && num3 >= PromoActionsManager.MobileAdvert.CountRoundReplaceProviders * PromoActionsManager.MobileAdvert.AdProviders.Count)
					{
						CurrentState = new ConnectionState();
						return;
					}
					LoadVideo("Update");
					CurrentState = new WaitingState(waitingState.StartTime);
				}
				else if ((double)(Time.realtimeSinceStartup - waitingState.StartTime) > num)
				{
					SwitchAdProvider();
					CurrentState = new ConnectionState();
				}
			}
			else if ((double)(Time.realtimeSinceStartup - waitingState.StartTime) > num)
			{
				CurrentState = new ConnectionState();
			}
			return;
		}
		WatchingState watchingState = TryGetState<WatchingState>();
		if (watchingState != null)
		{
			if (Application.isEditor && Time.realtimeSinceStartup - watchingState.StartTime > 1f)
			{
				watchingState.SimulateCallbackInEditor("CLOSE_FINISHED");
			}
			if (watchingState.AdClosed.IsCompleted)
			{
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogFormat("[Rilisoft] Watching rewarded video completed: '{0}'", watchingState.AdClosed.Result);
				}
				Storager.setString("Ads.PendingRewardTimestamp", string.Empty);
				if (watchingState.AdClosed.Result.Equals("CLOSE_FINISHED", StringComparison.Ordinal))
				{
					CurrentState = AwardState.Instance;
				}
				else if (watchingState.AdClosed.Result.Equals("ERROR", StringComparison.Ordinal))
				{
					ResetAdProvider();
					CurrentState = new WatchState(DateTime.MinValue);
				}
				else if (watchingState.AdClosed.Result.Equals("CLOSE_ABORTED", StringComparison.Ordinal))
				{
					CurrentState = new WatchState(DateTime.MinValue);
				}
				else
				{
					UnityEngine.Debug.LogWarning(string.Format("[Rilisoft] Unsupported result for rewarded video: “{0}”", new object[1] { watchingState.AdClosed.Result }));
					CurrentState = new WatchState(DateTime.MinValue);
				}
			}
		}
		else
		{
			ConnectionState connectionState = TryGetState<ConnectionState>();
			if (connectionState != null && Time.realtimeSinceStartup - connectionState.StartTime > 3f)
			{
				CurrentState = IdleState.Instance;
			}
		}
	}

	public int GetAdvertCountDuringCurrentPeriod(DateTime now)
	{
		if (!Storager.hasKey("Ads.FreeCurrencyDuringCurrentPeriod"))
		{
			return 0;
		}
		string @string = Storager.getString("Ads.FreeCurrencyDuringCurrentPeriod");
		if (_advertCountCache.Key == @string)
		{
			return _advertCountCache.Value;
		}
		Dictionary<string, object> dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (Application.isEditor || Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogWarningFormat("Cannot parse '{0}' to dictionary: {1}", "Ads.FreeCurrencyDuringCurrentPeriod", @string);
			}
			int num = 0;
			_advertCountCache = new KeyValuePair<string, int>(@string, num);
			return num;
		}
		string text = now.ToString("yyyy-MM-dd");
		string text2 = dictionary.Keys.Max();
		if (text.CompareTo(text2) < 0)
		{
			int result = int.MaxValue;
			int.TryParse(text2.Replace("-", string.Empty), out result);
			result = Math.Max(10000000, result);
			_advertCountCache = new KeyValuePair<string, int>(@string, result);
			return result;
		}
		object value;
		if (dictionary.TryGetValue(text, out value))
		{
			int num2 = ((value as List<object>) ?? new List<object>()).OfType<string>().Count();
			_advertCountCache = new KeyValuePair<string, int>(@string, num2);
			return num2;
		}
		int num3 = 0;
		_advertCountCache = new KeyValuePair<string, int>(@string, num3);
		return num3;
	}

	public int AddAdvertTime(DateTime time)
	{
		string key = time.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		string item = time.ToString("T", CultureInfo.InvariantCulture);
		bool flag = Storager.hasKey("Ads.FreeCurrencyDuringCurrentPeriod");
		if (!flag)
		{
			Dictionary<string, object> advertTimeThenNotify = new Dictionary<string, object> { 
			{
				key,
				new List<string>(1) { item }
			} };
			SetAdvertTimeThenNotify(advertTimeThenNotify);
			return 1;
		}
		Dictionary<string, object> dictionary = (Json.Deserialize(flag ? Storager.getString("Ads.FreeCurrencyDuringCurrentPeriod") : "{}") as Dictionary<string, object>) ?? new Dictionary<string, object>();
		object value;
		if (dictionary.TryGetValue(key, out value))
		{
			List<string> list = ((value as List<object>) ?? new List<object>()).OfType<string>().ToList();
			list.Add(item);
			dictionary[key] = list.ToList();
			SetAdvertTimeThenNotify(dictionary);
			return list.Count;
		}
		dictionary[key] = new List<string>(1) { item };
		SetAdvertTimeThenNotify(dictionary);
		return 1;
	}

	public KeyValuePair<int, DateTime> LastAdvertShow(DateTime date)
	{
		KeyValuePair<int, DateTime> result = new KeyValuePair<int, DateTime>(int.MinValue, DateTime.MinValue);
		string key = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
		bool flag = Storager.hasKey("Ads.FreeCurrencyDuringCurrentPeriod");
		if (!flag)
		{
			return result;
		}
		object value;
		if (((Json.Deserialize(flag ? Storager.getString("Ads.FreeCurrencyDuringCurrentPeriod") : "{}") as Dictionary<string, object>) ?? new Dictionary<string, object>()).TryGetValue(key, out value))
		{
			List<object> list = (value as List<object>) ?? new List<object>();
			if (list.Count == 0)
			{
				return result;
			}
			List<string> list2 = list.OfType<string>().ToList();
			if (list2.Count == 0)
			{
				return result;
			}
			string text = list2.Max();
			DateTime result2;
			if (DateTime.TryParseExact(text, "T", CultureInfo.InvariantCulture, DateTimeStyles.None, out result2))
			{
				return new KeyValuePair<int, DateTime>(value: new DateTime(date.Year, date.Month, date.Day, result2.Hour, result2.Minute, result2.Second, DateTimeKind.Utc), key: list2.Count - 1);
			}
			UnityEngine.Debug.LogWarning("Couldnot parse last time advert shown: " + text);
			return result;
		}
		return result;
	}

	private void SetAdvertTimeThenNotify(Dictionary<string, object> d)
	{
		SetAdvertTime(d);
		EventHandler advertTimeChanged = this.AdvertTimeChanged;
		if (advertTimeChanged != null)
		{
			advertTimeChanged(this, EventArgs.Empty);
		}
	}

	private static void SetAdvertTime(Dictionary<string, object> d)
	{
		string val = ((d != null) ? Json.Serialize(d) : "{}");
		Storager.setString("Ads.FreeCurrencyDuringCurrentPeriod", val);
	}

	private static void SetCookieAcceptPolicy()
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log(string.Format("Setting cookie accept policy is dumb on {0}.", new object[1] { Application.platform }));
		}
	}

	public static int CountMoneyForAward(int index)
	{
		if (AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			return ChestInLobbyPointMemento.DefaultAward;
		}
		ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
		if (chestInLobby == null)
		{
			return ChestInLobbyPointMemento.DefaultAward;
		}
		return 2;
	}

	private void SetCacheDirty()
	{
		_simplifiedInterfaceCache = null;
	}
}
