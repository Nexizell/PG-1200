using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class BankViewTickets : AbstractBankView
{
	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass35_0
	{
		public bool itemBlinked;

		public int gemsReqiredCount;

		public Action _003C_003E9__6;

		internal void _003CScrollToGemsCoroutine_003Eb__0()
		{
			itemBlinked = false;
		}

		internal bool _003CScrollToGemsCoroutine_003Eb__1()
		{
			return itemBlinked;
		}

		internal bool _003CScrollToGemsCoroutine_003Eb__4(AbstractBankViewItem i)
		{
			return (i.isX3Item ? (i.purchaseInfo.Count * 3) : i.purchaseInfo.Count) >= gemsReqiredCount;
		}

		internal void _003CScrollToGemsCoroutine_003Eb__6()
		{
			itemBlinked = false;
		}

		internal bool _003CScrollToGemsCoroutine_003Eb__7()
		{
			return itemBlinked;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CScrollToGemsCoroutine_003Ed__35 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool blinkSelectedItem;

		public BankViewTickets _003C_003E4__this;

		private _003C_003Ec__DisplayClass35_0 _003C_003E8__1;

		public Func<AbstractBankViewItem, bool> itemsFilter;

		public Action onComplete;

		public bool blinkItemTypes;

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
		public _003CScrollToGemsCoroutine_003Ed__35(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			AbstractBankViewItem abstractBankViewItem;
			List<AbstractBankViewItem> list2;
			List<AbstractBankViewItem> source;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				_003C_003E8__1 = new _003C_003Ec__DisplayClass35_0();
				_003C_003E8__1.itemBlinked = false;
				if (blinkSelectedItem && _003C_003E4__this.LastClickedItem != null)
				{
					_003C_003E8__1.itemBlinked = true;
					_003C_003E4__this.LastClickedItem.Blink(1, delegate
					{
						_003C_003E8__1.itemBlinked = false;
					});
					_003C_003E2__current = new WaitWhile(() => _003C_003E8__1.itemBlinked);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00b2;
			case 1:
				_003C_003E1__state = -1;
				goto IL_00b2;
			case 2:
			{
				_003C_003E1__state = -1;
				if (!blinkItemTypes)
				{
					break;
				}
				List<AbstractBankViewItem> list = (_003C_003E4__this.gemsItemGrid.GetComponentsInChildren<AbstractBankViewItem>() ?? new AbstractBankViewItem[0]).Where(itemsFilter).ToList();
				if (!list.Any())
				{
					break;
				}
				_003C_003E8__1.itemBlinked = true;
				foreach (AbstractBankViewItem item in list)
				{
					item.Blink(2, delegate
					{
						_003C_003E8__1.itemBlinked = false;
					});
				}
				_003C_003E2__current = new WaitWhile(() => _003C_003E8__1.itemBlinked);
				_003C_003E1__state = 3;
				return true;
			}
			case 3:
				{
					_003C_003E1__state = -1;
					break;
				}
				IL_00b2:
				_003C_003E8__1.gemsReqiredCount = -1;
				if (_003C_003E4__this.LastClickedItem != null && _003C_003E4__this.LastClickedItem is ExchangeBankViewItem)
				{
					_003C_003E8__1.gemsReqiredCount = _003C_003E4__this.LastClickedItem.ExchangeInfo.GemsPrice - Storager.getInt("GemsCurrency");
				}
				abstractBankViewItem = null;
				list2 = (_003C_003E4__this.gemsItemGrid.GetComponentsInChildren<AbstractBankViewItem>() ?? new AbstractBankViewItem[0]).OrderBy((AbstractBankViewItem i) => i.transform.localPosition.x).ToList();
				source = list2.Where(itemsFilter).ToList();
				if (source.Any())
				{
					if (_003C_003E8__1.gemsReqiredCount > 0)
					{
						AbstractBankViewItem abstractBankViewItem2 = source.OrderBy((AbstractBankViewItem i) => i.purchaseInfo.Count).FirstOrDefault((AbstractBankViewItem i) => (i.isX3Item ? (i.purchaseInfo.Count * 3) : i.purchaseInfo.Count) >= _003C_003E8__1.gemsReqiredCount);
						abstractBankViewItem = ((abstractBankViewItem2 != null) ? abstractBankViewItem2 : source.First());
					}
					else
					{
						abstractBankViewItem = source.First();
					}
				}
				if (abstractBankViewItem != null)
				{
					int itemSiblingIndex = list2.IndexOf(abstractBankViewItem);
					_003C_003E4__this.ScrollMover.MoveTo(itemSiblingIndex);
					_003C_003E2__current = new WaitWhile(() => _003C_003E4__this.ScrollMover.IsMoving);
					_003C_003E1__state = 2;
					return true;
				}
				if (onComplete != null)
				{
					onComplete();
				}
				return false;
			}
			if (onComplete != null)
			{
				onComplete();
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

	public TicketsRewardedVideoController ticketsRewardedVideoController;

	public Transform freeTicketsTimerPoint;

	public UIWidget x3Discount;

	public GameObject btnTabContainer;

	public UIButton btnTabTickets;

	public UIButton btnTabGems;

	public UIScrollView ticketsScrollView;

	public UIGrid ticketsItemGrid;

	public AbstractBankViewItem ticketsItemPrefab;

	public AbstractBankViewItem freeTicketsItemPrefab;

	public UIScrollView gemsScrollView;

	public UIGrid gemsItemGrid;

	public AbstractBankViewItem gemsItemPrefab;

	public AbstractBankViewItem inappBonusItemPrefab;

	private bool m_areBankContentsEnabled;

	private static List<BankExchangeItemData> ticketChangeItems = new List<BankExchangeItemData>
	{
		new BankExchangeItemData
		{
			InAppId = 1,
			GemsPrice = 10,
			CurrencyCount = 1000
		},
		new BankExchangeItemData
		{
			InAppId = 2,
			GemsPrice = 20,
			CurrencyCount = 2000
		},
		new BankExchangeItemData
		{
			InAppId = 3,
			GemsPrice = 30,
			CurrencyCount = 3000
		}
	};

	public override bool AreBankContentsEnabled
	{
		get
		{
			return m_areBankContentsEnabled;
		}
		set
		{
			bool areBankContentsEnabled = m_areBankContentsEnabled;
			m_areBankContentsEnabled = value;
			btnTabContainer.SetActiveSafeSelf(value);
			if (value)
			{
				if (!areBankContentsEnabled)
				{
					bool isEnabled = btnTabTickets.isEnabled;
					ticketsScrollView.gameObject.SetActiveSafeSelf(!isEnabled);
					gemsScrollView.gameObject.SetActiveSafeSelf(isEnabled);
					UpdateUi();
					ResetScrollView(isEnabled);
				}
			}
			else
			{
				ticketsScrollView.gameObject.SetActiveSafeSelf(value);
				gemsScrollView.gameObject.SetActiveSafeSelf(value);
			}
		}
	}

	private UIScrollMover ScrollMover
	{
		get
		{
			return gemsScrollView.GetOrAddComponent<UIScrollMover>();
		}
	}

	protected override void OnEnable()
	{
		UpdateUi();
		UIButton uIButton = btnTabGems;
		uIButton = ((base.DesiredCurrency == null) ? btnTabTickets : ((base.DesiredCurrency == "TicketsCurrency") ? btnTabTickets : btnTabGems));
		base.DesiredCurrency = null;
		OnBtnTabClick(uIButton);
		if (connectionProblemLabel != null)
		{
			connectionProblemLabel.text = LocalizationStore.Get("Key_0278");
		}
		base.OnEnable();
	}

	protected override void Start()
	{
		ticketsScrollView.GetComponent<UIPanel>().UpdateAnchors();
		gemsScrollView.GetComponent<UIPanel>().UpdateAnchors();
		ResetScrollView(false);
		ResetScrollView(true);
		base.Start();
	}

	private static void ClearGrid(UIGrid itemGrid)
	{
		while (itemGrid.transform.childCount > 0)
		{
			Transform child = itemGrid.transform.GetChild(0);
			child.gameObject.SetActive(false);
			child.parent = null;
			UnityEngine.Object.Destroy(child.gameObject);
		}
	}

	private void PopulateTicketsGrid()
	{
		if (ticketChangeItems != null)
		{
			foreach (BankExchangeItemData ticketChangeItem in ticketChangeItems)
			{
				if (ticketChangeItem == null)
				{
					UnityEngine.Debug.LogErrorFormat("PopulateTicketsGrid changeItems is null");
					continue;
				}
				if (ticketChangeItem.InAppId < 1)
				{
					UnityEngine.Debug.LogErrorFormat("PopulateTicketsGrid changeItems InAppId is null or empty");
					continue;
				}
				if (ticketChangeItem.CurrencyCount < 1)
				{
					UnityEngine.Debug.LogErrorFormat("PopulateTicketsGrid changeItems InAppId '{0}' coins count < 0", ticketChangeItem.InAppId);
					continue;
				}
				if (ticketChangeItem.GemsPrice < 1)
				{
					UnityEngine.Debug.LogErrorFormat("PopulateTicketsGrid changeItems InAppId '{0}' gems price < 0", ticketChangeItem.InAppId);
					continue;
				}
				ticketsItemPrefab.gameObject.SetActiveSafeSelf(true);
				AbstractBankViewItem abstractBankViewItem = UnityEngine.Object.Instantiate(ticketsItemPrefab);
				abstractBankViewItem.transform.SetParent(ticketsItemGrid.transform);
				abstractBankViewItem.transform.localScale = Vector3.one;
				abstractBankViewItem.transform.localPosition = Vector3.zero;
				abstractBankViewItem.transform.localRotation = Quaternion.identity;
				UpdateItem(abstractBankViewItem, null, ticketChangeItem);
			}
		}
		ticketsItemPrefab.gameObject.SetActiveSafeSelf(false);
		if (ticketsRewardedVideoController.IsButtonAvailable() && new string[3]
		{
			"CampaignChooseBox",
			Defs.MainMenuScene,
			"LevelComplete"
		}.Contains(SceneManagerHelper.ActiveSceneName))
		{
			freeTicketsItemPrefab.gameObject.SetActiveSafeSelf(true);
			AbstractBankViewItem abstractBankViewItem2 = UnityEngine.Object.Instantiate(freeTicketsItemPrefab);
			abstractBankViewItem2.transform.SetParent(ticketsItemGrid.transform);
			abstractBankViewItem2.transform.localScale = Vector3.one;
			abstractBankViewItem2.transform.localPosition = Vector3.zero;
			abstractBankViewItem2.transform.localRotation = Quaternion.identity;
			UpdateItem(abstractBankViewItem2, null, null);
			freeTicketsItemPrefab.gameObject.SetActiveSafeSelf(false);
		}
	}

	private void PopulateGemsGrid(List<Dictionary<string, object>> inappBonusActions)
	{
		gemsItemPrefab.gameObject.SetActiveSafeSelf(true);
		for (int i = 0; i < AbstractBankView.gemsPurchasesInfo.Count; i++)
		{
			AbstractBankViewItem abstractBankViewItem = UnityEngine.Object.Instantiate(gemsItemPrefab);
			abstractBankViewItem.transform.SetParent(gemsItemGrid.transform);
			abstractBankViewItem.transform.localScale = Vector3.one;
			abstractBankViewItem.transform.localPosition = Vector3.zero;
			abstractBankViewItem.transform.localRotation = Quaternion.identity;
			UpdateItem(abstractBankViewItem, AbstractBankView.gemsPurchasesInfo[i], null);
		}
		gemsItemPrefab.gameObject.SetActiveSafeSelf(false);
		if (inappBonusActions != null)
		{
			for (int j = 0; j < inappBonusActions.Count; j++)
			{
				Dictionary<string, object> dictionary = inappBonusActions[j];
				try
				{
					object value;
					if (dictionary.TryGetValue("isGems", out value) && value != null)
					{
						if (!Convert.ToBoolean(value))
						{
							continue;
						}
					}
					else
					{
						UnityEngine.Debug.LogErrorFormat("PopulateItemGrid: no isGems key {0}", Json.Serialize(dictionary));
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in PopulateItemGrid: {0}", ex);
				}
				PurchaseEventArgs purchaseEventArgs = null;
				InappRememberedBonus actualBonusSizeForInappBonus = InappBonuessController.Instance.GetActualBonusSizeForInappBonus(dictionary);
				if (actualBonusSizeForInappBonus != null)
				{
					if (!actualBonusSizeForInappBonus.InappId.IsNullOrEmpty())
					{
						int indexOfInappWithBonus = Array.IndexOf(StoreKitEventListener.inappOffersIds, actualBonusSizeForInappBonus.InappId);
						if (indexOfInappWithBonus != -1)
						{
							purchaseEventArgs = AbstractBankView.offersPurchasesInfo.FirstOrDefault((PurchaseEventArgs purchaseInfo) => purchaseInfo.Index == indexOfInappWithBonus);
							if (purchaseEventArgs == null)
							{
								UnityEngine.Debug.LogErrorFormat("PopulateGemsGrid inappBonusPurchaseInfo == null isGems = {0} , inappBonusAction = {1}, bonus.InappId = {2}", true, Json.Serialize(dictionary), actualBonusSizeForInappBonus.InappId);
							}
						}
						else
						{
							UnityEngine.Debug.LogErrorFormat("PopulateGemsGrid indexOfInappWithBonus == -1, isGems = {0} , inappBonusAction = {1}, bonus.InappId = {2}", true, Json.Serialize(dictionary), actualBonusSizeForInappBonus.InappId);
						}
					}
					else
					{
						UnityEngine.Debug.LogErrorFormat("PopulateGemsGrid: bonus.InappId.IsNullOrEmpty() isGems = {0} , inappBonusAction = {1}", true, Json.Serialize(dictionary));
					}
				}
				else
				{
					UnityEngine.Debug.LogErrorFormat("PopulateGemsGrid: bonus == null isGems = {0} , inappBonusAction = {1}", true, Json.Serialize(dictionary));
				}
				if (purchaseEventArgs != null)
				{
					inappBonusItemPrefab.gameObject.SetActiveSafeSelf(true);
					AbstractBankViewItem abstractBankViewItem2 = UnityEngine.Object.Instantiate(inappBonusItemPrefab);
					abstractBankViewItem2.transform.SetParent(gemsItemGrid.transform);
					abstractBankViewItem2.transform.localScale = Vector3.one;
					abstractBankViewItem2.transform.localPosition = Vector3.zero;
					abstractBankViewItem2.transform.localRotation = Quaternion.identity;
					UpdateItem(abstractBankViewItem2, purchaseEventArgs, null);
				}
			}
		}
		inappBonusItemPrefab.gameObject.SetActiveSafeSelf(false);
		ResetScrollView(true);
	}

	public void OnBtnTabClick(UIButton btnTab)
	{
		bool flag = btnTab == btnTabGems;
		btnTabTickets.isEnabled = flag;
		btnTabGems.isEnabled = !flag;
		ticketsScrollView.gameObject.SetActiveSafeSelf(!flag);
		gemsScrollView.gameObject.SetActiveSafeSelf(flag);
		ResetScrollView(flag);
		if (btnTab != btnTabTickets && btnTab != btnTabGems)
		{
			UnityEngine.Debug.LogErrorFormat("Unknown btnTab");
		}
		UpdateX3();
	}

	public override void UpdateUi()
	{
		ClearGrid(ticketsItemGrid);
		ClearGrid(gemsItemGrid);
		if (!AreBankContentsEnabled)
		{
			IsX3Bank = false;
			return;
		}
		List<Dictionary<string, object>> inappBonusActions = null;
		try
		{
			inappBonusActions = BalanceController.GetCurrentInnapBonus();
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in UpdateUi BalanceController.GetCurrentInnapBonus: {0}", ex);
		}
		UpdateTicketsGrid();
		PopulateGemsGrid(inappBonusActions);
		SortItemGrid(true, inappBonusActions);
		UpdateX3();
	}

	private void UpdateTicketsGrid()
	{
		PopulateTicketsGrid();
		SortItemGrid(false, null);
	}

	private void UpdateX3()
	{
		try
		{
			IsX3Bank = AreBankContentsEnabled && PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.IsEventX3Active;
			x3Discount.alpha = (btnTabTickets.isEnabled ? 1f : 0f);
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in UpdateX3: {0}", ex);
		}
	}

	private void SortItemGrid(bool isGems, List<Dictionary<string, object>> inappBonusActions)
	{
		Transform transform = (isGems ? gemsItemGrid : ticketsItemGrid).transform;
		List<AbstractBankViewItem> list = new List<AbstractBankViewItem>();
		for (int i = 0; i < transform.childCount; i++)
		{
			AbstractBankViewItem component = transform.GetChild(i).GetComponent<AbstractBankViewItem>();
			list.Add(component);
		}
		list.Sort(new BankItemsComparer(inappBonusActions));
		for (int j = 0; j < list.Count; j++)
		{
			list[j].gameObject.name = string.Format("{0:00}", new object[1] { j });
		}
		ResetScrollView(isGems);
	}

	private void ResetScrollView(bool isGems)
	{
		UIScrollView obj = (isGems ? gemsScrollView : ticketsScrollView);
		UIGrid obj2 = (isGems ? gemsItemGrid : ticketsItemGrid);
		if (!isGems)
		{
			ticketsScrollView.currentMomentum = Vector3.zero;
		}
		obj2.Reposition();
		obj.ResetPosition();
	}

	protected override IEnumerable<AbstractBankViewItem> AllItems()
	{
		return (gemsItemGrid.GetComponentsInChildren<AbstractBankViewItem>() ?? new AbstractBankViewItem[0]).Concat(ticketsItemGrid.GetComponentsInChildren<AbstractBankViewItem>() ?? new AbstractBankViewItem[0]);
	}

	public static void RewriteTicketChangeItems(List<BankExchangeItemData> _changeItems)
	{
		ticketChangeItems = _changeItems;
	}

	public override void GoToGems(bool blinkSelectedItem, bool blinkItemTypes, Action onComplete)
	{
		OnBtnTabClick(btnTabGems);
		Func<AbstractBankViewItem, bool> itemsFilter = (AbstractBankViewItem item) => item is BankViewItem && item.purchaseInfo.Type == PurchaseEventArgs.PurchaseType.Gems;
		StartCoroutine(ScrollToGemsCoroutine(itemsFilter, blinkSelectedItem, blinkItemTypes, onComplete));
	}

	private IEnumerator ScrollToGemsCoroutine(Func<AbstractBankViewItem, bool> itemsFilter, bool blinkSelectedItem, bool blinkItemTypes, Action onComplete)
	{
		bool itemBlinked = false;
		if (blinkSelectedItem && LastClickedItem != null)
		{
			itemBlinked = true;
			LastClickedItem.Blink(1, delegate
			{
				itemBlinked = false;
			});
			yield return new WaitWhile(() => itemBlinked);
		}
		int gemsReqiredCount = -1;
		if (LastClickedItem != null && LastClickedItem is ExchangeBankViewItem)
		{
			gemsReqiredCount = LastClickedItem.ExchangeInfo.GemsPrice - Storager.getInt("GemsCurrency");
		}
		AbstractBankViewItem abstractBankViewItem = null;
		List<AbstractBankViewItem> list = (gemsItemGrid.GetComponentsInChildren<AbstractBankViewItem>() ?? new AbstractBankViewItem[0]).OrderBy((AbstractBankViewItem i) => i.transform.localPosition.x).ToList();
		List<AbstractBankViewItem> source = list.Where(itemsFilter).ToList();
		if (source.Any())
		{
			if (gemsReqiredCount > 0)
			{
				AbstractBankViewItem abstractBankViewItem2 = source.OrderBy((AbstractBankViewItem i) => i.purchaseInfo.Count).FirstOrDefault((AbstractBankViewItem i) => (i.isX3Item ? (i.purchaseInfo.Count * 3) : i.purchaseInfo.Count) >= gemsReqiredCount);
				abstractBankViewItem = ((abstractBankViewItem2 != null) ? abstractBankViewItem2 : source.First());
			}
			else
			{
				abstractBankViewItem = source.First();
			}
		}
		if (abstractBankViewItem != null)
		{
			int itemSiblingIndex = list.IndexOf(abstractBankViewItem);
			ScrollMover.MoveTo(itemSiblingIndex);
			yield return new WaitWhile(() => ScrollMover.IsMoving);
			if (blinkItemTypes)
			{
				List<AbstractBankViewItem> list2 = (gemsItemGrid.GetComponentsInChildren<AbstractBankViewItem>() ?? new AbstractBankViewItem[0]).Where(itemsFilter).ToList();
				if (list2.Any())
				{
					itemBlinked = true;
					foreach (AbstractBankViewItem item in list2)
					{
						item.Blink(2, delegate
						{
							itemBlinked = false;
						});
					}
					yield return new WaitWhile(() => itemBlinked);
				}
			}
			if (onComplete != null)
			{
				onComplete();
			}
		}
		else if (onComplete != null)
		{
			onComplete();
		}
	}

	protected override void Awake()
	{
		base.Awake();
		ChooseMiniGameController.AddFreeTicketsTimerToPoint(freeTicketsTimerPoint);
		ticketsRewardedVideoController.ButtonAvailabilityChanged += TicketsRewardedVideoController_ButtonAvailabilityChanged;
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		ticketsRewardedVideoController.ButtonAvailabilityChanged -= TicketsRewardedVideoController_ButtonAvailabilityChanged;
	}

	private void TicketsRewardedVideoController_ButtonAvailabilityChanged(object sender, EventArgs e)
	{
		UpdateUi();
	}
}
