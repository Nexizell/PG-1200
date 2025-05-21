using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public class BalanceController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003C_003Ec__DisplayClass100_0
	{
		public Task futureToWait;

		internal bool _003CGetBalansFromServer_003Eb__0()
		{
			return futureToWait.IsCompleted;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CGetBalansFromServer_003Ed__100 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BalanceController _003C_003E4__this;

		private WWW _003Cdownload_003E5__1;

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
		public _003CGetBalansFromServer_003Ed__100(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			_003C_003Ec__DisplayClass100_0 CS_0024_003C_003E8__locals0;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_002a;
			case 1:
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(PersistentCacheManager.Instance.GetValue(balanceURL)))
				{
					return false;
				}
				goto IL_00b2;
			case 2:
				_003C_003E1__state = -1;
				goto IL_002a;
			case 3:
			{
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(_003Cdownload_003E5__1.error))
				{
					if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
					{
						UnityEngine.Debug.LogWarning("GetBalans error: " + _003Cdownload_003E5__1.error);
					}
					_003C_003E2__current = new WaitForRealSeconds(30f);
					_003C_003E1__state = 4;
					return true;
				}
				string value = URLs.Sanitize(_003Cdownload_003E5__1);
				if (!string.IsNullOrEmpty(value))
				{
					using (new ScopeLogger("GetBalansFromServer()", "Saving to storager", Defs.IsDeveloperBuild))
					{
						_003C_003E4__this._encryptedPlayerPrefs.SetString(balanceKey, value);
					}
					using (new ScopeLogger("GetBalansFromServer()", "Saving to cache", Defs.IsDeveloperBuild))
					{
						PersistentCacheManager.Instance.SetValue(balanceURL, value);
					}
					_003C_003E4__this.ParseConfig();
					if (UnityEngine.Debug.isDebugBuild)
					{
						UnityEngine.Debug.Log("GetConfigABtestBalans");
					}
					return false;
				}
				_003Cdownload_003E5__1 = null;
				goto IL_002a;
			}
			case 4:
				{
					_003C_003E1__state = -1;
					goto IL_002a;
				}
				IL_00b2:
				new WWWForm();
				_003Cdownload_003E5__1 = Tools.CreateWwwIfNotConnected(balanceURL);
				if (_003Cdownload_003E5__1 == null)
				{
					_003C_003E2__current = new WaitForRealSeconds(30f);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = _003Cdownload_003E5__1;
				_003C_003E1__state = 3;
				return true;
				IL_002a:
				CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass100_0
				{
					futureToWait = PersistentCacheManager.Instance.FirstResponse
				};
				if (_003C_003E4__this._encryptedPlayerPrefs.HasKey(balanceKey) || !string.IsNullOrEmpty(_003C_003E4__this._encryptedPlayerPrefs.GetString(balanceKey)))
				{
					_003C_003E2__current = new WaitUntil(() => CS_0024_003C_003E8__locals0.futureToWait.IsCompleted);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00b2;
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
	internal sealed class _003C_003Ec__DisplayClass116_0
	{
		public Task futureToWait;

		internal bool _003CGetActionsFromServer_003Eb__0()
		{
			return futureToWait.IsCompleted;
		}
	}

	[CompilerGenerated]
	internal sealed class _003CGetActionsFromServer_003Ed__116 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public BalanceController _003C_003E4__this;

		private WWW _003Cdownload_003E5__1;

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
		public _003CGetActionsFromServer_003Ed__116(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			_003C_003Ec__DisplayClass116_0 CS_0024_003C_003E8__locals0;
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				goto IL_002e;
			case 1:
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(PersistentCacheManager.Instance.GetValue(inappsActionsURL)))
				{
					return false;
				}
				goto IL_00b6;
			case 2:
				_003C_003E1__state = -1;
				goto IL_002e;
			case 3:
			{
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(_003Cdownload_003E5__1.error))
				{
					if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
					{
						UnityEngine.Debug.LogWarning("GetInappsBonuses error: " + _003Cdownload_003E5__1.error);
					}
					_003C_003E2__current = new WaitForRealSeconds(30f);
					_003C_003E1__state = 4;
					return true;
				}
				string value = URLs.Sanitize(_003Cdownload_003E5__1);
				if (!string.IsNullOrEmpty(value))
				{
					using (new ScopeLogger("GetInappsBonuses()", "Saving to storager", Defs.IsDeveloperBuild))
					{
						_003C_003E4__this._encryptedPlayerPrefs.SetString(inappsActionsCacheKey, value);
					}
					using (new ScopeLogger("GetInappsBonuses()", "Saving to cache", Defs.IsDeveloperBuild))
					{
						PersistentCacheManager.Instance.SetValue(inappsActionsURL, value);
					}
					_003C_003E4__this.ParseConfigInappBonuses();
					if (UnityEngine.Debug.isDebugBuild)
					{
						UnityEngine.Debug.Log("GetConfigABtestBalans");
					}
					return false;
				}
				_003C_003E2__current = new WaitForRealSeconds(300f);
				_003C_003E1__state = 5;
				return true;
			}
			case 4:
				_003C_003E1__state = -1;
				goto IL_002e;
			case 5:
				{
					_003C_003E1__state = -1;
					_003Cdownload_003E5__1 = null;
					goto IL_002e;
				}
				IL_00b6:
				new WWWForm();
				_003Cdownload_003E5__1 = Tools.CreateWwwIfNotConnected(inappsActionsURL);
				if (_003Cdownload_003E5__1 == null)
				{
					_003C_003E2__current = new WaitForRealSeconds(30f);
					_003C_003E1__state = 2;
					return true;
				}
				_003C_003E2__current = _003Cdownload_003E5__1;
				_003C_003E1__state = 3;
				return true;
				IL_002e:
				CS_0024_003C_003E8__locals0 = new _003C_003Ec__DisplayClass116_0
				{
					futureToWait = PersistentCacheManager.Instance.FirstResponse
				};
				if (_003C_003E4__this._encryptedPlayerPrefs.HasKey(inappsActionsCacheKey) || !string.IsNullOrEmpty(_003C_003E4__this._encryptedPlayerPrefs.GetString(inappsActionsCacheKey)))
				{
					_003C_003E2__current = new WaitUntil(() => CS_0024_003C_003E8__locals0.futureToWait.IsCompleted);
					_003C_003E1__state = 1;
					return true;
				}
				goto IL_00b6;
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

	public const string SHOWN_ACTION_IDS_KEY = "BalanceController.SHOWN_ACTION_IDS_KEY";

	public static string curencyActionName = "currence";

	public static string leprechaunActionName = "leprechaun";

	public static string petActionName = "pet";

	public static string weaponActionName = "weapon";

	public static string gadgetActionName = "gadget";

	public static List<string> supportedInappBonusIds = new List<string> { curencyActionName, petActionName, weaponActionName, gadgetActionName, leprechaunActionName };

	[HideInInspector]
	public string jsonConfig;

	public static BalanceController sharedController = null;

	[SerializeField]
	protected internal string encryptedPlayerprefsKey;

	private static SaltedInt _FreeTicketsPerPack = new SaltedInt(SaltedInt._prng.Next(), 1);

	private static SaltedInt _MaxFreeTickets = new SaltedInt(SaltedInt._prng.Next(), 30);

	private static SaltedInt _TimeGiveFreeTickets = new SaltedInt(SaltedInt._prng.Next(), 14400);

	private static SaltedInt _lobbyItemsLevelRequired = new SaltedInt(SaltedInt._prng.Next(), 5);

	private static SaltedInt unlockMiniGames = new SaltedInt(SaltedInt._prng.Next(), 3);

	public static Dictionary<GameConnect.GameMode, int> levelRequired = new Dictionary<GameConnect.GameMode, int>();

	public static Dictionary<string, int> expBoxAwards = new Dictionary<string, int>();

	public static Dictionary<string, int> coinsBoxAwards = new Dictionary<string, int>();

	public static Dictionary<string, int> gemsBoxAwards = new Dictionary<string, int>();

	private static Dictionary<GameConnect.GameMode, MinigameParameters> _minigameParameters = new Dictionary<GameConnect.GameMode, MinigameParameters>();

	private static SaltedInt _box2Tickets = new SaltedInt(SaltedInt._prng.Next(), 50);

	private static SaltedInt _box3Tickets = new SaltedInt(SaltedInt._prng.Next(), 150);

	private static SaltedInt _CountWelcomeTickets = new SaltedInt(SaltedInt._prng.Next(), 10);

	public static readonly string balanceKey = "balanceKey";

	public static readonly string inappsActionsCacheKey = "inappsActionsCacheKey";

	public static Dictionary<string, float[]> dpsWeapons = new Dictionary<string, float[]>();

	public static Dictionary<string, float[]> damageWeapons = new Dictionary<string, float[]>();

	public static Dictionary<string, int> survivalDamageWeapons = new Dictionary<string, int>();

	private static Dictionary<string, List<ItemPrice>> _gunPricesFromServerNew = new Dictionary<string, List<ItemPrice>>();

	public static Dictionary<string, float> damageGadgetes = new Dictionary<string, float>();

	public static Dictionary<string, float> amplificationGadgetes = new Dictionary<string, float>();

	public static Dictionary<string, float> dpsGadgetes = new Dictionary<string, float>();

	public static Dictionary<string, int> survivalDamageGadgetes = new Dictionary<string, int>();

	public static Dictionary<string, float> cooldownGadgetes = new Dictionary<string, float>();

	public static Dictionary<string, float> durationGadgetes = new Dictionary<string, float>();

	public static Dictionary<string, float> durabilityGadgetes = new Dictionary<string, float>();

	public static Dictionary<string, float> healGadgetes = new Dictionary<string, float>();

	public static Dictionary<string, float> hpsGadgetes = new Dictionary<string, float>();

	public static Dictionary<string, float> damagePets = new Dictionary<string, float>();

	public static Dictionary<string, int> dpsPets = new Dictionary<string, int>();

	public static Dictionary<string, int> survivalDamagePets = new Dictionary<string, int>();

	public static Dictionary<string, float> respawnTimePets = new Dictionary<string, float>();

	public static Dictionary<string, float> speedPets = new Dictionary<string, float>();

	public static Dictionary<string, int> cashbackPets = new Dictionary<string, int>();

	public static Dictionary<string, float> hpPets = new Dictionary<string, float>();

	public static Dictionary<string, int> timeEggs = new Dictionary<string, int>();

	public static Dictionary<string, int> victoriasEggs = new Dictionary<string, int>();

	public static Dictionary<string, int> ratingEggs = new Dictionary<string, int>();

	public static Dictionary<string, List<EggPetInfo>> rarityPetsInEggs = new Dictionary<string, List<EggPetInfo>>();

	public static Dictionary<string, int> timeLobbyItems = new Dictionary<string, int>();

	public static Dictionary<string, int> xpLobbyItems = new Dictionary<string, int>();

	public static Dictionary<string, ItemPrice> priceLobbyItems = new Dictionary<string, ItemPrice>();

	public static Dictionary<string, ItemPrice> priceTimeLobbyItems = new Dictionary<string, ItemPrice>();

	public static Dictionary<string, ItemPrice> priceFullLobbyItems = new Dictionary<string, ItemPrice>();

	public static Dictionary<string, int> levelLobbyItems = new Dictionary<string, int>();

	public static List<Dictionary<string, object>> inappsBonus = new List<Dictionary<string, object>>();

	public static Dictionary<string, object> inappsBonusSegments = new Dictionary<string, object>();

	private static Dictionary<string, List<ItemPrice>> _gadgetPricesFromServerNew = new Dictionary<string, List<ItemPrice>>();

	public static int startCapitalCoins = 0;

	public static int startCapitalGems = 0;

	public static bool startCapitalEnabled = false;

	public static ItemPrice competitionAward = new ItemPrice(0, "Coins");

	public static int countPlaceAwardInCompetion = 0;

	private static Dictionary<string, ItemPrice> _pricesFromServer = new Dictionary<string, ItemPrice>();

	private static string _inappObj = null;

	private static string _ticketsObj = null;

	private static string _inappObjBonus = null;

	private static List<string> _curentGadgetesIDs = null;

	private static List<string> _keysInappBonusActionGiven = null;

	private static List<Dictionary<string, object>> cacheCurrentInnapBonus = null;

	private static int countFrameInCache = -1;

	private static Dictionary<string, int> curPackDict = new Dictionary<string, int>();

	private static Dictionary<string, DateTime> timeNextUpdateDict = new Dictionary<string, DateTime>();

	private EncryptedPlayerPrefs _encryptedPlayerPrefs;

	private string EncryptedPlayerprefsKey
	{
		get
		{
			return encryptedPlayerprefsKey ?? string.Empty;
		}
	}

	public static int FreeTicketsPerPack
	{
		get
		{
			return _FreeTicketsPerPack.Value;
		}
	}

	public static int MaxFreeTickets
	{
		get
		{
			return _MaxFreeTickets.Value;
		}
	}

	public static int TimeGiveFreeTickets
	{
		get
		{
			return _TimeGiveFreeTickets.Value;
		}
	}

	public static int LobbyItemsLevelRequired
	{
		get
		{
			return _lobbyItemsLevelRequired.Value;
		}
	}

	public static int MiniGamesLevelRequired
	{
		get
		{
			return unlockMiniGames.Value;
		}
	}

	public static int CountWelcomeTickets
	{
		get
		{
			return _CountWelcomeTickets.Value;
		}
	}

	public static string balanceURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/balance/balance_test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/balance/balance_androd.json";
				}
				return "https://secure.pixelgunserver.com/pixelgun3d-config/balance/balance_amazon.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/balance/balance_wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/balance/balance_ios.json";
		}
	}

	public static string inappsActionsURL
	{
		get
		{
			if (Defs.IsDeveloperBuild)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/BonusInnappsWithGroup/test.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
			{
				if (Defs.AndroidEdition != Defs.RuntimeAndroidEdition.Amazon)
				{
					return "https://secure.pixelgunserver.com/pixelgun3d-config/BonusInnappsWithGroup/android.json";
				}
				return "https://secure.pixelgunserver.com/pixelgun3d-config/BonusInnappsWithGroup/amazon.json";
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64)
			{
				return "https://secure.pixelgunserver.com/pixelgun3d-config/BonusInnappsWithGroup/wp.json";
			}
			return "https://secure.pixelgunserver.com/pixelgun3d-config/BonusInnappsWithGroup/ios.json";
		}
	}

	public static Dictionary<string, List<ItemPrice>> GunPricesFromServer
	{
		get
		{
			return _gunPricesFromServerNew;
		}
		set
		{
			_gunPricesFromServerNew = value;
		}
	}

	public static Dictionary<string, List<ItemPrice>> GadgetPricesFromServer
	{
		get
		{
			return _gadgetPricesFromServerNew;
		}
		set
		{
			_gadgetPricesFromServerNew = value;
		}
	}

	public static Dictionary<string, ItemPrice> pricesFromServer
	{
		get
		{
			return _pricesFromServer;
		}
		set
		{
			_pricesFromServer = value;
		}
	}

	private static List<string> curentGadgetesIDs
	{
		get
		{
			if (_curentGadgetesIDs == null)
			{
				try
				{
					string @string = Storager.getString(Defs.keyInappPresentIDGadgetkey);
					if (!string.IsNullOrEmpty(@string))
					{
						List<object> list = Json.Deserialize(@string) as List<object>;
						if (list != null && list.Count == 3)
						{
							_curentGadgetesIDs = new List<string>();
							for (int i = 0; i < list.Count; i++)
							{
								_curentGadgetesIDs.Add(list[i].ToString());
							}
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.Log("Parse curentGadgetesIDs: " + ex);
					return null;
				}
			}
			return _curentGadgetesIDs;
		}
		set
		{
			_curentGadgetesIDs = value;
			string val = Json.Serialize(_curentGadgetesIDs);
			Storager.setString(Defs.keyInappPresentIDGadgetkey, val);
		}
	}

	public static List<string> keysInappBonusActionGiven
	{
		get
		{
			if (_keysInappBonusActionGiven == null)
			{
				_keysInappBonusActionGiven = new List<string>();
				string @string = Storager.getString(Defs.keysInappBonusGivenkey);
				List<object> list = null;
				if (!string.IsNullOrEmpty(@string))
				{
					list = Json.Deserialize(@string) as List<object>;
				}
				if (list != null)
				{
					foreach (object item in list)
					{
						_keysInappBonusActionGiven.Add(item.ToString());
					}
				}
			}
			return _keysInappBonusActionGiven;
		}
	}

	public static event Action UpdatedBankView;

	internal static int GetUnlockLevel(GameConnect.GameMode gameMode)
	{
		int value;
		if (levelRequired.TryGetValue(gameMode, out value))
		{
			return value;
		}
		return int.MaxValue;
	}

	public static MinigameParameters ParametersForMiniGameType(GameConnect.GameMode typeOfMiniGame)
	{
		if (_minigameParameters.ContainsKey(typeOfMiniGame))
		{
			return _minigameParameters[typeOfMiniGame];
		}
		return new MinigameParameters(4, 3);
	}

	public static int UnlockCampaignBoxPrice(int boxIndex)
	{
		switch (boxIndex)
		{
		case 1:
			return _box2Tickets.Value;
		case 2:
			return _box3Tickets.Value;
		default:
			UnityEngine.Debug.LogErrorFormat("UnlockCampaignBoxPrice: incorrect box index {0}", boxIndex);
			return 0;
		}
	}

	private void Awake()
	{
		byte[] masterKey = Convert.FromBase64String(EncryptedPlayerprefsKey);
		_encryptedPlayerPrefs = new EncryptedPlayerPrefs(masterKey);
	}

	private void Start()
	{
		sharedController = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		ParseConfig();
		ParseConfigInappBonuses();
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 || Application.isEditor)
		{
			StartCoroutine("GetBalansFromServer");
			StartCoroutine("GetActionsFromServer");
		}
	}

	private void UpdateBalansFromServer()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64 || Application.isEditor)
		{
			StopCoroutine("GetBalansFromServer");
			StartCoroutine("GetBalansFromServer");
			StopCoroutine("GetActionsFromServer");
			StartCoroutine("GetActionsFromServer");
		}
	}

	private IEnumerator GetBalansFromServer()
	{
		string value;
		while (true)
		{
			Task futureToWait = PersistentCacheManager.Instance.FirstResponse;
			if (_encryptedPlayerPrefs.HasKey(balanceKey) || !string.IsNullOrEmpty(_encryptedPlayerPrefs.GetString(balanceKey)))
			{
				yield return new WaitUntil(() => futureToWait.IsCompleted);
				if (!string.IsNullOrEmpty(PersistentCacheManager.Instance.GetValue(balanceURL)))
				{
					yield break;
				}
			}
			new WWWForm();
			WWW download = Tools.CreateWwwIfNotConnected(balanceURL);
			if (download == null)
			{
				yield return new WaitForRealSeconds(30f);
				continue;
			}
			yield return download;
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("GetBalans error: " + download.error);
				}
				yield return new WaitForRealSeconds(30f);
			}
			else
			{
				value = URLs.Sanitize(download);
				if (!string.IsNullOrEmpty(value))
				{
					break;
				}
			}
		}
		using (new ScopeLogger("GetBalansFromServer()", "Saving to storager", Defs.IsDeveloperBuild))
		{
			_encryptedPlayerPrefs.SetString(balanceKey, value);
		}
		using (new ScopeLogger("GetBalansFromServer()", "Saving to cache", Defs.IsDeveloperBuild))
		{
			PersistentCacheManager.Instance.SetValue(balanceURL, value);
		}
		ParseConfig();
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("GetConfigABtestBalans");
		}
	}

	public void ParseConfig(bool isFirstParse = false)
	{
		Dictionary<string, object> dictionary = null;
		string @string = Storager.getString("abTestBalansConfig2Key");
		if (!string.IsNullOrEmpty(@string))
		{
			dictionary = Json.Deserialize(@string) as Dictionary<string, object>;
			if (dictionary == null)
			{
				Storager.setString("abTestBalansConfig2Key", string.Empty);
				UnityEngine.Debug.LogError("AB TEST BALANCE CONFIG NOT CORRECT !!!");
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("jsonConfigABTest = ' " + @string + "'");
				}
				return;
			}
			if (dictionary.ContainsKey("NameConfig"))
			{
				ParseABTestBalansNameConfig(dictionary["NameConfig"], isFirstParse);
			}
			if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.A)
			{
				if (_encryptedPlayerPrefs.HasKey(balanceKey) || !string.IsNullOrEmpty(_encryptedPlayerPrefs.GetString(balanceKey)))
				{
					jsonConfig = _encryptedPlayerPrefs.GetString(balanceKey);
				}
				dictionary = Json.Deserialize(jsonConfig) as Dictionary<string, object>;
			}
		}
		else
		{
			if (Defs.abTestBalansCohort != 0)
			{
				AnalyticsStuff.LogABTest("New Balance", Defs.abTestBalansCohortName, false);
				Defs.abTestBalansCohort = Defs.ABTestCohortsType.NONE;
				Defs.abTestBalansCohortName = string.Empty;
				if (FriendsController.sharedController != null)
				{
					FriendsController.sharedController.SendOurData();
				}
			}
			if (_encryptedPlayerPrefs.HasKey(balanceKey) || !string.IsNullOrEmpty(_encryptedPlayerPrefs.GetString(balanceKey)))
			{
				jsonConfig = _encryptedPlayerPrefs.GetString(balanceKey);
			}
			if (string.IsNullOrEmpty(jsonConfig))
			{
				UnityEngine.Debug.LogError("BALANCE CONFIG EMPTY !!!");
				return;
			}
			try
			{
				dictionary = Json.Deserialize(jsonConfig) as Dictionary<string, object>;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.LogError("Balans Controller Error parse config: " + ex.Message);
			}
		}
		if (dictionary == null)
		{
			UnityEngine.Debug.LogError("BALANCE CONFIG NOT CORRECT !!!");
			return;
		}
		pricesFromServer.Clear();
		if (dictionary.ContainsKey("Weapons"))
		{
			ParseWeaponsConfig(dictionary["Weapons"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("Gadgets"))
		{
			ParseGadgetsConfig(dictionary["Gadgets"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("Pets"))
		{
			ParsePetsConfig(dictionary["Pets"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("Eggs"))
		{
			ParseEggsConfig(dictionary["Eggs"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("LobbyItems"))
		{
			ParseLobbyItemsConfig(dictionary["LobbyItems"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("ItemPrices"))
		{
			ParseItemPricesConfig(dictionary["ItemPrices"] as Dictionary<string, object>);
		}
		if (dictionary.ContainsKey("Levelling"))
		{
			ParseLevelingConfig(dictionary["Levelling"]);
		}
		bool flag = false;
		if (dictionary.ContainsKey("Inapps"))
		{
			flag = ParseInappsConfig(dictionary["Inapps"]);
		}
		if (flag && BalanceController.UpdatedBankView != null)
		{
			BalanceController.UpdatedBankView();
		}
		if (dictionary.ContainsKey("Tickets"))
		{
			ParseTicketsConfig(dictionary["Tickets"]);
		}
		if (dictionary.ContainsKey("RewardsWithMini"))
		{
			ParseAwardConfig(dictionary["RewardsWithMini"]);
		}
		if (dictionary.ContainsKey("Levelling"))
		{
			ParseLevelingConfig(dictionary["Levelling"]);
		}
		if (dictionary.ContainsKey("SpecialEvents"))
		{
			ParseSpecialEventsConfig(dictionary["SpecialEvents"]);
		}
	}

	private static void ParseABTestBalansNameConfig(object obj, bool isFirstParse)
	{
		Dictionary<string, object> dictionary = (obj as List<object>)[0] as Dictionary<string, object>;
		if (dictionary.ContainsKey("Group"))
		{
			Defs.abTestBalansCohort = (Defs.ABTestCohortsType)Enum.Parse(typeof(Defs.ABTestCohortsType), dictionary["Group"] as string);
			if (Defs.abTestBalansCohort == Defs.ABTestCohortsType.B)
			{
				Defs.isABTestBalansCohortActual = true;
			}
			if (Application.isEditor)
			{
				UnityEngine.Debug.Log("Defs.abTestBalansCohort = " + Defs.abTestBalansCohort);
			}
		}
		if (!dictionary.ContainsKey("NameGroup"))
		{
			return;
		}
		Defs.abTestBalansCohortName = dictionary["NameGroup"] as string;
		if (isFirstParse)
		{
			AnalyticsStuff.LogABTest("New Balance", Defs.abTestBalansCohortName);
			if (FriendsController.sharedController != null)
			{
				FriendsController.sharedController.SendOurData();
			}
		}
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("abTestBalansCohortName = " + Defs.abTestBalansCohortName.ToString());
		}
	}

	private void ParseWeaponsConfig(Dictionary<string, object> _weaponsConfig)
	{
		dpsWeapons.Clear();
		damageWeapons.Clear();
		survivalDamageWeapons.Clear();
		GunPricesFromServer.Clear();
		foreach (KeyValuePair<string, object> item in _weaponsConfig)
		{
			if (string.IsNullOrEmpty(item.Key))
			{
				continue;
			}
			Dictionary<string, object> dictionary = item.Value as Dictionary<string, object>;
			int num = 1;
			if (dictionary.ContainsKey("D1"))
			{
				float[] array = new float[6];
				for (int i = 2; i <= 6; i++)
				{
					if (dictionary.ContainsKey("D" + i))
					{
						num = i;
					}
				}
				float num2 = 0.1f;
				float num3 = 0.1f;
				float result;
				if (float.TryParse(dictionary["D1"].ToString(), out result))
				{
					num2 = result;
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseWeaponsConfig: not parse " + item.Key + " parametr D1");
				}
				if (num > 1)
				{
					float result2;
					if (float.TryParse(dictionary["D" + num].ToString(), out result2))
					{
						num3 = result2;
					}
					else if (Application.isEditor)
					{
						UnityEngine.Debug.LogError("ParseWeaponsConfig: not parse " + item.Key + " parametr D" + num);
					}
				}
				else
				{
					num3 = num2;
				}
				for (int j = 1; j <= 6; j++)
				{
					array[j - 1] = ((j < num) ? (num2 + (num3 - num2) / (float)(num - 1) * (float)(j - 1)) : num3);
				}
				dpsWeapons.Add("Weapon" + item.Key, array);
			}
			if (dictionary.ContainsKey("U1"))
			{
				float[] array2 = new float[6];
				float num4 = 0.1f;
				float num5 = 0.1f;
				float result3;
				if (float.TryParse(dictionary["U1"].ToString(), out result3))
				{
					num4 = result3;
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseWeaponsConfig: not parse " + item.Key + " parametr U1");
				}
				if (num > 1)
				{
					float result4;
					if (float.TryParse(dictionary["U" + num].ToString(), out result4))
					{
						num5 = result4;
					}
					else if (Application.isEditor)
					{
						UnityEngine.Debug.LogError("ParseWeaponsConfig: not parse " + item.Key + " parametr U" + num);
					}
				}
				else
				{
					num5 = num4;
				}
				for (int k = 1; k <= 6; k++)
				{
					array2[k - 1] = ((k < num) ? (num4 + (num5 - num4) / (float)(num - 1) * (float)(k - 1)) : num5);
				}
				damageWeapons.Add("Weapon" + item.Key, array2);
			}
			if (dictionary.ContainsKey("S"))
			{
				int result5;
				if (int.TryParse(dictionary["S"].ToString(), out result5))
				{
					survivalDamageWeapons.Add(item.Key, result5);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseWeaponsConfig: not parse " + item.Key + " parametr Survival_damage ");
				}
			}
			if (dictionary.ContainsKey("P"))
			{
				string text = dictionary["P"].ToString();
				int num6 = Convert.ToInt32(text.Substring(1));
				int price = (dictionary.ContainsKey("oP") ? Convert.ToInt32(dictionary["oP"]) : num6);
				string currency = (text.Substring(0, 1).Equals("C") ? "Coins" : "GemsCurrency");
				GunPricesFromServer.Add("Weapon" + item.Key, new List<ItemPrice>
				{
					new ItemPrice(price, currency),
					new ItemPrice(num6, currency)
				});
			}
		}
	}

	private void ParseGadgetsConfig(Dictionary<string, object> _gadgetsConfig)
	{
		GadgetPricesFromServer.Clear();
		damageGadgetes.Clear();
		dpsGadgetes.Clear();
		survivalDamageGadgetes.Clear();
		cooldownGadgetes.Clear();
		durationGadgetes.Clear();
		durabilityGadgetes.Clear();
		healGadgetes.Clear();
		hpsGadgetes.Clear();
		foreach (KeyValuePair<string, object> item in _gadgetsConfig)
		{
			if (string.IsNullOrEmpty(item.Key))
			{
				continue;
			}
			Dictionary<string, object> dictionary = item.Value as Dictionary<string, object>;
			string key = "gadget_" + item.Key;
			string text = "CD";
			if (dictionary.ContainsKey(text))
			{
				float result;
				if (float.TryParse(dictionary[text].ToString(), out result))
				{
					cooldownGadgetes.Add(key, result);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseGadgetConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "T";
			if (dictionary.ContainsKey(text))
			{
				float result2;
				if (float.TryParse(dictionary[text].ToString(), out result2))
				{
					durationGadgetes.Add(key, result2);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseGadgetConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "D";
			if (dictionary.ContainsKey(text))
			{
				float result3;
				if (float.TryParse(dictionary[text].ToString(), out result3))
				{
					damageGadgetes.Add(key, result3);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseGadgetConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "DPS";
			if (dictionary.ContainsKey(text))
			{
				float result4;
				if (float.TryParse(dictionary[text].ToString(), out result4))
				{
					dpsGadgetes.Add(key, result4);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseGadgetConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "SD";
			if (dictionary.ContainsKey(text))
			{
				int result5;
				if (int.TryParse(dictionary[text].ToString(), out result5))
				{
					survivalDamageGadgetes.Add(key, result5);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseGadgetConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "Dur";
			if (dictionary.ContainsKey(text))
			{
				float result6;
				if (float.TryParse(dictionary[text].ToString(), out result6))
				{
					durabilityGadgetes.Add(key, result6);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseGadgetConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "H";
			if (dictionary.ContainsKey(text))
			{
				float result7;
				if (float.TryParse(dictionary[text].ToString(), out result7))
				{
					healGadgetes.Add(key, result7);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseGadgetConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "HPS";
			if (dictionary.ContainsKey(text))
			{
				float result8;
				if (float.TryParse(dictionary[text].ToString(), out result8))
				{
					hpsGadgetes.Add(key, result8);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseGadgetConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "P";
			if (!dictionary.ContainsKey(text))
			{
				continue;
			}
			string text2 = dictionary[text].ToString();
			int num = Convert.ToInt32(text2.Substring(1));
			string key2 = "oP";
			int result9;
			if (dictionary.ContainsKey(key2))
			{
				if (!int.TryParse(dictionary[key2].ToString(), out result9))
				{
					result9 = num;
					if (Application.isEditor)
					{
						UnityEngine.Debug.LogError("ParseGadgetConfig: not parse " + item.Key + " parametr " + text);
					}
				}
			}
			else
			{
				result9 = num;
			}
			string currency = (text2.Substring(0, 1).Equals("C") ? "Coins" : "GemsCurrency");
			GadgetPricesFromServer.Add(key, new List<ItemPrice>
			{
				new ItemPrice(result9, currency),
				new ItemPrice(num, currency)
			});
		}
	}

	private void ParsePetsConfig(Dictionary<string, object> _petsConfig)
	{
		damagePets.Clear();
		dpsPets.Clear();
		survivalDamagePets.Clear();
		respawnTimePets.Clear();
		hpPets.Clear();
		speedPets.Clear();
		cashbackPets.Clear();
		foreach (KeyValuePair<string, object> item in _petsConfig)
		{
			if (string.IsNullOrEmpty(item.Key))
			{
				continue;
			}
			Dictionary<string, object> dictionary = item.Value as Dictionary<string, object>;
			string key = "pet_" + item.Key;
			string text = "R";
			if (dictionary.ContainsKey(text))
			{
				float result;
				if (float.TryParse(dictionary[text].ToString(), out result))
				{
					respawnTimePets.Add(key, result);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParsePetsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "HP";
			if (dictionary.ContainsKey(text))
			{
				float result2;
				if (float.TryParse(dictionary[text].ToString(), out result2))
				{
					hpPets.Add(key, result2);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParsePetsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "DPS";
			if (dictionary.ContainsKey(text))
			{
				int result3;
				if (int.TryParse(dictionary[text].ToString(), out result3))
				{
					dpsPets.Add(key, result3);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParsePetsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "D";
			if (dictionary.ContainsKey(text))
			{
				float result4;
				if (float.TryParse(dictionary[text].ToString(), out result4))
				{
					damagePets.Add(key, result4);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParsePetsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "SD";
			if (dictionary.ContainsKey(text))
			{
				int result5;
				if (int.TryParse(dictionary[text].ToString(), out result5))
				{
					survivalDamagePets.Add(key, result5);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParsePetsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "S";
			if (dictionary.ContainsKey(text))
			{
				float result6;
				if (float.TryParse(dictionary[text].ToString(), out result6))
				{
					speedPets.Add(key, result6);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParsePetsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "C";
			if (dictionary.ContainsKey(text))
			{
				int result7;
				if (int.TryParse(dictionary[text].ToString(), out result7))
				{
					cashbackPets.Add(key, result7);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParsePetsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "P";
			if (dictionary.ContainsKey(text))
			{
				string text2 = dictionary[text].ToString();
				int price = Convert.ToInt32(text2.Substring(1));
				string currency = (text2.Substring(0, 1).Equals("C") ? "Coins" : "GemsCurrency");
				pricesFromServer.Add(key, new ItemPrice(price, currency));
			}
		}
	}

	private void ParseEggsConfig(Dictionary<string, object> _eggsConfig)
	{
		timeEggs.Clear();
		victoriasEggs.Clear();
		ratingEggs.Clear();
		rarityPetsInEggs.Clear();
		foreach (KeyValuePair<string, object> item in _eggsConfig)
		{
			if (string.IsNullOrEmpty(item.Key))
			{
				continue;
			}
			Dictionary<string, object> dictionary = item.Value as Dictionary<string, object>;
			string key = item.Key;
			string text = "T";
			if (dictionary.ContainsKey(text))
			{
				int result;
				if (int.TryParse(dictionary[text].ToString(), out result))
				{
					timeEggs.Add(key, result);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseEggsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "V";
			if (dictionary.ContainsKey(text))
			{
				int result2;
				if (int.TryParse(dictionary[text].ToString(), out result2))
				{
					victoriasEggs.Add(key, result2);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseEggsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "Rat";
			if (dictionary.ContainsKey(text))
			{
				int result3;
				if (int.TryParse(dictionary[text].ToString(), out result3))
				{
					ratingEggs.Add(key, result3);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseEggsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "C";
			if (dictionary.ContainsKey(text))
			{
				float result4;
				if (float.TryParse(dictionary[text].ToString(), out result4))
				{
					if (!rarityPetsInEggs.ContainsKey(key))
					{
						rarityPetsInEggs.Add(key, new List<EggPetInfo>());
					}
					EggPetInfo eggPetInfo = new EggPetInfo();
					eggPetInfo.Rarity = ItemDb.ItemRarity.Common;
					eggPetInfo.Chance = result4;
					rarityPetsInEggs[key].Add(eggPetInfo);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseEggsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "U";
			if (dictionary.ContainsKey(text))
			{
				float result5;
				if (float.TryParse(dictionary[text].ToString(), out result5))
				{
					if (!rarityPetsInEggs.ContainsKey(key))
					{
						rarityPetsInEggs.Add(key, new List<EggPetInfo>());
					}
					EggPetInfo eggPetInfo2 = new EggPetInfo();
					eggPetInfo2.Rarity = ItemDb.ItemRarity.Uncommon;
					eggPetInfo2.Chance = result5;
					rarityPetsInEggs[key].Add(eggPetInfo2);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseEggsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "R";
			if (dictionary.ContainsKey(text))
			{
				float result6;
				if (float.TryParse(dictionary[text].ToString(), out result6))
				{
					if (!rarityPetsInEggs.ContainsKey(key))
					{
						rarityPetsInEggs.Add(key, new List<EggPetInfo>());
					}
					EggPetInfo eggPetInfo3 = new EggPetInfo();
					eggPetInfo3.Rarity = ItemDb.ItemRarity.Rare;
					eggPetInfo3.Chance = result6;
					rarityPetsInEggs[key].Add(eggPetInfo3);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseEggsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "E";
			if (dictionary.ContainsKey(text))
			{
				float result7;
				if (float.TryParse(dictionary[text].ToString(), out result7))
				{
					if (!rarityPetsInEggs.ContainsKey(key))
					{
						rarityPetsInEggs.Add(key, new List<EggPetInfo>());
					}
					EggPetInfo eggPetInfo4 = new EggPetInfo();
					eggPetInfo4.Rarity = ItemDb.ItemRarity.Epic;
					eggPetInfo4.Chance = result7;
					rarityPetsInEggs[key].Add(eggPetInfo4);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseEggsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "L";
			if (dictionary.ContainsKey(text))
			{
				float result8;
				if (float.TryParse(dictionary[text].ToString(), out result8))
				{
					if (!rarityPetsInEggs.ContainsKey(key))
					{
						rarityPetsInEggs.Add(key, new List<EggPetInfo>());
					}
					EggPetInfo eggPetInfo5 = new EggPetInfo();
					eggPetInfo5.Rarity = ItemDb.ItemRarity.Legendary;
					eggPetInfo5.Chance = result8;
					rarityPetsInEggs[key].Add(eggPetInfo5);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseEggsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "M";
			if (dictionary.ContainsKey(text))
			{
				float result9;
				if (float.TryParse(dictionary[text].ToString(), out result9))
				{
					if (!rarityPetsInEggs.ContainsKey(key))
					{
						rarityPetsInEggs.Add(key, new List<EggPetInfo>());
					}
					EggPetInfo eggPetInfo6 = new EggPetInfo();
					eggPetInfo6.Rarity = ItemDb.ItemRarity.Mythic;
					eggPetInfo6.Chance = result9;
					rarityPetsInEggs[key].Add(eggPetInfo6);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseEggsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "P";
			if (dictionary.ContainsKey(text))
			{
				string text2 = dictionary[text].ToString();
				int price = Convert.ToInt32(text2.Substring(1));
				string currency = (text2.Substring(0, 1).Equals("C") ? "Coins" : "GemsCurrency");
				pricesFromServer.Add((key == "SI") ? "Eggs.SuperIncubatorId" : key, new ItemPrice(price, currency));
			}
		}
	}

	private void ParseLobbyItemsConfig(Dictionary<string, object> _lobbyItemsConfig)
	{
		timeLobbyItems.Clear();
		xpLobbyItems.Clear();
		priceLobbyItems.Clear();
		priceTimeLobbyItems.Clear();
		priceFullLobbyItems.Clear();
		levelLobbyItems.Clear();
		foreach (KeyValuePair<string, object> item in _lobbyItemsConfig)
		{
			Dictionary<string, object> dictionary = item.Value as Dictionary<string, object>;
			string text = "T";
			if (dictionary.ContainsKey(text))
			{
				int result;
				if (int.TryParse(dictionary[text].ToString(), out result))
				{
					timeLobbyItems.Add(item.Key, result);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseLobbyItemsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "XP";
			if (dictionary.ContainsKey(text))
			{
				int result2;
				if (int.TryParse(dictionary[text].ToString(), out result2))
				{
					xpLobbyItems.Add(item.Key, result2);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseLobbyItemsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "L";
			if (dictionary.ContainsKey(text))
			{
				int result3;
				if (int.TryParse(dictionary[text].ToString(), out result3))
				{
					levelLobbyItems.Add(item.Key, result3);
				}
				else if (Application.isEditor)
				{
					UnityEngine.Debug.LogError("ParseLobbyItemsConfig: not parse " + item.Key + " parametr " + text);
				}
			}
			text = "P";
			if (dictionary.ContainsKey(text))
			{
				string text2 = dictionary[text].ToString();
				int price = Convert.ToInt32(text2.Substring(1));
				string currency = (text2.Substring(0, 1).Equals("C") ? "Coins" : "GemsCurrency");
				priceLobbyItems.Add(item.Key, new ItemPrice(price, currency));
			}
			text = "FP";
			if (dictionary.ContainsKey(text))
			{
				string text3 = dictionary[text].ToString();
				int price2 = Convert.ToInt32(text3.Substring(1));
				string currency2 = (text3.Substring(0, 1).Equals("C") ? "Coins" : "GemsCurrency");
				priceFullLobbyItems.Add(item.Key, new ItemPrice(price2, currency2));
			}
			text = "TP";
			if (dictionary.ContainsKey(text))
			{
				string text4 = dictionary[text].ToString();
				int price3 = Convert.ToInt32(text4.Substring(1));
				string currency3 = (text4.Substring(0, 1).Equals("C") ? "Coins" : "GemsCurrency");
				priceTimeLobbyItems.Add(item.Key, new ItemPrice(price3, currency3));
			}
		}
	}

	private void ParseItemPricesConfig(Dictionary<string, object> _itemPricesConfig)
	{
		foreach (KeyValuePair<string, object> item in _itemPricesConfig)
		{
			string text = item.Value.ToString();
			int price = Convert.ToInt32(text.Substring(1));
			string currency = (text.Substring(0, 1).Equals("C") ? "Coins" : "GemsCurrency");
			pricesFromServer.Add(item.Key, new ItemPrice(price, currency));
		}
	}

	private static void ParseLevelingConfig(object obj)
	{
		List<object> list = obj as List<object>;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> obj2 = list[i] as Dictionary<string, object>;
			int num = Convert.ToInt32(obj2["L"]);
			int coins = Convert.ToInt32(obj2["C"]);
			int gems = Convert.ToInt32(obj2["G"]);
			ExperienceController.RewriteLevelingParametersForLevel(num - 1, coins, gems);
		}
	}

	private static bool ParseInappsConfig(object obj)
	{
		string text = Json.Serialize(obj);
		if (text == _inappObj)
		{
			return false;
		}
		_inappObj = text;
		List<object> list = obj as List<object>;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> obj2 = list[i] as Dictionary<string, object>;
			int priceId = Convert.ToInt32(obj2["Real"]);
			int coinQuantity = Convert.ToInt32(obj2["C"]);
			int gemsQuantity = Convert.ToInt32(obj2["G"]);
			int bonusCoins = Convert.ToInt32(obj2["BC"]);
			int bonusGems = Convert.ToInt32(obj2["BG"]);
			VirtualCurrencyHelper.RewriteInappsQuantity(priceId, coinQuantity, gemsQuantity, bonusCoins, bonusGems);
		}
		return true;
	}

	private static bool ParseTicketsConfig(object obj)
	{
		string text = Json.Serialize(obj);
		if (text == _ticketsObj)
		{
			return false;
		}
		_ticketsObj = text;
		List<object> list = obj as List<object>;
		List<BankExchangeItemData> list2 = new List<BankExchangeItemData>();
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			if (dictionary.ContainsKey("Curr"))
			{
				string text2 = dictionary["Curr"].ToString();
				if (text2.Equals("Gems"))
				{
					BankExchangeItemData item = new BankExchangeItemData
					{
						InAppId = list2.Count + 1,
						GemsPrice = Convert.ToInt32(dictionary["Price"]),
						CurrencyCount = Convert.ToInt32(dictionary["Tickets"])
					};
					list2.Add(item);
				}
				if (text2.Equals("Hour"))
				{
					_FreeTicketsPerPack.Value = Convert.ToInt32(dictionary["Tickets"]);
					_TimeGiveFreeTickets.Value = Convert.ToInt32(dictionary["Price"]) * 3600;
				}
				if (text2.Equals("Max"))
				{
					_MaxFreeTickets.Value = Convert.ToInt32(dictionary["Tickets"]);
				}
				if (text2.Equals("Start"))
				{
					_CountWelcomeTickets.Value = Convert.ToInt32(dictionary["Tickets"]);
				}
			}
		}
		if (list2.Count > 0)
		{
			BankViewTickets.RewriteTicketChangeItems(list2);
		}
		return true;
	}

	private static void ParseAwardConfig(object obj)
	{
		_minigameParameters.Clear();
		levelRequired.Clear();
		expBoxAwards.Clear();
		coinsBoxAwards.Clear();
		gemsBoxAwards.Clear();
		List<object> list = obj as List<object>;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			string text = Convert.ToString(dictionary["Mode"]);
			int[] array = new int[10];
			int num = 5;
			if (dictionary.ContainsKey("Sc"))
			{
				num = Convert.ToInt32(dictionary["Sc"]);
			}
			for (int j = 1; j <= 10; j++)
			{
				if (dictionary.ContainsKey(j.ToString()))
				{
					array[j - 1] = Convert.ToInt32(dictionary[j.ToString()]);
				}
			}
			if (text.Equals("XP Team"))
			{
				AdminSettingsController.expAvardTeamFight[0] = array;
				AdminSettingsController.minScoreTeamFight = num;
				levelRequired.Add(GameConnect.GameMode.TeamFight, 1);
			}
			if (text.Equals("Coins Team"))
			{
				AdminSettingsController.coinAvardTeamFight[0] = array;
				AdminSettingsController.minScoreTeamFight = num;
			}
			if (text.Equals("XP DM"))
			{
				AdminSettingsController.expAvardDeathMath[0] = array;
				AdminSettingsController.minScoreDeathMath = num;
				levelRequired.Add(GameConnect.GameMode.Deathmatch, Convert.ToInt32(dictionary["Unlock"]));
			}
			if (text.Equals("Coins DM"))
			{
				AdminSettingsController.coinAvardDeathMath[0] = array;
				AdminSettingsController.minScoreDeathMath = num;
			}
			if (text.Equals("XP Coop"))
			{
				AdminSettingsController.expAvardTimeBattle = array;
				AdminSettingsController.minScoreTimeBattle = num;
				int ticketsPrice = Convert.ToInt32(dictionary["Tickets"]);
				int num2 = Convert.ToInt32(dictionary["Unlock"]);
				_minigameParameters.Add(GameConnect.GameMode.TimeBattle, new MinigameParameters(num2, ticketsPrice));
			}
			if (text.Equals("Coins Coop"))
			{
				AdminSettingsController.coinAvardTimeBattle = array;
				AdminSettingsController.minScoreTimeBattle = num;
			}
			if (text.Equals("XP Flag"))
			{
				AdminSettingsController.expAvardFlagCapture[0] = array;
				AdminSettingsController.minScoreFlagCapture = num;
				levelRequired.Add(GameConnect.GameMode.FlagCapture, Convert.ToInt32(dictionary["Unlock"]));
			}
			if (text.Equals("Coins Flag"))
			{
				AdminSettingsController.coinAvardFlagCapture[0] = array;
				AdminSettingsController.minScoreFlagCapture = num;
			}
			if (text.Equals("XP Points"))
			{
				AdminSettingsController.expAvardCapturePoint[0] = array;
				AdminSettingsController.minScoreCapturePoint = num;
				levelRequired.Add(GameConnect.GameMode.CapturePoints, Convert.ToInt32(dictionary["Unlock"]));
			}
			if (text.Equals("Coins Points"))
			{
				AdminSettingsController.coinAvardCapturePoint[0] = array;
				AdminSettingsController.minScoreCapturePoint = num;
			}
			if (text.Equals("XP Duels"))
			{
				AdminSettingsController.expAvardDuel = array;
				AdminSettingsController.minScoreDuel = num;
				levelRequired.Add(GameConnect.GameMode.Duel, Convert.ToInt32(dictionary["Unlock"]));
			}
			if (text.Equals("Coins Duels"))
			{
				AdminSettingsController.coinAvardDuel = array;
				AdminSettingsController.minScoreDuel = num;
			}
			if (text.Equals("XP Deadly"))
			{
				AdminSettingsController.expAvardDeadlyGames = array;
				int ticketsPrice2 = Convert.ToInt32(dictionary["Tickets"]);
				int num3 = Convert.ToInt32(dictionary["Unlock"]);
				_minigameParameters.Add(GameConnect.GameMode.DeadlyGames, new MinigameParameters(num3, ticketsPrice2));
			}
			if (text.Equals("Coins Deadly"))
			{
				AdminSettingsController.coinAvardDeadlyGames = array;
			}
			if (text.Equals("XP Escape"))
			{
				AdminSettingsController.expAvardDeathEscape = array;
				int ticketsPrice3 = Convert.ToInt32(dictionary["Tickets"]);
				int num4 = Convert.ToInt32(dictionary["Unlock"]);
				_minigameParameters.Add(GameConnect.GameMode.DeathEscape, new MinigameParameters(num4, ticketsPrice3));
			}
			if (text.Equals("Coins Escape"))
			{
				AdminSettingsController.coinAvardDeathEscape = array;
			}
			if (text.Equals("Coins Run"))
			{
				int ticketsPrice4 = Convert.ToInt32(dictionary["Tickets"]);
				int num5 = Convert.ToInt32(dictionary["Unlock"]);
				_minigameParameters.Add(GameConnect.GameMode.SpeedRun, new MinigameParameters(num5, ticketsPrice4));
			}
			if (text.Equals("XP Spleef"))
			{
				AdminSettingsController.expAvardSpleef = array;
				int ticketsPrice5 = Convert.ToInt32(dictionary["Tickets"]);
				int num6 = Convert.ToInt32(dictionary["Unlock"]);
				_minigameParameters.Add(GameConnect.GameMode.Spleef, new MinigameParameters(num6, ticketsPrice5));
			}
			if (text.Equals("Coins Spleef"))
			{
				AdminSettingsController.coinAvardSpleef = array;
			}
			if (text.Equals("Sandbox"))
			{
				AdminSettingsController.expAvardSpleef = array;
				int ticketsPrice6 = Convert.ToInt32(dictionary["Tickets"]);
				int num7 = Convert.ToInt32(dictionary["Unlock"]);
				_minigameParameters.Add(GameConnect.GameMode.Dater, new MinigameParameters(num7, ticketsPrice6));
			}
			if (text.Equals("Arena"))
			{
				AdminSettingsController.expAvardSpleef = array;
				int ticketsPrice7 = Convert.ToInt32(dictionary["Tickets"]);
				int num8 = Convert.ToInt32(dictionary["Unlock"]);
				_minigameParameters.Add(GameConnect.GameMode.Arena, new MinigameParameters(num8, ticketsPrice7));
			}
			if (text.Equals("XP box1"))
			{
				expBoxAwards.Add("Box_1", Convert.ToInt32(dictionary["1"]));
			}
			if (text.Equals("Coins box1"))
			{
				coinsBoxAwards.Add("Box_1", Convert.ToInt32(dictionary["1"]));
			}
			if (text.Equals("Gems box1"))
			{
				gemsBoxAwards.Add("Box_1", Convert.ToInt32(dictionary["1"]));
			}
			if (text.Equals("XP box2"))
			{
				int value = Convert.ToInt32(dictionary["Tickets"]);
				expBoxAwards.Add("Box_2", Convert.ToInt32(dictionary["1"]));
				_box2Tickets.Value = value;
			}
			if (text.Equals("Coins box2"))
			{
				coinsBoxAwards.Add("Box_2", Convert.ToInt32(dictionary["1"]));
			}
			if (text.Equals("Gems box2"))
			{
				gemsBoxAwards.Add("Box_2", Convert.ToInt32(dictionary["1"]));
			}
			if (text.Equals("XP box3"))
			{
				int value2 = Convert.ToInt32(dictionary["Tickets"]);
				expBoxAwards.Add("Box_3", Convert.ToInt32(dictionary["1"]));
				_box3Tickets.Value = value2;
			}
			if (text.Equals("Coins box3"))
			{
				coinsBoxAwards.Add("Box_3", Convert.ToInt32(dictionary["1"]));
			}
			if (text.Equals("Gems box3"))
			{
				gemsBoxAwards.Add("Box_3", Convert.ToInt32(dictionary["1"]));
			}
			if (text.Equals("MiniGamesButton"))
			{
				unlockMiniGames.Value = Convert.ToInt32(dictionary["Unlock"]);
			}
			if (text.Equals("LobbyItemsButton"))
			{
				_lobbyItemsLevelRequired.Value = Convert.ToInt32(dictionary["Unlock"]);
			}
		}
	}

	private static void ParseSpecialEventsConfig(object obj)
	{
		List<object> list = obj as List<object>;
		for (int i = 0; i < list.Count; i++)
		{
			Dictionary<string, object> dictionary = list[i] as Dictionary<string, object>;
			string text = Convert.ToString(dictionary["Event"]);
			if (text.Equals("StartCapital"))
			{
				if (Convert.ToBoolean(dictionary["Enable"]))
				{
					int num = Convert.ToInt32(dictionary["Coins"]);
					int num2 = Convert.ToInt32(dictionary["Gems"]);
					startCapitalCoins = num;
					startCapitalGems = num2;
					startCapitalEnabled = true;
				}
				else
				{
					startCapitalEnabled = false;
				}
			}
			if (!text.Equals("СompetitionAward") && !text.Equals("CompetitionAward"))
			{
				continue;
			}
			if (Convert.ToBoolean(dictionary["Enable"]))
			{
				int num3 = Convert.ToInt32(dictionary["Coins"]);
				int num4 = Convert.ToInt32(dictionary["Gems"]);
				countPlaceAwardInCompetion = Convert.ToInt32(dictionary["XP"]);
				if (num3 > 0)
				{
					competitionAward = new ItemPrice(num3, "Coins");
				}
				else if (num4 > 0)
				{
					competitionAward = new ItemPrice(num4, "GemsCurrency");
				}
				else
				{
					competitionAward = new ItemPrice(0, "Coins");
				}
			}
			else
			{
				competitionAward = new ItemPrice(0, "Coins");
				countPlaceAwardInCompetion = 0;
			}
		}
	}

	private IEnumerator GetActionsFromServer()
	{
		string value;
		while (true)
		{
			Task futureToWait = PersistentCacheManager.Instance.FirstResponse;
			if (_encryptedPlayerPrefs.HasKey(inappsActionsCacheKey) || !string.IsNullOrEmpty(_encryptedPlayerPrefs.GetString(inappsActionsCacheKey)))
			{
				yield return new WaitUntil(() => futureToWait.IsCompleted);
				if (!string.IsNullOrEmpty(PersistentCacheManager.Instance.GetValue(inappsActionsURL)))
				{
					yield break;
				}
			}
			new WWWForm();
			WWW download = Tools.CreateWwwIfNotConnected(inappsActionsURL);
			if (download == null)
			{
				yield return new WaitForRealSeconds(30f);
				continue;
			}
			yield return download;
			if (!string.IsNullOrEmpty(download.error))
			{
				if (UnityEngine.Debug.isDebugBuild || Application.isEditor)
				{
					UnityEngine.Debug.LogWarning("GetInappsBonuses error: " + download.error);
				}
				yield return new WaitForRealSeconds(30f);
				continue;
			}
			value = URLs.Sanitize(download);
			if (!string.IsNullOrEmpty(value))
			{
				break;
			}
			yield return new WaitForRealSeconds(300f);
		}
		using (new ScopeLogger("GetInappsBonuses()", "Saving to storager", Defs.IsDeveloperBuild))
		{
			_encryptedPlayerPrefs.SetString(inappsActionsCacheKey, value);
		}
		using (new ScopeLogger("GetInappsBonuses()", "Saving to cache", Defs.IsDeveloperBuild))
		{
			PersistentCacheManager.Instance.SetValue(inappsActionsURL, value);
		}
		ParseConfigInappBonuses();
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("GetConfigABtestBalans");
		}
	}

	public void ParseConfigInappBonuses()
	{
		if (ParseInappsBonusConfig(_encryptedPlayerPrefs.GetString(inappsActionsCacheKey)) && BalanceController.UpdatedBankView != null)
		{
			BalanceController.UpdatedBankView();
		}
	}

	private static bool ParseInappsBonusConfig(string _jsonConfig)
	{
		if (_jsonConfig == _inappObjBonus)
		{
			return false;
		}
		_inappObjBonus = _jsonConfig;
		inappsBonus.Clear();
		inappsBonusSegments.Clear();
		Dictionary<string, object> dictionary = Json.Deserialize(_jsonConfig) as Dictionary<string, object>;
		if (dictionary != null)
		{
			if (dictionary.ContainsKey("segments"))
			{
				foreach (KeyValuePair<string, object> item2 in dictionary["segments"] as Dictionary<string, object>)
				{
					Dictionary<string, object> dictionary2 = item2.Value as Dictionary<string, object>;
					if (dictionary2.ContainsKey("Paying") && !Convert.ToBoolean(dictionary2["Paying"]) && StoreKitEventListener.IsPayingUser())
					{
						continue;
					}
					if (dictionary2.ContainsKey("Countrys"))
					{
						string @string = Storager.getString(Defs.countryKey);
						if (!string.IsNullOrEmpty(@string))
						{
							List<object> list = Json.Deserialize(dictionary2["Countrys"].ToString()) as List<object>;
							if (list != null)
							{
								bool flag = false;
								foreach (object item3 in list)
								{
									if (item3.ToString() == @string)
									{
										flag = true;
										break;
									}
								}
								if (!flag)
								{
									continue;
								}
							}
						}
					}
					if (dictionary2.ContainsKey("Continents"))
					{
						string string2 = Storager.getString(Defs.continentKey);
						if (!string.IsNullOrEmpty(string2))
						{
							List<object> list2 = Json.Deserialize(dictionary2["Continents"].ToString()) as List<object>;
							if (list2 != null)
							{
								bool flag2 = false;
								foreach (object item4 in list2)
								{
									if (item4.ToString() == string2)
									{
										flag2 = true;
										break;
									}
								}
								if (!flag2)
								{
									continue;
								}
							}
						}
					}
					inappsBonusSegments.Add(item2.Key, item2.Value);
				}
			}
			if (dictionary.ContainsKey("actions"))
			{
				List<object> list3 = dictionary["actions"] as List<object>;
				for (int i = 0; i < list3.Count; i++)
				{
					Dictionary<string, object> dictionary3 = list3[i] as Dictionary<string, object>;
					if (!dictionary3.ContainsKey("ID"))
					{
						continue;
					}
					string item = Convert.ToString(dictionary3["ID"]);
					if (!supportedInappBonusIds.Contains(item))
					{
						continue;
					}
					Dictionary<string, object> dictionary4 = new Dictionary<string, object>();
					string value = Convert.ToString(dictionary3["ID"]);
					int num = Convert.ToInt32(dictionary3["Real"]);
					bool flag3 = Convert.ToString(dictionary3["Cur"]) != "Coins";
					string value2 = string.Empty;
					string[] inappOffersIds = StoreKitEventListener.inappOffersIds;
					int index = 0;
					for (int j = 0; j < VirtualCurrencyHelper.coinPriceIds.Length; j++)
					{
						if (VirtualCurrencyHelper.coinPriceIds[j] == num)
						{
							value2 = inappOffersIds[j];
							index = j;
							break;
						}
					}
					if (string.IsNullOrEmpty(value2))
					{
						continue;
					}
					dictionary4.Add("action", value);
					dictionary4.Add("isGems", flag3);
					dictionary4.Add("Start", dictionary3["Start"]);
					dictionary4.Add("End", dictionary3["End"]);
					float num2 = (float)num / (float)(VirtualCurrencyHelper.gemsInappsQuantity[index] * 3);
					float num3 = (BankController.isBankOneCurrency ? (num2 * 0.6f) : ((float)num / (float)(VirtualCurrencyHelper.coinInappsQuantity[index] * 3)));
					dictionary4.Add("Real", dictionary3["Real"]);
					dictionary4.Add("priceGems", num2);
					dictionary4.Add("priceCoins", num3);
					dictionary4.Add("id", value2);
					if (dictionary3.ContainsKey("Group"))
					{
						dictionary4.Add("Group", dictionary3["Group"]);
					}
					if (dictionary3.ContainsKey("C"))
					{
						dictionary4.Add("Coins", dictionary3["C"]);
					}
					if (dictionary3.ContainsKey("G"))
					{
						dictionary4.Add("GemsCurrency", dictionary3["G"]);
					}
					if (dictionary3.ContainsKey("Type"))
					{
						dictionary4.Add("Type", dictionary3["Type"]);
					}
					if (dictionary3.ContainsKey("Packs"))
					{
						dictionary4.Add("Pack", dictionary3["Packs"]);
					}
					if (dictionary3.ContainsKey("Count"))
					{
						dictionary4.Add("Count", dictionary3["Count"]);
					}
					if (dictionary3.ContainsKey("White"))
					{
						dictionary4.Add("White", dictionary3["White"]);
					}
					if (dictionary3.ContainsKey("Black"))
					{
						dictionary4.Add("Black", dictionary3["Black"]);
					}
					if (dictionary3.ContainsKey("Ids"))
					{
						List<object> list4 = Json.Deserialize(Convert.ToString(dictionary3["Ids"])) as List<object>;
						if (list4 != null)
						{
							dictionary4.Add("Ids", list4);
						}
					}
					if (dictionary3.ContainsKey("AddBonus"))
					{
						string text = dictionary3["AddBonus"].ToString();
						int num4 = Convert.ToInt32(text.Substring(1));
						string value3 = (text.Substring(0, 1).Equals("C") ? "Coins" : "GemsCurrency");
						dictionary4.Add("AddBonusCount", num4);
						dictionary4.Add("AddBonusCurrency", value3);
					}
					inappsBonus.Add(dictionary4);
				}
			}
		}
		return true;
	}

	public static string GetCurrenceCurrentInnapBonus()
	{
		string result = string.Empty;
		List<Dictionary<string, object>> currentInnapBonus = GetCurrentInnapBonus();
		bool flag = false;
		bool flag2 = false;
		if (currentInnapBonus != null)
		{
			foreach (Dictionary<string, object> item in currentInnapBonus)
			{
				if (item.ContainsKey("isGems"))
				{
					if (Convert.ToBoolean(item["isGems"]))
					{
						flag = true;
					}
					else
					{
						flag2 = true;
					}
				}
			}
		}
		if (flag)
		{
			result = "GemsCurrency";
		}
		else if (flag2)
		{
			result = "Coins";
		}
		return result;
	}

	public static bool isActiveInnapBonus()
	{
		if (!TrainingController.TrainingCompleted || ExperienceController.sharedController.currentLevel < 2 || FriendsController.ServerTime < 1)
		{
			return false;
		}
		List<string> currentSegments = GetCurrentSegments();
		foreach (Dictionary<string, object> inappsBonu in inappsBonus)
		{
			string item = string.Empty;
			if (inappsBonu.ContainsKey("Start"))
			{
				if (inappsBonu.ContainsKey("Group"))
				{
					item = inappsBonu["Start"].ToString() + inappsBonu["Group"].ToString();
				}
				else if (inappsBonu.ContainsKey("id"))
				{
					item = inappsBonu["Start"].ToString() + inappsBonu["id"].ToString();
				}
			}
			if (!inappsBonu.ContainsKey("Type") || keysInappBonusActionGiven.Contains(item))
			{
				continue;
			}
			DateTime dateTime = DateTime.MinValue;
			DateTime dateTime2 = DateTime.MinValue;
			DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime(FriendsController.ServerTime);
			if (inappsBonu.ContainsKey("Start") && inappsBonu.ContainsKey("End"))
			{
				dateTime = Convert.ToDateTime(inappsBonu["Start"], CultureInfo.InvariantCulture);
				dateTime2 = Convert.ToDateTime(inappsBonu["End"], CultureInfo.InvariantCulture);
			}
			if (!(dateTime <= currentTimeByUnixTime) || !(currentTimeByUnixTime <= dateTime2))
			{
				continue;
			}
			object value;
			if (inappsBonu.TryGetValue("White", out value))
			{
				bool flag = false;
				List<object> list = Json.Deserialize(value.ToString()) as List<object>;
				for (int i = 0; i < list.Count; i++)
				{
					if (currentSegments.Contains(list[i].ToString()))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					continue;
				}
			}
			object value2;
			if (inappsBonu.TryGetValue("Black", out value2))
			{
				bool flag2 = false;
				List<object> list2 = Json.Deserialize(value2.ToString()) as List<object>;
				for (int j = 0; j < list2.Count; j++)
				{
					if (currentSegments.Contains(list2[j].ToString()))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					continue;
				}
			}
			if (inappsBonu.ContainsKey("action") && Convert.ToString(inappsBonu["action"]) == petActionName)
			{
				string value3 = string.Empty;
				if (inappsBonu.ContainsKey("Ids"))
				{
					value3 = GetCurentPetID(inappsBonu["Start"].ToString(), inappsBonu["Ids"] as List<object>);
				}
				if (string.IsNullOrEmpty(value3))
				{
					continue;
				}
			}
			if (inappsBonu.ContainsKey("action") && Convert.ToString(inappsBonu["action"]) == weaponActionName)
			{
				string value4 = string.Empty;
				if (inappsBonu.ContainsKey("Ids"))
				{
					value4 = GetCurentWeaponID(inappsBonu["Start"].ToString(), inappsBonu["Ids"] as List<object>);
				}
				if (string.IsNullOrEmpty(value4))
				{
					continue;
				}
			}
			if (!inappsBonu.ContainsKey("action") || !(Convert.ToString(inappsBonu["action"]) == gadgetActionName) || GetCurentGadgetesIDs(inappsBonu["Start"].ToString(), inappsBonu["Ids"] as List<object>) != null)
			{
				return true;
			}
		}
		return false;
	}

	private static string GetCurentPetID(string _key, List<object> _ids)
	{
		if (Storager.getString(Defs.keyInappBonusStartActionForPresentIDPetkey) == _key)
		{
			return Storager.getString(Defs.keyInappPresentIDPetkey);
		}
		List<string> list = new List<string>();
		for (int i = 0; i < _ids.Count; i++)
		{
			list.Add(_ids[i].ToString());
		}
		string firstSmallestUpPet = Singleton<PetsManager>.Instance.GetFirstSmallestUpPet(list);
		if (!string.IsNullOrEmpty(firstSmallestUpPet))
		{
			Storager.setString(Defs.keyInappPresentIDPetkey, firstSmallestUpPet);
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDPetkey, _key);
		}
		return firstSmallestUpPet;
	}

	private static bool isWeaponAvalibleForBonus(string weaponTag)
	{
		ItemRecord byTag = ItemDb.GetByTag(weaponTag);
		if (byTag == null || byTag.StorageId == null)
		{
			return false;
		}
		return Storager.getInt(byTag.StorageId) <= 0;
	}

	private static bool isGadgetAvalibleForBonus(string gadgetTag)
	{
		if (!GadgetsInfo.info.ContainsKey(gadgetTag))
		{
			return false;
		}
		return !GadgetsInfo.IsBought(gadgetTag);
	}

	private static string GetCurentWeaponID(string _key, List<object> _ids)
	{
		if (Storager.getString(Defs.keyInappBonusStartActionForPresentIDWeaponRedkey) == _key)
		{
			string @string = Storager.getString(Defs.keyInappPresentIDWeaponRedkey);
			if (isWeaponAvalibleForBonus(@string))
			{
				return @string;
			}
		}
		string text = null;
		int num = ExpController.OurTierForAnyPlace();
		int num2 = num;
		while (num2 >= num - 1 && num2 >= 0)
		{
			List<object> list = _ids[num2] as List<object>;
			for (int i = 0; i < list.Count; i++)
			{
				if (isWeaponAvalibleForBonus(list[i].ToString()))
				{
					text = list[i].ToString();
					break;
				}
			}
			if (!string.IsNullOrEmpty(text))
			{
				break;
			}
			num2--;
		}
		if (!string.IsNullOrEmpty(text))
		{
			Storager.setString(Defs.keyInappPresentIDWeaponRedkey, text);
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDWeaponRedkey, _key);
		}
		return text;
	}

	private static bool isAvalibleListGadgetes(List<string> _ids)
	{
		if (_ids != null)
		{
			for (int i = 0; i < _ids.Count; i++)
			{
				if (GadgetsInfo.IsBought(_ids[i]))
				{
					return false;
				}
			}
			return true;
		}
		return false;
	}

	private static List<string> GetCurentGadgetesIDs(string _key, List<object> _ids)
	{
		if (Storager.getString(Defs.keyInappBonusStartActionForPresentIDGadgetkey) == _key)
		{
			List<string> list = curentGadgetesIDs;
			if (isAvalibleListGadgetes(list))
			{
				return list;
			}
		}
		List<string> list2 = new List<string>();
		int num = ExpController.OurTierForAnyPlace();
		for (int i = 0; i < _ids.Count; i++)
		{
			List<object> list3 = _ids[i] as List<object>;
			string text = null;
			for (int num2 = num; num2 >= 0; num2--)
			{
				List<object> list4 = list3[num2] as List<object>;
				for (int j = 0; j < list4.Count; j++)
				{
					if (!GadgetsInfo.IsBought(list4[j].ToString()))
					{
						text = list4[j].ToString();
						break;
					}
				}
				if (!string.IsNullOrEmpty(text))
				{
					list2.Add(text);
					break;
				}
			}
		}
		if (list2.Count == 3)
		{
			curentGadgetesIDs = list2;
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDGadgetkey, _key);
			return list2;
		}
		return null;
	}

	public static void AddKeysInappBonusActionGiven(string _key)
	{
		if (!keysInappBonusActionGiven.Contains(_key))
		{
			keysInappBonusActionGiven.Add(_key);
			Storager.setString(Defs.keysInappBonusGivenkey, Json.Serialize(keysInappBonusActionGiven));
		}
	}

	private static List<string> GetCurrentSegments()
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, object> inappsBonusSegment in inappsBonusSegments)
		{
			Dictionary<string, object> dictionary = inappsBonusSegment.Value as Dictionary<string, object>;
			object value;
			if (dictionary.TryGetValue("MinSumReal", out value))
			{
				int num = Convert.ToInt32(value);
				if (Storager.getInt(Defs.sumInnapsKey) < num)
				{
					continue;
				}
			}
			object value2;
			if (dictionary.TryGetValue("MaxSumReal", out value2))
			{
				int num2 = Convert.ToInt32(value2);
				if (Storager.getInt(Defs.sumInnapsKey) > num2)
				{
					continue;
				}
			}
			if (dictionary.ContainsKey("Paying") && Convert.ToBoolean(dictionary["Paying"]) != StoreKitEventListener.IsPayingUser())
			{
				continue;
			}
			if (dictionary.ContainsKey("Countrys"))
			{
				string @string = Storager.getString(Defs.countryKey);
				if (!string.IsNullOrEmpty(@string))
				{
					List<object> list2 = Json.Deserialize(dictionary["Countrys"].ToString()) as List<object>;
					bool flag = false;
					if (list2 != null)
					{
						foreach (object item in list2)
						{
							if (item.ToString() == @string)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							continue;
						}
					}
				}
			}
			if (dictionary.ContainsKey("Continents"))
			{
				string string2 = Storager.getString(Defs.continentKey);
				if (!string.IsNullOrEmpty(string2))
				{
					List<object> list3 = Json.Deserialize(dictionary["Continents"].ToString()) as List<object>;
					bool flag2 = false;
					if (list3 != null)
					{
						foreach (object item2 in list3)
						{
							if (item2.ToString() == string2)
							{
								flag2 = true;
								break;
							}
						}
						if (!flag2)
						{
							continue;
						}
					}
				}
			}
			list.Add(inappsBonusSegment.Key);
		}
		return list;
	}

	public static List<Dictionary<string, object>> GetCurrentInnapBonus()
	{
		if (!TrainingController.TrainingCompleted || ExperienceController.sharedController.currentLevel < 2 || FriendsController.ServerTime < 1)
		{
			return null;
		}
		if (countFrameInCache == Time.frameCount)
		{
			return cacheCurrentInnapBonus;
		}
		List<string> currentSegments = GetCurrentSegments();
		List<Dictionary<string, object>> list = null;
		foreach (Dictionary<string, object> inappsBonu in inappsBonus)
		{
			string text = string.Empty;
			if (inappsBonu.ContainsKey("Start"))
			{
				if (inappsBonu.ContainsKey("Group"))
				{
					text = inappsBonu["Start"].ToString() + inappsBonu["Group"].ToString();
				}
				else if (inappsBonu.ContainsKey("id"))
				{
					text = inappsBonu["Start"].ToString() + inappsBonu["id"].ToString();
				}
			}
			if (!inappsBonu.ContainsKey("Type") || keysInappBonusActionGiven.Contains(text))
			{
				continue;
			}
			Dictionary<string, object> dictionary = null;
			DateTime currentTimeByUnixTime = Tools.GetCurrentTimeByUnixTime(FriendsController.ServerTime);
			object obj = inappsBonu["Start"];
			object value = inappsBonu["End"];
			DateTime dateTime = Convert.ToDateTime(obj, CultureInfo.InvariantCulture);
			DateTime dateTime2 = Convert.ToDateTime(value, CultureInfo.InvariantCulture);
			if (!(dateTime <= currentTimeByUnixTime) || !(currentTimeByUnixTime <= dateTime2))
			{
				continue;
			}
			object value2;
			if (inappsBonu.TryGetValue("White", out value2))
			{
				bool flag = false;
				List<object> list2 = Json.Deserialize(value2.ToString()) as List<object>;
				for (int i = 0; i < list2.Count; i++)
				{
					if (currentSegments.Contains(list2[i].ToString()))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					continue;
				}
			}
			object value3;
			if (inappsBonu.TryGetValue("Black", out value3))
			{
				bool flag2 = false;
				List<object> list3 = Json.Deserialize(value3.ToString()) as List<object>;
				for (int j = 0; j < list3.Count; j++)
				{
					if (currentSegments.Contains(list3[j].ToString()))
					{
						flag2 = true;
						break;
					}
				}
				if (flag2)
				{
					continue;
				}
			}
			string text2 = inappsBonu["Type"].ToString();
			bool num = text2 == "packs";
			float num2 = Convert.ToSingle(inappsBonu["priceGems"]);
			float num3 = Convert.ToSingle(inappsBonu["priceCoins"]);
			int num4 = Convert.ToInt32(inappsBonu["Real"]);
			float num5 = -1f * (float)num4;
			dictionary = new Dictionary<string, object>();
			string text3 = Convert.ToString(inappsBonu["action"]);
			dictionary.Add("action", text3);
			dictionary.Add("Key", text);
			dictionary.Add("End", Mathf.RoundToInt((float)dateTime2.Subtract(currentTimeByUnixTime).TotalSeconds));
			dictionary.Add("ID", inappsBonu["id"]);
			object value4;
			if (inappsBonu.TryGetValue("Coins", out value4))
			{
				int num6 = Convert.ToInt32(value4);
				dictionary.Add("Coins", value4);
				num5 += (float)num6 * num3;
			}
			object value5;
			if (inappsBonu.TryGetValue("GemsCurrency", out value5))
			{
				int num7 = Convert.ToInt32(value5);
				dictionary.Add("GemsCurrency", value5);
				num5 += (float)num7 * num2;
			}
			if (num)
			{
				int fullPack = Convert.ToInt32(inappsBonu["Pack"]);
				dictionary.Add("Pack", GetCurrentPack(text, dateTime, currentTimeByUnixTime, dateTime2, fullPack));
			}
			dictionary.Add("Type", text2);
			object value6 = null;
			if (inappsBonu.TryGetValue("Count", out value6))
			{
				dictionary.Add("Count", value6);
			}
			dictionary.Add("isGems", inappsBonu["isGems"]);
			if (text3 == curencyActionName)
			{
				float num8 = num5 / (float)num4;
				dictionary.Add("Profit", num8);
			}
			else if (text3 == petActionName)
			{
				string value7 = string.Empty;
				if (inappsBonu.ContainsKey("Ids"))
				{
					value7 = GetCurentPetID(obj.ToString(), inappsBonu["Ids"] as List<object>);
				}
				int result = 0;
				if (inappsBonu.ContainsKey("Count"))
				{
					int.TryParse(value6.ToString(), out result);
				}
				if (string.IsNullOrEmpty(value7) || result == 0)
				{
					continue;
				}
				dictionary.Add("Pet", value7);
				dictionary.Add("Quantity", result);
			}
			else if (text3 == weaponActionName)
			{
				string text4 = string.Empty;
				if (inappsBonu.ContainsKey("Ids"))
				{
					text4 = GetCurentWeaponID(obj.ToString(), inappsBonu["Ids"] as List<object>);
				}
				if (string.IsNullOrEmpty(text4))
				{
					continue;
				}
				ItemPrice itemPrice = ShopNGUIController.GetItemPrice(text4, ShopNGUIController.CategoryNames.SpecilCategory);
				num5 = ((!(itemPrice.Currency == "Coins")) ? (num5 + (float)itemPrice.Price * num2) : (num5 + (float)itemPrice.Price * num3));
				float num9 = num5 / (float)num4;
				dictionary.Add("Profit", num9);
				dictionary.Add("Weapon", text4);
			}
			else if (text3 == leprechaunActionName)
			{
				int num10 = Convert.ToInt32(inappsBonu["AddBonusCount"]) * Convert.ToInt32(value6);
				num5 = ((!(Convert.ToString(inappsBonu["AddBonusCurrency"]) == "Coins")) ? (num5 + (float)num10 * num2) : (num5 + (float)num10 * num3));
				float num11 = num5 / (float)num4;
				dictionary.Add("Profit", num11);
				dictionary.Add("CurrencyLeprechaun", inappsBonu["AddBonusCurrency"]);
				dictionary.Add("PerDayLeprechaun", inappsBonu["AddBonusCount"]);
				dictionary.Add("DaysLeprechaun", value6);
			}
			else if (text3 == gadgetActionName)
			{
				List<string> list4 = GetCurentGadgetesIDs(obj.ToString(), inappsBonu["Ids"] as List<object>);
				if (list4 == null)
				{
					continue;
				}
				for (int k = 0; k < list4.Count; k++)
				{
					ItemPrice itemPrice2 = ShopNGUIController.GetItemPrice(list4[k], ShopNGUIController.CategoryNames.SupportCategory);
					num5 = ((!(itemPrice2.Currency == "Coins")) ? (num5 + (float)itemPrice2.Price * num2) : (num5 + (float)itemPrice2.Price * num3));
				}
				float num12 = num5 / (float)num4;
				dictionary.Add("Profit", num12);
				dictionary.Add("Gadgets", list4);
			}
			if (list == null)
			{
				list = new List<Dictionary<string, object>>();
			}
			list.Add(dictionary);
		}
		cacheCurrentInnapBonus = list;
		countFrameInCache = Time.frameCount;
		SendAnalyticsForNewActions(list);
		return list;
	}

	private static void SendAnalyticsForNewActions(List<Dictionary<string, object>> _curInAppBonusList)
	{
		if (SceneManagerHelper.ActiveSceneName == "AppCenter" || _curInAppBonusList == null)
		{
			return;
		}
		try
		{
			List<string> list = (Json.Deserialize(Storager.getString("BalanceController.SHOWN_ACTION_IDS_KEY")) as List<object>).OfType<string>().ToList();
			bool flag = false;
			foreach (Dictionary<string, object> _curInAppBonus in _curInAppBonusList)
			{
				try
				{
					string item = _curInAppBonus["Key"] as string;
					if (!list.Contains(item))
					{
						list.Add(item);
						flag = true;
						AnalyticsStuff.CustomSpecialOffersSalesShown(_curInAppBonus["ID"] as string);
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in foreach (var action in _curInAppBonusList): {0}", ex);
				}
			}
			if (flag)
			{
				Storager.setString("BalanceController.SHOWN_ACTION_IDS_KEY", Json.Serialize(list));
			}
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in SendAnalyticsForNewActions: {0}", ex2);
		}
	}

	private static int GetCurrentPack(string keyAction, DateTime start, DateTime now, DateTime end, int fullPack)
	{
		if (end.Subtract(now).TotalSeconds == 0.0)
		{
			return 0;
		}
		if (!timeNextUpdateDict.ContainsKey(keyAction))
		{
			timeNextUpdateDict.Add(keyAction, DateTime.MinValue);
		}
		if (!curPackDict.ContainsKey(keyAction))
		{
			curPackDict.Add(keyAction, -1);
		}
		DateTime dateTime = timeNextUpdateDict[keyAction];
		int num = curPackDict[keyAction];
		if (num == -1 || now.Subtract(dateTime).TotalSeconds > 15.0)
		{
			num = CurPackForDate(start, now, end, fullPack);
			timeNextUpdateDict[keyAction] = now + TimeSpan.FromSeconds(UnityEngine.Random.Range(3, 7));
			curPackDict[keyAction] = num;
			return num;
		}
		if (now > dateTime)
		{
			int num2 = UnityEngine.Random.Range(3, 7);
			DateTime now2 = now + TimeSpan.FromSeconds(num2);
			int num3 = CurPackForDate(start, now2, end, fullPack);
			if (num > num3)
			{
				int num4 = UnityEngine.Random.Range(0, num - num3);
				num -= num4;
			}
			else
			{
				num = num3;
			}
			if (num < 1)
			{
				num = 1;
			}
			curPackDict[keyAction] = num;
			timeNextUpdateDict[keyAction] = now + TimeSpan.FromSeconds(num2);
		}
		return num;
	}

	private static int CurPackForDate(DateTime start, DateTime now, DateTime end, int fullPack)
	{
		double totalSeconds = end.Subtract(now).TotalSeconds;
		double totalSeconds2 = end.Subtract(start).TotalSeconds;
		return (int)(totalSeconds / totalSeconds2 * (double)fullPack);
	}

	private void OnApplicationPause(bool pause)
	{
		if (!pause)
		{
			Invoke("UpdateBalansFromServer", 2f);
		}
	}

	private void OnDestroy()
	{
		sharedController = null;
	}
}
