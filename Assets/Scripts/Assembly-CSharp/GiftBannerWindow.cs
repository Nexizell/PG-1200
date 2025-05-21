using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public class GiftBannerWindow : BannerWindow
{
	public enum StepAnimation
	{
		none = 0,
		WaitForShowAward = 1,
		ShowAward = 2,
		waitForClose = 3
	}

	[CompilerGenerated]
	internal sealed class _003COnAnimOpenGift_003Ed__59 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GiftBannerWindow _003C_003E4__this;

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
		public _003COnAnimOpenGift_003Ed__59(int _003C_003E1__state)
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
				_003C_003E4__this.CancelInvoke("StartNextStep");
				_003C_003E4__this.HideDarkFon();
				AnimationGift.instance.StopAnimForGetGift();
				if (GiftBannerWindow.onOpenInfoGift != null)
				{
					GiftBannerWindow.onOpenInfoGift();
				}
				_003C_003E4__this.panelInfoGift.SetInfoButton(_003C_003E4__this.awardSlot);
				_003C_003E4__this.awardSlot = null;
				_003C_003E2__current = new WaitForSeconds(1f);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				BankController.canShowIndication = true;
				_003C_003E4__this.animatorBanner.SetTrigger("GiftInfoShow");
				_003C_003E2__current = new WaitForSeconds(1.5f);
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.curStateAnimAward = StepAnimation.waitForClose;
				_003C_003E4__this.canTapOnGift = true;
				_003C_003E4__this.Invoke("StartNextStep", _003C_003E4__this.delayBeforeNextStep);
				return false;
			}
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
	internal sealed class _003CWaitAndSort_003Ed__61 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GiftBannerWindow _003C_003E4__this;

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
		public _003CWaitAndSort_003Ed__61(int _003C_003E1__state)
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
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				_003C_003E4__this.scrollGift.transform.parent.localScale = Vector3.one;
				_003C_003E4__this.scrollGift.transform.localScale = Vector3.one;
				_003C_003E4__this.scrollGift.Sort();
				_003C_003E2__current = null;
				_003C_003E1__state = 2;
				return true;
			case 2:
				_003C_003E1__state = -1;
				break;
			case 3:
				_003C_003E1__state = -1;
				break;
			}
			if (_003C_003E4__this.scrollGift.transform.parent.localScale.Equals(Vector3.one))
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 3;
				return true;
			}
			_003C_003E4__this.scrollGift.Sort(true);
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

	public static GiftBannerWindow instance;

	public GameObject butBuy;

	public GameObject butGift;

	public GameObject bannerObj;

	public GiftScroll scrollGift;

	public UILabel lbPriceForBuy;

	public UILabel lbTimer;

	public UISprite sprDarkFon;

	public GameObject objSound;

	public UISprite sprFonScroll;

	public GiftHUDItem panelInfoGift;

	public Animator animatorBanner;

	public GameObject objLabelTapGift;

	public float speedAnimCenter = 2f;

	[SerializeField]
	protected internal TextGroup _freeSpinsText;

	public static bool blockedButton;

	public const string keyTrigOpenBanner = "OpenGiftPanel";

	public const string keyTrigTapButton = "OpenGiftBtnRelease";

	public const string keyTrigShowInfoGift = "GiftInfoShow";

	public const string keyTrigCloseInfoGift = "GiftInfoClose";

	private SlotInfo awardSlot;

	private float delayBeforeNextStep = 5f;

	private bool canTapOnGift = true;

	private Coroutine crtForShowAward;

	private bool needShowGift;

	[HideInInspector]
	public bool needForceShowAwardGift;

	private IDisposable _backSubscription;

	private bool needPlayStartAnim = true;

	public static bool isForceClose;

	public static bool isActiveBanner;

	public StepAnimation curStateAnimAward;

	private bool _waitBankClose;

	public static event Action onClose;

	public static event Action onGetGift;

	public static event Action onHideInfoGift;

	public static event Action onOpenInfoGift;

	private void Awake()
	{
		instance = this;
		if (animatorBanner != null)
		{
			animatorBanner = GetComponent<Animator>();
		}
		GiftController.OnUpdateTimer += UpdateLabelTimer;
		GiftController.OnTimerEnded += OnEndTimer;
	}

	private void OnDestroy()
	{
		GiftController.OnUpdateTimer -= UpdateLabelTimer;
		GiftController.OnTimerEnded -= OnEndTimer;
		instance = null;
	}

	private void OnEnable()
	{
		if (!bannerObj.activeSelf)
		{
			needPlayStartAnim = true;
		}
		if ((bool)objSound)
		{
			objSound.SetActive(Defs.isSoundFX);
		}
		SetViewState();
		if (GiftController.Instance != null)
		{
			if (GiftController.Instance.ActiveGift)
			{
				GiftController.Instance.CheckAvaliableSlots();
			}
			else
			{
				SetVisibleBanner(false);
				if (GiftBannerWindow.onClose != null)
				{
					GiftBannerWindow.onClose();
				}
			}
		}
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(CloseBanner, "Gift (Gotcha)");
	}

	private void OnDisable()
	{
		needPlayStartAnim = true;
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	public void ShowShop()
	{
		if (!blockedButton)
		{
			SetVisibleBanner(false);
			_waitBankClose = true;
			MainMenuController.sharedController.ShowBankWindow();
		}
	}

	private void Update()
	{
		if (_waitBankClose && !BankController.Instance.InterfaceEnabled)
		{
			_waitBankClose = false;
			needPlayStartAnim = true;
			OpenBannerWindow();
		}
	}

	public void GetGift()
	{
		if (!blockedButton)
		{
			GetGiftCore(false);
		}
	}

	public void OpenBannerWindow()
	{
		if (_waitBankClose)
		{
			return;
		}
		UnityEngine.Debug.Log("=>>>> OpenBannerWindow " + isForceClose);
		if (isForceClose)
		{
			return;
		}
		UnityEngine.Debug.Log("=>>>> OpenBannerWindow " + needPlayStartAnim);
		if (needPlayStartAnim)
		{
			SetVisibleBanner(true);
			needPlayStartAnim = false;
			scrollGift.SetCanDraggable(true);
			HideDarkFon();
			animatorBanner.SetTrigger("OpenGiftPanel");
			if (GiftScroll.canReCreateSlots)
			{
				scrollGift.Sort(true);
			}
		}
	}

	private void GetGiftCore(bool isForMoneyGift)
	{
		if (!isForMoneyGift && !GiftController.Instance.CanGetGift)
		{
			return;
		}
		ButtonClickSound.Instance.PlayClick();
		BankController.UpdateAllIndicatorsMoney();
		BankController.canShowIndication = false;
		SlotInfo gift = GiftController.Instance.GetGift(isForMoneyGift);
		if (gift == null)
		{
			throw new Exception("failed get gift");
		}
		awardSlot = CopySlot(gift);
		if (awardSlot != null)
		{
			if (awardSlot.gift != null)
			{
				AnalyticsStuff.LogDailyGift(awardSlot.gift.Id, awardSlot.category.Type, awardSlot.CountGift, isForMoneyGift);
			}
			else
			{
				UnityEngine.Debug.LogError("GetGiftCore: awardSlot.gift = null");
			}
		}
		else
		{
			UnityEngine.Debug.LogError("GetGiftCore: awardSlot = null");
		}
		blockedButton = true;
		scrollGift.SetCanDraggable(false);
		scrollGift.AnimScrollGift(awardSlot.numInScroll);
		animatorBanner.SetTrigger("OpenGiftBtnRelease");
		GiftScroll.canReCreateSlots = false;
		GiftController.Instance.ReCreateSlots();
		ShowDarkFon();
		StartShowAwardGift();
		if (GiftBannerWindow.onGetGift != null)
		{
			GiftBannerWindow.onGetGift();
		}
		_freeSpinsText.gameObject.SetActive(false);
		if (!(objSound != null))
		{
			return;
		}
		GameObject childGameObject = objSound.GetChildGameObject("SoundGatchaInfoShow", true);
		if (childGameObject != null)
		{
			AudioSource component = childGameObject.GetComponent<AudioSource>();
			if (component != null)
			{
				component.enabled = awardSlot.category.Type != GiftCategoryType.Coins && awardSlot.category.Type != GiftCategoryType.Gems && awardSlot.category.Type != GiftCategoryType.Tickets;
			}
		}
	}

	private SlotInfo CopySlot(SlotInfo curSlot)
	{
		SlotInfo slotInfo = new SlotInfo();
		slotInfo.gift = new GiftInfo();
		slotInfo.gift.Id = curSlot.gift.Id;
		slotInfo.gift.Count.Value = curSlot.gift.Count.Value;
		slotInfo.gift.KeyTranslateInfo = curSlot.gift.KeyTranslateInfo;
		slotInfo.CountGift = curSlot.CountGift;
		slotInfo.numInScroll = curSlot.numInScroll;
		slotInfo.category = curSlot.category;
		slotInfo.isActiveEvent = curSlot.isActiveEvent;
		return slotInfo;
	}

	public void BuyCanGetGift()
	{
		if (!blockedButton)
		{
			bool buySuccess = false;
			ItemPrice price = new ItemPrice(GiftController.Instance.CostBuyCanGetGift.Value, "GemsCurrency");
			ShopNGUIController.TryToBuy(base.transform.root.gameObject, price, delegate
			{
				buySuccess = true;
				GetGiftCore(true);
			}, null, null, null, null, delegate
			{
				_waitBankClose = true;
			});
			if (!buySuccess)
			{
				SetVisibleBanner(false);
			}
			SetViewState();
		}
	}

	public void StartShowAwardGift()
	{
		if (awardSlot != null)
		{
			canTapOnGift = false;
			curStateAnimAward = StepAnimation.WaitForShowAward;
			AnimationGift.instance.StartAnimForGetGift();
			CancelInvoke("StartNextStep");
			Invoke("StartNextStep", delayBeforeNextStep);
		}
		else
		{
			CloseInfoGift();
		}
	}

	public void OnClickGift()
	{
		if (canTapOnGift)
		{
			StartNextStep();
		}
	}

	private void StartNextStep()
	{
		switch (curStateAnimAward)
		{
		case StepAnimation.WaitForShowAward:
			CancelInvoke("StartNextStep");
			curStateAnimAward = StepAnimation.ShowAward;
			StartNextStep();
			break;
		case StepAnimation.ShowAward:
			crtForShowAward = StartCoroutine(OnAnimOpenGift());
			break;
		case StepAnimation.waitForClose:
			CloseInfoGift();
			break;
		}
	}

	private IEnumerator OnAnimOpenGift()
	{
		CancelInvoke("StartNextStep");
		HideDarkFon();
		AnimationGift.instance.StopAnimForGetGift();
		if (GiftBannerWindow.onOpenInfoGift != null)
		{
			GiftBannerWindow.onOpenInfoGift();
		}
		panelInfoGift.SetInfoButton(awardSlot);
		awardSlot = null;
		yield return new WaitForSeconds(1f);
		BankController.canShowIndication = true;
		animatorBanner.SetTrigger("GiftInfoShow");
		yield return new WaitForSeconds(1.5f);
		curStateAnimAward = StepAnimation.waitForClose;
		canTapOnGift = true;
		Invoke("StartNextStep", delayBeforeNextStep);
	}

	public void CloseInfoGift(bool isForce = false)
	{
		canTapOnGift = true;
		CancelInvoke("StartNextStep");
		SpringPanel component = scrollGift.GetComponent<SpringPanel>();
		if ((bool)component)
		{
			UnityEngine.Object.Destroy(component);
		}
		if (crtForShowAward != null)
		{
			StopCoroutine(crtForShowAward);
		}
		animatorBanner.SetTrigger("GiftInfoClose");
		crtForShowAward = null;
		curStateAnimAward = StepAnimation.none;
		GiftScroll.canReCreateSlots = true;
		scrollGift.SetCanDraggable(true);
		SetViewState();
		HideDarkFon();
		Invoke("UnlockedBut", 1.5f);
		if (scrollGift != null)
		{
			scrollGift.UpdateListButton();
		}
		if (GiftBannerWindow.onHideInfoGift != null)
		{
			GiftBannerWindow.onHideInfoGift();
		}
		StartCoroutine(WaitAndSort());
		if (!isForce && FriendsController.ServerTime < 0)
		{
			AnimationGift.instance.CheckVisibleGift();
			ForceCloseAll();
		}
	}

	private IEnumerator WaitAndSort()
	{
		yield return null;
		scrollGift.transform.parent.localScale = Vector3.one;
		scrollGift.transform.localScale = Vector3.one;
		scrollGift.Sort();
		yield return null;
		while (scrollGift.transform.parent.localScale.Equals(Vector3.one))
		{
			yield return null;
		}
		scrollGift.Sort(true);
	}

	private void UnlockedBut()
	{
		blockedButton = false;
	}

	public void CloseBanner()
	{
		if (!_waitBankClose && !blockedButton && (bool)BannerWindowController.SharedController && bannerObj != null && bannerObj.activeSelf)
		{
			ButtonClickSound.Instance.PlayClick();
			SetVisibleBanner(false);
			if (GiftBannerWindow.onClose != null)
			{
				GiftBannerWindow.onClose();
			}
			isActiveBanner = false;
		}
	}

	public void CloseBannerEndAnimtion()
	{
		BannerWindowController.SharedController.HideBannerWindow();
		SetVisibleBanner(true);
		if (MainMenuController.sharedController != null)
		{
			MainMenuController.sharedController.ShowSavePanel();
		}
	}

	public void SetVisibleBanner(bool val)
	{
		if (bannerObj != null)
		{
			bannerObj.SetActive(val);
		}
	}

	public string GetNameSpriteForSlot(SlotInfo curSlot)
	{
		GiftCategoryType giftCategoryType = curSlot.category.Type;
		if (giftCategoryType == GiftCategoryType.ArmorAndHat)
		{
			return "shop_icons_armor";
		}
		return "";
	}

	private void SetViewState()
	{
		lbPriceForBuy.text = GiftController.Instance.CostBuyCanGetGift.Value.ToString();
		string text = LocalizationStore.Get("Key_2196");
		int num = GiftController.Instance.FreeSpins + (GiftController.Instance.CanGetTimerGift ? 1 : 0);
		if (GiftController.Instance.CanGetFreeSpinGift)
		{
			text = string.Format(text, new object[1] { (num > 1) ? num.ToString() : "" });
		}
		_freeSpinsText.Text = text;
		_freeSpinsText.gameObject.SetActiveSafe(GiftController.Instance.CanGetFreeSpinGift && num > 1);
		butBuy.SetActiveSafe(!GiftController.Instance.CanGetGift);
		butGift.SetActiveSafe(GiftController.Instance.CanGetGift);
		lbTimer.gameObject.SetActive(!GiftController.Instance.CanGetTimerGift);
		objLabelTapGift.SetActive(!GiftController.Instance.CanGetTimerGift);
	}

	public void ShowDarkFon()
	{
	}

	public void HideDarkFon()
	{
	}

	public void AnimFonShow(bool val)
	{
		if (sprDarkFon != null)
		{
			if (val)
			{
				TweenAlpha.Begin(sprDarkFon.gameObject, 1f, 0.4f);
			}
			else
			{
				TweenAlpha.Begin(sprDarkFon.gameObject, 0.1f, 0f);
			}
		}
	}

	private void UpdateLabelTimer(string curTime)
	{
		SetViewState();
		if (lbTimer != null)
		{
			lbTimer.text = curTime;
		}
	}

	private void OnEndTimer()
	{
		SetViewState();
	}

	private void OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			ForceCloseAll();
		}
	}

	public void ForceCloseAll()
	{
		if (!(instance == null) && curStateAnimAward == StepAnimation.none && isActiveBanner)
		{
			isActiveBanner = false;
			isForceClose = true;
			needPlayStartAnim = true;
			blockedButton = false;
			BankController.canShowIndication = true;
			canTapOnGift = true;
			CloseInfoGift(true);
			HideDarkFon();
			SetVisibleBanner(false);
			MainMenuController.canRotationLobbyPlayer = true;
			if (GiftBannerWindow.onClose != null)
			{
				GiftBannerWindow.onClose();
			}
			if (AnimationGift.instance != null)
			{
				AnimationGift.instance.ResetAnimation();
			}
			curStateAnimAward = StepAnimation.none;
		}
	}

	[ContextMenu("UpdateSizeScroll")]
	public void UpdateSizeScroll()
	{
		int num = scrollGift.listButton.Count;
		if (num > 8)
		{
			num = 8;
		}
		num--;
		int num2 = num * scrollGift.wrapScript.itemSize;
		UIPanel panel = scrollGift.scView.panel;
		sprFonScroll.SetDimensions(num2 + 30, (int)sprFonScroll.localSize.y);
		panel.baseClipRegion = new Vector4(panel.baseClipRegion.x, panel.baseClipRegion.y, num2, panel.baseClipRegion.w);
	}

	[ContextMenu("simulate pause")]
	private void SimPause()
	{
		ForceCloseAll();
	}
}
