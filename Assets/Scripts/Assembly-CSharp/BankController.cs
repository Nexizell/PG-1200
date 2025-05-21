using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

public sealed class BankController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CWaitForIndicationGems_003Ed__70 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public string currency;

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
		public _003CWaitForIndicationGems_003Ed__70(int _003C_003E1__state)
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
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (!canShowIndication)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			if (currency == "TicketsCurrency")
			{
				FireTicketsAddedEvent(0);
			}
			else
			{
				CoinsMessage.FireCoinsAddedEvent(currency == "GemsCurrency");
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

	public const int InitialIosGems = 0;

	public const int InitialIosCoins = 0;

	public static bool isBankActive;

	public AbstractBankView bankViewCommon;

	public AbstractBankView bankViewOneCurrency;

	public AbstractBankView bankViewTickets;

	public GameObject uiRoot;

	public ChestBonusView bonusDetailView;

	public AudioClip deniedClip;

	public static bool canShowIndication;

	public static bool isBankOneCurrency;

	private bool stateMainMenuOnOpenBank;

	private bool firsEnterToBankOccured;

	private float tmOfFirstEnterTheBank;

	private bool _lockInterfaceEnabledCoroutine;

	private int _counterEn;

	private IDisposable _backSubscription;

	private string m_lastPrintedDismissReason = string.Empty;

	private AbstractBankView m_currentBankView;

	private string _debugMessage = string.Empty;

	private bool _escapePressed;

	private static float _lastTimePurchaseButtonPressed;

	private float m_timeWhenReutrnedFromPause = float.MinValue;

	private static BankController _instance;

	private readonly Lazy<bool> _timeTamperingDetected = new Lazy<bool>(delegate
	{
		bool num = FreeAwardController.Instance.TimeTamperingDetected();
		if (num && Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogWarning("FreeAwardController: time tampering detected in Bank.");
		}
		return num;
	});

	private AbstractBankView NormalBankView
	{
		get
		{
			if (isBankOneCurrency)
			{
				return bankViewOneCurrency;
			}
			return bankViewCommon;
		}
	}

	public AbstractBankView CurrentBankView
	{
		get
		{
			return m_currentBankView;
		}
		private set
		{
			m_currentBankView = value;
		}
	}

	public bool InterfaceEnabled
	{
		get
		{
			if (CurrentBankView != null)
			{
				return CurrentBankView.gameObject.activeInHierarchy;
			}
			return false;
		}
	}

	public bool InterfaceEnabledCoroutineLocked
	{
		get
		{
			return _lockInterfaceEnabledCoroutine;
		}
	}

	public static BankController Instance
	{
		get
		{
			return _instance;
		}
	}

	private List<Dictionary<string, object>> CurrentInappBonuses { get; set; }

	private float TimeWhenReturnedFromPause
	{
		get
		{
			return m_timeWhenReutrnedFromPause;
		}
		set
		{
			m_timeWhenReutrnedFromPause = value;
		}
	}

	public static event Action<int> TicketsAdded;

	public static event Action OpenBank;

	public static event Action CloseBank;

	public static event Action onUpdateMoney;

	public event EventHandler BackRequested
	{
		add
		{
			if (bankViewTickets != null)
			{
				bankViewTickets.BackButtonPressed += value;
			}
			if (bankViewCommon != null)
			{
				bankViewCommon.BackButtonPressed += value;
			}
			if (bankViewOneCurrency != null)
			{
				bankViewOneCurrency.BackButtonPressed += value;
			}
			EscapePressed += value;
		}
		remove
		{
			if (bankViewTickets != null)
			{
				bankViewTickets.BackButtonPressed -= value;
			}
			if (bankViewCommon != null)
			{
				bankViewCommon.BackButtonPressed -= value;
			}
			if (bankViewOneCurrency != null)
			{
				bankViewOneCurrency.BackButtonPressed -= value;
			}
			EscapePressed -= value;
		}
	}

	private event EventHandler EscapePressed;

	public static void GetCurrentTickets(out int earnedTickets, out int purchasedTickets)
	{
		earnedTickets = Storager.getInt(string.Format("{0}_{1}", new object[2]
		{
			"TicketsCurrency",
			AnalyticsConstants.AccrualType.Earned
		}));
		purchasedTickets = Storager.getInt(string.Format("{0}_{1}", new object[2]
		{
			"TicketsCurrency",
			AnalyticsConstants.AccrualType.Purchased
		}));
	}

	public static int NumOfTickets()
	{
		int earnedTickets;
		int purchasedTickets;
		GetCurrentTickets(out earnedTickets, out purchasedTickets);
		return earnedTickets + purchasedTickets;
	}

	public static void UpdateAllIndicatorsMoney()
	{
		if (BankController.onUpdateMoney != null)
		{
			BankController.onUpdateMoney();
		}
	}

	public static void IndicateSpendingMoney(ItemPrice price)
	{
		if (price.Currency == "TicketsCurrency")
		{
			FireTicketsAddedEvent(price.Price);
		}
		else
		{
			CoinsMessage.FireCoinsAddedEvent(price.Currency == "GemsCurrency");
		}
	}

	public static void SpendMoney(ItemPrice price)
	{
		if (price.Currency == "TicketsCurrency")
		{
			string key = string.Format("{0}_{1}", new object[2]
			{
				"TicketsCurrency",
				AnalyticsConstants.AccrualType.Purchased
			});
			int @int = Storager.getInt(key);
			if (@int >= price.Price)
			{
				@int -= price.Price;
				Storager.setInt(key, @int);
			}
			else
			{
				Storager.setInt(key, 0);
				int num = price.Price - @int;
				string key2 = string.Format("{0}_{1}", new object[2]
				{
					"TicketsCurrency",
					AnalyticsConstants.AccrualType.Earned
				});
				int int2 = Storager.getInt(key2);
				Storager.setInt(key2, Mathf.Max(0, int2 - num));
			}
		}
		else
		{
			Storager.setInt(price.Currency, Mathf.Max(0, Storager.getInt(price.Currency) - price.Price));
		}
		ShopNGUIController.SpendBoughtCurrency(price.Currency, price.Price);
		if (Application.platform != RuntimePlatform.IPhonePlayer)
		{
			PlayerPrefs.Save();
		}
		if (ABTestController.useBuffSystem)
		{
			BuffSystem.instance.OnSomethingPurchased();
		}
	}

	public static void GiveInitialNumOfCoins()
	{
		if (!Storager.hasKey("Coins"))
		{
			Storager.setInt("Coins", 0);
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
		}
		if (!Storager.hasKey("GemsCurrency"))
		{
			switch (BuildSettings.BuildTargetPlatform)
			{
			case RuntimePlatform.IPhonePlayer:
				Storager.setInt("GemsCurrency", 0);
				break;
			case RuntimePlatform.Android:
				Storager.setInt("GemsCurrency", 0);
				break;
			}
			if (Application.platform != RuntimePlatform.IPhonePlayer)
			{
				PlayerPrefs.Save();
			}
		}
	}

	public void SetInterfaceEnabledWithDesiredCurrency(bool enabled, string desiredCurrency, bool isNotEnoughCurrency = false)
	{
		if (enabled)
		{
			if (BankController.OpenBank != null)
			{
				BankController.OpenBank();
			}
		}
		else if (BankController.CloseBank != null)
		{
			BankController.CloseBank();
		}
		if (MainMenuController.sharedController != null && !MainMenuController.sharedController.InMiniGamesScreen)
		{
			if (enabled)
			{
				stateMainMenuOnOpenBank = MainMenuController.sharedController.gameObject.activeSelf;
				MainMenuController.sharedController.gameObject.SetActive(false);
			}
			else
			{
				MainMenuController.sharedController.gameObject.SetActive(stateMainMenuOnOpenBank);
			}
		}
		SetInterfaceEnabledCore((desiredCurrency == "TicketsCurrency") ? bankViewTickets : NormalBankView, enabled, desiredCurrency, isNotEnoughCurrency);
	}

	private void SetInterfaceEnabledCore(AbstractBankView bankViewToShow, bool value, string desiredCurrency, bool isNotEnoughCurrency)
	{
		if (!value && _backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
		_lockInterfaceEnabledCoroutine = true;
		int num = _counterEn++;
		UnityEngine.Debug.Log("InterfaceEnabledCoroutine " + num + " start: " + value.ToString());
		try
		{
			if (value && !firsEnterToBankOccured)
			{
				firsEnterToBankOccured = true;
				tmOfFirstEnterTheBank = Time.realtimeSinceStartup;
			}
			if (value)
			{
				UpdateCurrenntInappBonus(null);
			}
			if (bankViewToShow != CurrentBankView && CurrentBankView != null)
			{
				CurrentBankView.gameObject.SetActive(false);
				CurrentBankView = null;
			}
			if (bankViewToShow != null)
			{
				if (value)
				{
					bankViewToShow.DesiredCurrency = desiredCurrency;
					bankViewToShow.CurrencyThatNotEnough = (isNotEnoughCurrency ? desiredCurrency : null);
				}
				bankViewToShow.gameObject.SetActive(value);
				CurrentBankView = (value ? bankViewToShow : null);
			}
			uiRoot.SetActive(value);
			if (!value)
			{
				ActivityIndicator.IsActiveIndicator = false;
			}
			FreeAwardShowHandler.CheckShowChest(value);
			if (value)
			{
				coinsShop.thisScript.RefreshProductsIfNeed("BankController.SetInterfaceEnabledCore()");
				OnEventX3AmazonBonusUpdated();
			}
		}
		finally
		{
			if (value)
			{
				if (_backSubscription != null)
				{
					_backSubscription.Dispose();
				}
				_backSubscription = BackSystem.Instance.Register(HandleEscape, "Bank");
			}
			if (Device.IsLoweMemoryDevice)
			{
				ActivityIndicator.ClearMemory();
			}
			_lockInterfaceEnabledCoroutine = false;
			UnityEngine.Debug.Log("InterfaceEnabledCoroutine " + num + " stop: " + value.ToString());
		}
	}

	private void HandleEscape()
	{
		if (FreeAwardController.Instance != null && !FreeAwardController.Instance.IsInState<FreeAwardController.IdleState>())
		{
			FreeAwardController.Instance.HandleClose();
			_escapePressed = false;
		}
		else
		{
			_escapePressed = true;
		}
	}

	private void Awake()
	{
		BalanceController.UpdatedBankView += BalanceController_UpdatedBankView;
		InappBonuessController.OnGiveInappBonus += InappBonuessController_OnGiveInappBonus;
		GiveInitialNumOfCoins();
	}

	private void InappBonuessController_OnGiveInappBonus(InappRememberedBonus bonus)
	{
		TimeWhenReturnedFromPause = float.MinValue;
	}

	private void Start()
	{
		_instance = this;
		PromoActionsManager.EventX3Updated += OnEventX3Updated;
		if (bankViewTickets != null)
		{
			bankViewTickets.PurchaseButtonPressed += HandlePurchaseButtonPressed;
		}
		if (bankViewCommon != null)
		{
			bankViewCommon.PurchaseButtonPressed += HandlePurchaseButtonPressed;
		}
		if (bankViewOneCurrency != null)
		{
			bankViewOneCurrency.PurchaseButtonPressed += HandlePurchaseButtonPressed;
		}
		PromoActionsManager.EventAmazonX3Updated += OnEventX3AmazonBonusUpdated;
		bankViewCommon.freeAwardButton.gameObject.SetActiveSafeSelf(false);
		bankViewOneCurrency.freeAwardButton.gameObject.SetActiveSafeSelf(false);
		bankViewTickets.freeAwardButton.gameObject.SetActiveSafeSelf(false);
	}

	private void BalanceController_UpdatedBankView()
	{
		try
		{
			CurrentInappBonuses = BalanceController.GetCurrentInnapBonus();
			if (CurrentBankView != null && InterfaceEnabled)
			{
				CurrentBankView.UpdateUi();
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in BalanceController_UpdatedBankView: {0}", ex);
		}
	}

	private void OnEventX3Updated()
	{
		try
		{
			if (CurrentBankView != null)
			{
				CurrentBankView.UpdateUi();
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in OnEventX3Updated: {0}", ex);
		}
	}

	private void OnEventX3AmazonBonusUpdated()
	{
		if (CurrentBankView == null || CurrentBankView.eventX3AmazonBonusWidget == null)
		{
			return;
		}
		try
		{
			GameObject obj = CurrentBankView.eventX3AmazonBonusWidget.gameObject;
			obj.SetActive(PromoActionsManager.sharedManager.IsAmazonEventX3Active);
			UILabel[] componentsInChildren = obj.GetComponentsInChildren<UILabel>();
			UILabel uILabel = CurrentBankView.Map((AbstractBankView b) => b.amazonEventCaptionLabel) ?? componentsInChildren.FirstOrDefault((UILabel l) => "CaptionLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase));
			PromoActionsManager.AmazonEventInfo o = PromoActionsManager.sharedManager.Map((PromoActionsManager p) => p.AmazonEvent);
			if (uILabel != null)
			{
				uILabel.text = o.Map((PromoActionsManager.AmazonEventInfo e) => e.Caption) ?? string.Empty;
			}
			UILabel[] obj2 = (CurrentBankView.Map((AbstractBankView b) => b.amazonEventTitleLabel) ?? componentsInChildren.FirstOrDefault((UILabel l) => "TitleLabel".Equals(l.name, StringComparison.OrdinalIgnoreCase))).Map((UILabel t) => t.GetComponentsInChildren<UILabel>()) ?? new UILabel[0];
			float num = o.Map((PromoActionsManager.AmazonEventInfo e) => e.Percentage);
			string text = LocalizationStore.Get("Key_1672");
			UILabel[] array = obj2;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].text = ((!"Key_1672".Equals(text, StringComparison.OrdinalIgnoreCase)) ? string.Format(text, new object[1] { num }) : string.Empty);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in OnEventX3AmazonBonusUpdated: {0}", ex);
		}
	}

	private void UpdateCurrenntInappBonus(Action onUpdate)
	{
		List<Dictionary<string, object>> currentInnapBonus = BalanceController.GetCurrentInnapBonus();
		if (!InappBonuessController.AreInappBonusesEquals(currentInnapBonus, CurrentInappBonuses))
		{
			if (onUpdate != null)
			{
				onUpdate();
			}
			CurrentInappBonuses = currentInnapBonus;
		}
	}

	private void CheckInappBonusActionChanged()
	{
		try
		{
			Action onUpdate = delegate
			{
				if (InterfaceEnabled && CurrentBankView != null)
				{
					CurrentBankView.UpdateUi();
				}
			};
			UpdateCurrenntInappBonus(onUpdate);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in BankController.Update: {0}", ex);
		}
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			TimeWhenReturnedFromPause = Time.realtimeSinceStartup;
		}
	}

	private void Update()
	{
		if (!InterfaceEnabled)
		{
			_escapePressed = false;
			return;
		}
		if (FriendsController.ServerTime != -1 || Time.realtimeSinceStartup - TimeWhenReturnedFromPause > 10f)
		{
			CheckInappBonusActionChanged();
		}
		UpdateMiscUiOnView(bankViewCommon);
		UpdateMiscUiOnView(bankViewOneCurrency);
		UpdateMiscUiOnView(bankViewTickets);
		EventHandler escapePressed = this.EscapePressed;
		if (_escapePressed && escapePressed != null)
		{
			escapePressed(this, EventArgs.Empty);
			_escapePressed = false;
		}
	}

	private void LateUpdate()
	{
		if (InterfaceEnabled && ExperienceController.sharedController != null && !_lockInterfaceEnabledCoroutine)
		{
			ExperienceController.sharedController.isShowRanks = false;
		}
	}

	private void UpdateMiscUiOnView(AbstractBankView bankView)
	{
		if (bankView == null || !bankView.gameObject.activeSelf)
		{
			return;
		}
		if (coinsShop.IsWideLayoutAvailable)
		{
			bankView.ConnectionProblemLabelEnabled = false;
			bankView.CrackersWarningLabelEnabled = true;
			bankView.AreBankContentsEnabled = false;
			bankView.PurchaseSuccessfulLabelEnabled = false;
		}
		else
		{
			if (!(coinsShop.thisScript != null))
			{
				return;
			}
			ActivityIndicator.IsActiveIndicator = StoreKitEventListener.purchaseInProcess;
			bankView.touchBlockObject.SetActiveSafe(StoreKitEventListener.purchaseInProcess);
			if (coinsShop.IsNoConnection)
			{
				if (Time.realtimeSinceStartup - tmOfFirstEnterTheBank > 3f)
				{
					bankView.ConnectionProblemLabelEnabled = true;
				}
				bankView.AreBankContentsEnabled = false;
				bankView.PurchaseSuccessfulLabelEnabled = false;
			}
			else
			{
				bankView.ConnectionProblemLabelEnabled = false;
				bankView.AreBankContentsEnabled = true;
			}
			bankView.PurchaseSuccessfulLabelEnabled = StoreKitEventListener.PurchasedRecently;
		}
	}

	private void OnDestroy()
	{
		PromoActionsManager.EventX3Updated -= OnEventX3Updated;
		PromoActionsManager.EventAmazonX3Updated -= OnEventX3AmazonBonusUpdated;
		if (bankViewTickets != null)
		{
			bankViewTickets.PurchaseButtonPressed -= HandlePurchaseButtonPressed;
		}
		if (bankViewCommon != null)
		{
			bankViewCommon.PurchaseButtonPressed -= HandlePurchaseButtonPressed;
		}
		if (bankViewOneCurrency != null)
		{
			bankViewOneCurrency.PurchaseButtonPressed -= HandlePurchaseButtonPressed;
		}
		BalanceController.UpdatedBankView -= BalanceController_UpdatedBankView;
		InappBonuessController.OnGiveInappBonus -= InappBonuessController_OnGiveInappBonus;
	}

	private void HandlePurchaseButtonPressed(AbstractBankViewItem item)
	{
		if (item is FreeTicketsBankViewItem)
		{
			ButtonClickSound.Instance.PlayClick();
			if (TicketsRewardedVideoController.Instance != null)
			{
				TicketsRewardedVideoController.Instance.OnTicketsRewardedVideoButton();
			}
		}
		else if (item is ExchangeBankViewItem)
		{
			UnityEngine.Debug.LogFormat(">>> Exchange {0} pressed", item.ExchangeInfo.InAppId);
			if (Storager.getInt("GemsCurrency") >= item.ExchangeInfo.GemsPrice)
			{
				int gemsPrice = item.ExchangeInfo.GemsPrice;
				gemsPrice *= ((gemsPrice <= 0) ? 1 : (-1));
				int @int = Storager.getInt("GemsCurrency");
				Storager.setInt("GemsCurrency", @int + gemsPrice);
				if (item is ExchangeTicketsBankViewItem)
				{
					AddTickets(item.ExchangeInfo.CurrencyCount, true, AnalyticsConstants.AccrualType.Purchased);
					AnalyticsStuff.MiniGamesSales(item.ExchangeInfo.GemsPrice.ToString(), false);
				}
				else
				{
					AddCoins(item.ExchangeInfo.CurrencyCount);
				}
				StoreKitEventListener.RememberSuccessfulPurchaseTime();
			}
			else
			{
				if (deniedClip != null && Defs.isSoundFX)
				{
					NGUITools.PlaySound(deniedClip);
				}
				CurrentBankView.GoToGems(true, true, null);
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64 && Time.realtimeSinceStartup - _lastTimePurchaseButtonPressed < 1f)
		{
			UnityEngine.Debug.Log("Bank button pressed but ignored");
		}
		else
		{
			_lastTimePurchaseButtonPressed = Time.realtimeSinceStartup;
			if (StoreKitEventListener.purchaseInProcess)
			{
				UnityEngine.Debug.Log("Cannot perform request while purchase is in progress.");
			}
			if (coinsShop.thisScript != null)
			{
				coinsShop.thisScript.HandlePurchaseButton(item);
			}
			else
			{
				UnityEngine.Debug.LogErrorFormat("HandlePurchaseButtonPressed coinsShop.thisScript == null");
			}
		}
	}

	public static void AddCoins(int count, bool needIndication = true, AnalyticsConstants.AccrualType accrualType = AnalyticsConstants.AccrualType.Earned)
	{
		int @int = Storager.getInt("Coins");
		Storager.setInt("Coins", @int + count);
		AnalyticsFacade.CurrencyAccrual(count, "Coins", accrualType);
		if (needIndication)
		{
			CoinsMessage.FireCoinsAddedEvent();
		}
	}

	public static void AddGems(int count, bool needIndication = true, AnalyticsConstants.AccrualType accrualType = AnalyticsConstants.AccrualType.Earned)
	{
		int @int = Storager.getInt("GemsCurrency");
		Storager.setInt("GemsCurrency", @int + count);
		AnalyticsFacade.CurrencyAccrual(count, "GemsCurrency", accrualType);
		if (needIndication)
		{
			CoinsMessage.FireCoinsAddedEvent(true);
		}
	}

	public static void AddTickets(int count, bool needIndication = true, AnalyticsConstants.AccrualType accrualType = AnalyticsConstants.AccrualType.Earned)
	{
		string key = string.Format("{0}_{1}", new object[2] { "TicketsCurrency", accrualType });
		int @int = Storager.getInt(key);
		Storager.setInt(key, @int + count);
		AnalyticsFacade.CurrencyAccrual(count, "TicketsCurrency", accrualType);
		if (needIndication)
		{
			FireTicketsAddedEvent(count);
		}
	}

	private static void FireTicketsAddedEvent(int count)
	{
		Action<int> ticketsAdded = BankController.TicketsAdded;
		if (ticketsAdded != null)
		{
			ticketsAdded(count);
		}
	}

	public static IEnumerator WaitForIndicationGems(string currency)
	{
		while (!canShowIndication)
		{
			yield return null;
		}
		if (currency == "TicketsCurrency")
		{
			FireTicketsAddedEvent(0);
		}
		else
		{
			CoinsMessage.FireCoinsAddedEvent(currency == "GemsCurrency");
		}
	}

	public void FreeAwardButtonClick()
	{
		ButtonClickSound.TryPlayClick();
		if (FreeAwardController.Instance == null || !FreeAwardController.Instance.AdvertCountLessThanLimit() || AdsConfigManager.Instance.LastLoadedConfig == null)
		{
			return;
		}
		ChestInLobbyPointMemento chestInLobby = AdsConfigManager.Instance.LastLoadedConfig.AdPointsConfig.ChestInLobby;
		if (chestInLobby == null)
		{
			return;
		}
		string playerCategory = AdsConfigManager.GetPlayerCategory(AdsConfigManager.Instance.LastLoadedConfig);
		List<double> finalRewardedVideoDelayMinutes = chestInLobby.GetFinalRewardedVideoDelayMinutes(playerCategory);
		if (finalRewardedVideoDelayMinutes.Count == 0)
		{
			return;
		}
		DateTime? serverTime = FriendsController.GetServerTime();
		if (serverTime.HasValue)
		{
			DateTime date = serverTime.Value.Date;
			KeyValuePair<int, DateTime> keyValuePair = FreeAwardController.Instance.LastAdvertShow(date);
			int num = Math.Max(0, keyValuePair.Key + 1);
			if (num <= finalRewardedVideoDelayMinutes.Count)
			{
				DateTime dateTime = ((keyValuePair.Value < date) ? date : keyValuePair.Value);
				FreeAwardController.Instance.SetWatchState(dateTime + TimeSpan.FromMinutes(finalRewardedVideoDelayMinutes[num]));
			}
		}
	}

	static BankController()
	{
		BankController.TicketsAdded = null;
		canShowIndication = true;
		isBankOneCurrency = false;
	}
}
