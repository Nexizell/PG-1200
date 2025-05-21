using System;
using System.Collections.Generic;
using System.Linq;
using Rilisoft.NullExtensions;
using Unity.Linq;
using UnityEngine;

namespace Rilisoft
{
	public class LobbyCraftController : MonoBehaviour
	{
		[Serializable]
		public class LobbyItemsGroupSettings 
		{
			public LobbyItemGroupType Group;

			public string LocalizationKey;

			public AudioClip Clip;
		}

		public const string PLACE_POINTS_ROOT_NAME = "[LobbyItemsPlacePoints]";

		[SerializeField]
		protected internal GameObject _mainPanel;

		[SerializeField]
		protected internal GameObject _screenPanel;

		[SerializeField]
		protected internal GameObject _cameraSettingsPanel;

		[SerializeField]
		protected internal GameObject _categoriesContainer;

		[SerializeField]
		protected internal GameObject _categoryRootObj;

		[SerializeField]
		protected internal GameObject _categoryDecorationsObj;

		[SerializeField]
		protected internal GameObject _categoryBuildingsObj;

		[SerializeField]
		protected internal GameObject _categoryBackgroundsObj;

		[SerializeField]
		protected internal GameObject _itemsScrollContainer;

		[SerializeField]
		protected internal UIScrollView _itemsScroll;

		[SerializeField]
		protected internal CraftScrollItemView _scrollDefaultItem;

		[SerializeField]
		protected internal LobbyItemSpeedUpWindow _speedUpWindow;

		[SerializeField]
		protected internal PrefabHandler _buildedWindowPrefabHandler;

		[SerializeField]
		protected internal GameObject _connectionErrorBanner;

		[SerializeField]
		protected internal TextGroup _headerText;

		[SerializeField]
		protected internal GameObject _tutorialHintsContainer;

		[SerializeField]
		protected internal GameObject _likeGuiObj;

		[SerializeField]
		protected internal GameObject _likeButtonObj;

		[Header("-= sounds =-")]
		[SerializeField]
		protected internal AudioClip _speedUpAudio;

		[SerializeField]
		protected internal AudioClip _craftBeganAudio;

		[SerializeField]
		protected internal AudioClip _cameraZoomInAudio;

		[SerializeField]
		protected internal AudioClip _cameraZoomOutAudio;

		[SerializeField]
		protected internal List<LobbyItemsGroupSettings> _groupsSettings;

		private UIScrollManager _itemsScrollManager;

		private UIGrid _itemsScrollItemsGrid;

		public static LobbyCraftController Instance;

		private LobbyItemGroupType _currentScrollGroup;

		private LobbyItemGroupType _currentGroupValue;

		private readonly List<ILobbyItemView> _scrollViews = new List<ILobbyItemView>();

		private LazyObject<CraftBuildingReadyWindow> _buildedWindow;

		private bool _interfaceEnabled;

		private bool _lobbyChanged;

		private bool _enemyLobbyViewed;

		private readonly Dictionary<LobbyItemInfo.LobbyItemSlot, Transform> _placePoints = new Dictionary<LobbyItemInfo.LobbyItemSlot, Transform>();

		private readonly Dictionary<LobbyItemInfo.LobbyItemSlot, ILobbyItemView> _sceneViews = new Dictionary<LobbyItemInfo.LobbyItemSlot, ILobbyItemView>();

		private readonly List<ILobbyItemView> _scrollViewsPool = new List<ILobbyItemView>();

		private LobbyItem _selectedItem;

		private bool _moveScroll;

		private bool _updateScroll;

		private bool _reloadScrollItems;

		private LobbyItem _rememberedSelectedItem;

		private Queue<LobbyItem> _craftedItemsShowQueue = new Queue<LobbyItem>();

		private bool _selfVisibleBeforeZoomIn;

		private bool _mainMenuVisibleBeforeZoomIn;

		private LobbyItem _craftedAndShowingItem;

		private int _tutorialStageValue;

		private LobbyItemGroupType _currentGroup
		{
			get
			{
				return _currentGroupValue;
			}
			set
			{
				_currentGroupValue = value;
				if (_headerText != null)
				{
					_headerText.Text = GetLocalizedNameForGroup(_currentGroupValue);
				}
			}
		}

		private CraftBuildingReadyWindow BuildedWindow
		{
			get
			{
				if (_buildedWindow == null)
				{
					_buildedWindow = new LazyObject<CraftBuildingReadyWindow>(_buildedWindowPrefabHandler.Prefab, base.gameObject);
					_buildedWindow.Value.OnWindowClose += OnCraftBuildingWindowClose;
				}
				return _buildedWindow.Value;
			}
		}

		public bool InterfaceEnabled
		{
			get
			{
				return _interfaceEnabled;
			}
			set
			{
				if (_interfaceEnabled == value)
				{
					return;
				}
				_interfaceEnabled = value;
				_mainPanel.SetActive(_interfaceEnabled);
				_categoriesContainer.SetActive(_interfaceEnabled);
				_itemsScrollContainer.SetActive(false);
				if (value)
				{
					_screenPanel.SetActive(false);
				}
				if (MainMenuController.sharedController != null)
				{
					MainMenuController.sharedController.mainPanel.SetActiveSafe(!_interfaceEnabled);
				}
				if (_interfaceEnabled)
				{
					_lobbyChanged = false;
					if (MainMenuController.sharedController != null)
					{
						MainMenuController.sharedController.BackPressed += MainMenuBackHandler;
					}
					if (_headerText != null)
					{
						_headerText.Text = GetLocalizedNameForGroup(_currentGroupValue);
					}
					LobbyItem playerCraftedAndNotShownItem = Singleton<LobbyItemsController>.Instance.PlayerCraftedAndNotShownItem;
					if (playerCraftedAndNotShownItem != null)
					{
						ShowCraftedItem(playerCraftedAndNotShownItem);
					}
					if (!TutorialCompleted)
					{
						TutorialStage = 0;
					}
				}
				else
				{
					if (MainMenuController.sharedController != null)
					{
						MainMenuController.sharedController.BackPressed -= MainMenuBackHandler;
					}
					if (!TutorialCompleted)
					{
						TutorialStage = -1;
						HideAllTutorialHints();
					}
					if (_lobbyChanged)
					{
						SetMySceneViews(false, false);
						BackgroundsManager.Instance.MakeLobbyScreenshot();
						SetMySceneViews(true, false);
						if (FriendsController.sharedController != null)
						{
							FriendsController.sharedController.SendOurData();
						}
					}
				}
				if (LobbyCraftController.OnInterfaceEnabledChanged != null)
				{
					LobbyCraftController.OnInterfaceEnabledChanged();
				}
			}
		}

		public bool IsInitialized { get; protected set; }

		private int TutorialStage
		{
			get
			{
				return _tutorialStageValue;
			}
			set
			{
				HideAllTutorialHints();
				ShowTutorialHintForStage(value);
				_tutorialStageValue = value;
			}
		}

		private bool TutorialCompleted
		{
			get
			{
				return LobbyItemsController.TutorialCompleted;
			}
		}

		public static event Action OnInterfaceEnabledChanged;

		private void MainMenuBackHandler(object sender, EventArgs e)
		{
			NavigateBack();
		}

		private void Awake()
		{
			Init();
			BankController.CloseBank -= OnCloseBank;
			BankController.CloseBank += OnCloseBank;
		}

		public void Init()
		{
			if (!IsInitialized)
			{
				IsInitialized = true;
				_scrollViewsPool.Add(_scrollDefaultItem);
				for (int i = 0; i < 10; i++)
				{
					ILobbyItemView component = UnityEngine.Object.Instantiate(_scrollDefaultItem.gameObject).GetComponent<ILobbyItemView>();
					component.Setup(null, this, null);
					component.Hide();
					_scrollViewsPool.Add(component);
				}
				Instance = this;
				_mainPanel.SetActive(false);
				_screenPanel.SetActive(false);
				_scrollDefaultItem.gameObject.SetActive(false);
				_itemsScrollItemsGrid = _itemsScroll.gameObject.GetComponentInChildren<UIGrid>();
				_itemsScrollManager = _itemsScroll.gameObject.GetComponent<UIScrollManager>();
				LobbyItemsController.OnCraftStarted -= UpdateViewsByItem;
				LobbyItemsController.OnCraftStarted += UpdateViewsByItem;
				LobbyItemsController.OnCraftFinished -= OnCraftFinished;
				LobbyItemsController.OnCraftFinished += OnCraftFinished;
				LobbyItemsController.OnItemAdded -= UpdateViewsByItem;
				LobbyItemsController.OnItemAdded += UpdateViewsByItem;
				LobbyItemsController.OnEquipChanged -= OnEquipChanged;
				LobbyItemsController.OnEquipChanged += OnEquipChanged;
				_speedUpWindow.gameObject.SetActive(false);
				if (Singleton<LobbyItemsController>.Instance.IsReady)
				{
					SetMySceneViews(true, false);
				}
				else
				{
					LobbyItemsController.OnReadyChanged += LobbyItemsControllerOnReadyChanged;
				}
				Singleton<LobbyItemsController>.Instance.OnAnyPlayerDataUpdated += ReloadItems;
			}
		}

		private void Start()
		{
			_itemsScroll.GetComponent<UIPanel>().UpdateAnchors();
		}

		private void LateUpdate()
		{
			if (_reloadScrollItems)
			{
				_reloadScrollItems = false;
				PopulateItemsScroll();
			}
			else if (_updateScroll)
			{
				_updateScroll = false;
				UpdateItemsScroll();
			}
			else if (_moveScroll)
			{
				_moveScroll = false;
				if (_itemsScroll.gameObject.activeInHierarchy)
				{
					ScrollItemsTo(_selectedItem);
				}
			}
		}

		private void OnApplicationPause(bool pause)
		{
			if (pause && _craftedAndShowingItem != null)
			{
				GetCraftReward(_craftedAndShowingItem);
			}
		}

		private void OnDestroy()
		{
			LobbyItemsController.OnCraftStarted -= UpdateViewsByItem;
			LobbyItemsController.OnCraftFinished -= UpdateViewsByItem;
			LobbyItemsController.OnCraftFinished -= OnCraftFinished;
			LobbyItemsController.OnItemAdded -= UpdateViewsByItem;
			LobbyItemsController.OnEquipChanged -= OnEquipChanged;
			LobbyItemsController.OnReadyChanged -= LobbyItemsControllerOnReadyChanged;
			Singleton<LobbyItemsController>.Instance.OnAnyPlayerDataUpdated -= ReloadItems;
			if (_craftedAndShowingItem != null)
			{
				GetCraftReward(_craftedAndShowingItem);
			}
		}

		public string GetLocalizedNameForGroup(LobbyItemGroupType groupType)
		{
			LobbyItemsGroupSettings lobbyItemsGroupSettings = _groupsSettings.FirstOrDefault((LobbyItemsGroupSettings gs) => gs.Group == groupType);
			if (lobbyItemsGroupSettings == null)
			{
				return string.Empty;
			}
			return LocalizationStore.Get(lobbyItemsGroupSettings.LocalizationKey);
		}

		public AudioClip GetSoundForGroup(LobbyItemGroupType groupType)
		{
			LobbyItemsGroupSettings lobbyItemsGroupSettings = _groupsSettings.FirstOrDefault((LobbyItemsGroupSettings gs) => gs.Group == groupType);
			if (lobbyItemsGroupSettings == null)
			{
				return null;
			}
			return lobbyItemsGroupSettings.Clip;
		}

		private void LobbyItemsControllerOnReadyChanged(bool b)
		{
			if (b)
			{
				SetMySceneViews(true, false);
			}
		}

		private void ReloadItems()
		{
			if (_speedUpWindow.isActiveAndEnabled)
			{
				_speedUpWindow.Hide();
			}
			if (_currentScrollGroup != 0)
			{
				_reloadScrollItems = true;
			}
			SetMySceneViews(true, false);
		}

		private Transform GetPlacePointForSlot(LobbyItemInfo.LobbyItemSlot slot)
		{
			if (!_placePoints.Any())
			{
				foreach (GameObject item in GameObject.Find("[LobbyItemsPlacePoints]").Children())
				{
					LobbyItemInfo.LobbyItemSlot? lobbyItemSlot = item.name.ToEnum<LobbyItemInfo.LobbyItemSlot>();
					if (lobbyItemSlot.HasValue)
					{
						if (!_placePoints.ContainsKey(lobbyItemSlot.Value))
						{
							_placePoints.Add(lobbyItemSlot.Value, item.transform);
							continue;
						}
						Debug.LogErrorFormat("duplicate lobby item place point: '{0}'", item.name);
					}
					else
					{
						Debug.LogErrorFormat("unknown lobby item place point name: '{0}'", item.name);
					}
				}
			}
			if (!_placePoints.ContainsKey(slot))
			{
				return null;
			}
			return _placePoints[slot];
		}

		private void PopulateItemsScroll()
		{
			_itemsScroll.GetComponent<UIPanel>().UpdateAnchors();
			CleanupScroll();
			_itemsScrollItemsGrid.gameObject.SetChildSiblingIndexesByPositions(false, true, true);
			foreach (LobbyItem item in from i in Singleton<LobbyItemsController>.Instance.ItemsPerGroup(_currentScrollGroup)
				orderby i.Info.Sort
				select i)
			{
				ILobbyItemView lobbyItemView = null;
				if (_scrollViewsPool.Any())
				{
					lobbyItemView = _scrollViewsPool.First();
					_scrollViewsPool.Remove(lobbyItemView);
				}
				else
				{
					lobbyItemView = UnityEngine.Object.Instantiate(_scrollDefaultItem.gameObject).GetComponent<ILobbyItemView>();
				}
				lobbyItemView.Setup(_itemsScrollItemsGrid.transform, this, item);
				if (_selectedItem != null && _selectedItem == item)
				{
					lobbyItemView.IsSelected = true;
				}
				lobbyItemView.UpdateView();
				_scrollViews.Add(lobbyItemView);
			}
			_itemsScrollItemsGrid.Reposition();
			_itemsScroll.ResetPosition();
			_itemsScrollItemsGrid.Reposition();
			bool flag = (float)_scrollViews.Count() * _itemsScrollItemsGrid.cellWidth > _itemsScroll.panel.width;
			if (!flag)
			{
				_itemsScroll.ResetPosition();
			}
			_itemsScroll.enabled = flag && TutorialCompleted;
		}

		private void UpdateItemsScroll()
		{
			if (_currentScrollGroup == LobbyItemGroupType.None)
			{
				return;
			}
			if (_selectedItem != null && !LobbyItemsController.SlotInGroup(_selectedItem.Slot, _currentScrollGroup))
			{
				GoToGroup(LobbyItemsController.GetGroupForSlot(_selectedItem.Slot));
			}
			_categoriesContainer.SetActiveSafe(false);
			_itemsScrollContainer.SetActiveSafe(true);
			ILobbyItemView lobbyItemView = _scrollViews.FirstOrDefault((ILobbyItemView v) => v.LobbyItem == _selectedItem);
			foreach (ILobbyItemView scrollView in _scrollViews)
			{
				if (scrollView != lobbyItemView)
				{
					scrollView.IsSelected = false;
					scrollView.UpdateView();
				}
			}
			lobbyItemView.Do(delegate(ILobbyItemView v)
			{
				v.IsSelected = true;
				v.UpdateView();
			});
			bool flag = (float)_scrollViews.Count() * _itemsScrollItemsGrid.cellWidth > _itemsScroll.panel.width;
			if (!flag)
			{
				_itemsScroll.ResetPosition();
			}
			_itemsScroll.enabled = flag && TutorialCompleted;
		}

		private void CleanupScroll()
		{
			foreach (ILobbyItemView scrollView in _scrollViews)
			{
				scrollView.Hide();
				_scrollViewsPool.Add(scrollView);
			}
			_scrollViews.Clear();
		}

		public void SelectItemOnScrollRequest(LobbyItem itemToSelect)
		{
			if (_enemyLobbyViewed || _likeGuiObj.activeInHierarchy)
			{
				return;
			}
			if (itemToSelect != null && itemToSelect == _selectedItem)
			{
				itemToSelect = null;
			}
			if (!TutorialCompleted)
			{
				if (itemToSelect != null && itemToSelect.Info.Id == "decor_big_military_1")
				{
					SelectItem(itemToSelect, false);
				}
			}
			else
			{
				SelectItem(itemToSelect, false);
			}
		}

		public void SelectItemOnSceneRequest(LobbyItem itemToSelect)
		{
			if (TutorialCompleted)
			{
				SelectItem(itemToSelect, true);
			}
		}

		private void SelectItem(LobbyItem itemToSelect, bool moveScroll)
		{
			if (!InterfaceEnabled || _craftedAndShowingItem != null || _screenPanel.activeInHierarchy || _cameraSettingsPanel.activeInHierarchy || _likeGuiObj.activeInHierarchy)
			{
				return;
			}
			_selectedItem = itemToSelect;
			_moveScroll = moveScroll;
			if (ButtonClickSound.Instance != null && Defs.isSoundFX)
			{
				ButtonClickSound.Instance.PlayClick();
			}
			if (_itemsScroll.gameObject.activeInHierarchy)
			{
				if (_selectedItem != null)
				{
					UpdateViewsByItem(_selectedItem);
					_selectedItem = itemToSelect;
					UpdateViewsByItem(_selectedItem);
				}
				_updateScroll = true;
				_selectedItem = itemToSelect;
				_moveScroll = moveScroll;
			}
			else
			{
				_moveScroll = true;
				GoToGroup(LobbyItemsController.GetGroupForSlot(itemToSelect.Slot));
			}
			SetMySceneViews(true);
		}

		public void SetMySceneViews(bool showCraftebleItems, bool useAsync = true)
		{
			_enemyLobbyViewed = false;
			List<LobbyItem> allPlayerEquipedItems = Singleton<LobbyItemsController>.Instance.AllPlayerEquipedItems;
			UpdateSceneViews(allPlayerEquipedItems, showCraftebleItems, useAsync);
		}

		public void SetSceneViews(IEnumerable<string> itemsIds, bool useAsync = true)
		{
			List<LobbyItem> list = new List<LobbyItem>();
			foreach (string itemId in itemsIds)
			{
				if (!itemId.IsNullOrEmpty())
				{
					LobbyItem lobbyItem = Singleton<LobbyItemsController>.Instance.AllItems.FirstOrDefault((LobbyItem i) => i.Info.Id == itemId);
					if (lobbyItem != null)
					{
						list.Add(lobbyItem);
						continue;
					}
					Debug.LogError(string.Format("item wit id '{0}' not found", new object[1] { itemId }));
				}
			}
			SetSceneViews(list, useAsync);
		}

		public void SetSceneViews(IEnumerable<LobbyItem> items, bool useAsync = true)
		{
			_enemyLobbyViewed = true;
			UpdateSceneViews(items, false, useAsync);
		}

		private void UpdateSceneViews(IEnumerable<LobbyItem> itemsToView, bool showCraftebleItems, bool useAsync = true)
		{
			Dictionary<LobbyItemInfo.LobbyItemSlot, LobbyItem> dictionary = new Dictionary<LobbyItemInfo.LobbyItemSlot, LobbyItem>();
			foreach (LobbyItem item in itemsToView)
			{
				dictionary.Add(item.Slot, item);
			}
			if (_selectedItem != null)
			{
				if (_selectedItem.Info.Level > ExpController.Instance.Rank)
				{
					LobbyItem itemInSlot = Singleton<LobbyItemsController>.Instance.GetItemInSlot(_selectedItem.Slot);
					if (itemInSlot != null)
					{
						if (dictionary.ContainsKey(itemInSlot.Slot))
						{
							if (dictionary[itemInSlot.Slot] != itemInSlot)
							{
								dictionary[itemInSlot.Slot] = itemInSlot;
							}
						}
						else
						{
							dictionary.Add(itemInSlot.Slot, itemInSlot);
						}
					}
				}
				else if (dictionary.ContainsKey(_selectedItem.Slot))
				{
					if (dictionary[_selectedItem.Slot] != _selectedItem)
					{
						dictionary[_selectedItem.Slot] = _selectedItem;
					}
				}
				else
				{
					dictionary.Add(_selectedItem.Slot, _selectedItem);
				}
			}
			if (showCraftebleItems)
			{
				LobbyItem craftingItem = Singleton<LobbyItemsController>.Instance.CraftingItem;
				if (craftingItem != null && craftingItem.Slot != LobbyItemInfo.LobbyItemSlot.terrain && craftingItem.Slot != LobbyItemInfo.LobbyItemSlot.skybox)
				{
					if (dictionary.ContainsKey(craftingItem.Slot))
					{
						LobbyItem lobbyItem = dictionary[craftingItem.Slot];
						if (_selectedItem == null || _selectedItem != lobbyItem)
						{
							dictionary[craftingItem.Slot] = craftingItem;
						}
					}
					else
					{
						dictionary.Add(craftingItem.Slot, craftingItem);
					}
				}
			}
			foreach (KeyValuePair<LobbyItemInfo.LobbyItemSlot, LobbyItem> item2 in dictionary)
			{
				bool isSelected = _selectedItem != null && item2.Value == _selectedItem;
				if (_sceneViews.ContainsKey(item2.Key))
				{
					if (_sceneViews[item2.Key].LobbyItem != item2.Value)
					{
						_sceneViews[item2.Key].Kill();
						_sceneViews.Remove(item2.Key);
						CreateSceneView(item2.Value, isSelected, useAsync);
					}
				}
				else
				{
					CreateSceneView(item2.Value, isSelected, useAsync);
				}
			}
			List<LobbyItemInfo.LobbyItemSlot> list = new List<LobbyItemInfo.LobbyItemSlot>();
			foreach (KeyValuePair<LobbyItemInfo.LobbyItemSlot, ILobbyItemView> sceneView in _sceneViews)
			{
				if (!dictionary.ContainsKey(sceneView.Key))
				{
					list.Add(sceneView.Key);
				}
			}
			foreach (LobbyItemInfo.LobbyItemSlot item3 in list)
			{
				_sceneViews[item3].Kill();
				_sceneViews.Remove(item3);
			}
		}

		private void CreateSceneView(LobbyItem lobbyItem, bool isSelected, bool async)
		{
			Action<LobbyItem, GameObject> action = delegate(LobbyItem item, GameObject prefab)
			{
				if (item != null && prefab != null)
				{
					if (_sceneViews.ContainsKey(item.Slot))
					{
						_sceneViews[item.Slot].Kill();
						_sceneViews.Remove(item.Slot);
					}
					ILobbyItemView component = UnityEngine.Object.Instantiate(prefab).GetComponent<ILobbyItemView>();
					Transform placePointForSlot = GetPlacePointForSlot(lobbyItem.Slot);
					component.Setup(placePointForSlot, this, lobbyItem);
					component.IsSelected = isSelected;
					component.UpdateView();
					_sceneViews.Add(item.Slot, component);
				}
			};
			if (async)
			{
				GetPlacePointForSlot(lobbyItem.Slot).gameObject.GetOrAddComponent<SceneViewPrefabLoadOperation>().Setup(lobbyItem, (LobbyItem item) => _sceneViews.ContainsKey(item.Slot) && _sceneViews[item.Slot] != null, action);
			}
			else
			{
				GameObject arg = Resources.Load<GameObject>(lobbyItem.PrefabPath);
				action(lobbyItem, arg);
			}
		}

		public void GoToLobbyItem(LobbyItem lobbyItem)
		{
			LobbyItemGroupType groupForSlot = LobbyItemsController.GetGroupForSlot(lobbyItem.Slot);
			GoToGroup(groupForSlot);
			_updateScroll = true;
			_selectedItem = lobbyItem;
			_moveScroll = true;
		}

		public void GoToGroup(LobbyItemGroupType groupType, bool playGroupSound = true)
		{
			if (!TutorialCompleted)
			{
				switch (groupType)
				{
				case LobbyItemGroupType.None:
					_categoriesContainer.SetActive(true);
					_categoryRootObj.SetActiveSafe(true);
					_categoryBuildingsObj.SetActiveSafe(false);
					_categoryDecorationsObj.SetActiveSafe(false);
					_categoryBackgroundsObj.SetActiveSafe(false);
					TutorialStage = 0;
					break;
				case LobbyItemGroupType.Decorations:
					_categoriesContainer.SetActive(true);
					_categoryRootObj.SetActiveSafe(false);
					_categoryBuildingsObj.SetActiveSafe(false);
					_categoryDecorationsObj.SetActiveSafe(true);
					_categoryBackgroundsObj.SetActiveSafe(false);
					TutorialStage = 1;
					break;
				case LobbyItemGroupType.BigDecor:
					_categoriesContainer.SetActiveSafe(false);
					_itemsScrollContainer.SetActive(true);
					if (_currentScrollGroup == LobbyItemGroupType.None || _currentScrollGroup != groupType)
					{
						CleanupScroll();
						_currentScrollGroup = groupType;
						_reloadScrollItems = true;
					}
					else
					{
						_updateScroll = true;
						_moveScroll = false;
					}
					_selectedItem = Singleton<LobbyItemsController>.Instance.AllItems.FirstOrDefault((LobbyItem i) => i.Info.Id == "decor_big_military_1");
					TutorialStage = 2;
					SetMySceneViews(true, false);
					break;
				default:
					return;
				}
			}
			else
			{
				_currentGroup = groupType;
				switch (groupType)
				{
				case LobbyItemGroupType.None:
					_categoriesContainer.SetActive(true);
					_categoryRootObj.SetActiveSafe(true);
					_categoryBuildingsObj.SetActiveSafe(false);
					_categoryDecorationsObj.SetActiveSafe(false);
					_categoryBackgroundsObj.SetActiveSafe(false);
					break;
				case LobbyItemGroupType.Buildings:
					_categoriesContainer.SetActive(true);
					_categoryRootObj.SetActiveSafe(false);
					_categoryBuildingsObj.SetActiveSafe(true);
					_categoryDecorationsObj.SetActiveSafe(false);
					_categoryBackgroundsObj.SetActiveSafe(false);
					break;
				case LobbyItemGroupType.Decorations:
					_categoriesContainer.SetActive(true);
					_categoryRootObj.SetActiveSafe(false);
					_categoryBuildingsObj.SetActiveSafe(false);
					_categoryDecorationsObj.SetActiveSafe(true);
					_categoryBackgroundsObj.SetActiveSafe(false);
					break;
				case LobbyItemGroupType.Backgrounds:
					_categoriesContainer.SetActive(true);
					_categoryRootObj.SetActiveSafe(false);
					_categoryBuildingsObj.SetActiveSafe(false);
					_categoryDecorationsObj.SetActiveSafe(false);
					_categoryBackgroundsObj.SetActiveSafe(true);
					break;
				default:
					_categoriesContainer.SetActiveSafe(false);
					_itemsScrollContainer.SetActive(true);
					if (_currentScrollGroup == LobbyItemGroupType.None || _currentScrollGroup != groupType)
					{
						CleanupScroll();
						_currentScrollGroup = groupType;
						_reloadScrollItems = true;
					}
					else
					{
						_updateScroll = true;
						_moveScroll = false;
					}
					break;
				}
			}
			if (playGroupSound)
			{
				AudioClip soundForGroup = GetSoundForGroup(_currentGroup);
				if (soundForGroup != null && Defs.isSoundFX)
				{
					NGUITools.PlaySound(soundForGroup);
				}
			}
		}

		private void ScrollItemsTo(LobbyItem lobbyItem)
		{
			for (int i = 0; i < _scrollViews.Count; i++)
			{
				if (_scrollViews[i].LobbyItem == lobbyItem)
				{
					_itemsScrollManager.ScrollTo(i);
					return;
				}
			}
			_itemsScrollManager.ScrollTo(0);
		}

		private void OnEquipChanged(LobbyItemInfo.LobbyItemSlot lobbyItemSlot, LobbyItem oldItem, LobbyItem newItem)
		{
			if (_enemyLobbyViewed || _likeGuiObj.activeInHierarchy)
			{
				return;
			}
			_lobbyChanged = true;
			if (oldItem != null)
			{
				if (oldItem == _selectedItem)
				{
					_selectedItem = null;
				}
				if (_craftedAndShowingItem == null && !_screenPanel.activeInHierarchy)
				{
					UpdateViewsByItem(oldItem);
				}
			}
			if (newItem != null)
			{
				_selectedItem = newItem;
				if (_craftedAndShowingItem == null && !_screenPanel.activeInHierarchy)
				{
					UpdateViewsByItem(newItem);
				}
			}
			if (_craftedAndShowingItem == null && !_screenPanel.activeInHierarchy)
			{
				SetMySceneViews(true);
			}
		}

		private void UpdateViewsByItem(LobbyItem lobbyItem)
		{
			if (!_enemyLobbyViewed && !_likeGuiObj.activeInHierarchy && lobbyItem != null)
			{
				ILobbyItemView lobbyItemView = _scrollViews.FirstOrDefault((ILobbyItemView v) => v.LobbyItem != null && v.LobbyItem == lobbyItem);
				if (lobbyItemView != null)
				{
					lobbyItemView.IsSelected = lobbyItem == _selectedItem;
					lobbyItemView.UpdateView();
				}
				ILobbyItemView lobbyItemView2 = _sceneViews.Values.FirstOrDefault((ILobbyItemView v) => v.LobbyItem == lobbyItem);
				if (lobbyItemView2 != null)
				{
					lobbyItemView2.IsSelected = lobbyItem == _selectedItem;
					lobbyItemView2.UpdateView();
				}
			}
		}

		public void ShowConnectionErrorBanner()
		{
			if (_connectionErrorBanner != null)
			{
				_connectionErrorBanner.SetActive(true);
			}
		}

		public void CraftRequest(CraftScrollItemView view)
		{
			if (view == null)
			{
				return;
			}
			if (!LobbyItemsController.CurrentTime.HasValue)
			{
				ShowConnectionErrorBanner();
			}
			else
			{
				if (view.LobbyItem.IsExists || view.LobbyItem.IsCrafting)
				{
					return;
				}
				if (!TutorialCompleted)
				{
					if (!(view.LobbyItem.Info.Id == "decor_big_military_1"))
					{
						return;
					}
					TutorialStage = 3;
				}
				if (Singleton<LobbyItemsController>.Instance.CraftingItem != null)
				{
					_speedUpWindow.Show(Singleton<LobbyItemsController>.Instance.CraftingItem, view.LobbyItem, SpeedUpWindowCloseCallback);
					return;
				}
				TryBuy(view.LobbyItem.Info.PriceBuy, delegate(bool b)
				{
					if (b && Singleton<LobbyItemsController>.Instance.CraftingItem == null && Singleton<LobbyItemsController>.Instance.StartCraft(view.LobbyItem))
					{
						if (Defs.isSoundFX)
						{
							NGUITools.PlaySound(_craftBeganAudio);
						}
						AnalyticsStuff.LogLobbyItemBuy(view.LobbyItem);
					}
				});
			}
		}

		private void SpeedUpWindowCloseCallback(LobbyItemSpeedUpWindow.CloseResult closeResult, LobbyItem speedUpItem, LobbyItem buyItem)
		{
			if (!LobbyItemsController.CurrentTime.HasValue)
			{
				return;
			}
			switch (closeResult)
			{
			case LobbyItemSpeedUpWindow.CloseResult.SpeedUp:
			{
				if (speedUpItem == null || Singleton<LobbyItemsController>.Instance.CraftingItem != speedUpItem)
				{
					break;
				}
				int speedUpGemsPrice = speedUpItem.PriceSpeedUpLeft.Price;
				TryBuy(speedUpItem.PriceSpeedUpLeft, delegate(bool speedUpBuyResult)
				{
					if (speedUpBuyResult && Singleton<LobbyItemsController>.Instance.SpeedUpNow(speedUpItem))
					{
						if (Defs.isSoundFX)
						{
							NGUITools.PlaySound(_speedUpAudio);
						}
						AnalyticsStuff.LogLobbyItemSpeedUpBuy(speedUpItem, speedUpGemsPrice);
					}
				});
				break;
			}
			case LobbyItemSpeedUpWindow.CloseResult.BuyNow:
				if (buyItem == null)
				{
					break;
				}
				TryBuy(buyItem.Info.PriceInstant, delegate(bool nextItemCraftBuyResult)
				{
					if (nextItemCraftBuyResult && Singleton<LobbyItemsController>.Instance.AddItemNow(buyItem))
					{
						if (Defs.isSoundFX)
						{
							NGUITools.PlaySound(_craftBeganAudio);
						}
						Singleton<LobbyItemsController>.Instance.Equip(buyItem);
						buyItem.PlayerInfo.IsCraftedAndNotShown = true;
						ShowCraftedItem(buyItem);
						AnalyticsStuff.LogLobbyItemBuy(buyItem);
					}
				});
				break;
			}
		}

		public void SpeedUpRequest(CraftScrollItemView view)
		{
			SpeedUpCraftingItem();
		}

		public void SpeedUpCraftingItem()
		{
			if (!LobbyItemsController.CurrentTime.HasValue)
			{
				ShowConnectionErrorBanner();
			}
			else
			{
				if (Singleton<LobbyItemsController>.Instance.CraftingItem == null)
				{
					return;
				}
				LobbyItem lobbyItem = Singleton<LobbyItemsController>.Instance.CraftingItem;
				if (!LobbyItemsController.CurrentTime.HasValue || Singleton<LobbyItemsController>.Instance.CraftingItem != lobbyItem)
				{
					return;
				}
				int gemsPrice = lobbyItem.PriceSpeedUpLeft.Price;
				TryBuy(lobbyItem.PriceSpeedUpLeft, delegate(bool b)
				{
					if (b && Singleton<LobbyItemsController>.Instance.SpeedUpNow(lobbyItem))
					{
						if (Defs.isSoundFX)
						{
							NGUITools.PlaySound(_speedUpAudio);
						}
						AnalyticsStuff.LogLobbyItemSpeedUpBuy(lobbyItem, gemsPrice);
					}
				});
			}
		}

		public void PlaceRequest(CraftScrollItemView view)
		{
			Singleton<LobbyItemsController>.Instance.Equip(view.LobbyItem);
		}

		public void RemoveRequest(CraftScrollItemView view)
		{
			if (view == null)
			{
				return;
			}
			LobbyItem lobbyItem = view.LobbyItem;
			if (!LobbyItemsController.SlotCanBeEmpty(lobbyItem.Slot))
			{
				if (!lobbyItem.IsDefaultItem)
				{
					Singleton<LobbyItemsController>.Instance.UnEquip(lobbyItem.Slot);
				}
			}
			else
			{
				Singleton<LobbyItemsController>.Instance.UnEquip(lobbyItem.Slot);
			}
		}

		private void TryBuy(ItemPrice price, Action<bool> callback)
		{
			ShopNGUIController.TryToBuy(null, price, delegate
			{
				callback(true);
			}, delegate
			{
				callback(false);
			});
		}

		private void ToScreenView()
		{
			if (!TutorialCompleted)
			{
				HideAllTutorialHints();
			}
			if (!_screenPanel.activeInHierarchy)
			{
				_rememberedSelectedItem = _selectedItem;
				_selectedItem = null;
				_mainPanel.SetActiveSafe(false);
				_screenPanel.SetActiveSafe(true);
				_cameraSettingsPanel.SetActiveSafe(false);
				SetMySceneViews(false);
				_likeGuiObj.SetActive(false);
			}
		}

		private void ToLikePanel()
		{
			if (!TutorialCompleted)
			{
				HideAllTutorialHints();
			}
			if (!_likeGuiObj.activeInHierarchy)
			{
				_rememberedSelectedItem = _selectedItem;
				_selectedItem = null;
				_mainPanel.SetActiveSafe(false);
				_screenPanel.SetActiveSafe(false);
				_cameraSettingsPanel.SetActiveSafe(false);
				_likeGuiObj.SetActive(true);
			}
		}

		private void ToCameraSettings()
		{
			if (!TutorialCompleted)
			{
				HideAllTutorialHints();
			}
			if (!_cameraSettingsPanel.activeInHierarchy)
			{
				_rememberedSelectedItem = _selectedItem;
				_selectedItem = null;
				_mainPanel.SetActiveSafe(false);
				_screenPanel.SetActiveSafe(false);
				_cameraSettingsPanel.SetActiveSafe(true);
				_likeGuiObj.SetActive(false);
				SetMySceneViews(false);
			}
		}

		private void NavigateBack()
		{
			if (_craftedAndShowingItem != null)
			{
				return;
			}
			if (_screenPanel.activeInHierarchy || _cameraSettingsPanel.activeInHierarchy || _likeGuiObj.activeInHierarchy)
			{
				_selectedItem = _rememberedSelectedItem;
				_mainPanel.SetActiveSafeSelf(true);
				_categoriesContainer.SetActiveSafeSelf(true);
				_itemsScrollContainer.SetActiveSafeSelf(false);
				_screenPanel.SetActiveSafeSelf(false);
				_cameraSettingsPanel.SetActiveSafeSelf(false);
				_likeGuiObj.SetActiveSafeSelf(false);
				if (_currentScrollGroup != 0)
				{
					GoToGroup(_currentScrollGroup, false);
				}
				else if (!TutorialCompleted)
				{
					if (_categoryRootObj.activeInHierarchy)
					{
						TutorialStage = 0;
					}
					else if (_categoryDecorationsObj.activeInHierarchy)
					{
						TutorialStage = 1;
					}
				}
				SetMySceneViews(true);
				LobbyItem playerCraftedAndNotShownItem = Singleton<LobbyItemsController>.Instance.PlayerCraftedAndNotShownItem;
				if (playerCraftedAndNotShownItem != null)
				{
					ShowCraftedItem(playerCraftedAndNotShownItem);
				}
				return;
			}
			_selectedItem = null;
			if (_itemsScrollContainer.activeInHierarchy)
			{
				_itemsScrollContainer.SetActive(false);
				LobbyItemGroupType parentGroup = LobbyItemsController.GetParentGroup(_currentScrollGroup);
				_currentScrollGroup = LobbyItemGroupType.None;
				GoToGroup(parentGroup, false);
			}
			else
			{
				_currentScrollGroup = LobbyItemGroupType.None;
				if (_categoryRootObj.activeInHierarchy)
				{
					InterfaceEnabled = false;
				}
				else
				{
					GoToGroup(LobbyItemGroupType.None, false);
				}
			}
			SetMySceneViews(true);
		}

		private void OnCraftFinished(LobbyItem lobbyItem)
		{
			if (_enemyLobbyViewed || _likeGuiObj.activeInHierarchy || lobbyItem == null)
			{
				return;
			}
			if (lobbyItem.Info.Id == "decor_big_military_1")
			{
				HideAllTutorialHints();
			}
			if (_craftedAndShowingItem == null && !_screenPanel.activeInHierarchy && !_cameraSettingsPanel.activeInHierarchy)
			{
				UpdateViewsByItem(lobbyItem);
			}
			if (InterfaceEnabled && (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled) && !_screenPanel.activeInHierarchy && !_cameraSettingsPanel.activeInHierarchy && !_likeGuiObj.activeInHierarchy)
			{
				if (_craftedAndShowingItem != null)
				{
					_craftedItemsShowQueue.Enqueue(lobbyItem);
				}
				else
				{
					ShowCraftedItem(lobbyItem);
				}
			}
		}

		private void OnCloseBank()
		{
			if (InterfaceEnabled)
			{
				LobbyItem playerCraftedAndNotShownItem = Singleton<LobbyItemsController>.Instance.PlayerCraftedAndNotShownItem;
				if (playerCraftedAndNotShownItem != null)
				{
					ShowCraftedItem(playerCraftedAndNotShownItem);
				}
			}
		}

		private void ShowCraftedItem(LobbyItem craftedItem = null)
		{
			if (craftedItem == null)
			{
				craftedItem = Singleton<LobbyItemsController>.Instance.PlayerCraftedAndNotShownItem;
			}
			if (craftedItem == null)
			{
				return;
			}
			_craftedAndShowingItem = craftedItem;
			if (_speedUpWindow.IsVisible)
			{
				_speedUpWindow.Hide();
			}
			_lobbyChanged = true;
			if (MainMenuController.sharedController == null || MainMenuController.sharedController.rotateCamera == null || !MainMenuController.sharedController.rotateCamera.CanZoomTo(_craftedAndShowingItem))
			{
				GetCraftReward(_craftedAndShowingItem);
				return;
			}
			_selfVisibleBeforeZoomIn = _mainPanel.activeSelf;
			_mainPanel.SetActive(false);
			MainMenuController.sharedController.rotateCamera.PlayLobbyItemZoom(_craftedAndShowingItem);
			_mainMenuVisibleBeforeZoomIn = MainMenuController.sharedController.mainPanel.activeSelf;
			MainMenuController.sharedController.mainPanel.SetActiveSafe(false);
			if (Defs.isSoundFX)
			{
				NGUITools.PlaySound(_cameraZoomInAudio);
			}
		}

		public void GetCraftReward(LobbyItem item)
		{
			if (item != null && item.PlayerInfo.IsCraftedAndNotShown)
			{
				item.PlayerInfo.IsCraftedAndNotShown = false;
				ExperienceController.sharedController.AddExperience(item.Info.ExpForCrafting);
				_craftedAndShowingItem = null;
				if (item.Info.Id == "decor_big_military_1")
				{
					TutorialStage = 4;
				}
			}
		}

		private void OnCraftBuildingWindowClose(LobbyItem item)
		{
			MainMenuController.sharedController.rotateCamera.PlayFromItem();
			if (Defs.isSoundFX)
			{
				NGUITools.PlaySound(_cameraZoomOutAudio);
			}
		}

		private GameObject GetTutorialHintForStage(int stage)
		{
			return _tutorialHintsContainer.GetChildGameObject(stage.ToString(), true);
		}

		private void ShowTutorialHintForStage(int stage)
		{
			GameObject tutorialHintForStage = GetTutorialHintForStage(stage);
			if (tutorialHintForStage != null)
			{
				tutorialHintForStage.SetActive(true);
			}
		}

		private void HideAllTutorialHints()
		{
			_tutorialHintsContainer.Children().ForEach(delegate(GameObject c)
			{
				c.SetActive(false);
			});
		}

		public void OnZoomIn()
		{
			BuildedWindow.Show(_craftedAndShowingItem);
		}

		public void OnZoomOut()
		{
			BuildedWindow.Hide();
			if (_selfVisibleBeforeZoomIn)
			{
				_mainPanel.SetActive(true);
				_moveScroll = false;
				_updateScroll = true;
			}
			if (_mainMenuVisibleBeforeZoomIn)
			{
				MainMenuController.sharedController.mainPanel.SetActiveSafe(true);
			}
			if (_craftedItemsShowQueue.Any())
			{
				GetCraftReward(_craftedAndShowingItem);
				LobbyItem lobbyItem = _craftedItemsShowQueue.Dequeue();
				if (lobbyItem != null)
				{
					UpdateViewsByItem(lobbyItem);
					SetMySceneViews(true);
					ShowCraftedItem(lobbyItem);
				}
			}
			else
			{
				if (_selfVisibleBeforeZoomIn && _craftedAndShowingItem != null)
				{
					SelectItem(_craftedAndShowingItem, true);
				}
				GetCraftReward(_craftedAndShowingItem);
			}
		}

		public void U_BackButtonAction()
		{
			NavigateBack();
		}

		public void U_ToScreenViewAction()
		{
			ToScreenView();
		}

		public void U_ToCameraSettingsAction()
		{
			ToCameraSettings();
		}

		public void U_ToLikePanel()
		{
			ToLikePanel();
		}
	}
}
