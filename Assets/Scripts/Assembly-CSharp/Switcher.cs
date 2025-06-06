using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Prime31;
using Rilisoft;
using UnityEngine;

[RequireComponent(typeof(FrameStopwatchScript))]
public sealed class Switcher : MonoBehaviour
{
	public static Dictionary<string, int> sceneNameToGameNum;

	public static Dictionary<string, int> counCreateMobsInLevel;

	public static string LoadingInResourcesPath;

	public static string[] loadingNames;

	public GameObject balanceControllerPrefab;

	public GameObject coinsShopPrefab;

	public GameObject nickStackPrefab;

	public GameObject skinsManagerPrefab;

	public GameObject ExperienceControllerPrefab;

	public GameObject weaponManagerPrefab;

	public GameObject experienceGuiPrefab;

	public GameObject bankGuiPrefab;

	public GameObject freeAwardGuiPrefab;

	public GameObject buttonClickSoundPrefab;

	public GameObject faceBookControllerPrefab;

	public GameObject potionsControllerPrefab;

	public GameObject protocolListGetterPrefab;

	public GameObject updateCheckerPrefab;

	public GameObject promoActionsManagerPrefab;

	public GameObject starterPackManagerPrefab;

	public GameObject tempItemsControllerPrefab;

	public GameObject remotePushNotificationControllerPrefab;

	public GameObject premiumAccountControllerPrefab;

	public GameObject twitterControllerPrefab;

	public GameObject sponsorPayPluginHolderPrefab;

	public GameObject giftControllerPrefab;

	public GameObject disabler;

	public GameObject sceneInfoController;

	private Rect plashkaCoinsRect;

	private Texture fonToDraw;

	private bool _newLaunchingApproach;

	public static Stopwatch timer;

	private static bool _initialAppVersionInitialized;

	private static string _InitialAppVersion;

	public static GameObject comicsSound;

	private float _progress;

	private float oldProgress;

	internal const string AbuseMethodKey = "AbuseMethod";

	private static AbuseMetod? _abuseMethod;

	public static string InitialAppVersion
	{
		get
		{
			if (!_initialAppVersionInitialized)
			{
				_InitialAppVersion = PlayerPrefs.GetString(Defs.InitialAppVersionKey);
				_initialAppVersionInitialized = true;
			}
			return _InitialAppVersion;
		}
		private set
		{
			_InitialAppVersion = value;
			_initialAppVersionInitialized = true;
		}
	}

	internal static AbuseMetod AbuseMethod
	{
		get
		{
			if (!_abuseMethod.HasValue)
			{
				_abuseMethod = (AbuseMetod)Storager.getInt("AbuseMethod");
			}
			return _abuseMethod.Value;
		}
	}

	internal static IEnumerable<float> InitializeStorager()
	{
		float progress = 0f;
		if (!Storager.hasKey(Defs.countryKey))
		{
			Storager.setString(Defs.countryKey, "");
			Storager.setString(Defs.continentKey, "");
			Storager.setString("inappBonusesConfigKey", "");
		}
		DoubleReward.InitializeStoragerKeysIfNeeded();
		if (!Storager.hasKey(Defs.keysInappBonusGivenkey))
		{
			Storager.setString(Defs.keysInappBonusGivenkey, "");
		}
		if (!Storager.hasKey(Defs.keyInappPresentIDWeaponRedkey))
		{
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDWeaponRedkey, "");
			Storager.setString(Defs.keyInappPresentIDWeaponRedkey, "");
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDPetkey, "");
			Storager.setString(Defs.keyInappPresentIDPetkey, "");
			Storager.setString(Defs.keyInappBonusStartActionForPresentIDGadgetkey, "");
			Storager.setString(Defs.keyInappPresentIDGadgetkey, "");
		}
		if (!Storager.hasKey(Defs.initValsInKeychain15))
		{
			Storager.setInt(Defs.initValsInKeychain15, 0);
			Storager.setInt(Defs.LobbyLevelApplied, 1);
			Storager.setString(Defs.CapeEquppedSN, Defs.CapeNoneEqupped);
			Storager.setString(Defs.HatEquppedSN, Defs.HatNoneEqupped);
			Storager.setInt(Defs.IsFirstLaunchFreshInstall, 1);
			yield return progress;
		}
		else if (Storager.getInt(Defs.LobbyLevelApplied) == 0)
		{
			Storager.setInt(Defs.ShownLobbyLevelSN, 3);
		}
		try
		{
			string @string = Storager.getString(Defs.HatEquppedSN);
			if (@string != null && (TempItemsController.PriceCoefs.ContainsKey(@string) || Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].Contains(@string)))
			{
				Storager.setString(Defs.HatEquppedSN, Defs.HatNoneEqupped);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogError("Exception in Trying to unequip armor hat or temp armor hat (mistakenly got from gocha as a gift): " + ex);
		}
		if (!Storager.hasKey(Defs.IsFirstLaunchFreshInstall))
		{
			Storager.setInt(Defs.IsFirstLaunchFreshInstall, 0);
		}
		progress = 0.25f;
		if (Application.isEditor || (Application.platform == RuntimePlatform.IPhonePlayer && UnityEngine.Debug.isDebugBuild) || (Application.platform == RuntimePlatform.IPhonePlayer && !Storager.hasKey(Defs.initValsInKeychain17)))
		{
			Storager.setInt(Defs.initValsInKeychain17, 0);
			float value = SecondsFrom1970();
			PlayerPrefs.SetFloat(Defs.TimeFromWhichShowEnder_SN, value);
		}
		if (!Storager.hasKey(Defs.initValsInKeychain27))
		{
			Storager.setInt(Defs.initValsInKeychain27, 0);
			Storager.setString(Defs.BootsEquppedSN, Defs.BootsNoneEqupped);
			yield return progress;
		}
		progress = 0.5f;
		yield return progress;
		if (!Storager.hasKey(Defs.initValsInKeychain40))
		{
			Storager.setInt(Defs.initValsInKeychain40, 0);
			Storager.setString(Defs.ArmorNewEquppedSN, Defs.ArmorNewNoneEqupped);
			Storager.setInt("GrenadeID", 5);
			yield return progress;
		}
		if (!Storager.IsInitialized(Defs.initValsInKeychain41))
		{
			Storager.setInt(Defs.initValsInKeychain41, 0);
			string hatBought = null;
			string visualHatArmor = null;
			if (Storager.getInt("hat_Almaz_1") > 0)
			{
				hatBought = "hat_Army_3";
				Storager.setInt("hat_Almaz_1", 0);
				Storager.setInt("hat_Royal_1", 0);
				Storager.setInt("hat_Steel_1", 0);
				visualHatArmor = "hat_Almaz_1";
				yield return progress;
			}
			else if (Storager.getInt("hat_Royal_1") > 0)
			{
				hatBought = "hat_Army_2";
				Storager.setInt("hat_Royal_1", 0);
				Storager.setInt("hat_Steel_1", 0);
				visualHatArmor = "hat_Royal_1";
				yield return progress;
			}
			else if (Storager.getInt("hat_Steel_1") > 0)
			{
				hatBought = "hat_Army_1";
				Storager.setInt("hat_Steel_1", 0);
				visualHatArmor = "hat_Steel_1";
				yield return progress;
			}
			if (hatBought != null)
			{
				string string2 = Storager.getString(Defs.HatEquppedSN);
				if (string2.Equals("hat_Almaz_1") || string2.Equals("hat_Royal_1") || string2.Equals("hat_Steel_1"))
				{
					Storager.setString(Defs.HatEquppedSN, hatBought);
				}
				for (int i = 0; i <= Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0].IndexOf(hatBought); i++)
				{
					Storager.setInt(Wear.wear[ShopNGUIController.CategoryNames.HatsCategory][0][i], 1);
					yield return progress;
				}
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				Storager.setString(Defs.VisualHatArmor, string.Empty);
			}
			if (visualHatArmor != null)
			{
				Storager.setString(Defs.VisualHatArmor, visualHatArmor);
			}
			if (!Storager.hasKey("LikeID"))
			{
				Storager.setInt("LikeID", 5);
			}
			yield return progress;
			string armorBought = null;
			string visualArmor = null;
			if (Storager.getInt("Armor_Almaz_1") > 0)
			{
				armorBought = "Armor_Army_3";
				Storager.setInt("Armor_Almaz_1", 0);
				Storager.setInt("Armor_Royal_1", 0);
				Storager.setInt("Armor_Steel_1", 0);
				visualArmor = "Armor_Almaz_1";
				yield return progress;
			}
			else if (Storager.getInt("Armor_Royal_1") > 0)
			{
				armorBought = "Armor_Army_2";
				Storager.setInt("Armor_Royal_1", 0);
				Storager.setInt("Armor_Steel_1", 0);
				visualArmor = "Armor_Royal_1";
				yield return progress;
			}
			else if (Storager.getInt("Armor_Steel_1") > 0)
			{
				armorBought = "Armor_Army_1";
				Storager.setInt("Armor_Steel_1", 0);
				visualArmor = "Armor_Steel_1";
				yield return progress;
			}
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				Storager.setString(Defs.VisualArmor, string.Empty);
			}
			if (visualArmor != null)
			{
				Storager.setString(Defs.VisualArmor, visualArmor);
			}
			yield return progress;
			if (armorBought != null)
			{
				string string3 = Storager.getString(Defs.ArmorNewEquppedSN);
				if (string3.Equals("Armor_Almaz_1") || string3.Equals("Armor_Royal_1") || string3.Equals("Armor_Steel_1"))
				{
					Storager.setString(Defs.ArmorNewEquppedSN, armorBought);
					yield return progress;
				}
				for (int j = 0; j <= Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].IndexOf(armorBought); j++)
				{
					Storager.setInt(Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0][j], 1);
					yield return progress;
				}
			}
		}
		progress = 0.75f;
		if (!Storager.IsInitialized(Defs.initValsInKeychain43))
		{
			Storager.SetInitialized(Defs.initValsInKeychain43);
			PlayerPrefs.SetString(Defs.StartTimeShowBannersKey, DateTimeOffset.UtcNow.ToString("s"));
			PlayerPrefs.Save();
			yield return progress;
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
			{
				Storager.setInt(Defs.NeedTakeMarathonBonus, 0);
				Storager.setInt(Defs.NextMarafonBonusIndex, 0);
				yield return progress;
			}
		}
		if (!Storager.hasKey(GearManager.MusicBox))
		{
			Storager.setInt(GearManager.MusicBox, 2);
			Storager.setInt(GearManager.Wings, 2);
			Storager.setInt(GearManager.Bear, 2);
			Storager.setInt(GearManager.BigHeadPotion, 2);
		}
		Defs.StartTimeShowBannersString = PlayerPrefs.GetString(Defs.StartTimeShowBannersKey, string.Empty);
		UnityEngine.Debug.Log(" StartTimeShowBannersString=" + Defs.StartTimeShowBannersString);
		if (!Storager.IsInitialized(Defs.initValsInKeychain44))
		{
			Storager.SetInitialized(Defs.initValsInKeychain44);
			if (Storager.hasKey(Defs.NextMarafonBonusIndex) && Storager.getInt(Defs.NextMarafonBonusIndex) == -1)
			{
				Storager.setInt(Defs.NextMarafonBonusIndex, 0);
			}
			yield return progress;
		}
		if (!Storager.IsInitialized(Defs.initValsInKeychain45))
		{
			Storager.SetInitialized(Defs.initValsInKeychain45);
			Storager.setInt(Defs.PremiumEnabledFromServer, 0);
			if (Storager.getInt("currentLevel2") == 0)
			{
				PlayerPrefs.SetString(Defs.DateOfInstallAppForInAppPurchases041215, DateTime.UtcNow.ToString("s"));
			}
			yield return progress;
		}
		if (!Storager.IsInitialized(Defs.initValsInKeychain46))
		{
			Storager.SetInitialized(Defs.initValsInKeychain46);
			Storager.setString("MaskEquippedSN", "MaskNoneEquipped");
			yield return progress;
		}
		if (!Storager.hasKey("Win Count Timestamp"))
		{
			Storager.setString("Win Count Timestamp", "{ \"1970-01-01\": 0 }");
		}
		if (!Storager.hasKey("StartTimeShowStarterPack"))
		{
			Storager.setString("StartTimeShowStarterPack", string.Empty);
			yield return progress;
		}
		if (!Storager.hasKey("TimeEndStarterPack"))
		{
			Storager.setString("TimeEndStarterPack", string.Empty);
			yield return progress;
		}
		if (!Storager.hasKey("NextNumberStarterPack"))
		{
			Storager.setInt("NextNumberStarterPack", 0);
			yield return progress;
		}
		if (!Storager.hasKey(Defs.ArmorEquppedSN))
		{
			Storager.setString(Defs.ArmorEquppedSN, Defs.ArmorNoneEqupped);
		}
		if (!Storager.hasKey(Defs.ShowSorryWeaponAndArmor))
		{
			Storager.setInt(Defs.ShowSorryWeaponAndArmor, 0);
		}
		if (Storager.getInt(Defs.IsFirstLaunchFreshInstall) > 0)
		{
			Storager.setInt(Defs.IsFirstLaunchFreshInstall, 0);
		}
		if (!Storager.hasKey(Defs.NewbieEventX3StartTime))
		{
			Storager.setString(Defs.NewbieEventX3StartTime, 0L.ToString());
			Storager.setString(Defs.NewbieEventX3StartTimeAdditional, 0L.ToString());
			Storager.setString(Defs.NewbieEventX3LastLoggedTime, 0L.ToString());
			PlayerPrefs.SetInt(Defs.WasNewbieEventX3, 0);
		}
		if (!PlayerPrefs.HasKey(Defs.LastTimeUpdateAvailableShownSN))
		{
			DateTime dateTime = new DateTime(1970, 1, 9, 0, 0, 0);
			DateTimeOffset dateTimeOffset = new DateTimeOffset(dateTime);
			PlayerPrefs.SetString(Defs.LastTimeUpdateAvailableShownSN, dateTimeOffset.ToString("s"));
			PlayerPrefs.Save();
		}
		string string4 = PlayerPrefs.GetString(Defs.LastTimeUpdateAvailableShownSN);
		DateTimeOffset result = default(DateTimeOffset);
		if (!DateTimeOffset.TryParse(string4, out result) && UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.LogWarning("Cannot parse " + string4);
		}
		if (DateTimeOffset.Now - result > TimeSpan.FromHours(12.0))
		{
			PlayerPrefs.SetInt(Defs.UpdateAvailableShownTimesSN, 3);
			PlayerPrefs.SetString(Defs.LastTimeUpdateAvailableShownSN, DateTimeOffset.Now.ToString("s"));
			yield return progress;
		}
		yield return progress;
		if (!PlayerPrefs.HasKey(Defs.AdvertWindowShownLastTime))
		{
			PlayerPrefs.SetInt(Defs.AdvertWindowShownCount, 3);
			PlayerPrefs.SetString(Defs.AdvertWindowShownLastTime, PromoActionsManager.CurrentUnixTime.ToString());
		}
		long result2;
		long.TryParse(PlayerPrefs.GetString(Defs.AdvertWindowShownLastTime), out result2);
		float num = (Defs.IsDeveloperBuild ? (1f / 12f) : 12f);
		if (PromoActionsManager.CurrentUnixTime - result2 > (long)TimeSpan.FromHours(num).TotalSeconds)
		{
			PlayerPrefs.SetInt(Defs.AdvertWindowShownCount, 3);
			PlayerPrefs.SetString(Defs.AdvertWindowShownLastTime, PromoActionsManager.CurrentUnixTime.ToString());
		}
		yield return progress;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			if (!Storager.hasKey(Defs.LevelsWhereGetCoinS))
			{
				CoinBonus.SetLevelsWhereGotBonus(string.Empty, VirtualCurrencyBonusType.Coin);
			}
			if (!Storager.hasKey(Defs.LevelsWhereGotGems))
			{
				CoinBonus.SetLevelsWhereGotBonus("[]", VirtualCurrencyBonusType.Gem);
			}
			if (!Storager.hasKey(Defs.RatingFlag))
			{
				Storager.setInt(Defs.RatingDeathmatch, 0);
				Storager.setInt(Defs.RatingTeamBattle, 0);
				Storager.setInt(Defs.RatingHunger, 0);
				Storager.setInt(Defs.RatingFlag, 0);
			}
			if (!Storager.hasKey(Defs.RatingCapturePoint))
			{
				Storager.setInt(Defs.RatingCapturePoint, 0);
			}
		}
		PlayerPrefs.Save();
		yield return 1f;
	}

	static Switcher()
	{
		sceneNameToGameNum = new Dictionary<string, int>();
		counCreateMobsInLevel = new Dictionary<string, int>();
		LoadingInResourcesPath = "LevelLoadings";
		loadingNames = new string[17]
		{
			"Loading_coliseum", "loading_Cementery", "Loading_Maze", "Loading_City", "Loading_Hospital", "Loading_Jail", "Loading_end_world_2", "Loading_Arena", "Loading_Area52", "Loading_Slender",
			"Loading_Hell", "Loading_bloody_farm", "Loading_most", "Loading_school", "Loading_utopia", "Loading_sky", "Loading_winter"
		};
		timer = new Stopwatch();
		_initialAppVersionInitialized = false;
		_InitialAppVersion = string.Empty;
		sceneNameToGameNum.Add("Training", 0);
		sceneNameToGameNum.Add("Cementery", 1);
		sceneNameToGameNum.Add("Maze", 2);
		sceneNameToGameNum.Add("City", 3);
		sceneNameToGameNum.Add("Hospital", 4);
		sceneNameToGameNum.Add("Jail", 5);
		sceneNameToGameNum.Add("Gluk_2", 6);
		sceneNameToGameNum.Add("Arena", 7);
		sceneNameToGameNum.Add("Area52", 8);
		sceneNameToGameNum.Add("Slender", 9);
		sceneNameToGameNum.Add("Castle", 10);
		sceneNameToGameNum.Add("Farm", 11);
		sceneNameToGameNum.Add("Bridge", 12);
		sceneNameToGameNum.Add("School", 13);
		sceneNameToGameNum.Add("Utopia", 14);
		sceneNameToGameNum.Add("Sky_islands", 15);
		sceneNameToGameNum.Add("Winter", 16);
		sceneNameToGameNum.Add("Swamp_campaign3", 17);
		sceneNameToGameNum.Add("Castle_campaign3", 18);
		sceneNameToGameNum.Add("Space_campaign3", 19);
		sceneNameToGameNum.Add("Parkour", 20);
		sceneNameToGameNum.Add("Code_campaign3", 21);
		counCreateMobsInLevel.Add("Farm", 10);
		counCreateMobsInLevel.Add("Cementery", 15);
		counCreateMobsInLevel.Add("City", 20);
		counCreateMobsInLevel.Add("Hospital", 25);
		counCreateMobsInLevel.Add("Bridge", 25);
		counCreateMobsInLevel.Add("Jail", 30);
		counCreateMobsInLevel.Add("Slender", 30);
		counCreateMobsInLevel.Add("Area52", 35);
		counCreateMobsInLevel.Add("School", 35);
		counCreateMobsInLevel.Add("Utopia", 25);
		counCreateMobsInLevel.Add("Maze", 30);
		counCreateMobsInLevel.Add("Sky_islands", 30);
		counCreateMobsInLevel.Add("Winter", 30);
		counCreateMobsInLevel.Add("Castle", 35);
		counCreateMobsInLevel.Add("Gluk_2", 35);
		counCreateMobsInLevel.Add("Swamp_campaign3", 30);
		counCreateMobsInLevel.Add("Castle_campaign3", 35);
		counCreateMobsInLevel.Add("Space_campaign3", 25);
		counCreateMobsInLevel.Add("Parkour", 15);
		counCreateMobsInLevel.Add("Code_campaign3", 35);
	}

	private static double Hypot(double x, double y)
	{
		x = Math.Abs(x);
		y = Math.Abs(y);
		double num = Math.Max(x, y);
		double num2 = Math.Min(x, y) / num;
		return num * Math.Sqrt(1.0 + num2 * num2);
	}

	private IEnumerator ParseConfigsCoroutine()
	{
		float realtimeSinceStartup = Time.realtimeSinceStartup;
		using (new ScopeLogger("Switcher.Start()", "Parsing advert config", Defs.IsDeveloperBuild))
		{
			if (Storager.hasKey("abTestAdvertConfigKey"))
			{
				FriendsController.ParseABTestAdvertConfig();
			}
			else
			{
				Storager.setString("abTestAdvertConfigKey", string.Empty);
			}
		}
		if (Time.realtimeSinceStartup - realtimeSinceStartup > 1f / 60f)
		{
			float realtimeSinceStartup2 = Time.realtimeSinceStartup;
			yield return null;
		}
	}

	private IEnumerator Start()
	{
		Storager.setInt("currentLevel2", 1);
		Storager.setInt("currentLevel3", 1);
		//MemoryCheatHook.Initialize();
		oldProgress = 0f;
		UnityEngine.Debug.LogFormat("> Switcher.Start(): {0:f3}, {1}", Time.realtimeSinceStartup, Time.frameCount);
		yield return StartCoroutine(ParseConfigsCoroutine());
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			PlayComicsSound();
		}
		UnityEngine.Debug.Log("Switcher.Start() > InitializeSwitcher()");
		foreach (float item in InitializeSwitcher())
		{
			timer.Reset();
			timer.Start();
			oldProgress = _progress;
			ActivityIndicator.LoadingProgress = _progress;
			yield return item;
		}
		if (Storager.getInt("Coins") > 50000 || Storager.getInt("GemsCurrency") > 50000)
		{
			CheatDetectedBanner.ShowAndClearProgress();
		}
		using (new ScopeLogger("Switcher.Start()", "Loading main menu asynchronously", Defs.IsDeveloperBuild))
		{
			foreach (float item2 in LoadMainMenu())
			{
				timer.Reset();
				timer.Start();
				oldProgress = _progress;
				ActivityIndicator.LoadingProgress = _progress;
				yield return item2;
			}
		}
		UnityEngine.Debug.LogFormat("< Switcher.Start(): {0:f3}, {1}", Time.realtimeSinceStartup, Time.frameCount);
	}

	public static string LoadingCupTexture(int number)
	{
		return "loading_cups_" + number + (Device.isRetinaAndStrong ? "-hd" : string.Empty);
	}

	public IEnumerable<float> InitializeSwitcher()
	{
		UnityEngine.Debug.Log("> InitializeSwitcher()");
		Stopwatch _stopwatch = new Stopwatch();
		ProgressBounds bounds = new ProgressBounds();
		Action logBounds = delegate
		{
		};
		InGameTimeKeeper.Instance.Initialize();
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			string path = LoadingCupTexture(1);
			fonToDraw = Resources.Load<Texture>(path);
			string text = LocalizationStore.Get("Key_1925");
			ActivityIndicator.instance.legendLabel.text = text;
			ActivityIndicator.instance.legendLabel.gameObject.SetActive(true);
		}
		else
		{
			fonToDraw = ConnectScene.MainLoadingTexture();
		}
		ActivityIndicator.SetLoadingFon(fonToDraw);
		ActivityIndicator.IsShowWindowLoading = true;
		ActivityIndicator.instance.panelProgress.SetActive(true);
		bounds.SetBounds(0f, 0.09f);
		logBounds();
		_progress = bounds.LowerBound;
		yield return _progress;
		if (!PlayerPrefs.HasKey("First Launch (Advertisement)"))
		{
			PlayerPrefs.SetString("First Launch (Advertisement)", DateTimeOffset.UtcNow.ToString("s"));
		}
		if (!PlayerPrefs.HasKey(Defs.InitialAppVersionKey))
		{
			if (!PlayerPrefs.HasKey("NamePlayer"))
			{
				PlayerPrefs.SetString(Defs.InitialAppVersionKey, GlobalGameController.AppVersion);
			}
			else
			{
				PlayerPrefs.SetString(Defs.InitialAppVersionKey, "1.0.0");
			}
		}
		InitialAppVersion = PlayerPrefs.GetString(Defs.InitialAppVersionKey);
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			AbstractManager.initialize(typeof(GoogleIABManager));
		}
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			try
			{
				if (Defs.IsDeveloperBuild)
				{
					UnityEngine.Debug.Log("Switcher: Trying to initialize Google Play Games...");
				}
				GpgFacade.Instance.Initialize();
			}
			catch (Exception exception)
			{
				UnityEngine.Debug.LogException(exception);
			}
		}
		_progress = bounds.Clamp(_progress + 0.005f);
		yield return _progress;
		if (sponsorPayPluginHolderPrefab != null)
		{
			UnityEngine.Object.Instantiate(sponsorPayPluginHolderPrefab);
		}
		_progress = bounds.Clamp(_progress + 0.005f);
		yield return _progress;
		UnityEngine.Object.Instantiate(balanceControllerPrefab);
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		GlobalGameController.LeftHanded = PlayerPrefs.GetInt(Defs.LeftHandedSN, 1) == 1;
		if (!PlayerPrefs.HasKey(Defs.SwitchingWeaponsSwipeRegimSN))
		{
			double num = Hypot(Screen.width, Screen.height);
			int value = 0;
			if (Screen.dpi > 0f)
			{
				double num2 = num / (double)Screen.dpi;
				if (UnityEngine.Debug.isDebugBuild)
				{
					UnityEngine.Debug.Log(string.Format("Device dpi: {0},    diagonal: {1} px ({2}\")", new object[3]
					{
						Screen.dpi,
						num,
						num2
					}));
				}
				value = ((!(num2 < 6.8)) ? 1 : 0);
			}
			else if (UnityEngine.Debug.isDebugBuild)
			{
				UnityEngine.Debug.Log(string.Format("Device dpi: {0},    diagonal: {1} px", new object[2]
				{
					Screen.dpi,
					num
				}));
			}
			PlayerPrefs.SetInt(Defs.SwitchingWeaponsSwipeRegimSN, 0);
		}
		GlobalGameController.switchingWeaponSwipe = PlayerPrefs.GetInt(Defs.SwitchingWeaponsSwipeRegimSN, 0) == 1;
		string text2 = Load.LoadString("keyOldVersion");
		string appVersion = GlobalGameController.AppVersion;
		if (text2 != appVersion)
		{
			PlayerPrefs.SetInt("countSessionDayOnStartCorrentVersion", PlayerPrefs.GetInt(Defs.SessionDayNumberKey, 1));
			ReviewController.IsSendReview = false;
			ReviewController.ExistReviewForSend = false;
			ReviewController.CheckActiveReview();
			Save.SaveString("keyOldVersion", appVersion);
		}
		Tools.AddSessionNumber();
		CoroutineRunner.Instance.StartCoroutine(AnalyticsStuff.WaitInitializationThenLogGameDayCountCoroutine());
		if (!Storager.hasKey(Defs.WeaponsGotInCampaign))
		{
			Storager.setString(Defs.WeaponsGotInCampaign, "");
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		Screen.sleepTimeout = 180;
		if (SkinsController.sharedController == null && (bool)skinsManagerPrefab)
		{
			UnityEngine.Object.Instantiate(skinsManagerPrefab, Vector3.zero, Quaternion.identity);
			_progress = bounds.Clamp(_progress + 0.01f);
			yield return _progress;
			foreach (float item in SkinsController.sharedController.LoadSkinsInTexture())
			{
				float num6 = item;
				yield return _progress;
			}
		}
		if (PromoActionsManager.sharedManager == null && promoActionsManagerPrefab != null)
		{
			UnityEngine.Object.Instantiate(promoActionsManagerPrefab);
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (nickStackPrefab == null)
		{
			UnityEngine.Debug.LogError("Switcher.InitializeSwitcher():    nickStackPrefab == null");
		}
		else if (NickLabelStack.sharedStack == null)
		{
			UnityEngine.Object.Instantiate(nickStackPrefab, Vector3.zero, Quaternion.identity);
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (sceneInfoController == null)
		{
			UnityEngine.Debug.LogError("Switcher.InitializeSwitcher():    sceneInfoController == null");
		}
		else
		{
			UnityEngine.Object.Instantiate(sceneInfoController, Vector3.zero, Quaternion.identity);
		}
		if (ExperienceControllerPrefab == null)
		{
			UnityEngine.Debug.LogError("Switcher.InitializeSwitcher():    ExperienceControllerPrefab == null");
		}
		else if (ExperienceController.sharedController == null)
		{
			UnityEngine.Object.Instantiate(ExperienceControllerPrefab, Vector3.zero, Quaternion.identity);
			_progress = bounds.Lerp(_progress, 0.6f);
			yield return _progress;
			foreach (float item2 in ExperienceController.sharedController.InitController())
			{
				float num7 = item2;
				_progress = bounds.Clamp(_progress + 0.01f);
				yield return _progress;
			}
		}
		bounds.SetBounds(0.1f, 0.19f);
		logBounds();
		_progress = bounds.LowerBound;
		yield return _progress;
		if (experienceGuiPrefab != null)
		{
			if (ExpController.Instance == null)
			{
				UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate(experienceGuiPrefab, Vector3.zero, Quaternion.identity));
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("ExperienceGuiPrefab == null");
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (bankGuiPrefab != null)
		{
			if (BankController.Instance == null)
			{
				UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate(bankGuiPrefab, Vector3.zero, Quaternion.identity));
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("BankGuiPrefab == null");
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (freeAwardGuiPrefab != null)
		{
			if (FreeAwardController.Instance == null)
			{
				UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate(freeAwardGuiPrefab, Vector3.zero, Quaternion.identity));
			}
		}
		else
		{
			UnityEngine.Debug.LogWarning("freeAwardGuiPrefab == null");
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		AnalyticsFacade.Initialize();
		PersistentCache instance = PersistentCache.Instance;
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogFormat("Persistent cache: '{0}'", instance.PersistentDataPath);
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (RemotePushNotificationController.Instance == null && (bool)remotePushNotificationControllerPrefab)
		{
			UnityEngine.Object.Instantiate(remotePushNotificationControllerPrefab);
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (ShopNGUIController.sharedShop == null)
		{
			ResourceRequest shopTask = Resources.LoadAsync("ShopNGUI");
			while (!shopTask.isDone)
			{
				yield return _progress;
			}
			UnityEngine.Object shopP = shopTask.asset;
			_progress = bounds.Clamp(_progress + 0.01f);
			yield return _progress;
			UnityEngine.Object.Instantiate(shopP, Vector3.zero, Quaternion.identity);
		}
		bounds.SetBounds(0.2f, 0.29f);
		logBounds();
		_progress = bounds.LowerBound;
		yield return _progress;
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (Storager.getInt("InitBanerAwardCompition") == 0)
		{
			if (RatingSystem.instance.currentLeague == RatingSystem.RatingLeague.Adamant)
			{
				TournamentAvailableBannerWindow.CanShow = true;
				int trophiesSeasonThreshold = RatingSystem.instance.TrophiesSeasonThreshold;
				if (RatingSystem.instance.currentRating > trophiesSeasonThreshold)
				{
					int num3 = RatingSystem.instance.currentRating - trophiesSeasonThreshold;
					RatingSystem.instance.negativeRating += num3;
					RatingSystem.instance.UpdateLeagueEvent();
				}
			}
			Storager.setInt("InitBanerAwardCompition", 1);
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (FriendsController.sharedController == null)
		{
			ResourceRequest friendsControllerTask = Resources.LoadAsync("FriendsController");
			while (!friendsControllerTask.isDone)
			{
				yield return _progress;
			}
			UnityEngine.Object fcp = friendsControllerTask.asset;
			_progress = bounds.Clamp(_progress + 0.01f);
			yield return _progress;
			UnityEngine.Object.Instantiate(fcp, Vector3.zero, Quaternion.identity);
			yield return _progress;
			foreach (float item3 in FriendsController.sharedController.InitController())
			{
				float num8 = item3;
				yield return _progress;
			}
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			fonToDraw = Resources.Load<Texture>(LoadingCupTexture(2));
			foreach (float item4 in ActivityIndicator.instance.ReplaceLoadingFon(fonToDraw, 0.3f))
			{
				float num9 = item4;
				yield return _progress;
			}
			ActivityIndicator.instance.legendLabel.text = LocalizationStore.Get("Key_1926");
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		_stopwatch.Start();
		foreach (float item5 in InitializeStorager())
		{
			float num10 = item5;
			if (_stopwatch.ElapsedMilliseconds > 100)
			{
				_stopwatch.Reset();
				_stopwatch.Start();
				yield return _progress;
			}
		}
		_stopwatch.Reset();
		BankController.GiveInitialNumOfCoins();
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (disabler != null)
		{
			UnityEngine.Object.Instantiate(disabler);
		}
		bounds.SetBounds(0.3f, 0.39f);
		logBounds();
		_progress = bounds.LowerBound;
		yield return _progress;
		bounds.SetBounds(0.4f, 0.49f);
		logBounds();
		_progress = bounds.LowerBound;
		yield return _progress;
		_progress = bounds.Clamp(_progress + 0.001f);
		yield return _progress;
		_progress = bounds.Clamp(_progress + 0.001f);
		yield return _progress;
		WeaponManager.ActualizeWeaponsForCampaignProgress();
		_progress = bounds.Clamp(0.41f);
		yield return _progress;
		if (coinsShop.thisScript == null && (bool)coinsShopPrefab)
		{
			UnityEngine.Object.Instantiate(coinsShopPrefab);
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (FacebookController.sharedController == null && FacebookController.FacebookSupported && faceBookControllerPrefab != null)
		{
			UnityEngine.Object.Instantiate(faceBookControllerPrefab);
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (ButtonClickSound.Instance == null && buttonClickSoundPrefab != null)
		{
			UnityEngine.Object.Instantiate(buttonClickSoundPrefab);
		}
		_progress = bounds.Clamp(_progress + 0.005f);
		yield return _progress;
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		bool flag = TempItemsController.sharedController == null && tempItemsControllerPrefab != null;
		if (Defs.IsDeveloperBuild)
		{
			UnityEngine.Debug.LogFormat("Loading {0:P1} > Instantiate Temp Items: {1}", _progress, flag);
		}
		if (flag)
		{
			UnityEngine.Object.Instantiate(tempItemsControllerPrefab);
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (updateCheckerPrefab != null)
		{
			UnityEngine.Object.Instantiate(updateCheckerPrefab);
		}
		bounds.SetBounds(0.5f, 0.52f);
		logBounds();
		_progress = bounds.LowerBound;
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			yield return _progress;
			fonToDraw = Resources.Load<Texture>(LoadingCupTexture(3));
			foreach (float item6 in ActivityIndicator.instance.ReplaceLoadingFon(fonToDraw, 0.3f))
			{
				float num11 = item6;
				yield return _progress;
			}
			ActivityIndicator.instance.legendLabel.text = LocalizationStore.Get("Key_1927");
		}
		yield return _progress;
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		_progress = bounds.Clamp(_progress + 0.01f);
		WeaponManager wm = null;
		using (new ScopeLogger("Loading " + _progress.ToString("P1", CultureInfo.InvariantCulture), "Instantiate WeaponManager.", Defs.IsDeveloperBuild))
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(weaponManagerPrefab, Vector3.zero, Quaternion.identity);
			wm = gameObject.GetComponent<WeaponManager>();
		}
		bounds.SetBounds(0.52f, 0.88f);
		logBounds();
		_progress = bounds.LowerBound;
		yield return _progress;
		if (wm != null)
		{
			int i = 0;
			while (!wm.Initialized)
			{
				_progress = bounds.Clamp(_progress + 0.01f);
				yield return _progress;
				if (Launcher.UsingNewLauncher)
				{
					yield return -1f;
				}
				int num4 = i + 1;
				i = num4;
			}
		}
		yield return _progress;
		bounds.SetBounds(0.89f, 0.99f);
		logBounds();
		_progress = bounds.LowerBound;
		yield return _progress;
		SetUpPhoton(MiscAppsMenu.Instance.misc);
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		CheckHugeUpgrade();
		PerformEssentialInitialization("Coins", AbuseMetod.Coins);
		PerformEssentialInitialization("GemsCurrency", AbuseMetod.Gems);
		PerformExpendablesInitialization();
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		CampaignProgress.OpenNewBoxIfPossible();
		if (StarterPackController.Get == null && starterPackManagerPrefab != null)
		{
			UnityEngine.Object.Instantiate(starterPackManagerPrefab);
		}
		if (PotionsController.sharedController == null && potionsControllerPrefab != null)
		{
			UnityEngine.Object.Instantiate(potionsControllerPrefab);
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		QuestSystem.Instance.Initialize();
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (PremiumAccountController.Instance == null && premiumAccountControllerPrefab != null)
		{
			UnityEngine.Object.Instantiate(premiumAccountControllerPrefab);
		}
		_progress = bounds.Clamp(_progress + 0.01f);
		yield return _progress;
		if (GiftController.Instance == null && giftControllerPrefab != null)
		{
			UnityEngine.Object.Instantiate(giftControllerPrefab);
		}
		Screen.sleepTimeout = 180;
		_progress = bounds.Clamp(_progress + 0.01f);
		while (!Singleton<AchievementsManager>.Instance.IsReady)
		{
			yield return _progress;
		}
		_progress = 0.96f;
		yield return _progress;
	}

	private void SetUpPhoton(HiddenSettings settings)
	{
		string text = SelectPhotonAppId(settings);
		if (Defs.IsDeveloperBuild)
		{
			new Dictionary<string, string>
			{
				{ "appId", text },
				{
					"defaultAppId",
					PhotonNetwork.PhotonServerSettings.AppID
				}
			};
		}
		PhotonNetwork.PhotonServerSettings.AppID = text;
	}

	private static string SelectPhotonAppId(HiddenSettings settings)
	{
		byte[] bytes = Convert.FromBase64String(settings.PhotonAppIdPad);
		byte[] array = Convert.FromBase64String(settings.PhotonAppIdEncoded);
		byte[] array2 = new byte[array.Length];
		((ICollection)new BitArray(bytes).Xor(new BitArray(array))).CopyTo((Array)array2, 0);
		return Encoding.UTF8.GetString(array2, 0, array2.Length);
	}

	public static void PlayComicsSound()
	{
		if (!(comicsSound != null))
		{
			GameObject gameObject = Resources.Load<GameObject>("BackgroundMusic/Background_Comics");
			if (gameObject == null)
			{
				UnityEngine.Debug.LogWarning("ComicsSoundPrefab is null.");
				return;
			}
			comicsSound = UnityEngine.Object.Instantiate(gameObject);
			UnityEngine.Object.DontDestroyOnLoad(comicsSound);
		}
	}

	private static void CheckHugeUpgrade()
	{
		bool num = Storager.hasKey("Coins");
		bool flag = Storager.hasKey(Defs.ArmorNewEquppedSN);
		if (num && !flag)
		{
			AppendAbuseMethod(AbuseMetod.UpgradeFromVulnerableVersion);
			UnityEngine.Debug.LogError("Upgrade tampering detected: " + AbuseMethod);
		}
	}

	private static void PerformEssentialInitialization(string currencyKey, AbuseMetod abuseMethod)
	{
		if (!Storager.hasKey(currencyKey))
		{
			return;
		}
		int @int = Storager.getInt(currencyKey);
		DigestStorager.Instance.Set(currencyKey, @int);
	}

	[Obsolete("Because of issues with CryptoPlayerPrefs")]
	private static void PerformWeaponInitialization()
	{
		int value = WeaponManager.storeIDtoDefsSNMapping.Values.Where((string w) => Storager.getInt(w) == 1).Count();
		if (DigestStorager.Instance.ContainsKey("WeaponsCount"))
		{
			if (!DigestStorager.Instance.Verify("WeaponsCount", value))
			{
				AppendAbuseMethod(AbuseMetod.Weapons);
				UnityEngine.Debug.LogError("Weapon tampering detected: " + AbuseMethod);
			}
		}
		else
		{
			DigestStorager.Instance.Set("WeaponsCount", value);
		}
	}

	private static void PerformExpendablesInitialization()
	{
		byte[] value = new string[4]
		{
			GearManager.InvisibilityPotion,
			GearManager.Jetpack,
			GearManager.Turret,
			GearManager.Mech
		}.SelectMany((string key) => BitConverter.GetBytes(Storager.getInt(key))).ToArray();
		if (DigestStorager.Instance.ContainsKey("ExpendablesCount"))
		{
			if (!DigestStorager.Instance.Verify("ExpendablesCount", value))
			{
				AppendAbuseMethod(AbuseMetod.Expendables);
				UnityEngine.Debug.LogError("Expendables tampering detected: " + AbuseMethod);
			}
		}
		else
		{
			DigestStorager.Instance.Set("ExpendablesCount", value);
		}
	}

	private static void ClearProgress()
	{
	}

	public IEnumerable<float> LoadMainMenu()
	{
		using (new ScopeLogger("Switcher.LoadMainMenu()", Defs.IsDeveloperBuild))
		{
			if (!TrainingController.TrainingCompleted && CloudSyncController.AreProgressInCurrentPullResult())
			{
				TrainingController.OnGetProgress();
				TrainingController.ShouldSyncInLobbyAfterSkippingRancho = true;
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
			{
				GameConnect.SetGameMode(GameConnect.GameMode.Deathmatch);
				Defs.isMulti = false;
				GlobalGameController.Score = 0;
				WeaponManager.sharedManager.CurrentWeaponIndex = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>().ToList().FindIndex((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 1);
			}
			string sceneName = ((!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None) ? Defs.TrainingSceneName : DetermineSceneName());
			_progress = 0.96f;
			yield return _progress;
			AsyncOperation loadLevelTask = Singleton<SceneLoader>.Instance.LoadSceneAsync(sceneName);
			while (!loadLevelTask.isDone)
			{
				_progress = 0.96f + loadLevelTask.progress / 30f;
				yield return _progress;
			}
			CoroutineRunner.Instance.StartCoroutine(WaitLoadSceneAsyncOperation(loadLevelTask, sceneName, 0.96f));
		}
	}

	private IEnumerator WaitLoadSceneAsyncOperation(AsyncOperation loadSceneAsyncOperation, string sceneName, float leftBound)
	{
		using (new ScopeLogger("Switcher.WaitLoadSceneAsyncOperation(): " + sceneName, Defs.IsDeveloperBuild))
		{
			while (!loadSceneAsyncOperation.isDone)
			{
				_progress = leftBound + loadSceneAsyncOperation.progress / 50f;
				yield return _progress;
			}
		}
	}

	private static bool IsWeaponBought(string weaponTag)
	{
		string value;
		string value2;
		if (WeaponManager.tagToStoreIDMapping.TryGetValue(weaponTag, out value) && value != null && WeaponManager.storeIDtoDefsSNMapping.TryGetValue(value, out value2) && value2 != null && Storager.hasKey(value2))
		{
			return Storager.getInt(value2) > 0;
		}
		return false;
	}

	public static float SecondsFrom1970()
	{
		DateTime dateTime = new DateTime(1970, 1, 9, 0, 0, 0);
		return (float)(DateTime.Now - dateTime).TotalSeconds;
	}

	private void OnDestroy()
	{
		ActivityIndicator.IsShowWindowLoading = false;
	}

	private static string DetermineSceneName()
	{
		switch (GlobalGameController.currentLevel)
		{
		case -1:
			return Defs.MainMenuScene;
		case 0:
			return "Cementery";
		case 1:
			return "Maze";
		case 2:
			return "City";
		case 3:
			return "Hospital";
		case 4:
			return "Jail";
		case 5:
			return "Gluk_2";
		case 6:
			return "Arena";
		case 7:
			return "Area52";
		case 101:
			return "Training";
		case 8:
			return "Slender";
		case 9:
			return "Castle";
		default:
			return Defs.MainMenuScene;
		}
	}

	internal static void AppendAbuseMethod(AbuseMetod f)
	{
		return;
		// _abuseMethod = AbuseMethod | f;
		// Storager.setInt("AbuseMethod", (int)_abuseMethod.Value);
	}
}
