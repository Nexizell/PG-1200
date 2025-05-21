using System;
using Rilisoft;
using UnityEngine;

public class ExchangeBankViewItem : AbstractBankViewItem
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
		countLabel.text = exchangeInfo.CurrencyCount.ToString();
		priceLabel.text = exchangeInfo.GemsPrice.ToString();
		SetIcon();
	}

	protected override void OnEnable()
	{
	}

	protected override void OnDisable()
	{
	}

	protected override void SetIcon()
	{
		string path = "Textures/Bank/Static_Bank_Textures/Coins_Shop_" + base.ExchangeInfo.InAppId;
		icon.mainTexture = Resources.Load<Texture>(path);
	}
}
