using System;
using System.Collections.Generic;

public class ABTestBankOneCurrency : ABTestBase
{
	public override string currentFolder
	{
		get
		{
			return "BankOneCurrency";
		}
	}

	protected override void ApplyState(ABTestController.ABTestCohortsType _state, object settingsB)
	{
		base.ApplyState(_state, settingsB);
		BankController.isBankOneCurrency = _state == ABTestController.ABTestCohortsType.B;
		List<object> list = settingsB as List<object>;
		if (list == null)
		{
			return;
		}
		List<BankExchangeItemData> list2 = new List<BankExchangeItemData>();
		foreach (object item2 in list)
		{
			List<object> obj = item2 as List<object>;
			int num = Convert.ToInt32(obj[0]);
			int gemsPrice = Convert.ToInt32(obj[1]);
			int currencyCount = Convert.ToInt32(obj[2]);
			int inAppId = 0;
			for (int i = 0; i < VirtualCurrencyHelper.coinPriceIds.Length; i++)
			{
				if (VirtualCurrencyHelper.coinPriceIds[i] == num)
				{
					inAppId = i + 1;
					break;
				}
			}
			BankExchangeItemData item = new BankExchangeItemData
			{
				InAppId = inAppId,
				GemsPrice = gemsPrice,
				CurrencyCount = currencyCount
			};
			list2.Add(item);
		}
		BankViewOneCurrency.RewriteChangeItems(list2);
	}
}
