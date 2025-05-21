using System;
using I2.Loc;
using UnityEngine;

public sealed class RespawnWindowItemToBuy : MonoBehaviour
{
	public UITexture itemImage;

	public UILabel itemNameLabel;

	public GameObject itemPriceBtnBuyContainer;

	public UILabel needTierLabel;

	public UISprite itemPriceIcon;

	public UILabel itemPriceLabel;

	public UIButton btnBuy;

	[NonSerialized]
	public string itemTag;

	[NonSerialized]
	public int itemCategory;

	[NonSerialized]
	public ItemPrice itemPrice;

	[NonSerialized]
	public string nonLocalizedName;

	private static Color priceGemColor = new Color32(100, 230, byte.MaxValue, byte.MaxValue);

	private static Color priceCoinColor = new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue);

	public void SetWeaponTag(string itemTag, int? upgradeNum = null)
	{
		if (string.IsNullOrEmpty(itemTag))
		{
			base.gameObject.SetActive(false);
			return;
		}
		base.gameObject.SetActive(true);
		int num = ItemDb.GetItemCategory(itemTag);
		ShopNGUIController.CategoryNames categoryNames = (ShopNGUIController.CategoryNames)num;
		this.itemTag = itemTag;
		itemCategory = num;
		itemPrice = null;
		itemImage.mainTexture = ItemDb.GetItemIcon(itemTag, categoryNames, upgradeNum);
		itemNameLabel.text = GetItemName(itemTag, categoryNames, upgradeNum.HasValue ? upgradeNum.Value : 0);
		nonLocalizedName = GetItemNonLocalizedName(itemTag, categoryNames, upgradeNum.HasValue ? upgradeNum.Value : 0);
		if (!IsCanBuy(itemTag, categoryNames))
		{
			itemPriceBtnBuyContainer.gameObject.SetActive(false);
			needTierLabel.gameObject.SetActive(false);
			return;
		}
		itemPriceBtnBuyContainer.SetActive(true);
		needTierLabel.gameObject.SetActive(false);
		if (ShopNGUIController.IsWeaponCategory(categoryNames))
		{
			int tier = ItemDb.GetWeaponInfo(itemTag).tier;
			if (ExpController.Instance != null && ExpController.Instance.OurTier < tier)
			{
				itemPriceBtnBuyContainer.SetActive(false);
				needTierLabel.gameObject.SetActive(true);
				int num2 = ((tier >= 0 && tier < ExpController.LevelsForTiers.Length) ? ExpController.LevelsForTiers[tier] : ExpController.LevelsForTiers[ExpController.LevelsForTiers.Length - 1]);
				string text = string.Format("{0} {1} {2}", new object[3]
				{
					LocalizationStore.Key_0226,
					num2,
					LocalizationStore.Get("Key_1022")
				});
				needTierLabel.text = text;
			}
		}
		itemPrice = ShopNGUIController.GetItemPrice(itemTag, categoryNames);
		SetPrice(itemPriceIcon, itemPriceLabel, itemPrice);
	}

	public static string GetItemName(string itemTag, ShopNGUIController.CategoryNames itemCategory, int upgradeNum = 0)
	{
		if (itemTag == "LikeID")
		{
			return ScriptLocalization.Get("Key_1796");
		}
		if (GearManager.IsItemGear(itemTag))
		{
			upgradeNum = Mathf.Min(upgradeNum, GearManager.NumOfGearUpgrades);
			itemTag = GearManager.UpgradeIDForGear(itemTag, upgradeNum);
		}
		return ItemDb.GetItemName(itemTag, itemCategory);
	}

	public static string GetItemNonLocalizedName(string itemTag, ShopNGUIController.CategoryNames itemCategory, int upgradeNum = 0)
	{
		string shopId = itemTag;
		if (ShopNGUIController.IsWeaponCategory(itemCategory))
		{
			shopId = ItemDb.GetShopIdByTag(itemTag);
		}
		if (GearManager.IsItemGear(itemTag))
		{
			upgradeNum = Mathf.Min(upgradeNum, GearManager.NumOfGearUpgrades);
			shopId = GearManager.UpgradeIDForGear(itemTag, upgradeNum);
		}
		return ItemDb.GetItemNameNonLocalized(itemTag, shopId, itemCategory, itemTag);
	}

	private static bool IsCanBuy(string itemTag, ShopNGUIController.CategoryNames itemCategory)
	{
		if (ShopNGUIController.IsWeaponCategory(itemCategory))
		{
			bool num = ItemDb.IsCanBuy(itemTag) && !ItemDb.IsTemporaryGun(itemTag);
			bool flag = ItemDb.IsItemInInventory(itemTag);
			bool flag2 = ItemDb.HasWeaponNeedUpgradesForBuyNext(itemTag);
			string text = WeaponManager.FirstTagForOurTier(itemTag);
			if (num && !flag)
			{
				if (!flag2)
				{
					if (text != null)
					{
						return text.Equals(itemTag);
					}
					return false;
				}
				return true;
			}
			return false;
		}
		if (GearManager.IsItemGear(itemTag))
		{
			return false;
		}
		if (itemCategory == ShopNGUIController.CategoryNames.ArmorCategory)
		{
			if (!ItemDb.IsItemInInventory(itemTag))
			{
				return !TempItemsController.PriceCoefs.ContainsKey(itemTag);
			}
			return false;
		}
		return false;
	}

	private static void SetPrice(UISprite priceIcon, UILabel priceLabel, ItemPrice price)
	{
		bool flag = price.Currency == "GemsCurrency";
		priceIcon.spriteName = (flag ? "gem_znachek" : "ingame_coin");
		priceIcon.width = (flag ? 24 : 30);
		priceIcon.height = (flag ? 24 : 30);
		priceLabel.text = price.Price.ToString();
		priceLabel.color = (flag ? priceGemColor : priceCoinColor);
	}

	public void Reset()
	{
		itemImage.mainTexture = null;
		itemTag = string.Empty;
		nonLocalizedName = string.Empty;
	}
}
