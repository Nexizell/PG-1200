using System;
using UnityEngine;

public class LobbyItemInfo
{
	public enum LobbyItemSlot
	{
		Base = 0,
		wall = 1,
		gate = 2,
		kennel = 3,
		terrain = 4,
		road = 5,
		decor_small = 6,
		decor_big = 7,
		background_1 = 8,
		background_2 = 9,
		device_1 = 10,
		device_2 = 11,
		skybox = 12
	}

	[Serializable]
	public class LobbyItemBuff 
	{
		public LobbyItemBuffType BuffType;

		public float Value;
	}

	public enum LobbyItemBuffType
	{
		None = 0,
		PetSpeedUpMultiplier = 1,
		PetDamadeMultiplier = 2,
		FreeTicketsSpeedUp = 3,
		FreeGemsPerDay = 4,
		EggDropAccelerator = 5,
		FreeSpinsPerDay = 6
	}

	public LobbyItemSlot Slot { get; protected set; }

	public string Id { get; protected set; }

	public string Lkey { get; protected set; }

	public string EffectLkey { get; protected set; }

	public int Sort { get; protected set; }

	public int Level
	{
		get
		{
			int value;
			if (BalanceController.levelLobbyItems.TryGetValue(Id, out value))
			{
				return value;
			}
			return 1;
		}
	}

	public int ExpForCrafting
	{
		get
		{
			int value;
			if (BalanceController.xpLobbyItems.TryGetValue(Id, out value))
			{
				return value;
			}
			return 10;
		}
	}

	public long CraftTime
	{
		get
		{
			if (Id == "decor_big_military_1")
			{
				return 10L;
			}
			int value;
			if (BalanceController.timeLobbyItems.TryGetValue(Id, out value))
			{
				return (long)value * 60L;
			}
			return 1440L;
		}
	}

	public ItemPrice PriceBuy
	{
		get
		{
			ItemPrice value;
			if (BalanceController.priceLobbyItems.TryGetValue(Id, out value))
			{
				return value;
			}
			return new ItemPrice(100, "Coins");
		}
	}

	public ItemPrice PriceSpeedUp
	{
		get
		{
			ItemPrice value;
			if (BalanceController.priceTimeLobbyItems.TryGetValue(Id, out value))
			{
				return value;
			}
			return new ItemPrice(100, "GemsCurrency");
		}
	}

	public ItemPrice PriceInstant
	{
		get
		{
			ItemPrice value;
			if (BalanceController.priceFullLobbyItems.TryGetValue(Id, out value))
			{
				return value;
			}
			return new ItemPrice(200, "GemsCurrency");
		}
	}

	public string[] Materials { get; protected set; }

	public LobbyItemBuff[] Buffs { get; protected set; }

	public LobbyItemInfo(LobbyItemSlot cat, string id, string locKey, string[] materials, int sort, LobbyItemBuff[] buffs, string effectLocKey)
	{
		Slot = cat;
		Id = id;
		Lkey = locKey;
		Materials = materials;
		Sort = sort;
		Buffs = buffs;
		EffectLkey = effectLocKey;
	}
}
