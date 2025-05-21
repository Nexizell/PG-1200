using System;
using Rilisoft;
using UnityEngine;

public class FreeTicketsBankViewItem : AbstractBankViewItem
{
	public UILabel countLabel;

	public override bool isX3Item
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public override void Setup(IMarketProduct product, PurchaseEventArgs newPurchaseInfo, BankExchangeItemData exchangeInfo, EventHandler clickHandler)
	{
		base.Setup(product, newPurchaseInfo, exchangeInfo, clickHandler);
		Update();
	}

	protected override void OnEnable()
	{
	}

	protected override void OnDisable()
	{
	}

	protected override void SetIcon()
	{
		string path = "Textures/Bank/Static_Bank_Textures/Coins_Shop_1";
		icon.mainTexture = Resources.Load<Texture>(path);
	}

	protected override void Update()
	{
		try
		{
			countLabel.text = TicketsRewardedVideoController.Instance.GetCurrentReward().ToString();
		}
		catch (Exception ex)
		{
			Debug.LogErrorFormat("Exception in TicketsRewardedVideoController.Instance.GetCurrentReward: {0}", ex);
		}
	}
}
