using System.Collections.Generic;
using UnityEngine;

public class ChestBonusData
{
	public string linkKey;

	public bool isVisible;

	public List<ChestBonusItemData> items;

	public string GetItemCountOrTime()
	{
		if (items == null || items.Count == 0)
		{
			return string.Empty;
		}
		if (items.Count == 1)
		{
			ChestBonusItemData chestBonusItemData = items[0];
			int num = chestBonusItemData.timeLife / 24;
			if (chestBonusItemData.timeLife != -1)
			{
				return chestBonusItemData.GetTimeLabel(true);
			}
			return chestBonusItemData.count.ToString();
		}
		return string.Empty;
	}

	public Texture GetImage()
	{
		if (items == null || items.Count == 0)
		{
			return null;
		}
		string empty = string.Empty;
		if (items.Count == 1)
		{
			return ItemDb.GetTextureForShopItem(items[0].tag);
		}
		empty = "Textures/Bank/StarterPack_Weapon";
		return Resources.Load<Texture>(empty);
	}
}
