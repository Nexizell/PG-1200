using System;
using Rilisoft;
using UnityEngine;

public class RespawnWindowEquipmentItem : MonoBehaviour
{
	public UITexture itemImage;

	public UISprite emptyImage;

	[NonSerialized]
	public string itemTag;

	[NonSerialized]
	public int itemCategory;

	public void SetItemTag(string itemTag, int itemCategory)
	{
		bool flag = IsNoneEquipment(itemTag);
		itemImage.gameObject.SetActiveSafeSelf(!flag);
		emptyImage.gameObject.SetActiveSafeSelf(flag);
		this.itemTag = (flag ? null : itemTag);
		this.itemCategory = (flag ? (-1) : itemCategory);
		if (!flag)
		{
			itemImage.mainTexture = ItemDb.GetItemIcon(itemTag, (ShopNGUIController.CategoryNames)itemCategory);
		}
	}

	public void ResetImage()
	{
		itemImage.mainTexture = null;
	}

	private static bool IsNoneEquipment(string itemTag)
	{
		if (!string.IsNullOrEmpty(itemTag) && !(itemTag == Defs.HatNoneEqupped) && !(itemTag == Defs.ArmorNewNoneEqupped) && !(itemTag == Defs.CapeNoneEqupped) && !(itemTag == Defs.BootsNoneEqupped))
		{
			return itemTag == "MaskNoneEquipped";
		}
		return true;
	}
}
