using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class BankItemsComparer : IComparer<AbstractBankViewItem>
{
	private List<Dictionary<string, object>> m_inappBonusActions;

	public int Compare(AbstractBankViewItem x, AbstractBankViewItem y)
	{
		if (x is FreeTicketsBankViewItem)
		{
			return -1;
		}
		if (y is FreeTicketsBankViewItem)
		{
			return 1;
		}
		if (x is BonusBankViewItem && y is BonusBankViewItem)
		{
			if (m_inappBonusActions == null)
			{
				return 0;
			}
			try
			{
				BonusBankViewItem bonusBankViewItem = x as BonusBankViewItem;
				BonusBankViewItem bonusBankViewItem2 = y as BonusBankViewItem;
				string xUniqueId = bonusBankViewItem.InappBonusParameters["Key"] as string;
				string yUniqueId = bonusBankViewItem2.InappBonusParameters["Key"] as string;
				return m_inappBonusActions.FindIndex((Dictionary<string, object> bonus) => bonus["Key"] as string == xUniqueId).CompareTo(m_inappBonusActions.FindIndex((Dictionary<string, object> bonus) => bonus["Key"] as string == yUniqueId));
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in ItemsComparer.Compare: {0}", ex);
				return 0;
			}
		}
		if (x is BonusBankViewItem)
		{
			return -1;
		}
		if (y is BonusBankViewItem)
		{
			return 1;
		}
		if (x is ExchangeBankViewItem && y is ExchangeBankViewItem)
		{
			int num = x.ExchangeInfo.GemsPrice.CompareTo(y.ExchangeInfo.GemsPrice);
			if (StoreKitEventListener.IsPayingUser())
			{
				num *= -1;
			}
			return num;
		}
		if (x is ExchangeBankViewItem)
		{
			return 1;
		}
		if (y is ExchangeBankViewItem)
		{
			return -1;
		}
		int value = ((y != null) ? y.purchaseInfo.Count : 0);
		if (!StoreKitEventListener.IsPayingUser())
		{
			return x.purchaseInfo.Count.CompareTo(value);
		}
		return value.CompareTo(x.purchaseInfo.Count);
	}

	public BankItemsComparer(List<Dictionary<string, object>> inappBonusActions)
	{
		m_inappBonusActions = inappBonusActions;
	}
}
