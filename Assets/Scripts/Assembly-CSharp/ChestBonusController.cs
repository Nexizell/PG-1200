using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class ChestBonusController : MonoBehaviour
{
	public delegate void OnChestBonusEnabledDelegate();

	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass24_0
	{
		public Task futureToWait;

		internal bool _003CGetEventBonusInfoLoop_003Eb__0()
		{
			return futureToWait.IsCompleted;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CGetEventBonusInfoLoop_003Ed__24 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Task futureToWait;

		public ChestBonusController _003C_003E4__this;

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
		public _003CGetEventBonusInfoLoop_003Ed__24(int _003C_003E1__state)
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
				_003C_003E2__current = new WaitUntil(new _003C_003Ec__DisplayClass24_0
				{
					futureToWait = futureToWait
				}._003CGetEventBonusInfoLoop_003Eb__0);
				_003C_003E1__state = 1;
				return true;
			case 1:
				_003C_003E1__state = -1;
				goto IL_005f;
			case 2:
				_003C_003E1__state = -1;
				goto IL_00a4;
			case 3:
				{
					_003C_003E1__state = -1;
					goto IL_00a4;
				}
				IL_00a4:
				if (Time.realtimeSinceStartup - _003C_003E4__this._eventGetBonusInfoStartTime < 870f)
				{
					_003C_003E2__current = null;
					_003C_003E1__state = 3;
					return true;
				}
				goto IL_005f;
				IL_005f:
				_003C_003E2__current = _003C_003E4__this.StartCoroutine(_003C_003E4__this.DownloadDataAboutBonuses());
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
	internal sealed class _003CDownloadDataAboutBonuses_003Ed__26 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ChestBonusController _003C_003E4__this;

		private WWW _003CdownloadData_003E5__1;

		private string _003CbonusesDataAddress_003E5__2;

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
		public _003CDownloadDataAboutBonuses_003Ed__26(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			string text;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
			{
				_003C_003E1__state = -1;
				if (_003C_003E4__this._isGetBonusInfoRunning)
				{
					return false;
				}
				_003C_003E4__this._eventGetBonusInfoStartTime = Time.realtimeSinceStartup;
				_003C_003E4__this._isGetBonusInfoRunning = true;
				_003CbonusesDataAddress_003E5__2 = ChestBonusModel.GetUrlForDownloadBonusesData();
				string value = PersistentCacheManager.Instance.GetValue(_003CbonusesDataAddress_003E5__2);
				if (!string.IsNullOrEmpty(value))
				{
					text = value;
					break;
				}
				_003CdownloadData_003E5__1 = Tools.CreateWwwIfNotConnected(_003CbonusesDataAddress_003E5__2);
				if (_003CdownloadData_003E5__1 == null)
				{
					_003C_003E4__this._isGetBonusInfoRunning = false;
					return false;
				}
				_003C_003E2__current = _003CdownloadData_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(_003CdownloadData_003E5__1.error))
				{
					UnityEngine.Debug.LogError("DownloadDataAboutBonuses error: " + _003CdownloadData_003E5__1.error);
					_003C_003E4__this._bonusesData.Clear();
					_003C_003E4__this._isGetBonusInfoRunning = false;
					return false;
				}
				text = URLs.Sanitize(_003CdownloadData_003E5__1);
				PersistentCacheManager.Instance.SetValue(_003CbonusesDataAddress_003E5__2, text);
				_003CdownloadData_003E5__1 = null;
				break;
			}
			Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
			if (dictionary == null)
			{
				UnityEngine.Debug.LogWarning("DownloadDataAboutBonuses bonusesData = null");
				_003C_003E4__this._isGetBonusInfoRunning = false;
				return false;
			}
			chestBonusesObtainedOnceInCurrentRun = true;
			_003C_003E4__this._bonusesData.Clear();
			if (dictionary.ContainsKey("start"))
			{
				_003C_003E4__this._bonusesData.timeStart = Convert.ToInt32((long)dictionary["start"]);
			}
			if (dictionary.ContainsKey("duration"))
			{
				long val = Math.Min(Convert.ToInt64(dictionary["duration"]), 2147483647L);
				val = Math.Max(val, -2147483648L);
				_003C_003E4__this._bonusesData.duration = Convert.ToInt32(val);
			}
			if (_003C_003E4__this._bonusesData.timeStart == 0 || _003C_003E4__this._bonusesData.duration == 0)
			{
				_003C_003E4__this._isGetBonusInfoRunning = false;
				return false;
			}
			if (!dictionary.ContainsKey("bonuses"))
			{
				_003C_003E4__this._isGetBonusInfoRunning = false;
				return false;
			}
			List<object> list = dictionary["bonuses"] as List<object>;
			if (list != null)
			{
				_003C_003E4__this._bonusesData.bonuses = new List<ChestBonusData>();
				for (int i = 0; i < list.Count; i++)
				{
					Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
					if (dictionary2 == null)
					{
						continue;
					}
					ChestBonusData chestBonusData = new ChestBonusData();
					if (dictionary2.ContainsKey("linkKey"))
					{
						chestBonusData.linkKey = (string)dictionary2["linkKey"];
					}
					if (dictionary2.ContainsKey("isVisible"))
					{
						int num = Convert.ToInt32((long)dictionary2["isVisible"]);
						chestBonusData.isVisible = num == 1;
					}
					if (dictionary2.ContainsKey("items"))
					{
						List<object> list2 = dictionary2["items"] as List<object>;
						if (list2 != null)
						{
							chestBonusData.items = new List<ChestBonusItemData>();
							for (int j = 0; j < list2.Count; j++)
							{
								Dictionary<string, object> dictionary3 = list2[j] as Dictionary<string, object>;
								if (dictionary3 != null)
								{
									ChestBonusItemData chestBonusItemData = new ChestBonusItemData();
									if (dictionary3.ContainsKey("tag"))
									{
										chestBonusItemData.tag = (string)dictionary3["tag"];
									}
									if (dictionary3.ContainsKey("count"))
									{
										chestBonusItemData.count = Convert.ToInt32((long)dictionary3["count"]);
									}
									if (dictionary3.ContainsKey("timeLife"))
									{
										chestBonusItemData.timeLife = Convert.ToInt32((long)dictionary3["timeLife"]);
									}
									chestBonusData.items.Add(chestBonusItemData);
								}
							}
						}
					}
					_003C_003E4__this._bonusesData.bonuses.Add(chestBonusData);
				}
			}
			_003C_003E4__this._timeStartBonus = StarterPackModel.GetCurrentTimeByUnixTime(_003C_003E4__this._bonusesData.timeStart);
			int unixTime = _003C_003E4__this._bonusesData.timeStart + _003C_003E4__this._bonusesData.duration;
			_003C_003E4__this._timeEndBonus = StarterPackModel.GetCurrentTimeByUnixTime(unixTime);
			_003C_003E4__this.IsBonusActive = _003C_003E4__this.IsBonusActivate();
			if (ChestBonusController.OnChestBonusChange != null)
			{
				ChestBonusController.OnChestBonusChange();
			}
			_003C_003E4__this._isGetBonusInfoRunning = false;
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

	public static bool chestBonusesObtainedOnceInCurrentRun;

	private ChestBonusesData _bonusesData;

	private float _lastCheckEventTime;

	private bool _lastBonusActive;

	private DateTime _timeStartBonus;

	private DateTime _timeEndBonus;

	private bool _isGetBonusInfoRunning;

	private float _eventGetBonusInfoStartTime;

	private const float BonusUpdateTimeout = 870f;

	public static ChestBonusController Get { get; private set; }

	public bool IsBonusActive { get; private set; }

	public static event OnChestBonusEnabledDelegate OnChestBonusChange;

	private void Start()
	{
		Get = this;
		_bonusesData = new ChestBonusesData();
		_timeStartBonus = default(DateTime);
		_timeEndBonus = default(DateTime);
		Task firstResponse = PersistentCacheManager.Instance.FirstResponse;
		StartCoroutine(GetEventBonusInfoLoop(firstResponse));
	}

	private void OnDestroy()
	{
		Get = null;
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			StartCoroutine(DownloadDataAboutBonuses());
		}
	}

	private IEnumerator GetEventBonusInfoLoop(Task futureToWait)
	{
		yield return new WaitUntil(() => futureToWait.IsCompleted);
		while (true)
		{
			yield return StartCoroutine(DownloadDataAboutBonuses());
			while (Time.realtimeSinceStartup - _eventGetBonusInfoStartTime < 870f)
			{
				yield return null;
			}
		}
	}

	private void Update()
	{
		if (IsBonusActive && Time.realtimeSinceStartup - _lastCheckEventTime >= 1f)
		{
			IsBonusActive = IsBonusActivate();
			if (_lastBonusActive != IsBonusActive && ChestBonusController.OnChestBonusChange != null)
			{
				ChestBonusController.OnChestBonusChange();
				_lastBonusActive = IsBonusActive;
			}
			_lastCheckEventTime = Time.realtimeSinceStartup;
		}
	}

	private IEnumerator DownloadDataAboutBonuses()
	{
		if (_isGetBonusInfoRunning)
		{
			yield break;
		}
		_eventGetBonusInfoStartTime = Time.realtimeSinceStartup;
		_isGetBonusInfoRunning = true;
		string bonusesDataAddress = ChestBonusModel.GetUrlForDownloadBonusesData();
		string value = PersistentCacheManager.Instance.GetValue(bonusesDataAddress);
		string text;
		if (!string.IsNullOrEmpty(value))
		{
			text = value;
		}
		else
		{
			WWW downloadData = Tools.CreateWwwIfNotConnected(bonusesDataAddress);
			if (downloadData == null)
			{
				_isGetBonusInfoRunning = false;
				yield break;
			}
			yield return downloadData;
			if (!string.IsNullOrEmpty(downloadData.error))
			{
				UnityEngine.Debug.LogError("DownloadDataAboutBonuses error: " + downloadData.error);
				_bonusesData.Clear();
				_isGetBonusInfoRunning = false;
				yield break;
			}
			text = URLs.Sanitize(downloadData);
			PersistentCacheManager.Instance.SetValue(bonusesDataAddress, text);
		}
		Dictionary<string, object> dictionary = Json.Deserialize(text) as Dictionary<string, object>;
		if (dictionary == null)
		{
			UnityEngine.Debug.LogWarning("DownloadDataAboutBonuses bonusesData = null");
			_isGetBonusInfoRunning = false;
			yield break;
		}
		chestBonusesObtainedOnceInCurrentRun = true;
		_bonusesData.Clear();
		if (dictionary.ContainsKey("start"))
		{
			_bonusesData.timeStart = Convert.ToInt32((long)dictionary["start"]);
		}
		if (dictionary.ContainsKey("duration"))
		{
			long val = Math.Min(Convert.ToInt64(dictionary["duration"]), 2147483647L);
			val = Math.Max(val, -2147483648L);
			_bonusesData.duration = Convert.ToInt32(val);
		}
		if (_bonusesData.timeStart == 0 || _bonusesData.duration == 0)
		{
			_isGetBonusInfoRunning = false;
			yield break;
		}
		if (!dictionary.ContainsKey("bonuses"))
		{
			_isGetBonusInfoRunning = false;
			yield break;
		}
		List<object> list = dictionary["bonuses"] as List<object>;
		if (list != null)
		{
			_bonusesData.bonuses = new List<ChestBonusData>();
			for (int i = 0; i < list.Count; i++)
			{
				Dictionary<string, object> dictionary2 = list[i] as Dictionary<string, object>;
				if (dictionary2 == null)
				{
					continue;
				}
				ChestBonusData chestBonusData = new ChestBonusData();
				if (dictionary2.ContainsKey("linkKey"))
				{
					chestBonusData.linkKey = (string)dictionary2["linkKey"];
				}
				if (dictionary2.ContainsKey("isVisible"))
				{
					int num = Convert.ToInt32((long)dictionary2["isVisible"]);
					chestBonusData.isVisible = num == 1;
				}
				if (dictionary2.ContainsKey("items"))
				{
					List<object> list2 = dictionary2["items"] as List<object>;
					if (list2 != null)
					{
						chestBonusData.items = new List<ChestBonusItemData>();
						for (int j = 0; j < list2.Count; j++)
						{
							Dictionary<string, object> dictionary3 = list2[j] as Dictionary<string, object>;
							if (dictionary3 != null)
							{
								ChestBonusItemData chestBonusItemData = new ChestBonusItemData();
								if (dictionary3.ContainsKey("tag"))
								{
									chestBonusItemData.tag = (string)dictionary3["tag"];
								}
								if (dictionary3.ContainsKey("count"))
								{
									chestBonusItemData.count = Convert.ToInt32((long)dictionary3["count"]);
								}
								if (dictionary3.ContainsKey("timeLife"))
								{
									chestBonusItemData.timeLife = Convert.ToInt32((long)dictionary3["timeLife"]);
								}
								chestBonusData.items.Add(chestBonusItemData);
							}
						}
					}
				}
				_bonusesData.bonuses.Add(chestBonusData);
			}
		}
		_timeStartBonus = StarterPackModel.GetCurrentTimeByUnixTime(_bonusesData.timeStart);
		int unixTime = _bonusesData.timeStart + _bonusesData.duration;
		_timeEndBonus = StarterPackModel.GetCurrentTimeByUnixTime(unixTime);
		IsBonusActive = IsBonusActivate();
		if (ChestBonusController.OnChestBonusChange != null)
		{
			ChestBonusController.OnChestBonusChange();
		}
		_isGetBonusInfoRunning = false;
	}

	private bool IsBonusActivate()
	{
		if (_bonusesData.timeStart == 0 || _bonusesData.duration == 0)
		{
			return false;
		}
		DateTime utcNow = DateTime.UtcNow;
		if (utcNow >= _timeStartBonus)
		{
			return utcNow <= _timeEndBonus;
		}
		return false;
	}

	public void ShowBonusWindowForItem(PurchaseEventArgs purchaseInfo)
	{
		ChestBonusData bonusData = GetBonusData(purchaseInfo);
		BankController instance = BankController.Instance;
		if (bonusData != null && instance != null)
		{
			instance.bonusDetailView.Show(bonusData);
		}
	}

	public bool IsBonusActiveForItem(PurchaseEventArgs purchaseInfo)
	{
		if (!IsBonusActive)
		{
			return false;
		}
		ChestBonusData bonusData = GetBonusData(purchaseInfo);
		if (bonusData != null)
		{
			return bonusData.isVisible;
		}
		return false;
	}

	public ChestBonusData GetBonusData(PurchaseEventArgs purchaseInfo)
	{
		bool isGemsPack = purchaseInfo.Type == PurchaseEventArgs.PurchaseType.Gems;
		return GetBonusData(isGemsPack, purchaseInfo.Index);
	}

	private ChestBonusData GetBonusData(bool isGemsPack, int packOrder)
	{
		if (_bonusesData == null || _bonusesData.bonuses == null)
		{
			return null;
		}
		string text = (isGemsPack ? "gems" : "coins");
		string text2 = string.Format("{0}_{1}", new object[2]
		{
			text,
			packOrder + 1
		});
		for (int i = 0; i < _bonusesData.bonuses.Count; i++)
		{
			ChestBonusData chestBonusData = _bonusesData.bonuses[i];
			if (chestBonusData.linkKey == text2)
			{
				return chestBonusData;
			}
		}
		return null;
	}

	public static bool TryTakeChestBonus(bool isGemsPack, int packOrder)
	{
		ChestBonusController get = Get;
		if (get == null)
		{
			return false;
		}
		if (!get.IsBonusActive)
		{
			return false;
		}
		ChestBonusData bonusData = get.GetBonusData(isGemsPack, packOrder);
		if (bonusData == null)
		{
			return false;
		}
		if (bonusData.items == null || bonusData.items.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < bonusData.items.Count; i++)
		{
			ChestBonusItemData chestBonusItemData = bonusData.items[i];
			ShopNGUIController.CategoryNames itemCategory = (ShopNGUIController.CategoryNames)ItemDb.GetItemCategory(chestBonusItemData.tag);
			string text = chestBonusItemData.tag;
			int count = chestBonusItemData.count;
			int timeLife = chestBonusItemData.timeLife;
			int timeForRentIndexForOldTempWeapons = 0;
			if (timeLife != -1)
			{
				timeForRentIndexForOldTempWeapons = TempItemsController.RentIndexFromDays(timeLife / 24);
			}
			ShopNGUIController.ProvideItem(itemCategory, text, count, false, timeForRentIndexForOldTempWeapons, null, delegate(string wearId)
			{
				if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.weaponsInGame != null)
				{
					if (ShopNGUIController.GuiActive && ShopNGUIController.sharedShop != null)
					{
						int itemCategory2 = ItemDb.GetItemCategory(wearId);
						if (itemCategory2 != -1)
						{
							ShopNGUIController.EquipWearInCategoryIfNotEquiped(wearId, (ShopNGUIController.CategoryNames)itemCategory2, WeaponManager.sharedManager != null && WeaponManager.sharedManager.myPlayerMoveC != null);
						}
					}
					else
					{
						int itemCategory3 = ItemDb.GetItemCategory(wearId);
						if (itemCategory3 != -1)
						{
							ShopNGUIController.SetAsEquippedAndSendToServer(wearId, (ShopNGUIController.CategoryNames)itemCategory3);
							if (ShopNGUIController.IsWearCategory((ShopNGUIController.CategoryNames)itemCategory3))
							{
								ShopNGUIController.SendEquippedWearInCategory(wearId, (ShopNGUIController.CategoryNames)itemCategory3, "");
							}
						}
					}
				}
			});
			TempItemsController.sharedController.ExpiredItems.Remove(text);
		}
		return true;
	}
}
