using System;
using Rilisoft;
using UnityEngine;

public sealed class PurchasesMiniGameGuiController : MonoBehaviour
{
	public GameObject allInterafaceContainer;

	public PurchaseMiniGameView[] views;

	public NetworkStartTableNGUIController nguiController;

	private void Awake()
	{
		allInterafaceContainer.SetActiveSafeSelf(GameConnect.isSpleef);
		if (GameConnect.isSpleef)
		{
			PurchaseMiniGameItem[] array = new PurchaseMiniGameItem[3]
			{
				new PurchaseMiniGameItem
				{
					Id = WeaponTags.spleef_marker_1_Tag,
					price = new ItemPrice(0, "GemsCurrency")
				},
				new PurchaseMiniGameItem
				{
					Id = WeaponTags.spleef_marker_2_Tag,
					price = new ItemPrice(1, "GemsCurrency")
				},
				new PurchaseMiniGameItem
				{
					Id = WeaponTags.spleef_marker_3_Tag,
					price = new ItemPrice(2, "GemsCurrency")
				}
			};
			for (int i = 0; i < array.Length; i++)
			{
				views[i].Setup(array[i]);
			}
		}
		UpdateViews();
	}

	public void HandleItemClicked(PurchaseMiniGameView view)
	{
		if (!GameConnect.isSpleef)
		{
			return;
		}
		ShopNGUIController.TryToBuy(nguiController.allInterfaceContainer, view.Item.price, delegate
		{
			try
			{
				WeaponManager.sharedManager.AddSpleefWeapon(view.Item.Id);
				AnalyticsStuff.MiniGamesSales(view.Item.Id, true);
				UIPlaySound component = view.GetComponent<UIPlaySound>();
				if (component != null && Defs.isSoundFX)
				{
					component.Play();
				}
			}
			catch (Exception ex)
			{
				Debug.LogErrorFormat("Exception in HandleItemClicked AddSpleefWeapon: {0}", ex);
			}
		});
	}

	private void Update()
	{
		UpdateViews();
	}

	private void UpdateViews()
	{
		if (!GameConnect.isSpleef)
		{
			return;
		}
		for (int i = 0; i < views.Length; i++)
		{
			bool canBuy = views[i].Item.price.Price > 0 && !WeaponManager.sharedManager.HasSpleefGun(views[i].Item.Id);
			views[i].buttons.ForEach(delegate(UIButton b)
			{
				b.isEnabled = canBuy;
			});
			views[i].equipped.SetActiveSafeSelf(!canBuy);
			views[i].priceCurrency.gameObject.SetActiveSafeSelf(canBuy);
			views[i].price.ForEach(delegate(UILabel label)
			{
				label.gameObject.SetActiveSafeSelf(canBuy);
			});
		}
	}
}
