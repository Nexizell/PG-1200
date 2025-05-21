using System;
using System.Collections.Generic;
using UnityEngine;

public class StickersController : MonoBehaviour
{
	public static string ClassicSmileKey = "SmileKey";

	public static string ChristmasSmileKey = "ChristmasSmileKey";

	public static string EasterSmileKey = "EasterSmileKey";

	public static event Action onBuyPack;

	public static string KeyForBuyPack(TypePackSticker needPack)
	{
		switch (needPack)
		{
		case TypePackSticker.classic:
			return ClassicSmileKey;
		case TypePackSticker.christmas:
			return ChristmasSmileKey;
		case TypePackSticker.easter:
			return EasterSmileKey;
		default:
			return null;
		}
	}

	public static ItemPrice GetPricePack(TypePackSticker needPack)
	{
		return VirtualCurrencyHelper.Price(KeyForBuyPack(needPack));
	}

	public static bool IsBuyPack(TypePackSticker needPack)
	{
		return Storager.getInt(KeyForBuyPack(needPack)) == 1;
	}

	public static void BuyStickersPack(TypePackSticker buyPack)
	{
		Storager.setInt(KeyForBuyPack(buyPack), 1);
	}

	public static bool IsBuyAnyPack()
	{
		foreach (object value in Enum.GetValues(typeof(TypePackSticker)))
		{
			if ((TypePackSticker)value != 0 && IsBuyPack((TypePackSticker)value))
			{
				return true;
			}
		}
		return false;
	}

	public static bool IsBuyAllPack()
	{
		foreach (object value in Enum.GetValues(typeof(TypePackSticker)))
		{
			if ((TypePackSticker)value != 0 && (TypePackSticker)value != TypePackSticker.easter && !IsBuyPack((TypePackSticker)value))
			{
				return false;
			}
		}
		return true;
	}

	public static List<TypePackSticker> GetAvaliablePack()
	{
		List<TypePackSticker> list = new List<TypePackSticker>();
		foreach (object value in Enum.GetValues(typeof(TypePackSticker)))
		{
			if ((TypePackSticker)value != 0 && IsBuyPack((TypePackSticker)value))
			{
				list.Add((TypePackSticker)value);
			}
		}
		return list;
	}

	public static void EventPackBuy()
	{
		if (StickersController.onBuyPack != null)
		{
			StickersController.onBuyPack();
		}
	}
}
