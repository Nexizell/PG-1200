using System;
using Rilisoft;
using UnityEngine;

public abstract class RealInappBankViewItem : AbstractBankViewItem
{
	public override void Setup(IMarketProduct product, PurchaseEventArgs newPurchaseInfo, BankExchangeItemData exchangeInfo, EventHandler clickHandler)
	{
		base.Setup(product, newPurchaseInfo, exchangeInfo, clickHandler);
		base.MarketProduct = product;
	}

	public RealInappBankViewItem()
	{
	}
}
