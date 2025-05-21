using System;
using Rilisoft;
using Rilisoft.NullExtensions;
using UnityEngine;

public sealed class BuySmileBannerController : BannerWindow
{
	public static bool openedFromPromoActions;

	private IDisposable _backSubscription;

	public static string GetCurrentBuySmileContextName()
	{
		if (!(FriendsWindowGUI.Instance != null) || !FriendsWindowGUI.Instance.InterfaceEnabled)
		{
			if (!(ChatViewrController.sharedController != null))
			{
				return "Lobby";
			}
			return "Sandbox";
		}
		return "Friends";
	}

	private void OnEnable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(HandleEscape, "Buy Smiley Banner");
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void HandleEscape()
	{
		if (FriendsWindowGUI.Instance.Map((FriendsWindowGUI f) => f.InterfaceEnabled) || ChatViewrController.sharedController != null)
		{
			OnCloseClick();
		}
	}

	public void OnCloseClick()
	{
		ButtonClickSound.TryPlayClick();
		openedFromPromoActions = false;
		base.gameObject.SetActive(false);
	}

	public void BuyStickersPack(StickersPackItem curStickPack)
	{
		ItemPrice itemPrice = VirtualCurrencyHelper.Price(curStickPack.KeyForBuy);
		int priceAmount = itemPrice.Price;
		string priceCurrency = itemPrice.Currency;
		ShopNGUIController.TryToBuy(base.transform.root.gameObject, itemPrice, delegate
		{
			Storager.setInt(curStickPack.KeyForBuy, 1);
			try
			{
				string text = "Stickers";
				AnalyticsStuff.LogSales(curStickPack.KeyForBuy, text);
				AnalyticsFacade.InAppPurchase(curStickPack.KeyForBuy, text, 1, priceAmount, priceCurrency);
				openedFromPromoActions = false;
			}
			catch
			{
			}
			if (PrivateChatController.sharedController != null && PrivateChatController.sharedController.gameObject.activeInHierarchy)
			{
				PrivateChatController.sharedController.isBuySmile = true;
				if (!PrivateChatController.sharedController.isShowSmilePanel)
				{
					PrivateChatController.sharedController.showSmileButton.SetActive(true);
				}
				PrivateChatController.sharedController.buySmileButton.SetActive(false);
				OnCloseClick();
			}
			if (ChatViewrController.sharedController != null && ChatViewrController.sharedController.gameObject.activeInHierarchy)
			{
				ChatViewrController.sharedController.buySmileButton.SetActive(false);
				if (!ChatViewrController.sharedController.isShowSmilePanel)
				{
					ChatViewrController.sharedController.showSmileButton.SetActive(true);
				}
				OnCloseClick();
			}
			curStickPack.OnBuy();
			ButtonBannerHUD.OnUpdateBanners();
		});
	}
}
