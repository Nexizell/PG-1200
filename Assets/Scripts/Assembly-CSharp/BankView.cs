using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class BankView : AbstractBankView
{
	public GameObject btnTabContainer;

	public UIButton btnTabGold;

	public UIButton btnTabGems;

	public UIScrollView goldScrollView;

	public UIGrid goldItemGrid;

	public AbstractBankViewItem goldItemPrefab;

	public UIScrollView gemsScrollView;

	public UIGrid gemsItemGrid;

	public AbstractBankViewItem gemsItemPrefab;

	public AbstractBankViewItem inappBonusItemPrefab;

	private bool m_areBankContentsEnabled;

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
					bool isEnabled = btnTabGold.isEnabled;
					goldScrollView.gameObject.SetActiveSafeSelf(!isEnabled);
					gemsScrollView.gameObject.SetActiveSafeSelf(isEnabled);
					UpdateUi();
					ResetScrollView(isEnabled);
				}
			}
			else
			{
				goldScrollView.gameObject.SetActiveSafeSelf(value);
				gemsScrollView.gameObject.SetActiveSafeSelf(value);
			}
		}
	}

	protected override void OnEnable()
	{
		UpdateUi();
		UIButton btnTab = btnTabGems;
		if (base.DesiredCurrency != null)
		{
			btnTab = ((base.DesiredCurrency == "GemsCurrency") ? btnTabGems : btnTabGold);
		}
		else
		{
			try
			{
				List<Dictionary<string, object>> currentInnapBonus = BalanceController.GetCurrentInnapBonus();
				if (currentInnapBonus != null && currentInnapBonus.Count() > 0 && currentInnapBonus.All(delegate(Dictionary<string, object> inappBonus)
				{
					bool result = false;
					object value;
					if (inappBonus.TryGetValue("isGems", out value) && value != null)
					{
						result = !Convert.ToBoolean(value);
					}
					return result;
				}))
				{
					btnTab = btnTabGold;
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in BankView.OnEnable GetCurrenceCurrentInnapBonus: {0}", ex);
			}
		}
		base.DesiredCurrency = null;
		OnBtnTabClick(btnTab);
		if (connectionProblemLabel != null)
		{
			connectionProblemLabel.text = LocalizationStore.Get("Key_0278");
		}
		base.OnEnable();
	}

	protected override void Start()
	{
		goldScrollView.GetComponent<UIPanel>().UpdateAnchors();
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

	private void PopulateItemGrid(bool isGems, List<Dictionary<string, object>> inappBonusActions)
	{
		IList<PurchaseEventArgs> list = (isGems ? AbstractBankView.gemsPurchasesInfo : AbstractBankView.goldPurchasesInfo);
		UIGrid uIGrid = (isGems ? gemsItemGrid : goldItemGrid);
		AbstractBankViewItem abstractBankViewItem = (isGems ? gemsItemPrefab : goldItemPrefab);
		abstractBankViewItem.gameObject.SetActiveSafeSelf(true);
		for (int i = 0; i < list.Count; i++)
		{
			AbstractBankViewItem abstractBankViewItem2 = UnityEngine.Object.Instantiate(abstractBankViewItem);
			abstractBankViewItem2.transform.SetParent(uIGrid.transform);
			abstractBankViewItem2.transform.localScale = Vector3.one;
			abstractBankViewItem2.transform.localPosition = Vector3.zero;
			abstractBankViewItem2.transform.localRotation = Quaternion.identity;
			UpdateItem(abstractBankViewItem2, list[i], null);
		}
		abstractBankViewItem.gameObject.SetActiveSafeSelf(false);
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
						if (Convert.ToBoolean(value) != isGems)
						{
							continue;
						}
					}
					else
					{
						Debug.LogErrorFormat("PopulateItemGrid: no isGems key {0}", Json.Serialize(dictionary));
					}
				}
				catch (Exception ex)
				{
					Debug.LogErrorFormat("Exception in PopulateItemGrid: {0}", ex);
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
								Debug.LogErrorFormat("PopulateItemGrid inappBonusPurchaseInfo == null isGems = {0} , inappBonusAction = {1}, bonus.InappId = {2}", isGems, Json.Serialize(dictionary), actualBonusSizeForInappBonus.InappId);
							}
						}
						else
						{
							Debug.LogErrorFormat("PopulateItemGrid indexOfInappWithBonus == -1, isGems = {0} , inappBonusAction = {1}, bonus.InappId = {2}", isGems, Json.Serialize(dictionary), actualBonusSizeForInappBonus.InappId);
						}
					}
					else
					{
						Debug.LogErrorFormat("PopulateItemGrid: bonus.InappId.IsNullOrEmpty() isGems = {0} , inappBonusAction = {1}", isGems, Json.Serialize(dictionary));
					}
				}
				else
				{
					Debug.LogErrorFormat("PopulateItemGrid: bonus == null isGems = {0} , inappBonusAction = {1}", isGems, Json.Serialize(dictionary));
				}
				if (purchaseEventArgs != null)
				{
					inappBonusItemPrefab.gameObject.SetActiveSafeSelf(true);
					AbstractBankViewItem abstractBankViewItem3 = UnityEngine.Object.Instantiate(inappBonusItemPrefab);
					abstractBankViewItem3.transform.SetParent(uIGrid.transform);
					abstractBankViewItem3.transform.localScale = Vector3.one;
					abstractBankViewItem3.transform.localPosition = Vector3.zero;
					abstractBankViewItem3.transform.localRotation = Quaternion.identity;
					UpdateItem(abstractBankViewItem3, purchaseEventArgs, null);
				}
			}
		}
		inappBonusItemPrefab.gameObject.SetActiveSafeSelf(false);
		ResetScrollView(isGems);
	}

	public void OnBtnTabClick(UIButton btnTab)
	{
		bool flag = btnTab == btnTabGems;
		btnTabGold.isEnabled = flag;
		btnTabGems.isEnabled = !flag;
		goldScrollView.gameObject.SetActiveSafeSelf(!flag);
		gemsScrollView.gameObject.SetActiveSafeSelf(flag);
		ResetScrollView(flag);
		if (btnTab != btnTabGold && btnTab != btnTabGems)
		{
			Debug.LogErrorFormat("Unknown btnTab");
		}
	}

	public override void UpdateUi()
	{
		ClearGrid(goldItemGrid);
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
			Debug.LogErrorFormat("Exception in UpdateUi BalanceController.GetCurrentInnapBonus: {0}", ex);
		}
		PopulateItemGrid(false, inappBonusActions);
		PopulateItemGrid(true, inappBonusActions);
		SortItemGrid(false, inappBonusActions);
		SortItemGrid(true, inappBonusActions);
		try
		{
			IsX3Bank = PromoActionsManager.sharedManager != null && PromoActionsManager.sharedManager.IsEventX3Active;
		}
		catch (Exception ex2)
		{
			Debug.LogErrorFormat("Exception in UpdateUi: {0}", ex2);
		}
	}

	private void SortItemGrid(bool isGems, List<Dictionary<string, object>> inappBonusActions)
	{
		Transform transform = (isGems ? gemsItemGrid : goldItemGrid).transform;
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
		UIScrollView obj = (isGems ? gemsScrollView : goldScrollView);
		(isGems ? gemsItemGrid : goldItemGrid).Reposition();
		obj.ResetPosition();
	}

	protected override IEnumerable<AbstractBankViewItem> AllItems()
	{
		return (gemsItemGrid.GetComponentsInChildren<AbstractBankViewItem>() ?? new AbstractBankViewItem[0]).Concat(goldItemGrid.GetComponentsInChildren<AbstractBankViewItem>() ?? new AbstractBankViewItem[0]);
	}
}
