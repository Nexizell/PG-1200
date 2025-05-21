using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class TempItemsController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CStep_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public TempItemsController _003C_003E4__this;

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
		public _003CStep_003Ed__20(int _003C_003E1__state)
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
				return false;
			case 1:
				_003C_003E1__state = -1;
				break;
			case 2:
				_003C_003E1__state = -1;
				_003C_003E4__this.RemoveExpiredItems();
				break;
			}
			if (FriendsController.sharedController == null)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E2__current = _003C_003E4__this.StartCoroutine(FriendsController.sharedController.MyWaitForSeconds(1f));
			_003C_003E1__state = 2;
			return true;
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
	internal sealed class _003CMyWaitForSeconds_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		private float _003CstartTime_003E5__1;

		public float tm;

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
		public _003CMyWaitForSeconds_003Ed__21(int _003C_003E1__state)
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
				if (!(Time.realtimeSinceStartup - _003CstartTime_003E5__1 < tm))
				{
					return false;
				}
			}
			else
			{
				_003C_003E1__state = -1;
				_003CstartTime_003E5__1 = Time.realtimeSinceStartup;
			}
			_003C_003E2__current = null;
			_003C_003E1__state = 1;
			return true;
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

	public static TempItemsController sharedController;

	public List<string> ExpiredItems = new List<string>();

	public static Dictionary<string, List<float>> PriceCoefs;

	public static Dictionary<string, string> GunsMappingFromTempToConst;

	private const long _salt = 1002855644958404316L;

	private const string DurationKey = "Duration";

	private const string StartKey = "Start";

	private const string ExpiredItemsKey = "ExpiredITemptemsControllerKey";

	private Dictionary<string, Dictionary<string, SaltedLong>> Items = new Dictionary<string, Dictionary<string, SaltedLong>>();

	private static List<int> rentTms;

	static TempItemsController()
	{
		PriceCoefs = new Dictionary<string, List<float>>
		{
			{
				WeaponTags.Assault_Machine_Gun_Tag,
				new List<float> { 1f, 2f, 4f }
			},
			{
				WeaponTags.Impulse_Sniper_Rifle_Tag,
				new List<float> { 1f, 2.3333333f, 3.6666667f }
			},
			{
				"Armor_Adamant_3",
				new List<float> { 1f, 2.6666667f, 5.3333335f }
			},
			{
				"hat_Adamant_3",
				new List<float> { 1f, 2.6666667f, 5.3333335f }
			},
			{
				WeaponTags.RailRevolver_1_Tag,
				new List<float> { 1f, 2f, 4f }
			},
			{
				WeaponTags.Autoaim_Rocketlauncher_Tag,
				new List<float> { 1f, 2f, 3.125f }
			},
			{
				WeaponTags.TwoBoltersRent_Tag,
				new List<float> { 1f, 2f, 3.125f }
			},
			{
				WeaponTags.Red_StoneRent_Tag,
				new List<float> { 1f, 2f, 3.125f }
			},
			{
				WeaponTags.DragonGunRent_Tag,
				new List<float> { 1f, 2f, 3.125f }
			},
			{
				WeaponTags.PumpkinGunRent_Tag,
				new List<float> { 1f, 2f, 3.125f }
			},
			{
				WeaponTags.RayMinigunRent_Tag,
				new List<float> { 1f, 2f, 3.125f }
			}
		};
		GunsMappingFromTempToConst = new Dictionary<string, string>();
		rentTms = null;
		GunsMappingFromTempToConst.Add(WeaponTags.Assault_Machine_Gun_Tag, WeaponTags.Assault_Machine_GunBuy_Tag);
		GunsMappingFromTempToConst.Add(WeaponTags.Impulse_Sniper_Rifle_Tag, WeaponTags.Impulse_Sniper_RifleBuy_Tag);
		GunsMappingFromTempToConst.Add(WeaponTags.RailRevolver_1_Tag, WeaponTags.RailRevolverBuy_Tag);
		GunsMappingFromTempToConst.Add(WeaponTags.Autoaim_Rocketlauncher_Tag, WeaponTags.Autoaim_RocketlauncherBuy_Tag);
	}

	public static int RentIndexFromDays(int days)
	{
		int result = 0;
		switch (days)
		{
		case 1:
			result = 0;
			break;
		case 2:
			result = 3;
			break;
		case 3:
			result = 1;
			break;
		case 5:
			result = 4;
			break;
		case 7:
			result = 2;
			break;
		}
		return result;
	}

	public static bool IsCategoryContainsTempItems(ShopNGUIController.CategoryNames cat)
	{
		if (!ShopNGUIController.IsWeaponCategory(cat) && cat != ShopNGUIController.CategoryNames.ArmorCategory)
		{
			return cat == ShopNGUIController.CategoryNames.HatsCategory;
		}
		return true;
	}

	public void AddTemporaryItem(string tg, int tm)
	{
		AddTimeForItem(tg, (tm >= 0) ? tm : 0);
	}

	public static int RentTimeForIndex(int timeForRentIndex)
	{
		if (rentTms == null)
		{
			rentTms = new List<int> { 86400, 259200, 604800, 172800, 432000 };
		}
		int result = 86400;
		if (timeForRentIndex < rentTms.Count && timeForRentIndex >= 0)
		{
			result = rentTms[timeForRentIndex];
		}
		return result;
	}

	public bool CanShowExpiredBannerForTag(string tg)
	{
		return false;
	}

	public long TimeRemainingForItems(string tg)
	{
		return 0L;
	}

	public string TimeRemainingForItemString(string tg)
	{
		return RiliExtensions.GetTimeStringDays(TimeRemainingForItems(tg));
	}

	public void AddTimeForItem(string item, int time)
	{
	}

	public bool ContainsItem(string item)
	{
		return false;
	}

	private static void PrepareKeyForItemsJson()
	{
		if (!Storager.hasKey(Defs.TempItemsDictionaryKey))
		{
			Storager.setString(Defs.TempItemsDictionaryKey, "{}");
		}
	}

	private static bool ItemIsArmorOrHat(string tg)
	{
		int itemCategory = ItemDb.GetItemCategory(tg);
		switch (itemCategory)
		{
		default:
			return itemCategory == 6;
		case 7:
			return true;
		case -1:
			return false;
		}
	}

	private void Awake()
	{
		sharedController = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		DeserializeItems();
		DeserializeExpiredObjects();
		CheckForTimeHack();
		RemoveExpiredItems();
	}

	private void Start()
	{
		StartCoroutine(Step());
	}

	private void RemoveExpiredItems()
	{
	}

	private void RemoveTemporaryItem(string key)
	{
		if (ItemIsArmorOrHat(key))
		{
			Wear.RemoveTemporaryWear(key);
		}
		else
		{
			WeaponManager.sharedManager.RemoveTemporaryItem(key);
		}
	}

	private IEnumerator Step()
	{
		yield break;
	}

	public IEnumerator MyWaitForSeconds(float tm)
	{
		float startTime = Time.realtimeSinceStartup;
		do
		{
			yield return null;
		}
		while (Time.realtimeSinceStartup - startTime < tm);
	}

	private void CheckForTimeHack()
	{
	}

	private static Dictionary<string, Dictionary<string, SaltedLong>> ToSaltedDictionary(Dictionary<string, Dictionary<string, long>> normalDict)
	{
		if (normalDict == null)
		{
			return null;
		}
		Dictionary<string, Dictionary<string, SaltedLong>> dictionary = new Dictionary<string, Dictionary<string, SaltedLong>>();
		foreach (KeyValuePair<string, Dictionary<string, long>> item in normalDict)
		{
			Dictionary<string, SaltedLong> dictionary2 = new Dictionary<string, SaltedLong>();
			if (item.Value != null)
			{
				foreach (KeyValuePair<string, long> item2 in item.Value)
				{
					if (item2.Key != null)
					{
						dictionary2.Add(item2.Key, new SaltedLong(1002855644958404316L, item2.Value));
					}
				}
			}
			dictionary.Add(item.Key, dictionary2);
		}
		return dictionary;
	}

	private static Dictionary<string, Dictionary<string, long>> ToNormalDictionary(Dictionary<string, Dictionary<string, SaltedLong>> saltedDict_)
	{
		if (saltedDict_ == null)
		{
			return null;
		}
		Dictionary<string, Dictionary<string, long>> dictionary = new Dictionary<string, Dictionary<string, long>>();
		foreach (KeyValuePair<string, Dictionary<string, SaltedLong>> item in saltedDict_)
		{
			Dictionary<string, long> dictionary2 = new Dictionary<string, long>();
			if (item.Value != null)
			{
				foreach (KeyValuePair<string, SaltedLong> item2 in item.Value)
				{
					if (item2.Key != null)
					{
						dictionary2.Add(item2.Key, item2.Value.Value);
					}
				}
			}
			dictionary.Add(item.Key, dictionary2);
		}
		return dictionary;
	}

	private void DeserializeItems()
	{
		PrepareKeyForItemsJson();
		object obj = Json.Deserialize(Storager.getString(Defs.TempItemsDictionaryKey));
		if (obj == null)
		{
			UnityEngine.Debug.LogWarning("Error Deserializing temp items JSON");
			return;
		}
		Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
		if (dictionary == null)
		{
			UnityEngine.Debug.LogWarning("Error casting to dict in deserializing temp items JSON");
			return;
		}
		Dictionary<string, Dictionary<string, long>> dictionary2 = new Dictionary<string, Dictionary<string, long>>();
		foreach (KeyValuePair<string, object> item in dictionary)
		{
			if (item.Value == null)
			{
				UnityEngine.Debug.LogWarning("Error kvp.Value == null kvp.Key = " + item.Key + " in deserializing temp items JSON");
				continue;
			}
			Dictionary<string, object> dictionary3 = item.Value as Dictionary<string, object>;
			object value;
			if (dictionary3 == null)
			{
				UnityEngine.Debug.LogWarning("Error innerDict == null kvp.Key = " + item.Key + " in deserializing temp items JSON");
			}
			else if (dictionary3.TryGetValue("Duration", out value) && value != null)
			{
				long value2;
				try
				{
					value2 = (long)value;
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogWarning("Error unboxing DurationValue in deserializing temp items JSON: " + ex.Message);
					continue;
				}
				object value3;
				if (dictionary3.TryGetValue("Start", out value3) && value3 != null)
				{
					long value4;
					try
					{
						value4 = (long)value3;
					}
					catch (Exception ex2)
					{
						UnityEngine.Debug.LogWarning("Error unboxing StartValue in deserializing temp items JSON: " + ex2.Message);
						continue;
					}
					dictionary2.Add(item.Key, new Dictionary<string, long>
					{
						{ "Start", value4 },
						{ "Duration", value2 }
					});
				}
				else
				{
					UnityEngine.Debug.LogWarning(" ! (innerDict.TryGetValue(StartKey,out StartValueObj) && StartValueObj != null) in deserializing temp items JSON");
				}
			}
			else
			{
				UnityEngine.Debug.LogWarning(" ! (innerDict.TryGetValue(DurationKey,out DurationValueObj) && DurationValueObj != null) in deserializing temp items JSON");
			}
		}
		Items = ToSaltedDictionary(dictionary2);
	}

	private void SerializeItems()
	{
		Dictionary<string, Dictionary<string, long>> obj = ToNormalDictionary(Items ?? new Dictionary<string, Dictionary<string, SaltedLong>>());
		Storager.setString(Defs.TempItemsDictionaryKey, Json.Serialize(obj));
	}

	private void DeserializeExpiredObjects()
	{
		if (!Storager.hasKey("ExpiredITemptemsControllerKey"))
		{
			Storager.setString("ExpiredITemptemsControllerKey", "[]");
		}
		object obj = Json.Deserialize(Storager.getString("ExpiredITemptemsControllerKey"));
		if (obj == null)
		{
			UnityEngine.Debug.LogWarning("Error Deserializing expired items JSON");
			return;
		}
		List<object> list = obj as List<object>;
		if (list == null)
		{
			UnityEngine.Debug.LogWarning("Error casting expired items obj to list");
			return;
		}
		try
		{
			ExpiredItems.Clear();
			foreach (string item in list)
			{
				ExpiredItems.Add(item);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogWarning("Exception when iterating expired items list: " + ex);
		}
	}

	private void SerializeExpiredItems()
	{
		Storager.setString("ExpiredITemptemsControllerKey", Json.Serialize(ExpiredItems));
	}

	private static long GetLastSuspendTime()
	{
		return PromoActionsManager.GetUnixTimeFromStorage(Defs.LastTimeTempItemsSuspended);
	}

	private static void SaveSuspendTime()
	{
		Storager.setString(Defs.LastTimeTempItemsSuspended, PromoActionsManager.CurrentUnixTime.ToString());
	}

	private void OnDestroy()
	{
	}

	public void TakeTemporaryItemToPlayer(ShopNGUIController.CategoryNames categoryName, string tag, int indexTimeLife)
	{
		ExpiredItems.Remove(tag);
	}
}
