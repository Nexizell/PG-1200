using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyItemsController : Singleton<LobbyItemsController>
	{
		public const string LOBBY_ITEMS_PLAYER_DATA_KEY = "lobby_items";

		private bool _isReady;

		private List<LobbyItem> _allItems;

		public const string TutorialItemId = "decor_big_military_1";

		public static LobbyItemGroupType[] TutorialGroups = new LobbyItemGroupType[2]
		{
			LobbyItemGroupType.Decorations,
			LobbyItemGroupType.BigDecor
		};

		private static bool _tutorialCompleted;

		private static readonly List<LobbyItemGroup> Groups = new List<LobbyItemGroup>
		{
			new LobbyItemGroup(LobbyItemGroupType.Buildings, null, LobbyItemGroupType.None),
			new LobbyItemGroup(LobbyItemGroupType.Base, new LobbyItemInfo.LobbyItemSlot[1], LobbyItemGroupType.Buildings),
			new LobbyItemGroup(LobbyItemGroupType.Gate, new LobbyItemInfo.LobbyItemSlot[1] { LobbyItemInfo.LobbyItemSlot.gate }, LobbyItemGroupType.Buildings),
			new LobbyItemGroup(LobbyItemGroupType.Fance, new LobbyItemInfo.LobbyItemSlot[1] { LobbyItemInfo.LobbyItemSlot.wall }, LobbyItemGroupType.Buildings),
			new LobbyItemGroup(LobbyItemGroupType.Decorations, null, LobbyItemGroupType.None),
			new LobbyItemGroup(LobbyItemGroupType.Terrain, new LobbyItemInfo.LobbyItemSlot[1] { LobbyItemInfo.LobbyItemSlot.terrain }, LobbyItemGroupType.Decorations),
			new LobbyItemGroup(LobbyItemGroupType.Road, new LobbyItemInfo.LobbyItemSlot[1] { LobbyItemInfo.LobbyItemSlot.road }, LobbyItemGroupType.Decorations),
			new LobbyItemGroup(LobbyItemGroupType.Decor, new LobbyItemInfo.LobbyItemSlot[1] { LobbyItemInfo.LobbyItemSlot.decor_small }, LobbyItemGroupType.Decorations),
			new LobbyItemGroup(LobbyItemGroupType.BigDecor, new LobbyItemInfo.LobbyItemSlot[1] { LobbyItemInfo.LobbyItemSlot.decor_big }, LobbyItemGroupType.Decorations),
			new LobbyItemGroup(LobbyItemGroupType.Backgrounds, null, LobbyItemGroupType.None),
			new LobbyItemGroup(LobbyItemGroupType.BackgroundStatic, new LobbyItemInfo.LobbyItemSlot[1] { LobbyItemInfo.LobbyItemSlot.background_1 }, LobbyItemGroupType.Backgrounds),
			new LobbyItemGroup(LobbyItemGroupType.BackgroundDynamic, new LobbyItemInfo.LobbyItemSlot[1] { LobbyItemInfo.LobbyItemSlot.background_2 }, LobbyItemGroupType.Backgrounds),
			new LobbyItemGroup(LobbyItemGroupType.Effects, new LobbyItemInfo.LobbyItemSlot[1] { LobbyItemInfo.LobbyItemSlot.skybox }, LobbyItemGroupType.Backgrounds),
			new LobbyItemGroup(LobbyItemGroupType.PetKennel, new LobbyItemInfo.LobbyItemSlot[1] { LobbyItemInfo.LobbyItemSlot.kennel }, LobbyItemGroupType.None),
			new LobbyItemGroup(LobbyItemGroupType.Devices, new LobbyItemInfo.LobbyItemSlot[2]
			{
				LobbyItemInfo.LobbyItemSlot.device_1,
				LobbyItemInfo.LobbyItemSlot.device_2
			}, LobbyItemGroupType.None)
		};

		private static readonly Dictionary<LobbyItemInfo.LobbyItemSlot, string> _defaultPlayerItems = new Dictionary<LobbyItemInfo.LobbyItemSlot, string>
		{
			{
				LobbyItemInfo.LobbyItemSlot.skybox,
				"skybox_1"
			},
			{
				LobbyItemInfo.LobbyItemSlot.background_1,
				"background_2_military_1"
			},
			{
				LobbyItemInfo.LobbyItemSlot.terrain,
				"terrain_military_1"
			},
			{
				LobbyItemInfo.LobbyItemSlot.road,
				"road_military_1"
			},
			{
				LobbyItemInfo.LobbyItemSlot.Base,
				"base_military_1"
			},
			{
				LobbyItemInfo.LobbyItemSlot.wall,
				"wall_military_1"
			},
			{
				LobbyItemInfo.LobbyItemSlot.gate,
				"gate_military_1"
			}
		};

		private Dictionary<LobbyItemInfo.LobbyItemBuffType, float> _effectsDict;

		private Dictionary<LobbyItem, List<LobbyItemEffectHandler>> _effectHandlers;

		public bool IsReady
		{
			get
			{
				return _isReady;
			}
			private set
			{
				if (_isReady != value)
				{
					_isReady = value;
					if (LobbyItemsController.OnReadyChanged != null)
					{
						LobbyItemsController.OnReadyChanged(_isReady);
					}
				}
			}
		}

		public static long? CurrentTime
		{
			get
			{
				if (!TutorialCompleted)
				{
					return Tools.CurrentUnixTime;
				}
				if (FriendsController.ServerTime > 0)
				{
					return FriendsController.ServerTime;
				}
				return null;
			}
		}

		public List<LobbyItem> AllItems
		{
			get
			{
				return _allItems;
			}
		}

		public List<LobbyItem> AllPlayerItems
		{
			get
			{
				return AllItems.Where((LobbyItem i) => i.IsExists).ToList();
			}
		}

		public List<LobbyItem> AllPlayerEquipedItems
		{
			get
			{
				return AllItems.Where((LobbyItem i) => i.IsExists && i.IsEquiped).ToList();
			}
		}

		public LobbyItem PlayerCraftedAndNotShownItem
		{
			get
			{
				return AllItems.FirstOrDefault((LobbyItem i) => i.PlayerInfo.IsCrafted && i.PlayerInfo.IsCraftedAndNotShown);
			}
		}

		public LobbyItem CraftingItem { get; private set; }

		public Dictionary<LobbyItemInfo.LobbyItemSlot, LobbyItem> EquipedItems
		{
			get
			{
				Dictionary<LobbyItemInfo.LobbyItemSlot, LobbyItem> dictionary = new Dictionary<LobbyItemInfo.LobbyItemSlot, LobbyItem>();
				foreach (LobbyItem allItem in AllItems)
				{
					if (allItem.IsEquiped)
					{
						dictionary.Add(allItem.Slot, allItem);
					}
				}
				return dictionary;
			}
		}

		public static bool TutorialCompleted
		{
			get
			{
				if (!_tutorialCompleted)
				{
					_tutorialCompleted = Singleton<LobbyItemsController>.Instance.AllPlayerItems.Any((LobbyItem i) => i.Info.Id == "decor_big_military_1");
				}
				return _tutorialCompleted;
			}
		}

		public static event Action<LobbyItem> OnCraftStarted;

		public static event Action<LobbyItem> OnCraftFinished;

		public static event Action<LobbyItem> OnItemAdded;

		public static event Action<LobbyItemInfo.LobbyItemSlot, LobbyItem, LobbyItem> OnEquipChanged;

		public static event Action<bool> OnReadyChanged;

		public event Action OnAnyPlayerDataUpdated;

		public static event Action<LobbyItemInfo.LobbyItemBuffType, float> OnEffectChanged;

		internal int GetLikeCount()
		{
			if (!(FriendsController.sharedController != null) || FriendsController.sharedController.OurLobbyLikes == null)
			{
				return 0;
			}
			return FriendsController.sharedController.OurLobbyLikes.Likes;
		}

		public static bool IsDefaultItem(LobbyItem item)
		{
			if (item == null)
			{
				return false;
			}
			if (_defaultPlayerItems.ContainsKey(item.Slot))
			{
				if (_defaultPlayerItems[item.Slot] != null)
				{
					return _defaultPlayerItems[item.Slot] == item.Info.Id;
				}
				return false;
			}
			return false;
		}

		public static LobbyItem GetDefaultItemForSlot(LobbyItemInfo.LobbyItemSlot slot)
		{
			if (_defaultPlayerItems.ContainsKey(slot))
			{
				if (_defaultPlayerItems[slot].IsNullOrEmpty())
				{
					return null;
				}
				return Singleton<LobbyItemsController>.Instance.AllItems.FirstOrDefault((LobbyItem item) => item.Info.Id == _defaultPlayerItems[slot]);
			}
			return null;
		}

		public static bool SlotCanBeEmpty(LobbyItemInfo.LobbyItemSlot slot)
		{
			if (slot != LobbyItemInfo.LobbyItemSlot.skybox && slot != LobbyItemInfo.LobbyItemSlot.terrain && slot != LobbyItemInfo.LobbyItemSlot.road && slot != 0)
			{
				return slot != LobbyItemInfo.LobbyItemSlot.background_2;
			}
			return false;
		}

		public List<LobbyItem> ItemsPerGroup(LobbyItemGroupType groupType, bool andSubGroups = false)
		{
			List<LobbyItemInfo.LobbyItemSlot> slots = GetSlotsForGroup(groupType, andSubGroups);
			if (slots == null)
			{
				return null;
			}
			return AllItems.Where((LobbyItem i) => slots.Contains(i.Info.Slot)).ToList();
		}

		public static List<LobbyItemInfo.LobbyItemSlot> GetSlotsForGroup(LobbyItemGroupType groupType, bool andSubGroups)
		{
			LobbyItemGroup group = Groups.FirstOrDefault((LobbyItemGroup g) => g.GroupType == groupType);
			if (group == null)
			{
				return null;
			}
			List<LobbyItemInfo.LobbyItemSlot> list;
			if (andSubGroups)
			{
				list = Groups.Where((LobbyItemGroup g) => g.ParentGroupType == group.GroupType).SelectMany((LobbyItemGroup g) => g.ItemSlots).ToList();
				if (group.ItemSlots != null)
				{
					list.AddRange(group.ItemSlots);
				}
			}
			else
			{
				list = new List<LobbyItemInfo.LobbyItemSlot>();
				if (group.ItemSlots != null)
				{
					list.AddRange(group.ItemSlots);
				}
			}
			return list;
		}

		public static bool IsSubGroupOf(LobbyItemGroupType groupType, LobbyItemGroupType rootGroupType)
		{
			LobbyItemGroup lobbyItemGroup = Groups.FirstOrDefault((LobbyItemGroup g) => g.GroupType == groupType);
			if (lobbyItemGroup == null)
			{
				return false;
			}
			return lobbyItemGroup.ParentGroupType == rootGroupType;
		}

		public static LobbyItemGroupType GetParentGroup(LobbyItemGroupType groupType)
		{
			LobbyItemGroup lobbyItemGroup = Groups.FirstOrDefault((LobbyItemGroup g) => g.GroupType == groupType);
			if (lobbyItemGroup == null)
			{
				return LobbyItemGroupType.None;
			}
			return lobbyItemGroup.ParentGroupType;
		}

		public static List<LobbyItemGroupType> GetChildGroup(LobbyItemGroupType groupType)
		{
			return (from g in Groups
				where g.ParentGroupType == groupType
				select g.GroupType).ToList();
		}

		public static LobbyItemGroupType GetGroupForSlot(LobbyItemInfo.LobbyItemSlot slot)
		{
			return Groups.First((LobbyItemGroup g) => g.ItemSlots != null && g.ItemSlots.Contains(slot)).GroupType;
		}

		public static LobbyItemGroupType GetRootGroupForSlot(LobbyItemInfo.LobbyItemSlot slot)
		{
			LobbyItemGroupType groupType = Groups.First((LobbyItemGroup g) => g.ItemSlots != null && g.ItemSlots.Contains(slot)).GroupType;
			LobbyItemGroupType parentGroup = GetParentGroup(groupType);
			if (parentGroup == LobbyItemGroupType.None)
			{
				return groupType;
			}
			return parentGroup;
		}

		public List<string> GetPlayerEquipedItemsIds()
		{
			return AllPlayerEquipedItems.Select((LobbyItem i) => i.Info.Id).ToList();
		}

		public static bool SlotInGroup(LobbyItemInfo.LobbyItemSlot slot, LobbyItemGroupType group, bool checkSubGroups = false)
		{
			List<LobbyItemInfo.LobbyItemSlot> slotsForGroup = GetSlotsForGroup(group, checkSubGroups);
			if (slotsForGroup == null)
			{
				return false;
			}
			return slotsForGroup.Contains(slot);
		}

		public List<LobbyItem> GetItemsForSlot(LobbyItemInfo.LobbyItemSlot slot)
		{
			return AllItems.Where((LobbyItem i) => i.Info.Slot == slot).ToList();
		}

		public LobbyItem GetItemInSlot(LobbyItemInfo.LobbyItemSlot slot)
		{
			return AllItems.FirstOrDefault((LobbyItem i) => i.Info.Slot == slot && i.IsEquiped);
		}

		public bool StartCraft(LobbyItem item, bool completeNow = false)
		{
			if (!CurrentTime.HasValue)
			{
				Debug.LogError("CurrentTime is null");
				return false;
			}
			if (item == null)
			{
				Debug.LogError("item is null");
				return false;
			}
			if (item.IsCrafting && !completeNow)
			{
				Debug.LogErrorFormat("item '{0}' is crafting now", item.Info.Id);
				return false;
			}
			CraftingItem = item;
			if (item.PlayerInfo == null)
			{
				item.PlayerInfo = new LobbyItemPlayerInfo
				{
					InfoId = item.Info.Id
				};
			}
			item.PlayerInfo.CraftStarted = CurrentTime.Value;
			item.PlayerInfo.IsNew = false;
			if (completeNow)
			{
				item.PlayerInfo.CraftStarted += item.Info.CraftTime;
			}
			SavePlayerCurrentData();
			if (LobbyItemsController.OnCraftStarted != null)
			{
				LobbyItemsController.OnCraftStarted(CraftingItem);
			}
			return true;
		}

		public bool AddItemNow(LobbyItem item)
		{
			if (!CurrentTime.HasValue)
			{
				Debug.LogError("CurrentTime is null");
				return false;
			}
			if (item == null)
			{
				Debug.LogError("item is null");
				return false;
			}
			if (item.PlayerInfo == null)
			{
				item.PlayerInfo = new LobbyItemPlayerInfo
				{
					InfoId = item.Info.Id
				};
			}
			item.PlayerInfo.CraftStarted = CurrentTime.Value + item.Info.CraftTime;
			item.PlayerInfo.IsCrafted = true;
			item.PlayerInfo.IsNew = false;
			SavePlayerCurrentData();
			if (LobbyItemsController.OnItemAdded != null)
			{
				LobbyItemsController.OnItemAdded(item);
			}
			return true;
		}

		public bool SpeedUpNow(LobbyItem item)
		{
			if (!CurrentTime.HasValue)
			{
				Debug.LogError("CurrentTime is null");
				return false;
			}
			if (item == null)
			{
				Debug.LogError("item is null");
				return false;
			}
			if (item.IsExists)
			{
				Debug.LogErrorFormat("item '{0}' is crafted", item.Info.Id);
				return false;
			}
			if (item.PlayerInfo == null)
			{
				Debug.LogErrorFormat("item '{0}' is not player item", item.Info.Id);
				return false;
			}
			item.PlayerInfo.IsCrafted = true;
			item.PlayerInfo.IsCraftedAndNotShown = true;
			if (item == CraftingItem)
			{
				CraftingItem = null;
			}
			Equip(item);
			SavePlayerCurrentData();
			if (LobbyItemsController.OnCraftFinished != null)
			{
				LobbyItemsController.OnCraftFinished(item);
			}
			return true;
		}

		public void Equip(LobbyItem item)
		{
			if (item != null && item.IsExists)
			{
				LobbyItem itemInSlot = GetItemInSlot(item.Slot);
				if (itemInSlot != null)
				{
					itemInSlot.PlayerInfo.IsEquiped = false;
				}
				item.PlayerInfo.IsEquiped = true;
				item.PlayerInfo.EquipTime = FriendsController.ServerTime;
				SavePlayerCurrentData();
				if (LobbyItemsController.OnEquipChanged != null)
				{
					LobbyItemsController.OnEquipChanged(item.Slot, itemInSlot, item);
				}
			}
		}

		public void UnEquip(LobbyItemInfo.LobbyItemSlot slot)
		{
			LobbyItem itemInSlot = GetItemInSlot(slot);
			if (itemInSlot == null)
			{
				return;
			}
			if (!SlotCanBeEmpty(slot))
			{
				if (!IsDefaultItem(itemInSlot))
				{
					LobbyItem defaultItemForSlot = GetDefaultItemForSlot(slot);
					if (defaultItemForSlot != null)
					{
						defaultItemForSlot.PlayerInfo.IsEquiped = true;
						defaultItemForSlot.PlayerInfo.EquipTime = FriendsController.ServerTime;
					}
					itemInSlot.PlayerInfo.IsEquiped = false;
					SavePlayerCurrentData();
					if (LobbyItemsController.OnEquipChanged != null)
					{
						LobbyItemsController.OnEquipChanged(slot, itemInSlot, defaultItemForSlot);
					}
				}
			}
			else
			{
				itemInSlot.PlayerInfo.IsEquiped = false;
				itemInSlot.PlayerInfo.EquipTime = FriendsController.ServerTime;
				SavePlayerCurrentData();
				if (LobbyItemsController.OnEquipChanged != null)
				{
					LobbyItemsController.OnEquipChanged(slot, itemInSlot, null);
				}
			}
		}

		private void OnInstanceCreated()
		{
			InitItems();
			InitializeEffects();
		}

		private void Update()
		{
			if (CurrentTime.HasValue && CraftingItem != null && CraftingItem.CraftTimeLeft <= 0)
			{
				SpeedUpNow(CraftingItem);
			}
		}

		private void OnApplicationPause(bool paused)
		{
			if (paused)
			{
				SavePlayerCurrentData();
			}
		}

		private void OnDestroy()
		{
		}

		private void InitItems()
		{
			_allItems = new List<LobbyItem>();
			List<LobbyItemPlayerInfo> source = ReadPlayerData();
			foreach (LobbyItemInfo lobbyItemInfo in LobbyItemsInfo.info.Values)
			{
				LobbyItemPlayerInfo lobbyItemPlayerInfo = source.FirstOrDefault((LobbyItemPlayerInfo i) => i.InfoId == lobbyItemInfo.Id);
				if (lobbyItemPlayerInfo == null)
				{
					lobbyItemPlayerInfo = new LobbyItemPlayerInfo
					{
						InfoId = lobbyItemInfo.Id
					};
					lobbyItemPlayerInfo.IsNew = true;
				}
				LobbyItem item = new LobbyItem(lobbyItemInfo, lobbyItemPlayerInfo);
				_allItems.Add(item);
			}
			CraftingItem = _allItems.FirstOrDefault((LobbyItem i) => i.IsCrafting);
			foreach (KeyValuePair<LobbyItemInfo.LobbyItemSlot, string> kv in _defaultPlayerItems)
			{
				if (!_allItems.Any((LobbyItem i) => i.Info.Id == kv.Value))
				{
					Debug.LogErrorFormat("lobby item with id '{0}' not found", kv.Value);
				}
				else if (!source.Any((LobbyItemPlayerInfo pi) => pi.InfoId == kv.Value))
				{
					LobbyItemPlayerInfo playerInfo = new LobbyItemPlayerInfo
					{
						InfoId = kv.Value,
						IsCrafted = true,
						IsCraftedAndNotShown = false,
						IsNew = false
					};
					LobbyItem lobbyItem = _allItems.FirstOrDefault((LobbyItem i) => i.Info.Id == playerInfo.InfoId);
					lobbyItem.PlayerInfo = playerInfo;
					if (GetItemInSlot(lobbyItem.Slot) == null)
					{
						lobbyItem.PlayerInfo.IsEquiped = true;
						lobbyItem.PlayerInfo.EquipTime = 0L;
					}
				}
			}
			IsReady = true;
		}

		public static List<LobbyItemPlayerInfo> ReadPlayerData()
		{
			string @string = Storager.getString("lobby_items");
			if (@string.IsNullOrEmpty())
			{
				return new List<LobbyItemPlayerInfo>();
			}
			LobbyItemPlayerInfoSerializedObject lobbyItemPlayerInfoSerializedObject = JsonUtility.FromJson<LobbyItemPlayerInfoSerializedObject>(@string);
			if (lobbyItemPlayerInfoSerializedObject == null)
			{
				return new List<LobbyItemPlayerInfo>();
			}
			return lobbyItemPlayerInfoSerializedObject.Infos;
		}

		public static void SaveLobbyItemsPlayerData(LobbyItemPlayerInfoSerializedObject obj)
		{
			string val = JsonUtility.ToJson(obj);
			Storager.setString("lobby_items", val);
		}

		private void SavePlayerCurrentData()
		{
			if (AllItems != null)
			{
				List<LobbyItemPlayerInfo> infos = (from i in AllItems
					where i != null && i.PlayerInfo != null
					select i.PlayerInfo).ToList();
				SaveLobbyItemsPlayerData(new LobbyItemPlayerInfoSerializedObject
				{
					Infos = infos
				});
			}
		}

		public void CleanPlayerData()
		{
			SaveLobbyItemsPlayerData(new LobbyItemPlayerInfoSerializedObject
			{
				Infos = new List<LobbyItemPlayerInfo>()
			});
			InitItems();
			if (this.OnAnyPlayerDataUpdated != null)
			{
				this.OnAnyPlayerDataUpdated();
			}
		}

		public void ReReadPlayerData()
		{
			string craftingItemId = ((CraftingItem != null) ? CraftingItem.Info.Id : string.Empty);
			CraftingItem = null;
			foreach (LobbyItemPlayerInfo playerData in ReadPlayerData())
			{
				if (playerData == null || playerData.InfoId.IsNullOrEmpty())
				{
					Debug.LogError("UpdatePlayerData() playerData is null or InfoId is empty");
					continue;
				}
				LobbyItem lobbyItem = _allItems.FirstOrDefault((LobbyItem i) => i.Info.Id == playerData.InfoId);
				if (lobbyItem == null)
				{
					Debug.LogErrorFormat("UpdatePlayerData() item with Id '{0}' not found", playerData.InfoId);
				}
				else if (!lobbyItem.PlayerInfo.Equals(playerData))
				{
					lobbyItem.SetPlayerDataValues(playerData);
				}
			}
			RiliExtensions.ForEachEnum(delegate(LobbyItemInfo.LobbyItemSlot s)
			{
				if (AllItems.Count((LobbyItem i) => i.Slot == s && i.PlayerInfo.IsEquiped) > 1)
				{
					Debug.LogErrorFormat("myltipy equipped items in slot '{0}'", s);
					List<LobbyItem> list = (from i in AllItems
						where i.Slot == s && i.PlayerInfo.IsEquiped
						orderby i.Info.Sort descending
						select i).ToList();
					for (int j = 1; j < list.Count; j++)
					{
						list[j].PlayerInfo.IsEquiped = false;
					}
				}
			});
			if (this.OnAnyPlayerDataUpdated != null)
			{
				this.OnAnyPlayerDataUpdated();
			}
			if (craftingItemId.IsNullOrEmpty())
			{
				return;
			}
			LobbyItem lobbyItem2 = AllItems.FirstOrDefault((LobbyItem i) => i.Info.Id == craftingItemId);
			if (lobbyItem2 == null)
			{
				return;
			}
			if (lobbyItem2.IsExists)
			{
				if (LobbyItemsController.OnCraftFinished != null)
				{
					LobbyItemsController.OnCraftFinished(lobbyItem2);
				}
			}
			else
			{
				CraftingItem = lobbyItem2;
			}
		}

		public static float GetEffect(LobbyItemInfo.LobbyItemBuffType effectType)
		{
			if (!Singleton<LobbyItemsController>.Instance._effectsDict.ContainsKey(effectType))
			{
				return 0f;
			}
			return Singleton<LobbyItemsController>.Instance._effectsDict[effectType];
		}

		private void InitializeEffects()
		{
			_effectsDict = new Dictionary<LobbyItemInfo.LobbyItemBuffType, float>();
			if (_effectHandlers != null)
			{
				foreach (KeyValuePair<LobbyItem, List<LobbyItemEffectHandler>> effectHandler in _effectHandlers)
				{
					foreach (LobbyItemEffectHandler item in effectHandler.Value)
					{
						UnityEngine.Object.Destroy(item);
					}
				}
			}
			_effectHandlers = new Dictionary<LobbyItem, List<LobbyItemEffectHandler>>();
			foreach (LobbyItem allPlayerEquipedItem in Singleton<LobbyItemsController>.Instance.AllPlayerEquipedItems)
			{
				if (allPlayerEquipedItem.Info.Buffs != null && allPlayerEquipedItem.Info.Buffs.Any())
				{
					AddEffects(allPlayerEquipedItem);
				}
			}
			OnEquipChanged -= LobbyItemsController_OnEquipChanged;
			OnEquipChanged += LobbyItemsController_OnEquipChanged;
			Singleton<LobbyItemsController>.Instance.OnAnyPlayerDataUpdated -= InitializeEffects;
			Singleton<LobbyItemsController>.Instance.OnAnyPlayerDataUpdated += InitializeEffects;
		}

		private void LobbyItemsController_OnEquipChanged(LobbyItemInfo.LobbyItemSlot slot, LobbyItem oldItem, LobbyItem newItem)
		{
			RemoveEffects(oldItem);
			AddEffects(newItem);
		}

		private void AddEffects(LobbyItem lobbyItem)
		{
			if (lobbyItem == null || lobbyItem.Info.Buffs == null)
			{
				return;
			}
			if (!_effectHandlers.ContainsKey(lobbyItem))
			{
				_effectHandlers.Add(lobbyItem, CreateEffectHandlersForLobbyItem(lobbyItem));
			}
			LobbyItemInfo.LobbyItemBuff[] buffs = lobbyItem.Info.Buffs;
			foreach (LobbyItemInfo.LobbyItemBuff lobbyItemBuff in buffs)
			{
				if (!_effectsDict.ContainsKey(lobbyItemBuff.BuffType))
				{
					_effectsDict.Add(lobbyItemBuff.BuffType, lobbyItemBuff.Value);
				}
				else
				{
					_effectsDict[lobbyItemBuff.BuffType] += lobbyItemBuff.Value;
				}
				if (LobbyItemsController.OnEffectChanged != null)
				{
					LobbyItemsController.OnEffectChanged(lobbyItemBuff.BuffType, _effectsDict[lobbyItemBuff.BuffType]);
				}
			}
		}

		private void RemoveEffects(LobbyItem lobbyItem)
		{
			if (lobbyItem == null || lobbyItem.Info.Buffs == null)
			{
				return;
			}
			if (_effectHandlers.ContainsKey(lobbyItem))
			{
				foreach (LobbyItemEffectHandler item in _effectHandlers[lobbyItem])
				{
					item.Remove();
				}
				_effectHandlers.Remove(lobbyItem);
			}
			LobbyItemInfo.LobbyItemBuff[] buffs = lobbyItem.Info.Buffs;
			foreach (LobbyItemInfo.LobbyItemBuff lobbyItemBuff in buffs)
			{
				if (_effectsDict.ContainsKey(lobbyItemBuff.BuffType))
				{
					_effectsDict[lobbyItemBuff.BuffType] -= lobbyItemBuff.Value;
					if (LobbyItemsController.OnEffectChanged != null)
					{
						LobbyItemsController.OnEffectChanged(lobbyItemBuff.BuffType, _effectsDict[lobbyItemBuff.BuffType]);
					}
				}
			}
		}

		private List<LobbyItemEffectHandler> CreateEffectHandlersForLobbyItem(LobbyItem lobbyItem)
		{
			List<LobbyItemEffectHandler> list = new List<LobbyItemEffectHandler>();
			if (lobbyItem == null || lobbyItem.Info.Buffs == null)
			{
				return list;
			}
			LobbyItemInfo.LobbyItemBuff[] buffs = lobbyItem.Info.Buffs;
			foreach (LobbyItemInfo.LobbyItemBuff lobbyItemBuff in buffs)
			{
				LobbyItemEffectHandler lobbyItemEffectHandler = null;
				if (lobbyItemBuff.BuffType == LobbyItemInfo.LobbyItemBuffType.FreeGemsPerDay)
				{
					lobbyItemEffectHandler = base.gameObject.AddComponent<LobbyItemEffectFreeGems>();
				}
				else if (lobbyItemBuff.BuffType == LobbyItemInfo.LobbyItemBuffType.FreeSpinsPerDay)
				{
					lobbyItemEffectHandler = base.gameObject.AddComponent<LobbyItemEffectFreeSpins>();
				}
				if (lobbyItemEffectHandler != null)
				{
					lobbyItemEffectHandler.Inicialize(lobbyItem);
					list.Add(lobbyItemEffectHandler);
				}
			}
			return list;
		}
	}
}
