using System;
using System.Collections.Generic;
using System.Linq;
using I2.Loc;

using Rilisoft;
using UnityEngine;

public abstract class AbstractBankView : MonoBehaviour
{
	public static int[] discountsCoins = new int[7] { 0, 0, 7, 10, 12, 15, 33 };

	public static int[] discountsGems = new int[7] { 0, 0, 7, 10, 12, 15, 33 };

	public GameObject[] x3BankElements;

	public GameObject[] usualBankElements;

	public TweenColor colorBlinkForX3;

	public ButtonHandler backButton;

	public UILabel connectionProblemLabel;

	public UILabel crackersWarningLabel;

	public UILabel notEnoughCoinsLabel;

	public UILabel notEnoughGemsLabel;

	public UILabel notEnoughTicketsLabel;

	public UISprite purchaseSuccessfulLabel;

	public UILabel[] eventX3RemainTime;

	public UIButton freeAwardButton;

	public UIWidget eventX3AmazonBonusWidget;

	public UILabel amazonEventCaptionLabel;

	public UILabel amazonEventTitleLabel;

	public GameObject[] AdFreeLabels;

	public GameObject touchBlockObject;

	private UILabel _freeAwardButtonLagelCont;

	private float _lastUpdateTime;

	private string _localizeSaleLabel;

	private StoreKitEventListener _storeKitEventListener;

	private bool m_isX3Bank;

	public AbstractBankViewItem LastClickedItem { get; protected set; }

	public static IList<PurchaseEventArgs> goldPurchasesInfo
	{
		get
		{
			return new List<PurchaseEventArgs>
			{
				new PurchaseEventArgs(0, 0, 0m, PurchaseEventArgs.PurchaseType.Coins, discountsCoins[0]),
				new PurchaseEventArgs(1, 0, 0m, PurchaseEventArgs.PurchaseType.Coins, discountsCoins[1]),
				new PurchaseEventArgs(2, 0, 0m, PurchaseEventArgs.PurchaseType.Coins, discountsCoins[2]),
				new PurchaseEventArgs(3, 0, 0m, PurchaseEventArgs.PurchaseType.Coins, discountsCoins[3]),
				new PurchaseEventArgs(4, 0, 0m, PurchaseEventArgs.PurchaseType.Coins, discountsCoins[4]),
				new PurchaseEventArgs(5, 0, 0m, PurchaseEventArgs.PurchaseType.Coins, discountsCoins[5]),
				new PurchaseEventArgs(6, 0, 0m, PurchaseEventArgs.PurchaseType.Coins, discountsCoins[6])
			};
		}
	}

	public static IList<PurchaseEventArgs> gemsPurchasesInfo
	{
		get
		{
			return new List<PurchaseEventArgs>
			{
				new PurchaseEventArgs(0, 0, 0m, PurchaseEventArgs.PurchaseType.Gems, discountsGems[0]),
				new PurchaseEventArgs(1, 0, 0m, PurchaseEventArgs.PurchaseType.Gems, discountsGems[1]),
				new PurchaseEventArgs(2, 0, 0m, PurchaseEventArgs.PurchaseType.Gems, discountsGems[2]),
				new PurchaseEventArgs(3, 0, 0m, PurchaseEventArgs.PurchaseType.Gems, discountsGems[3]),
				new PurchaseEventArgs(4, 0, 0m, PurchaseEventArgs.PurchaseType.Gems, discountsGems[4]),
				new PurchaseEventArgs(5, 0, 0m, PurchaseEventArgs.PurchaseType.Gems, discountsGems[5]),
				new PurchaseEventArgs(6, 0, 0m, PurchaseEventArgs.PurchaseType.Gems, discountsGems[6])
			};
		}
	}

	public static IList<PurchaseEventArgs> offersPurchasesInfo
	{
		get
		{
			return new List<PurchaseEventArgs>
			{
				new PurchaseEventArgs(0, 0, 0m, PurchaseEventArgs.PurchaseType.Offer, 0),
				new PurchaseEventArgs(1, 0, 0m, PurchaseEventArgs.PurchaseType.Offer, 0),
				new PurchaseEventArgs(2, 0, 0m, PurchaseEventArgs.PurchaseType.Offer, 0),
				new PurchaseEventArgs(3, 0, 0m, PurchaseEventArgs.PurchaseType.Offer, 0),
				new PurchaseEventArgs(4, 0, 0m, PurchaseEventArgs.PurchaseType.Offer, 0),
				new PurchaseEventArgs(5, 0, 0m, PurchaseEventArgs.PurchaseType.Offer, 0),
				new PurchaseEventArgs(6, 0, 0m, PurchaseEventArgs.PurchaseType.Offer, 0)
			};
		}
	}

	public string CurrencyThatNotEnough { get; set; }

	public string DesiredCurrency { get; set; }

	public bool ConnectionProblemLabelEnabled
	{
		get
		{
			if (!(connectionProblemLabel != null))
			{
				return false;
			}
			return connectionProblemLabel.gameObject.GetActive();
		}
		set
		{
			if (connectionProblemLabel != null)
			{
				connectionProblemLabel.gameObject.SetActive(value);
			}
		}
	}

	public bool CrackersWarningLabelEnabled
	{
		get
		{
			if (!(crackersWarningLabel != null))
			{
				return false;
			}
			return crackersWarningLabel.gameObject.GetActive();
		}
		set
		{
			if (crackersWarningLabel != null)
			{
				crackersWarningLabel.gameObject.SetActive(value);
			}
		}
	}

	private bool NotEnoughTicketsLabelEnabled
	{
		get
		{
			if (!(notEnoughTicketsLabel != null))
			{
				return false;
			}
			return notEnoughTicketsLabel.gameObject.GetActive();
		}
		set
		{
			if (notEnoughTicketsLabel != null)
			{
				notEnoughTicketsLabel.gameObject.SetActiveSafeSelf(value);
			}
		}
	}

	private bool NotEnoughCoinsLabelEnabled
	{
		get
		{
			if (!(notEnoughCoinsLabel != null))
			{
				return false;
			}
			return notEnoughCoinsLabel.gameObject.GetActive();
		}
		set
		{
			if (notEnoughCoinsLabel != null)
			{
				notEnoughCoinsLabel.gameObject.SetActiveSafeSelf(value);
			}
		}
	}

	private bool NotEnoughGemsLabelEnabled
	{
		get
		{
			if (!(notEnoughGemsLabel != null))
			{
				return false;
			}
			return notEnoughGemsLabel.gameObject.GetActive();
		}
		set
		{
			if (notEnoughGemsLabel != null)
			{
				notEnoughGemsLabel.gameObject.SetActiveSafeSelf(value);
			}
		}
	}

	public bool PurchaseSuccessfulLabelEnabled
	{
		get
		{
			if (!(purchaseSuccessfulLabel != null))
			{
				return false;
			}
			return purchaseSuccessfulLabel.gameObject.GetActive();
		}
		set
		{
			if (purchaseSuccessfulLabel != null)
			{
				purchaseSuccessfulLabel.gameObject.SetActive(value);
			}
		}
	}

	public virtual bool IsX3Bank
	{
		get
		{
			return m_isX3Bank;
		}
		set
		{
			m_isX3Bank = value;
			for (int i = 0; i < x3BankElements.Length; i++)
			{
				if (x3BankElements[i] != null)
				{
					x3BankElements[i].SetActiveSafeSelf(value);
				}
			}
			for (int j = 0; j < usualBankElements.Length; j++)
			{
				if (usualBankElements[j] != null)
				{
					usualBankElements[j].SetActiveSafeSelf(!value);
				}
			}
			foreach (AbstractBankViewItem item in AllItems())
			{
				item.isX3Item = value;
			}
		}
	}

	public abstract bool AreBankContentsEnabled { get; set; }

	private UILabel _freeAwardButtonLabel
	{
		get
		{
			if (_freeAwardButtonLagelCont != null)
			{
				return _freeAwardButtonLagelCont;
			}
			if (freeAwardButton == null)
			{
				return _freeAwardButtonLagelCont;
			}
			return _freeAwardButtonLagelCont = freeAwardButton.GetComponentInChildren<UILabel>();
		}
	}

	public event Action<AbstractBankViewItem> PurchaseButtonPressed;

	public event EventHandler BackButtonPressed
	{
		add
		{
			if (backButton != null)
			{
				backButton.Clicked += value;
			}
		}
		remove
		{
			if (backButton != null)
			{
				backButton.Clicked -= value;
			}
		}
	}

	public abstract void UpdateUi();

	protected virtual void Awake()
	{
		BackButtonPressed += AbstractBankView_BackButtonPressed;
		_storeKitEventListener = StoreKitEventListener.Instance;
		if (_storeKitEventListener == null)
		{
			Debug.LogError("storeKitEventListener == null");
			return;
		}
		string desiredCurrency = DesiredCurrency;
		OnEnable();
		DesiredCurrency = desiredCurrency;
	}

	private void AbstractBankView_BackButtonPressed(object sender, EventArgs e)
	{
		try
		{
			if (Device.IsLoweMemoryDevice)
			{
				ActivityIndicator.ClearMemory();
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in AbstractBankView_BackButtonPressed: {0}", ex);
		}
	}

	protected virtual void OnEnable()
	{
		_localizeSaleLabel = LocalizationStore.Get("Key_0419");
	}

	protected virtual void Start()
	{
		bool state = StoreKitEventListener.IsPayingUser();
		GameObject[] adFreeLabels = AdFreeLabels;
		for (int i = 0; i < adFreeLabels.Length; i++)
		{
			adFreeLabels[i].SetActiveSafeSelf(state);
		}
	}

	protected virtual void Update()
	{
		if (Time.realtimeSinceStartup - _lastUpdateTime >= 0.5f)
		{
			TimeSpan timeSpan = TimeSpan.FromSeconds(PromoActionsManager.sharedManager.EventX3RemainedTime);
			string empty = string.Empty;
			empty = ((timeSpan.Days <= 0) ? string.Format("{0}: {1:00}:{2:00}:{3:00}", _localizeSaleLabel, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) : string.Format("{0}: {1} {2} {3:00}:{4:00}:{5:00}", _localizeSaleLabel, timeSpan.Days, (timeSpan.Days == 1) ? "Day" : "Days", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds));
			if (eventX3RemainTime != null)
			{
				for (int i = 0; i < eventX3RemainTime.Length; i++)
				{
					eventX3RemainTime[i].text = empty;
				}
			}
			if (colorBlinkForX3 != null && timeSpan.TotalHours < (double)Defs.HoursToEndX3ForIndicate && !colorBlinkForX3.enabled)
			{
				colorBlinkForX3.enabled = true;
			}
			_lastUpdateTime = Time.realtimeSinceStartup;
		}
		if (_freeAwardButtonLabel != null && freeAwardButton.isActiveAndEnabled)
		{
			_freeAwardButtonLabel.text = ((FreeAwardController.Instance.CurrencyForAward == "GemsCurrency") ? string.Format("[50CEFFFF]{0}[-]", new object[1] { ScriptLocalization.Get("Key_2046") }) : string.Format("[FFA300FF]{0}[-]", new object[1] { ScriptLocalization.Get("Key_1155") }));
		}
		bool flag = !coinsShop.IsWideLayoutAvailable && !coinsShop.IsNoConnection;
		NotEnoughCoinsLabelEnabled = CurrencyThatNotEnough == "Coins" && flag;
		NotEnoughGemsLabelEnabled = CurrencyThatNotEnough == "GemsCurrency" && flag;
		NotEnoughTicketsLabelEnabled = CurrencyThatNotEnough == "TicketsCurrency" && flag;
	}

	protected virtual void OnDestroy()
	{
		BackButtonPressed -= AbstractBankView_BackButtonPressed;
	}

	protected abstract IEnumerable<AbstractBankViewItem> AllItems();

	protected virtual void UpdateItem(AbstractBankViewItem item, PurchaseEventArgs purchaseInfo, BankExchangeItemData exchangeInfo)
	{
		if (item is FreeTicketsBankViewItem)
		{
			try
			{
				item.Setup(null, null, null, delegate
				{
					LastClickedItem = item;
					Action<AbstractBankViewItem> purchaseButtonPressed = this.PurchaseButtonPressed;
					if (purchaseButtonPressed != null)
					{
						purchaseButtonPressed(item);
					}
				});
				return;
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in setup of FreeTicketsBankViewItem: {0}", ex);
				return;
			}
		}
		if (purchaseInfo != null)
		{
			UpdateItemAsProduct(item, purchaseInfo);
		}
		else if (exchangeInfo != null)
		{
			UpdateItemAsExchangable(item, exchangeInfo);
		}
		else
		{
			Debug.LogError("unknown item data");
		}
	}

	private string[] InappIdsForPurchaseType(PurchaseEventArgs.PurchaseType type)
	{
		switch (type)
		{
		case PurchaseEventArgs.PurchaseType.Coins:
			return StoreKitEventListener.coinIds;
		case PurchaseEventArgs.PurchaseType.Gems:
			return StoreKitEventListener.gemsIds;
		default:
			return StoreKitEventListener.inappOffersIds;
		}
	}

	private int CountForPurchaseType(PurchaseEventArgs purchaseInfo)
	{
		if (purchaseInfo.Type == PurchaseEventArgs.PurchaseType.Coins)
		{
			return VirtualCurrencyHelper.coinInappsQuantity[purchaseInfo.Index];
		}
		if (purchaseInfo.Type == PurchaseEventArgs.PurchaseType.Gems)
		{
			return VirtualCurrencyHelper.gemsInappsQuantity[purchaseInfo.Index];
		}
		return 0;
	}

	private decimal PriceForPurchaseType(PurchaseEventArgs purchaseInfo)
	{
		if (purchaseInfo.Type == PurchaseEventArgs.PurchaseType.Coins)
		{
			return VirtualCurrencyHelper.coinPriceIds[purchaseInfo.Index];
		}
		if (purchaseInfo.Type == PurchaseEventArgs.PurchaseType.Gems)
		{
			return VirtualCurrencyHelper.gemsPriceIds[purchaseInfo.Index];
		}
		return VirtualCurrencyHelper.offersPriceIds[purchaseInfo.Index];
	}

	protected virtual void UpdateItemAsProduct(AbstractBankViewItem item, PurchaseEventArgs purchaseInfo)
	{
		string[] array = InappIdsForPurchaseType(purchaseInfo.Type);
		if (purchaseInfo.Index >= array.Length)
		{
			Debug.LogErrorFormat("UpdateItem: purchaseInfo.Index < inAppIds.Length");
			return;
		}
		string inappId = array[purchaseInfo.Index];
		purchaseInfo.Count = CountForPurchaseType(purchaseInfo);
		decimal num = PriceForPurchaseType(purchaseInfo);
		purchaseInfo.CurrencyAmount = num - 0.01m;
		string price = string.Format("${0}", new object[1] { purchaseInfo.CurrencyAmount });
		IMarketProduct marketProduct = null;
		string text = array[purchaseInfo.Index];
		if (marketProduct != null)
		{
			price = marketProduct.Price;
		}
		else
		{
			Debug.LogWarning("marketProduct == null,    id: " + text);
		}
		item.Price = price;
		try
		{
			item.Setup(marketProduct, purchaseInfo, null, delegate
			{
				LastClickedItem = item;
				Action<AbstractBankViewItem> purchaseButtonPressed = this.PurchaseButtonPressed;
				if (purchaseButtonPressed != null)
				{
					purchaseButtonPressed(item);
				}
			});
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in setup of BankViewItem: {0}", ex);
		}
	}

	protected virtual void UpdateItemAsExchangable(AbstractBankViewItem item, BankExchangeItemData exchangeInfo)
	{
		item.Price = string.Format("${0}", new object[1] { exchangeInfo.CurrencyCount });
		try
		{
			item.Setup(null, null, exchangeInfo, delegate
			{
				LastClickedItem = item;
				Action<AbstractBankViewItem> purchaseButtonPressed = this.PurchaseButtonPressed;
				if (purchaseButtonPressed != null)
				{
					purchaseButtonPressed(item);
				}
			});
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in setup of BankViewItem: {0}", ex);
		}
	}

	public virtual void GoToGems(bool blinkSelectedItem, bool blinkItemTypes, Action onComplete)
	{
	}

	protected virtual void ScrollToFirstItem<T>(bool blinkSelectedItem, bool blinkItemTypes, Action onComplete) where T : AbstractBankViewItem
	{
	}

	public AbstractBankView()
	{
	}
}
