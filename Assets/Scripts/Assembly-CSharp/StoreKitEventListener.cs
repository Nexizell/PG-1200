using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class StoreKitEventListener : MonoBehaviour
{
	public enum ContentType
	{
		Unknown = 0,
		Coins = 1,
		Gems = 2,
		StarterPack = 3,
		Offer = 4
	}

	internal sealed class StoreKitEventListenerState
	{
		public string Mode { get; set; }

		public string PurchaseKey { get; set; }

		public IDictionary<string, string> Parameters { get; private set; }

		public StoreKitEventListenerState()
		{
			Mode = string.Empty;
			PurchaseKey = string.Empty;
			Parameters = new Dictionary<string, string>();
		}
	}

	private static float s_timeOfSuccessfulPurchase;

	internal readonly ICollection<object> _skinProducts = new object[0];

	public const string coin1 = "coin1";

	public const string coin2 = "coin2";

	public const string coin3 = "coin3.";

	public const string coin4 = "coin4";

	public const string coin5 = "coin5";

	public const string coin7 = "coin7";

	public const string coin8 = "coin8";

	internal static readonly string[] coinIds;

	private static bool _billingSupported;

	public static StoreKitEventListener Instance;

	private static string gem1;

	private static string gem2;

	private static string gem3;

	private static string gem4;

	private static string gem5;

	private static string gem6;

	private static string gem7;

	private static string starterPack1;

	private static string starterPack2;

	private static string starterPack4;

	private static string starterPack6;

	private static string starterPack3;

	private static string starterPack5;

	private static string starterPack7;

	private static string starterPack8;

	public static readonly int[] realValue;

	public static readonly string[] gemsIds;

	public static readonly string[] inappOffersIds;

	public static readonly string[] starterPackIds;

	public static Dictionary<string, string> inAppsReadableNames;

	public static string elixirSettName;

	public static bool purchaseInProcess;

	public static bool restoreInProcess;

	public const string bigAmmoPackID = "bigammopack";

	public const string crystalswordID = "crystalsword";

	public const string fullHealthID = "Fullhealth";

	public const string minerWeaponID = "MinerWeapon";

	public static readonly string[] idsForSingle;

	public static readonly string[] idsForMulti;

	public GameObject messagePrefab;

	public static string[] categoryNames;

	public AudioClip onEarnCoinsSound;

	public AudioClip onEarnGemsSound;

	[NonSerialized]
	public static List<string> buyStarterPack;

	private static readonly StoreKitEventListenerState _state;

	public static bool billingSupported { get; private set; }

	public static bool PurchasedRecently
	{
		get
		{
			return Time.realtimeSinceStartup - s_timeOfSuccessfulPurchase <= 1.25f;
		}
	}

	public static bool BillingSupported
	{
		get
		{
			if (Application.isEditor)
			{
				return true;
			}
			return _billingSupported;
		}
	}

	internal static string elixirID
	{
		get
		{
			if (!GlobalGameController.isFullVersion)
			{
				return "elixirlite";
			}
			return "elixir";
		}
	}

	internal static StoreKitEventListenerState State
	{
		get
		{
			return _state;
		}
	}

	public static event Action productListReceivedEvent;

	public static decimal GetPriceFromPriceAmountMicros(long priceAmountMicros)
	{
		decimal d = priceAmountMicros;
		decimal d2 = 1000000m;
		return decimal.Divide(d, d2);
	}

	internal static void InitializeGoogleIab()
	{
	}

	public static void RememberSuccessfulPurchaseTime()
	{
		s_timeOfSuccessfulPurchase = Time.realtimeSinceStartup;
	}

	public static bool ShouldDelayCompletingTransactions()
	{
		try
		{
			return WeaponManager.sharedManager == null || (Time.realtimeSinceStartup - PromoActionsManager.startupTime < 45f && (!PromoActionsManager.x3InfoDownloadaedOnceDuringCurrentRun || !ChestBonusController.chestBonusesObtainedOnceInCurrentRun || !coinsShop.IsStoreAvailable));
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in ShouldDelayCompletingTransactions: " + ex);
			return false;
		}
	}

	public static void SendAfPurchaseApproximate_Android(string productId, string orderId, ContentType contentType)
	{
		decimal value;
		if (VirtualCurrencyHelper.ReferencePricesInUsd.TryGetValue(productId, out value))
		{
			decimal num = Math.Round(value, 0, MidpointRounding.AwayFromZero);
			Dictionary<string, string> eventParams = new Dictionary<string, string>
			{
				{
					"af_revenue",
					num.ToString("F2")
				},
				{
					"af_content_type",
					contentType.ToString()
				},
				{ "af_content_id", productId },
				{ "af_currency", "USD" },
				{ "af_validated", "true" },
				{ "af_receipt_id", orderId }
			};
			AnalyticsFacade.SendCustomEventToAppsFlyer("af_purchase_approximate", eventParams);
		}
		else
		{
			UnityEngine.Debug.LogErrorFormat("Cannot find price for product {0}", productId);
		}
	}

	public static void SendPaid_XXToFacebook(string productId)
	{
		decimal value;
		if (VirtualCurrencyHelper.ReferencePricesInUsd.TryGetValue(productId, out value))
		{
			decimal num = IncrementAccumulatedPayments(value);
			if (num >= 100m)
			{
				AnalyticsStuff.TrySendOnceToFacebook("paid_100", null, null);
			}
			else if (num >= 50m)
			{
				AnalyticsStuff.TrySendOnceToFacebook("paid_50", null, null);
			}
			else if (num >= 25m)
			{
				AnalyticsStuff.TrySendOnceToFacebook("paid_25", null, null);
			}
		}
		else
		{
			UnityEngine.Debug.LogErrorFormat("Cannot find price for product {0}", productId);
		}
	}

	private void Start()
	{
		StartCoroutine(RefreshProductsCoroutine());
	}

	public static void RefreshProducts()
	{
		CoroutineRunner.Instance.StartCoroutine(RefreshProductsCoroutine());
	}

	internal static IEnumerator RefreshProductsCoroutine()
	{
		yield break;
	}

	private static bool IsProductBuy(string productId)
	{
		return false;
	}

	private static void InitializeTestProductsWSA()
	{
	}

	private void Awake()
	{
		Instance = this;
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	static StoreKitEventListener()
	{
		s_timeOfSuccessfulPurchase = float.MinValue;
		coinIds = new string[8] { "coin1", "coin7", "coin2", "coin3.", "coin4", "coin5", "coin8", "coin9" };
		Instance = null;
		gem1 = "gem1wp";
		gem2 = "gem2wp";
		gem3 = "gem3wp";
		gem4 = "gem4wp";
		gem5 = "gem5wp";
		gem6 = "gem6wp";
		gem7 = "gem7wp";
		starterPack1 = "starterpack1";
		starterPack2 = "starterpack2";
		starterPack4 = "starterpack4";
		starterPack6 = "starterpack6";
		starterPack3 = "starterpack3";
		starterPack5 = "starterpack5";
		starterPack7 = "starterpack7";
		starterPack8 = "starterpack8";
		realValue = new int[7] { 1, 3, 5, 10, 20, 50, 100 };
		gemsIds = new string[7] { gem1, gem2, gem3, gem4, gem5, gem6, gem7 };
		inappOffersIds = new string[7] { "cool_offer", "good_offer", "awesome_offer", "super_offer", "mega_offer", "best_offer", "ultimate_offer" };
		starterPackIds = new string[8] { starterPack1, starterPack2, starterPack3, starterPack4, starterPack5, starterPack6, starterPack7, starterPack8 };
		inAppsReadableNames = new Dictionary<string, string>
		{
			{ "coin1", "Small Stack of Coins" },
			{ "coin7", "Medium Stack of Coins" },
			{ "coin2", "Big Stack of Coins" },
			{ "coin3.", "Huge Stack of Coins" },
			{ "coin4", "Chest with Coins" },
			{ "coin5", "Golden Chest with Coins" },
			{ "coin8", "Holy Grail" },
			{ gem1, "Few Gems" },
			{ gem2, "Handful of Gems" },
			{ gem3, "Pile of Gems" },
			{ gem4, "Chest with Gems" },
			{ gem5, "Treasure with Gems" },
			{ gem6, "Expensive Relic" },
			{ gem7, "Safe with Gems" },
			{ starterPack1, "Newbie Set" },
			{ "starterpack2", "Golden Coins Extra Pack" },
			{ starterPack3, "Trooper Set" },
			{ "starterpack4", "Gems Extra Pack" },
			{ starterPack5, "Veteran Set" },
			{ "starterpack6", "Mega Gems Pack" },
			{ starterPack7, "Hero Set" },
			{ starterPack8, "Winner Set" }
		};
		elixirSettName = Defs.NumberOfElixirsSett;
		purchaseInProcess = false;
		restoreInProcess = false;
		categoryNames = new string[5] { "Armory", "Guns", "Melee", "Special", "Gear" };
		buyStarterPack = new List<string>();
		_state = new StoreKitEventListenerState();
		idsForSingle = new string[11]
		{
			"bigammopack", "Fullhealth", "ironSword", "MinerWeapon", "steelAxe", "spas", elixirID, "glock", "chainsaw", "scythe",
			"shovel"
		};
		idsForMulti = new string[10]
		{
			idsForSingle[2],
			idsForSingle[3],
			"steelAxe",
			"woodenBow",
			"combatrifle",
			"spas",
			"goldeneagle",
			idsForSingle[7],
			idsForSingle[8],
			"famas"
		};
	}

	internal static bool IsPayingUser()
	{
		return Storager.getInt("PayingUser") > 0;
	}

	public void ProvideContent()
	{
	}

	private static IEnumerator WaitForFyberAndSetIsPaying()
	{
		while (FyberFacade.Instance == null)
		{
			yield return null;
		}
		FyberFacade.Instance.SetUserPaying("1");
	}

	internal static decimal IncrementAccumulatedPayments(decimal payment)
	{
		decimal result;
		if (!Storager.hasKey("Analytics.AccumulatedPayments") || !decimal.TryParse(Storager.getString("Analytics.AccumulatedPayments"), out result))
		{
			result = default(decimal);
		}
		decimal result2 = result + payment;
		Storager.setString("Analytics.AccumulatedPayments", result2.ToString(CultureInfo.InvariantCulture));
		return result2;
	}

	internal static void CheckIfFirstTimePayment()
	{
		if (!Storager.hasKey("PayingUser") || Storager.getInt("PayingUser") != 1)
		{
			Storager.setInt("PayingUser", 1);
			if (CoroutineRunner.Instance != null)
			{
				CoroutineRunner.Instance.StartCoroutine(WaitForFyberAndSetIsPaying());
			}
			else
			{
				UnityEngine.Debug.LogError("CheckIfFirstTimePayment CoroutineRunner.Instance == null");
			}
		}
	}

	public static int GetDollarsSpent()
	{
		return PlayerPrefs.GetInt("ALLCoins", 0) + PlayerPrefs.GetInt("ALLGems", 0);
	}

	internal static void SetLastPaymentTime()
	{
		string value = DateTime.UtcNow.ToString("s");
		PlayerPrefs.SetString("Last Payment Time", value);
		Storager.setInt("PayingUser", 1);
		PlayerPrefs.SetString("Last Payment Time (Advertisement)", value);
	}

	public static void LogVirtualCurrencyPurchased(string productId, int gemsCount, int coinsCount)
	{
		try
		{
			if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.AnyDiscountForTryGuns)
			{
				AnalyticsStuff.LogWEaponsSpecialOffers_MoneySpended(productId);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("LogVirtualCurrencyPurchased exception (Weapons Special Offers): " + ex);
		}
		try
		{
			if (GiftBannerWindow.instance != null && GiftBannerWindow.instance.IsShow)
			{
				AnalyticsStuff.LogDailyGiftPurchases(productId);
			}
			if (ShopNGUIController.sharedShop.GunThatWeUsedInPolygon != null)
			{
				AnalyticsFacade.SendCustomEvent("Polygon", new Dictionary<string, object> { 
				{
					"Money Spended",
					AnalyticsStuff.ReadableNameForInApp(productId)
				} });
			}
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.LogError("LogVirtualCurrencyPurchased exception: " + ex2);
		}
		if (gemsCount > 0)
		{
			ShopNGUIController.AddBoughtCurrency("GemsCurrency", gemsCount);
		}
		if (coinsCount > 0)
		{
			ShopNGUIController.AddBoughtCurrency("Coins", coinsCount);
		}
		string value = PlayerPrefs.GetInt(Defs.SessionNumberKey, 1).ToString();
		string value2 = ((ExperienceController.sharedController != null) ? ExperienceController.sharedController.currentLevel.ToString() : "Unknown");
		string deviceModel = SystemInfo.deviceModel;
		Dictionary<string, string> dictionary = new Dictionary<string, string>(State.Parameters)
		{
			{ State.PurchaseKey, productId },
			{ "Rank", value2 },
			{ "Session number", value },
			{ "Device model", deviceModel }
		};
		if (gemsCount > 0)
		{
			string value3 = string.Format("{0} ({1})", new object[2] { productId, gemsCount });
			dictionary["SKU"] = value3;
			AnalyticsFacade.SendCustomEventToAppsFlyer(("Gems Purchased " + State.Mode) ?? string.Empty, dictionary);
		}
		if (coinsCount > 0)
		{
			string value4 = string.Format("{0} ({1})", new object[2] { productId, coinsCount });
			dictionary["SKU"] = value4;
			AnalyticsFacade.SendCustomEventToAppsFlyer(("Coins Purchased " + State.Mode) ?? string.Empty, dictionary);
		}
		int num = Array.IndexOf(coinIds, productId);
		bool flag = false;
		if (num == -1)
		{
			num = Array.IndexOf(gemsIds, productId);
			if (num == -1)
			{
				num = Array.IndexOf(inappOffersIds, productId);
				if (num == -1)
				{
					num = Array.IndexOf(starterPackIds, productId);
					flag = true;
				}
			}
		}
		if (num != -1 && !flag && num < VirtualCurrencyHelper.gemsPriceIds.Length)
		{
			int num2 = VirtualCurrencyHelper.gemsPriceIds[num];
			int @int = Storager.getInt(Defs.sumInnapsKey);
			@int += num2;
			Storager.setInt(Defs.sumInnapsKey, @int);
			try
			{
				TechnicalCloudInfo technicalCloudInfo = JsonUtility.FromJson<TechnicalCloudInfo>(Storager.getString("TechnicalCloudInfo.TECHNICAL_CLOUD_INFO_STORAGER_KEY")) ?? new TechnicalCloudInfo();
				int num3 = Math.Max(technicalCloudInfo.TotalInapps, @int);
				if (technicalCloudInfo.TotalInapps != num3)
				{
					technicalCloudInfo.TotalInapps = num3;
					string val = JsonUtility.ToJson(technicalCloudInfo);
					Storager.setString("TechnicalCloudInfo.TECHNICAL_CLOUD_INFO_STORAGER_KEY", val);
				}
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		if (((BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon) || BuildSettings.BuildTargetPlatform == RuntimePlatform.MetroPlayerX64) && FriendsController.sharedController != null)
		{
			FriendsController.sharedController.SendAddPurchaseEvent(productId, string.Empty, 0f, string.Empty, string.Empty);
		}
		if (num == -1)
		{
			return;
		}
		if (ABTestController.useBuffSystem)
		{
			if (gemsCount > 0)
			{
				BuffSystem.instance.OnCurrencyBuyed(true, gemsCount);
			}
			if (coinsCount > 0)
			{
				BuffSystem.instance.OnCurrencyBuyed(false, coinsCount);
			}
		}
		int num4 = 0;
		try
		{
			int num5 = (flag ? VirtualCurrencyHelper.starterPackFakePrice[num] : VirtualCurrencyHelper.gemsPriceIds[num]);
			int num6 = PlayerPrefs.GetInt("ALLGems", 0);
			int num7 = PlayerPrefs.GetInt("ALLCoins", 0);
			if (gemsCount > 0)
			{
				num6 += num5;
				PlayerPrefs.SetInt("ALLGems", num6);
			}
			else
			{
				num7 += num5;
				PlayerPrefs.SetInt("ALLCoins", num7);
			}
			num4 = num6 + num7;
		}
		catch (Exception ex3)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in calculating totalDollarsSpent: {0}", ex3);
		}
		if (!flag)
		{
			if (gemsCount > 0)
			{
				Storager.setInt(Defs.AllCurrencyBought + "GemsCurrency", Storager.getInt(Defs.AllCurrencyBought + "GemsCurrency") + gemsCount);
			}
			if (coinsCount > 0)
			{
				Storager.setInt(Defs.AllCurrencyBought + "Coins", Storager.getInt(Defs.AllCurrencyBought + "Coins") + coinsCount);
			}
		}
		int int2 = PlayerPrefs.GetInt("CountPaying", 0);
		int2++;
		PlayerPrefs.SetInt("CountPaying", int2);
		if (int2 >= 1 && PlayerPrefs.GetInt("Paying_User", 0) == 0)
		{
			PlayerPrefs.SetInt("Paying_User", 1);
			FacebookController.LogEvent("Paying_User");
			UnityEngine.Debug.Log("Paying_User detected.");
		}
		if (int2 > 1 && PlayerPrefs.GetInt("Paying_User_Dolphin", 0) == 0)
		{
			PlayerPrefs.SetInt("Paying_User_Dolphin", 1);
			FacebookController.LogEvent("Paying_User_Dolphin");
			UnityEngine.Debug.Log("Paying_User_Dolphin detected.");
		}
		if (int2 > 3 && PlayerPrefs.GetInt("Paying_User_Whale", 0) == 0)
		{
			PlayerPrefs.SetInt("Paying_User_Whale", 1);
			FacebookController.LogEvent("Paying_User_Whale");
			UnityEngine.Debug.Log("Paying_User_Whale detected.");
		}
		if (num4 >= 100 && PlayerPrefs.GetInt("SendKit", 0) == 0)
		{
			PlayerPrefs.SetInt("SendKit", 1);
			FacebookController.LogEvent("Whale_detected");
			UnityEngine.Debug.Log("Whale detected.");
		}
		if (PlayerPrefs.GetInt("confirmed_1st_time", 0) == 0)
		{
			PlayerPrefs.SetInt("confirmed_1st_time", 1);
			FacebookController.LogEvent("Purchase_confirmed_1st_time");
			UnityEngine.Debug.Log("Purchase confirmed first time.");
		}
		if (PlayerPrefs.GetInt("Active_loyal_users_payed_send", 0) == 0 && PlayerPrefs.GetInt("PostFacebookCount", 0) > 2 && PlayerPrefs.GetInt("PostVideo", 0) > 0)
		{
			FacebookController.LogEvent("Active_loyal_users_payed");
			PlayerPrefs.SetInt("Active_loyal_users_payed_send", 1);
		}
	}
}
