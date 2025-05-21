using System;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyItem
	{
		public LobbyItemInfo Info { get; private set; }

		public LobbyItemPlayerInfo PlayerInfo { get; set; }

		public LobbyItemInfo.LobbyItemSlot Slot
		{
			get
			{
				return Info.Slot;
			}
		}

		public bool IsCrafting
		{
			get
			{
				if (PlayerInfo != null && !PlayerInfo.IsCrafted)
				{
					return PlayerInfo.CraftStarted > 0;
				}
				return false;
			}
		}

		public long CraftTimeLeft
		{
			get
			{
				if (!LobbyItemsController.CurrentTime.HasValue)
				{
					return long.MaxValue;
				}
				if (PlayerInfo.IsCrafted)
				{
					return 0L;
				}
				return (PlayerInfo.CraftStarted + Info.CraftTime - LobbyItemsController.CurrentTime).Value;
			}
		}

		public long CraftTimeElapsed
		{
			get
			{
				return Info.CraftTime - CraftTimeLeft;
			}
		}

		public bool IsExists
		{
			get
			{
				if (PlayerInfo != null)
				{
					return PlayerInfo.IsCrafted;
				}
				return false;
			}
		}

		public bool IsEquiped
		{
			get
			{
				if (PlayerInfo != null)
				{
					return PlayerInfo.IsEquiped;
				}
				return false;
			}
		}

		public ItemPrice PriceSpeedUpLeft
		{
			get
			{
				if (!LobbyItemsController.CurrentTime.HasValue || PlayerInfo == null || PlayerInfo.IsCrafted || PlayerInfo.CraftStarted < 1)
				{
					return Info.PriceSpeedUp;
				}
				float num = (float)CraftTimeLeft / (float)Info.CraftTime;
				int num2 = 0;
				if (Info.PriceSpeedUp.Price == 0)
				{
					num2 = 0;
				}
				else
				{
					num2 = Mathf.RoundToInt((float)Info.PriceSpeedUp.Price * num);
					if (num2 < 1)
					{
						num2 = 1;
					}
				}
				return new ItemPrice(num2, Info.PriceSpeedUp.Currency);
			}
		}

		public string TextureName
		{
			get
			{
				return Info.Id + "_icon1_big";
			}
		}

		public string TexturePath
		{
			get
			{
				return "OfferIcons/" + TextureName;
			}
		}

		public string PrefabName
		{
			get
			{
				return Info.Id;
			}
		}

		public string PrefabPath
		{
			get
			{
				return "LobbyItemsContent/" + PrefabName;
			}
		}

		public bool IsDefaultItem
		{
			get
			{
				return LobbyItemsController.IsDefaultItem(this);
			}
		}

		public bool CanShowIsNew
		{
			get
			{
				if (AvailableByLevel)
				{
					return PlayerInfo.IsNew;
				}
				return false;
			}
			set
			{
				PlayerInfo.IsNew = value;
			}
		}

		public bool AvailableByLevel
		{
			get
			{
				if (PlayerInfo != null && ExperienceController.sharedController != null)
				{
					return ExperienceController.sharedController.currentLevel >= Info.Level;
				}
				return false;
			}
		}

		public event Action OnPlayerDataSetted;

		public LobbyItem(LobbyItemInfo info, LobbyItemPlayerInfo playerInfo)
		{
			Info = info;
			PlayerInfo = playerInfo;
			Singleton<LobbyItemsController>.Instance.AllItems.Where((LobbyItem i) => i.PlayerInfo.IsEquiped);
		}

		public void SetPlayerDataValues(LobbyItemPlayerInfo playerData)
		{
			bool flag = false;
			if (PlayerInfo == null)
			{
				PlayerInfo = new LobbyItemPlayerInfo();
				flag = true;
			}
			if (!PlayerInfo.Equals(playerData))
			{
				PlayerInfo.CopyValues(playerData);
				flag = true;
			}
			if (flag && this.OnPlayerDataSetted != null)
			{
				this.OnPlayerDataSetted();
			}
		}
	}
}
