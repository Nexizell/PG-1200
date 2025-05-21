using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class GiftController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CWaitDrop_003Ed__76 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private int _003Citer_003E5__1;

		public GiftCategory cat;

		public bool isContains;

		public string id;

		private bool _003Clk_003E5__2;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CWaitDrop_003Ed__76(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				_003Clk_003E5__2 = true;
				_003Citer_003E5__1 = 0;
				GiftInfo giftInfo = null;
				break;
			}
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (_003Clk_003E5__2)
			{
				_003Citer_003E5__1++;
				GiftInfo giftInfo = cat.GetRandomGift();
				if (isContains ? giftInfo.Id.Contains(id) : (giftInfo.Id == id))
				{
					_003Clk_003E5__2 = false;
					UnityEngine.Debug.Log(string.Format("[TTT] found '{0}' iterations count: {1}", new object[2] { giftInfo.Id, _003Citer_003E5__1 }));
				}
				if (_003Citer_003E5__1 > 100)
				{
					UnityEngine.Debug.Log("[TTT] stop waiting");
					_003Clk_003E5__2 = false;
				}
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CGetDataFromServerLoop_003Ed__82 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GiftController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CGetDataFromServerLoop_003Ed__82(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_003f;
			case 1:
				_003C_003E1__state = -1;
				goto IL_003f;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E2__current = new WaitForSeconds(870f);
				_003C_003E1__state = 3;
				return true;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_004e;
				}
				IL_003f:
				if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_004e;
				IL_004e:
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadDataFormServer());
				_003C_003E1__state = 2;
				return true;
			}
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CDownloadDataFormServer_003Ed__84 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GiftController _003C_003E4__this;

		private string _003CurlDataAddress_003E5__1;

		private WWW _003CdownloadData_003E5__2;

		private int _003Citer_003E5__3;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CDownloadDataFormServer_003Ed__84(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				if (_003C_003E4__this._kDataLoading)
				{
					return false;
				}
				_003C_003E4__this._kDataLoading = true;
				_003CurlDataAddress_003E5__1 = UrlForLoadData;
				_003CdownloadData_003E5__2 = null;
				_003Citer_003E5__3 = 3;
				goto IL_00dc;
			case 1:
				_003C_003E1__state = -1;
				goto IL_008d;
			case 2:
				{
					_003C_003E1__state = -1;
					int num = _003Citer_003E5__3 - 1;
					_003Citer_003E5__3 = num;
					goto IL_00dc;
				}
				IL_008d:
				if (!_003CdownloadData_003E5__2.isDone)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 1;
					return true;
				}
				if (!string.IsNullOrEmpty(_003CdownloadData_003E5__2.error))
				{
					_003C_003E2__current = new WaitForSeconds(5f);
					_003C_003E1__state = 2;
					return true;
				}
				break;
				IL_00dc:
				if (_003Citer_003E5__3 <= 0)
				{
					break;
				}
				_003CdownloadData_003E5__2 = Tools.CreateWwwIfNotConnected(_003CurlDataAddress_003E5__1);
				if (_003CdownloadData_003E5__2 == null)
				{
					return false;
				}
				goto IL_008d;
			}
			if (_003CdownloadData_003E5__2 == null || !string.IsNullOrEmpty(_003CdownloadData_003E5__2.error))
			{
				if (Defs.IsDeveloperBuild && _003CdownloadData_003E5__2 != null)
				{
					UnityEngine.Debug.LogWarningFormat("Request to {0} failed: {1}", _003CurlDataAddress_003E5__1, _003CdownloadData_003E5__2.error);
				}
				_003C_003E4__this._kDataLoading = false;
				return false;
			}
			string text = URLs.Sanitize(_003CdownloadData_003E5__2);
			Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
			if (dictionary == null)
			{
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.LogError("Bad response: " + text);
				}
				_003C_003E4__this._kDataLoading = false;
				return false;
			}
			if (dictionary.ContainsKey("isActive"))
			{
				_003C_003E4__this._cfgGachaIsActive = Convert.ToBoolean(dictionary["isActive"], CultureInfo.InvariantCulture);
				if (!_003C_003E4__this._cfgGachaIsActive)
				{
					_003C_003E4__this._kDataLoading = false;
					_003C_003E4__this.OnDataLoaded();
					return false;
				}
			}
			if (dictionary.ContainsKey("price"))
			{
				_003C_003E4__this.CostBuyCanGetGift.Value = Convert.ToInt32(dictionary["price"], CultureInfo.InvariantCulture);
			}
			_003C_003E4__this._forNewPlayer.Clear();
			if (dictionary.ContainsKey("newPlayerEvent"))
			{
				List<object> list = dictionary["newPlayerEvent"] as List<object>;
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
						GiftNewPlayerInfo giftNewPlayerInfo = new GiftNewPlayerInfo();
						if (dictionary2.ContainsKey("typeCategory"))
						{
							giftNewPlayerInfo.TypeCategory = _003C_003E4__this.ParseToEnum(dictionary2["typeCategory"].ToString());
							if (dictionary2.ContainsKey("count"))
							{
								giftNewPlayerInfo.Count.Value = int.Parse(dictionary2["count"].ToString());
							}
							if (dictionary2.ContainsKey("percent"))
							{
								object value = dictionary2["percent"];
								giftNewPlayerInfo.Percent = (float)Convert.ToDouble(value, CultureInfo.InvariantCulture);
							}
							_003C_003E4__this._forNewPlayer.Add(giftNewPlayerInfo);
						}
					}
				}
			}
			_003C_003E4__this._categories.Clear();
			if (dictionary.ContainsKey("categories"))
			{
				List<object> list2 = dictionary["categories"] as List<object>;
				if (list2 != null)
				{
					for (int j = 0; j < list2.Count; j++)
					{
						Dictionary<string, object> dictionary3 = list2[j] as Dictionary<string, object>;
						if (dictionary3 == null)
						{
							continue;
						}
						GiftCategory giftCategory = new GiftCategory();
						if (!dictionary3.ContainsKey("typeCategory"))
						{
							continue;
						}
						giftCategory.Type = _003C_003E4__this.ParseToEnum(dictionary3["typeCategory"].ToString());
						if (giftCategory.Type == GiftCategoryType.Gear || giftCategory.Type == GiftCategoryType.Grenades)
						{
							continue;
						}
						if (dictionary3.ContainsKey("posInScroll"))
						{
							giftCategory.ScrollPosition = int.Parse(dictionary3["posInScroll"].ToString());
						}
						if (!dictionary3.ContainsKey("gifts"))
						{
							continue;
						}
						if (dictionary3.ContainsKey("keyTransInfo"))
						{
							giftCategory.KeyTranslateInfoCommon = dictionary3["keyTransInfo"].ToString();
						}
						List<object> list3 = dictionary3["gifts"] as List<object>;
						if (list3 != null)
						{
							for (int k = 0; k < list3.Count; k++)
							{
								Dictionary<string, object> dictionary4 = list3[k] as Dictionary<string, object>;
								if (dictionary4 == null)
								{
									continue;
								}
								GiftInfo giftInfo = new GiftInfo();
								switch (giftCategory.Type)
								{
								case GiftCategoryType.Coins:
									giftInfo.Id = "Coins";
									break;
								case GiftCategoryType.Gems:
									giftInfo.Id = "Gems";
									break;
								case GiftCategoryType.Tickets:
									giftInfo.Id = "Tickets";
									break;
								default:
									if (dictionary4.ContainsKey("idGift"))
									{
										giftInfo.Id = dictionary4["idGift"].ToString();
									}
									break;
								}
								if (dictionary4.ContainsKey("count"))
								{
									giftInfo.Count.Value = int.Parse(dictionary4["count"].ToString());
								}
								if (dictionary4.ContainsKey("percent"))
								{
									object value2 = dictionary4["percent"];
									giftInfo.PercentAddInSlot = (float)Convert.ToDouble(value2, CultureInfo.InvariantCulture);
								}
								if (dictionary4.ContainsKey("keyTransInfo"))
								{
									giftInfo.KeyTranslateInfo = dictionary4["keyTransInfo"].ToString();
								}
								if (giftInfo.Count.Value == 0)
								{
									giftInfo.Count.Value = 1;
								}
								giftCategory.AddGift(giftInfo);
							}
						}
						if (giftCategory.AnyGifts)
						{
							_003C_003E4__this._categories.Add(giftCategory);
						}
					}
				}
			}
			_003C_003E4__this.OnDataLoaded();
			_003C_003E4__this._kDataLoading = false;
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	internal sealed class _003CCheckAvailableGifts_003Ed__92 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public GiftController _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CCheckAvailableGifts_003Ed__92(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			int num = _003C_003E1__state;
			if (num != 0)
			{
				if (num != 1)
				{
					return false;
				}
				_003C_003E1__state = -1;
			}
			else
			{
				_003C_003E1__state = -1;
			}
			if (!(WeaponManager.sharedManager != null))
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this.RecreateSlots();
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public const string FREE_SPINS_STORAGER_KEY = "freeSpinsCount";

	public const string KEY_COUNT_GIFT_FOR_NEW_PLAYER = "keyCountGiftNewPlayer";

	public const string KEY_EDITOR_SKIN = "editor_Skin";

	public const string KEY_EDITOR_CAPE = "editor_Cape";

	public const string KEY_COLLECTION_GUNS_GRAY = "guns_gray";

	public const string KEY_COLLECTION_MASK = "equip_Mask";

	public const string KEY_COLLECTION_CAPE = "equip_Cape";

	public const string KEY_COLLECTION_BOOTS = "equip_Boots";

	public const string KEY_COLLECTION_HAT = "equip_Hat";

	public const string KEY_COLLECTION_WEAPONSKIN_RANDOM = "random";

	public const string KEY_COLLECTION_GADGETS_RANDOM = "gadget_random";

	private const string KEY_FOR_SAVE_SERVER_TIME = "SaveServerTime";

	private const string KEY_NEWPLAYER_ARMOR_GETTED = "keyIsGetArmorNewPlayer";

	private const string KEY_NEWPLAYER_SKIN_GETTED = "keyIsGetSkinNewPlayer";

	private const float UPDATE_DATA_FROM_SERVER_INTERVAL = 870f;

	private const int TIME_TO_NEXT_GIFT = 14400;

	public static GiftController Instance;

	public SaltedInt CostBuyCanGetGift = new SaltedInt(15461355, 0);

	private bool _canGetTimerGift;

	private SaltedFloatEps _saltedLocalTimer = new SaltedFloatEps(-1f);

	private int _oldTime = -1;

	[ReadOnly]
	[SerializeField]
	private readonly List<GiftCategory> _categories = new List<GiftCategory>();

	[ReadOnly]
	[SerializeField]
	private readonly List<SlotInfo> _slots = new List<SlotInfo>();

	[ReadOnly]
	[SerializeField]
	private readonly List<GiftNewPlayerInfo> _forNewPlayer = new List<GiftNewPlayerInfo>();

	private bool _cfgGachaIsActive;

	private static Dictionary<int, List<ItemRecord>> _grayCategoryWeapons;

	private SaltedInt _freeSpins = new SaltedInt(15461355, 0);

	private GiftCategoryType? _prevDroppedCategoryType;

	private bool _kAlreadyGenerateSlot;

	private bool _kDataLoading;

	private static string UrlForLoadData
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_ios.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_android.json";
				}
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_amazon.json";
				}
				return string.Empty;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/gift/gift_pixelgun_wp8.json";
			}
			return string.Empty;
		}
	}

	private float _localTimer
	{
		get
		{
			return _saltedLocalTimer.value;
		}
		set
		{
			_saltedLocalTimer.value = value;
		}
	}

	public float TimeLeft
	{
		get
		{
			return _localTimer;
		}
	}

	public List<SlotInfo> Slots
	{
		get
		{
			return _slots;
		}
	}

	public bool CanGetTimerGift
	{
		get
		{
			if (ActiveGift)
			{
				return _canGetTimerGift;
			}
			return false;
		}
	}

	public bool CanGetFreeSpinGift
	{
		get
		{
			if (ActiveGift)
			{
				return FreeSpins > 0;
			}
			return false;
		}
	}

	public bool CanGetGift
	{
		get
		{
			if (!CanGetTimerGift)
			{
				return CanGetFreeSpinGift;
			}
			return true;
		}
	}

	public bool ActiveGift
	{
		get
		{
			if (_cfgGachaIsActive && DataIsLoaded)
			{
				return FriendsController.ServerTime >= 0;
			}
			return false;
		}
	}

	public bool DataIsLoaded
	{
		get
		{
			if (_slots == null)
			{
				return false;
			}
			if (_slots.Count == 0)
			{
				return false;
			}
			return true;
		}
	}

	public static Dictionary<int, List<ItemRecord>> GrayCategoryWeapons
	{
		get
		{
			if (_grayCategoryWeapons == null)
			{
				_grayCategoryWeapons = new Dictionary<int, List<ItemRecord>>();
				_grayCategoryWeapons.Add(0, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon10"),
					ItemDb.GetByPrefabName("Weapon44"),
					ItemDb.GetByPrefabName("Weapon79")
				});
				_grayCategoryWeapons.Add(1, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon278"),
					ItemDb.GetByPrefabName("Weapon65"),
					ItemDb.GetByPrefabName("Weapon286")
				});
				_grayCategoryWeapons.Add(2, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon252"),
					ItemDb.GetByPrefabName("Weapon258"),
					ItemDb.GetByPrefabName("Weapon48"),
					ItemDb.GetByPrefabName("Weapon253")
				});
				_grayCategoryWeapons.Add(3, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon257"),
					ItemDb.GetByPrefabName("Weapon262"),
					ItemDb.GetByPrefabName("Weapon251")
				});
				_grayCategoryWeapons.Add(4, new List<ItemRecord>
				{
					ItemDb.GetByPrefabName("Weapon330"),
					ItemDb.GetByPrefabName("Weapon308")
				});
				_grayCategoryWeapons.Add(5, new List<ItemRecord> { ItemDb.GetByPrefabName("Weapon222") });
			}
			return _grayCategoryWeapons;
		}
	}

	public int FreeSpins
	{
		get
		{
			return _freeSpins.Value;
		}
	}

	public static int CountGetGiftForNewPlayer
	{
		get
		{
			return Storager.getInt("keyCountGiftNewPlayer");
		}
		set
		{
			if (value >= 0 && value < CountGetGiftForNewPlayer)
			{
				Storager.setInt("keyCountGiftNewPlayer", value);
			}
		}
	}

	private long LastTimeGetGift
	{
		get
		{
			return Storager.getInt("SaveServerTime");
		}
		set
		{
			int val = (int)value;
			Storager.setInt("SaveServerTime", val);
		}
	}

	internal TimeSpan FreeGachaAvailableIn
	{
		get
		{
			if (!Storager.hasKey("SaveServerTime"))
			{
				LastTimeGetGift = FriendsController.ServerTime - 14400 + 1;
			}
			long num = FriendsController.ServerTime - LastTimeGetGift;
			return TimeSpan.FromSeconds(14400 - num);
		}
	}

	internal event EventHandler FreeSpinCountChanged;

	public static event Action OnChangeSlots;

	public static event Action OnTimerEnded;

	public static event Action<string> OnUpdateTimer;

	private void RaiseFreeSpinCountChanged()
	{
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.Log("RaiseFreeSpinCountChanged()");
		}
		EventHandler freeSpinCountChanged = this.FreeSpinCountChanged;
		if (freeSpinCountChanged != null)
		{
			freeSpinCountChanged(this, EventArgs.Empty);
		}
	}

	public int IncrementFreeSpins(int increment)
	{
		int num = FreeSpins + increment;
		_freeSpins.Value = num;
		Storager.setInt("freeSpinsCount", _freeSpins.Value);
		RaiseFreeSpinCountChanged();
		return num;
	}

	private void Awake()
	{
		Instance = this;
		_freeSpins.Value = Storager.getInt("freeSpinsCount");
		_localTimer = -1f;
		_categories.Clear();
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!Storager.hasKey("SaveServerTime"))
		{
			Storager.setInt("keyCountGiftNewPlayer", 2);
		}
		if (!Storager.hasKey("keyCountGiftNewPlayer"))
		{
			Storager.setInt("keyCountGiftNewPlayer", 0);
		}
		if (!Storager.hasKey("keyIsGetArmorNewPlayer"))
		{
			Storager.setInt("keyIsGetArmorNewPlayer", 0);
		}
		if (!Storager.hasKey("keyIsGetSkinNewPlayer"))
		{
			Storager.setInt("keyIsGetSkinNewPlayer", 0);
		}
		Storager.getInt("keyCountGiftNewPlayer");
		StartCoroutine(GetDataFromServerLoop());
		FriendsController.ServerTimeUpdated += OnUpdateTimeFromServer;
	}

	private void OnDestroy()
	{
		FriendsController.ServerTimeUpdated -= OnUpdateTimeFromServer;
		Instance = null;
	}

	private void Update()
	{
		if (_localTimer > 0f)
		{
			_localTimer -= Time.deltaTime;
			if (_localTimer < 0f)
			{
				_localTimer = 0f;
			}
			_canGetTimerGift = false;
			if (_oldTime != (int)_localTimer)
			{
				_oldTime = (int)_localTimer;
				if (GiftController.OnUpdateTimer != null)
				{
					GiftController.OnUpdateTimer(GetStringTimer());
				}
			}
		}
		else if (!_canGetTimerGift && (int)_localTimer == 0)
		{
			_localTimer = -1f;
			_canGetTimerGift = true;
			if (GiftController.OnUpdateTimer != null)
			{
				GiftController.OnUpdateTimer(GetStringTimer());
			}
			if (GiftController.OnTimerEnded != null)
			{
				GiftController.OnTimerEnded();
			}
		}
	}

	public void SetTimer(int val)
	{
		if (val > 14400)
		{
			val = 14400;
		}
		if (val == 0)
		{
			LastTimeGetGift = FriendsController.ServerTime - 14400 + 1;
		}
		else
		{
			long lastTimeGetGift = FriendsController.ServerTime - (14400 - val);
			LastTimeGetGift = lastTimeGetGift;
		}
		OnUpdateTimeFromServer();
	}

	private GiftCategoryType ParseToEnum(string typeCat)
	{
		return typeCat.ToEnum<GiftCategoryType>() ?? GiftCategoryType.none;
	}

	public void SetGifts()
	{
		if (_cfgGachaIsActive)
		{
			if (_categories != null && _categories.Count > 0)
			{
				StartCoroutine(CheckAvailableGifts());
			}
			return;
		}
		_categories.Clear();
		_slots.Clear();
		if (GiftController.OnChangeSlots != null)
		{
			GiftController.OnChangeSlots();
		}
	}

	private void RecreateSlots()
	{
		if (_kAlreadyGenerateSlot || !_cfgGachaIsActive)
		{
			return;
		}
		_kAlreadyGenerateSlot = true;
		_slots.Clear();
		foreach (GiftCategory category in _categories)
		{
			category.CheckGifts();
			if (category.AvaliableGiftsCount < 1)
			{
				continue;
			}
			SlotInfo slotInfo = new SlotInfo();
			slotInfo.category = category;
			slotInfo.gift = category.GetRandomGift();
			if (slotInfo.gift != null && !string.IsNullOrEmpty(slotInfo.gift.Id))
			{
				slotInfo.percentGetSlot = category.PercentChance;
				slotInfo.positionInScroll = category.ScrollPosition;
				slotInfo.isActiveEvent = false;
				if (CountGetGiftForNewPlayer > 0)
				{
					SetPerGetGiftForNewPlayer(slotInfo);
				}
				_slots.Add(slotInfo);
			}
		}
		if (_prevDroppedCategoryType.HasValue)
		{
			SlotInfo slotInfo2 = _slots.FirstOrDefault((SlotInfo s) => s.category.Type == _prevDroppedCategoryType);
			if (slotInfo2 != null)
			{
				slotInfo2.NoDropped = true;
			}
		}
		_slots.Sort(delegate(SlotInfo left, SlotInfo right)
		{
			if (left == null && right == null)
			{
				return 0;
			}
			if (left == null)
			{
				return -1;
			}
			return (right == null) ? 1 : left.positionInScroll.CompareTo(right.positionInScroll);
		});
		if (GiftController.OnChangeSlots != null)
		{
			GiftController.OnChangeSlots();
		}
		OnUpdateTimeFromServer();
	}

	private IEnumerator WaitDrop(GiftCategory cat, string id, bool isContains = false)
	{
		bool lk = true;
		int iter = 0;
		while (lk)
		{
			iter++;
			GiftInfo randomGift = cat.GetRandomGift();
			if (isContains ? randomGift.Id.Contains(id) : (randomGift.Id == id))
			{
				lk = false;
				UnityEngine.Debug.Log(string.Format("[TTT] found '{0}' iterations count: {1}", new object[2] { randomGift.Id, iter }));
			}
			if (iter > 100)
			{
				UnityEngine.Debug.Log("[TTT] stop waiting");
				lk = false;
			}
			yield return null;
		}
	}

	public GiftNewPlayerInfo GetInfoNewPlayer(GiftCategoryType needCat)
	{
		return _forNewPlayer.Find((GiftNewPlayerInfo val) => val.TypeCategory == needCat);
	}

	private void SetPerGetGiftForNewPlayer(SlotInfo curSlot)
	{
		float percentGetSlot = 0f;
		int value = curSlot.gift.Count.Value;
		curSlot.isActiveEvent = true;
		GiftNewPlayerInfo infoNewPlayer = GetInfoNewPlayer(curSlot.category.Type);
		if (infoNewPlayer != null)
		{
			value = infoNewPlayer.Count.Value;
			if (curSlot.category.Type == GiftCategoryType.ArmorAndHat && Storager.getInt("keyIsGetArmorNewPlayer") == 0)
			{
				percentGetSlot = infoNewPlayer.Percent;
			}
			if (curSlot.category.Type == GiftCategoryType.Skins && Storager.getInt("keyIsGetSkinNewPlayer") == 0)
			{
				percentGetSlot = infoNewPlayer.Percent;
			}
			if (curSlot.category.Type == GiftCategoryType.Coins)
			{
				percentGetSlot = infoNewPlayer.Percent;
			}
			if (curSlot.category.Type == GiftCategoryType.Gems)
			{
				percentGetSlot = infoNewPlayer.Percent;
			}
			if (curSlot.category.Type == GiftCategoryType.Tickets)
			{
				percentGetSlot = infoNewPlayer.Percent;
			}
		}
		curSlot.percentGetSlot = percentGetSlot;
		curSlot.CountGift = value;
	}

	public void UpdateSlot(SlotInfo curSlot)
	{
		curSlot.category.CheckGifts();
		curSlot.gift = curSlot.category.GetRandomGift();
		if (curSlot.gift == null)
		{
			_slots.Remove(curSlot);
		}
		else
		{
			curSlot.percentGetSlot = curSlot.category.PercentChance;
			curSlot.positionInScroll = curSlot.category.ScrollPosition;
		}
		foreach (SlotInfo slot in _slots)
		{
			_categories.FirstOrDefault((GiftCategory c) => c == slot.category);
			if (CountGetGiftForNewPlayer > 0)
			{
				SetPerGetGiftForNewPlayer(slot);
			}
			else
			{
				slot.percentGetSlot = slot.category.PercentChance;
			}
		}
	}

	public void ReCreateSlots()
	{
		_kAlreadyGenerateSlot = false;
		SetGifts();
	}

	public SlotInfo GetRandomSlot()
	{
		return null;
	}

	private IEnumerator GetDataFromServerLoop()
	{
		while (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage <= TrainingController.NewTrainingCompletedStage.None)
		{
			yield return null;
		}
		while (true)
		{
			yield return StartCoroutine(DownloadDataFormServer());
			yield return new WaitForSeconds(870f);
		}
	}

	private IEnumerator DownloadDataFormServer()
	{
		if (_kDataLoading)
		{
			yield break;
		}
		_kDataLoading = true;
		string urlDataAddress = UrlForLoadData;
		WWW downloadData = null;
		int iter = 3;
		while (iter > 0)
		{
			downloadData = Tools.CreateWwwIfNotConnected(urlDataAddress);
			if (downloadData == null)
			{
				yield break;
			}
			while (!downloadData.isDone)
			{
				yield return null;
			}
			if (string.IsNullOrEmpty(downloadData.error))
			{
				break;
			}
			yield return new WaitForSeconds(5f);
			int num = iter - 1;
			iter = num;
		}
		if (downloadData == null || !string.IsNullOrEmpty(downloadData.error))
		{
			if (Defs.IsDeveloperBuild && downloadData != null)
			{
				UnityEngine.Debug.LogWarningFormat("Request to {0} failed: {1}", urlDataAddress, downloadData.error);
			}
			_kDataLoading = false;
			yield break;
		}
		string text = URLs.Sanitize(downloadData);
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		if (dictionary == null)
		{
			if (Defs.IsDeveloperBuild)
			{
				UnityEngine.Debug.LogError("Bad response: " + text);
			}
			_kDataLoading = false;
			yield break;
		}
		if (dictionary.ContainsKey("isActive"))
		{
			_cfgGachaIsActive = Convert.ToBoolean(dictionary["isActive"], CultureInfo.InvariantCulture);
			if (!_cfgGachaIsActive)
			{
				_kDataLoading = false;
				OnDataLoaded();
				yield break;
			}
		}
		if (dictionary.ContainsKey("price"))
		{
			CostBuyCanGetGift.Value = Convert.ToInt32(dictionary["price"], CultureInfo.InvariantCulture);
		}
		_forNewPlayer.Clear();
		if (dictionary.ContainsKey("newPlayerEvent"))
		{
			List<object> list = dictionary["newPlayerEvent"] as List<object>;
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
					GiftNewPlayerInfo giftNewPlayerInfo = new GiftNewPlayerInfo();
					if (dictionary2.ContainsKey("typeCategory"))
					{
						giftNewPlayerInfo.TypeCategory = ParseToEnum(dictionary2["typeCategory"].ToString());
						if (dictionary2.ContainsKey("count"))
						{
							giftNewPlayerInfo.Count.Value = int.Parse(dictionary2["count"].ToString());
						}
						if (dictionary2.ContainsKey("percent"))
						{
							object value = dictionary2["percent"];
							giftNewPlayerInfo.Percent = (float)Convert.ToDouble(value, CultureInfo.InvariantCulture);
						}
						_forNewPlayer.Add(giftNewPlayerInfo);
					}
				}
			}
		}
		_categories.Clear();
		if (dictionary.ContainsKey("categories"))
		{
			List<object> list2 = dictionary["categories"] as List<object>;
			if (list2 != null)
			{
				for (int j = 0; j < list2.Count; j++)
				{
					Dictionary<string, object> dictionary3 = list2[j] as Dictionary<string, object>;
					if (dictionary3 == null)
					{
						continue;
					}
					GiftCategory giftCategory = new GiftCategory();
					if (!dictionary3.ContainsKey("typeCategory"))
					{
						continue;
					}
					giftCategory.Type = ParseToEnum(dictionary3["typeCategory"].ToString());
					if (giftCategory.Type == GiftCategoryType.Gear || giftCategory.Type == GiftCategoryType.Grenades)
					{
						continue;
					}
					if (dictionary3.ContainsKey("posInScroll"))
					{
						giftCategory.ScrollPosition = int.Parse(dictionary3["posInScroll"].ToString());
					}
					if (!dictionary3.ContainsKey("gifts"))
					{
						continue;
					}
					if (dictionary3.ContainsKey("keyTransInfo"))
					{
						giftCategory.KeyTranslateInfoCommon = dictionary3["keyTransInfo"].ToString();
					}
					List<object> list3 = dictionary3["gifts"] as List<object>;
					if (list3 != null)
					{
						for (int k = 0; k < list3.Count; k++)
						{
							Dictionary<string, object> dictionary4 = list3[k] as Dictionary<string, object>;
							if (dictionary4 == null)
							{
								continue;
							}
							GiftInfo giftInfo = new GiftInfo();
							switch (giftCategory.Type)
							{
							case GiftCategoryType.Coins:
								giftInfo.Id = "Coins";
								break;
							case GiftCategoryType.Gems:
								giftInfo.Id = "Gems";
								break;
							case GiftCategoryType.Tickets:
								giftInfo.Id = "Tickets";
								break;
							default:
								if (dictionary4.ContainsKey("idGift"))
								{
									giftInfo.Id = dictionary4["idGift"].ToString();
								}
								break;
							}
							if (dictionary4.ContainsKey("count"))
							{
								giftInfo.Count.Value = int.Parse(dictionary4["count"].ToString());
							}
							if (dictionary4.ContainsKey("percent"))
							{
								object value2 = dictionary4["percent"];
								giftInfo.PercentAddInSlot = (float)Convert.ToDouble(value2, CultureInfo.InvariantCulture);
							}
							if (dictionary4.ContainsKey("keyTransInfo"))
							{
								giftInfo.KeyTranslateInfo = dictionary4["keyTransInfo"].ToString();
							}
							if (giftInfo.Count.Value == 0)
							{
								giftInfo.Count.Value = 1;
							}
							giftCategory.AddGift(giftInfo);
						}
					}
					if (giftCategory.AnyGifts)
					{
						_categories.Add(giftCategory);
					}
				}
			}
		}
		OnDataLoaded();
		_kDataLoading = false;
	}

	private void OnDataLoaded()
	{
		SetGifts();
	}

	public SlotInfo GetGift(bool ignoreAvailabilityCheck = false)
	{
		if (!ignoreAvailabilityCheck)
		{
			if (CanGetTimerGift)
			{
				_canGetTimerGift = false;
				_localTimer = -1f;
				ReSaveLastTimeSever();
				RaiseFreeSpinCountChanged();
			}
			else
			{
				if (FreeSpins <= 0)
				{
					return null;
				}
				_freeSpins.Value--;
				Storager.setInt("freeSpinsCount", _freeSpins.Value);
				RaiseFreeSpinCountChanged();
			}
		}
		List<SlotInfo> list = _slots.Where((SlotInfo s) => !s.NoDropped).ToList();
		float max = list.Sum((SlotInfo s) => s.percentGetSlot);
		float num = UnityEngine.Random.Range(0f, max);
		float num2 = 0f;
		SlotInfo slotInfo = null;
		for (int i = 0; i < list.Count; i++)
		{
			SlotInfo slotInfo2 = list[i];
			num2 += slotInfo2.percentGetSlot;
			if (num <= num2)
			{
				slotInfo = slotInfo2;
				slotInfo.numInScroll = _slots.IndexOf(slotInfo2);
				break;
			}
		}
		if (slotInfo != null)
		{
			CountGetGiftForNewPlayer--;
			GiveProductForSlot(slotInfo);
		}
		slotInfo.NoDropped = true;
		_prevDroppedCategoryType = slotInfo.category.Type;
		return slotInfo;
	}

	public void CheckAvaliableSlots()
	{
		bool flag = false;
		for (int num = _slots.Count - 1; num >= 0; num--)
		{
			SlotInfo slotInfo = _slots[num];
			if (slotInfo == null)
			{
				_slots.RemoveAt(num);
			}
			else
			{
				if (slotInfo.CheckAvaliableGift())
				{
					flag = true;
				}
				if (slotInfo.gift == null)
				{
					_slots.RemoveAt(num);
				}
			}
		}
		if (flag && GiftController.OnChangeSlots != null)
		{
			GiftController.OnChangeSlots();
		}
	}

	public void GiveProductForSlot(SlotInfo curSlot)
	{
		if (curSlot == null)
		{
			return;
		}
		switch (curSlot.category.Type)
		{
		case GiftCategoryType.Coins:
			BankController.AddCoins(curSlot.CountGift, false);
			StartCoroutine(BankController.WaitForIndicationGems("Coins"));
			break;
		case GiftCategoryType.Gems:
			BankController.AddGems(curSlot.CountGift, false);
			StartCoroutine(BankController.WaitForIndicationGems("GemsCurrency"));
			break;
		case GiftCategoryType.Tickets:
			BankController.AddTickets(curSlot.CountGift, false);
			StartCoroutine(BankController.WaitForIndicationGems("TicketsCurrency"));
			break;
		case GiftCategoryType.Skins:
			Storager.setInt("keyIsGetSkinNewPlayer", 1);
			ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.SkinsCategory, curSlot.gift.Id, 1, false, 0, null, null, false);
			break;
		case GiftCategoryType.Gear:
		{
			int int2 = Storager.getInt(curSlot.gift.Id);
			Storager.setInt(curSlot.gift.Id, int2 + curSlot.gift.Count.Value);
			break;
		}
		case GiftCategoryType.Grenades:
		{
			int @int = Storager.getInt(curSlot.gift.Id);
			Storager.setInt(curSlot.gift.Id, @int + curSlot.gift.Count.Value);
			break;
		}
		case GiftCategoryType.Wear:
			ShopNGUIController.ProvideItem(curSlot.gift.TypeShopCat.Value, curSlot.gift.Id);
			if (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
			{
				ShopNGUIController.sharedShop.wearEquipAction(curSlot.gift.TypeShopCat.Value, "", "");
			}
			break;
		case GiftCategoryType.ArmorAndHat:
			Storager.setInt("keyIsGetArmorNewPlayer", 1);
			if (curSlot.gift.TypeShopCat == ShopNGUIController.CategoryNames.ArmorCategory)
			{
				ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.ArmorCategory, curSlot.gift.Id);
				if (ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
				{
					ShopNGUIController.sharedShop.wearEquipAction(ShopNGUIController.CategoryNames.ArmorCategory, "", "");
				}
			}
			break;
		case GiftCategoryType.Gun1:
		case GiftCategoryType.Gun2:
		case GiftCategoryType.Gun3:
		case GiftCategoryType.Gun4:
		case GiftCategoryType.Guns_gray:
			if (WeaponManager.IsExclusiveWeapon(curSlot.gift.Id))
			{
				WeaponManager.ProvideExclusiveWeaponByTag(curSlot.gift.Id);
			}
			else
			{
				GiveProduct(curSlot.gift.TypeShopCat.Value, curSlot.gift.Id);
			}
			break;
		case GiftCategoryType.Editor:
			if (curSlot.gift.Id == "editor_Cape")
			{
				GiveProduct(ShopNGUIController.CategoryNames.CapesCategory, "cape_Custom");
				break;
			}
			if (curSlot.gift.Id == "editor_Skin")
			{
				Storager.setInt(Defs.SkinsMakerInProfileBought, 1);
				break;
			}
			UnityEngine.Debug.LogError(string.Format("[GIFT] unknown editor id: '{0}'", new object[1] { curSlot.gift.Id }));
			break;
		case GiftCategoryType.Masks:
			GiveProduct(ShopNGUIController.CategoryNames.MaskCategory, curSlot.gift.Id);
			break;
		case GiftCategoryType.Capes:
			GiveProduct(ShopNGUIController.CategoryNames.CapesCategory, curSlot.gift.Id);
			break;
		case GiftCategoryType.Boots:
			GiveProduct(ShopNGUIController.CategoryNames.BootsCategory, curSlot.gift.Id);
			break;
		case GiftCategoryType.Hats_random:
			GiveProduct(ShopNGUIController.CategoryNames.HatsCategory, curSlot.gift.Id);
			break;
		case GiftCategoryType.Stickers:
		{
			TypePackSticker? typePackSticker = curSlot.gift.Id.ToEnum<TypePackSticker>();
			if (!typePackSticker.HasValue)
			{
				throw new Exception("sticker id type parse error");
			}
			StickersController.BuyStickersPack(typePackSticker.Value);
			break;
		}
		case GiftCategoryType.Freespins:
			_freeSpins.Value += curSlot.gift.Count.Value;
			Storager.setInt("freeSpinsCount", _freeSpins.Value);
			RaiseFreeSpinCountChanged();
			break;
		case GiftCategoryType.WeaponSkin:
			GiveProduct(ShopNGUIController.CategoryNames.LeagueWeaponSkinsCategory, curSlot.gift.Id);
			break;
		case GiftCategoryType.Gadgets:
			if (GadgetsInfo.info.ContainsKey(curSlot.gift.Id))
			{
				GadgetInfo gadgetInfo = GadgetsInfo.info[curSlot.gift.Id];
				GiveProduct((ShopNGUIController.CategoryNames)gadgetInfo.Category, gadgetInfo.Id);
			}
			else
			{
				UnityEngine.Debug.LogErrorFormat("not found gadget: '{0}'", curSlot.gift.Id);
			}
			break;
		case GiftCategoryType.Event_content:
			break;
		}
	}

	private void GiveProduct(ShopNGUIController.CategoryNames category, string itemId, bool autoEquip = true)
	{
		ShopNGUIController.ProvideItem(category, itemId);
		if (autoEquip && ShopNGUIController.sharedShop != null && ShopNGUIController.sharedShop.wearEquipAction != null)
		{
			ShopNGUIController.sharedShop.wearEquipAction(category, "", "");
		}
	}

	public static List<string> GetAvailableGrayWeaponsTags()
	{
		int key = ExpController.OurTierForAnyPlace();
		return (from w in GrayCategoryWeapons[key]
			where Storager.getInt(w.StorageId) == 0
			select w.Tag).ToList();
	}

	private string GetRandomGrayWeapon()
	{
		List<string> availableGrayWeaponsTags = GetAvailableGrayWeaponsTags();
		if (!availableGrayWeaponsTags.Any())
		{
			return string.Empty;
		}
		int index = UnityEngine.Random.Range(0, availableGrayWeaponsTags.Count);
		return availableGrayWeaponsTags[index];
	}

	private IEnumerator CheckAvailableGifts()
	{
		while (!(WeaponManager.sharedManager != null))
		{
			yield return null;
		}
		RecreateSlots();
	}

	public void ReSaveLastTimeSever()
	{
		LastTimeGetGift = FriendsController.ServerTime;
		OnUpdateTimeFromServer();
	}

	public string GetStringTimer()
	{
		return RiliExtensions.GetTimeString((long)_localTimer);
	}

	private void OnUpdateTimeFromServer()
	{
		if (_slots.Count == 0)
		{
			StartCoroutine(DownloadDataFormServer());
		}
		else
		{
			if (FriendsController.ServerTime < 0)
			{
				return;
			}
			_localTimer = -1f;
			_canGetTimerGift = false;
			if (!Storager.hasKey("SaveServerTime"))
			{
				LastTimeGetGift = FriendsController.ServerTime - 14400 + 1;
			}
			int num = (int)(FriendsController.ServerTime - LastTimeGetGift);
			if (num >= 14400)
			{
				_canGetTimerGift = true;
				if (GiftController.OnTimerEnded != null)
				{
					GiftController.OnTimerEnded();
				}
			}
			else
			{
				_canGetTimerGift = false;
				_localTimer = 14400 - num;
			}
		}
	}

	public void TryGetData()
	{
		if (!DataIsLoaded)
		{
			StartCoroutine(DownloadDataFormServer());
		}
	}

	static GiftController()
	{
		GiftController.OnChangeSlots = delegate
		{
		};
		GiftController.OnTimerEnded = delegate
		{
		};
		GiftController.OnUpdateTimer = delegate
		{
		};
	}
}
