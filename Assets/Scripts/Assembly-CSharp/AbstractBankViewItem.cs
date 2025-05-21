using System;
using System.Collections.Generic;
using Rilisoft;
using UnityEngine;

public abstract class AbstractBankViewItem : MonoBehaviour
{
	public List<UILabel> inappNameLabels;

	public UITexture icon;

	public UILabel priceLabel;

	public UIButton btnBuy;

	[NonSerialized]
	public PurchaseEventArgs purchaseInfo;

	public GameObject aDFree;

	public GameObject[] x3Elements;

	public GameObject[] usualElements;

	public Blinking BlinkingObj;

	private Collider _btnBuyCollider;

	private float deltaX;

	private float lastDeltaX;

	private IMarketProduct m_marketProduct;

	private Dictionary<string, object> m_inappBonusParameters;

	private BankExchangeItemData m_exchangeInfo;

	private bool m_isX3Item;

	private ButtonHandler m_purchaseButtonScript;

	protected Collider BtnBuyCollider
	{
		get
		{
			if (_btnBuyCollider == null && btnBuy != null)
			{
				_btnBuyCollider = btnBuy.GetComponent<Collider>();
			}
			return _btnBuyCollider;
		}
	}

	public Dictionary<string, object> InappBonusParameters
	{
		get
		{
			return m_inappBonusParameters;
		}
		protected set
		{
			m_inappBonusParameters = value;
		}
	}

	public BankExchangeItemData ExchangeInfo
	{
		get
		{
			return m_exchangeInfo;
		}
		protected set
		{
			m_exchangeInfo = value;
		}
	}

	public virtual string Price
	{
		set
		{
			if (priceLabel != null)
			{
				priceLabel.text = value ?? "";
			}
		}
	}

	public virtual bool isX3Item
	{
		get
		{
			return m_isX3Item;
		}
		set
		{
			m_isX3Item = value;
			for (int i = 0; i < x3Elements.Length; i++)
			{
				if (x3Elements[i] != null)
				{
					x3Elements[i].SetActive(value);
				}
			}
			for (int j = 0; j < usualElements.Length; j++)
			{
				if (usualElements[j] != null)
				{
					usualElements[j].SetActive(!value);
				}
			}
		}
	}

	protected IMarketProduct MarketProduct
	{
		get
		{
			return m_marketProduct;
		}
		set
		{
			m_marketProduct = value;
		}
	}

	protected EventHandler PurchaseButtonHandler { get; set; }

	private ButtonHandler PurchaseButtonScript
	{
		get
		{
			if (m_purchaseButtonScript == null)
			{
				m_purchaseButtonScript = btnBuy.GetComponent<ButtonHandler>();
				if (m_purchaseButtonScript == null)
				{
					Debug.LogErrorFormat("BankViewItem.PurchaseButtonScript: m_purchaseButtonScript == null");
				}
			}
			return m_purchaseButtonScript;
		}
	}

	protected UIScrollView ScrollView { get; set; }

	public virtual void Setup(IMarketProduct product, PurchaseEventArgs newPurchaseInfo, BankExchangeItemData exchangeInfo, EventHandler clickHandler)
	{
		ScrollView = GetComponentInParent<UIScrollView>();
		purchaseInfo = newPurchaseInfo;
		ExchangeInfo = exchangeInfo;
		RemovePurchaseButtonHandler();
		PurchaseButtonHandler = clickHandler;
		AddPurchaseButtonHandler();
	}

	public void Blink(int blinkCount, Action onBlinkEnded)
	{
		if (BlinkingObj == null)
		{
			if (onBlinkEnded != null)
			{
				onBlinkEnded();
			}
			return;
		}
		float runAfterSecs = BlinkingObj.halfCycle * (float)blinkCount;
		BlinkingObj.gameObject.SetActive(true);
		CoroutineRunner.DeferredAction(runAfterSecs, delegate
		{
			BlinkingObj.gameObject.SetActive(false);
			if (onBlinkEnded != null)
			{
				onBlinkEnded();
			}
		});
	}

	protected static bool PaymentOccursInLastTwoWeeks()
	{
		string @string = PlayerPrefs.GetString("Last Payment Time", string.Empty);
		DateTime result;
		if (!string.IsNullOrEmpty(@string) && DateTime.TryParse(@string, out result))
		{
			return DateTime.UtcNow - result <= TimeSpan.FromDays(14.0);
		}
		return false;
	}

	protected virtual void Awake()
	{
		UpdateAdFree();
	}

	protected abstract void OnEnable();

	protected abstract void OnDisable();

	protected virtual void Update()
	{
		UpdateAdFree();
		if (ScrollView != null)
		{
			float num = 5f;
			if (ScrollView.isDragging)
			{
				deltaX = Mathf.Abs(UICamera.lastEventPosition.x - lastDeltaX);
			}
			bool flag = ScrollView.isDragging && Mathf.Abs(UICamera.lastEventPosition.x - lastDeltaX) > num;
			if (BtnBuyCollider != null)
			{
				BtnBuyCollider.enabled = !flag;
			}
			if (ScrollView.isDragging && btnBuy != null && btnBuy.enabled && (btnBuy.state == UIButtonColor.State.Pressed || btnBuy.state == UIButtonColor.State.Hover))
			{
				btnBuy.state = UIButtonColor.State.Normal;
			}
			lastDeltaX = UICamera.lastEventPosition.x;
		}
	}

	protected virtual void OnDestroy()
	{
		RemovePurchaseButtonHandler();
	}

	protected bool IsDiscounted()
	{
		if (purchaseInfo != null)
		{
			return purchaseInfo.Discount > 0;
		}
		return false;
	}

	protected abstract void SetIcon();

	protected void AddPurchaseButtonHandler()
	{
		try
		{
			if (PurchaseButtonHandler != null)
			{
				PurchaseButtonScript.Clicked += PurchaseButtonHandler;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in AddPurchaseButtonHandler: {0}", ex);
		}
	}

	protected void RemovePurchaseButtonHandler()
	{
		try
		{
			if (PurchaseButtonHandler != null)
			{
				PurchaseButtonScript.Clicked -= PurchaseButtonHandler;
				PurchaseButtonHandler = null;
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in RemovePurchaseButtonHandler: {0}", ex);
		}
	}

	private void UpdateAdFree()
	{
		if ((ScrollView != null && ScrollView.isDragging) || !(aDFree != null))
		{
			return;
		}
		try
		{
			int reasonCodeToDismissInterstitialConnectScene = ConnectScene.GetReasonCodeToDismissInterstitialConnectScene();
			if (reasonCodeToDismissInterstitialConnectScene == 0)
			{
				aDFree.SetActiveSafeSelf(true);
			}
			else if (reasonCodeToDismissInterstitialConnectScene / 100 == 3)
			{
				int num = reasonCodeToDismissInterstitialConnectScene % 100;
				if (num / 10 == 3)
				{
					switch (num % 10)
					{
					case 5:
						aDFree.SetActiveSafeSelf(true);
						break;
					case 6:
						aDFree.SetActiveSafeSelf(true);
						break;
					default:
						aDFree.SetActiveSafeSelf(false);
						break;
					}
				}
				else
				{
					aDFree.SetActiveSafeSelf(false);
				}
			}
			else
			{
				switch (reasonCodeToDismissInterstitialConnectScene)
				{
				case 600:
					aDFree.SetActiveSafeSelf(true);
					break;
				case 700:
					aDFree.SetActiveSafeSelf(true);
					break;
				default:
					aDFree.SetActiveSafeSelf(false);
					break;
				}
			}
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in UpdateAdFree: : {0}", ex);
		}
	}

	public AbstractBankViewItem()
	{
	}
}
