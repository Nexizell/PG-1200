using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;

public sealed class ExperienceController : MonoBehaviour
{
	public class PlayerProgress
	{
		public string IdOfGivenPreviousTierBestArmor;

		public int OldLevel;

		public int OldExp;

		public int IncrementExp;

		public AudioClip Exp2;

		public AudioClip Levelup;

		public AudioClip Tierup;

		public int CurrentExp
		{
			get
			{
				return OldExp + IncrementExp;
			}
		}

		public PlayerProgress(string idOfGivenPreviousTierBestArmor, int oldLevel, int oldExp, int incExp, AudioClip exp2, AudioClip levelup, AudioClip tierup = null)
		{
			IdOfGivenPreviousTierBestArmor = idOfGivenPreviousTierBestArmor;
			OldLevel = oldLevel;
			OldExp = oldExp;
			IncrementExp = incExp;
			Exp2 = exp2;
			Levelup = levelup;
			Tierup = tierup;
		}

		public void Set(string idOfGivenPreviousTierBestArmor, int oldLevel, int oldExperience, int incExp, AudioClip exp2, AudioClip levelup, AudioClip tierup = null)
		{
			IdOfGivenPreviousTierBestArmor = idOfGivenPreviousTierBestArmor;
			OldLevel = oldLevel;
			OldExp = oldExperience;
			IncrementExp = incExp;
			Exp2 = exp2;
			Levelup = levelup;
			Tierup = tierup;
		}
	}

	public static int[] MaxExpLevelsDefault = new int[37]
	{
		0, 15, 35, 50, 90, 100, 110, 115, 120, 125,
		130, 135, 140, 150, 160, 170, 180, 200, 220, 250,
		290, 340, 400, 470, 550, 640, 740, 850, 970, 1100,
		1240, 1390, 1550, 1720, 1900, 2090, 100000
	};

	public static int[] MaxExpLevels = InitMaxLevelMass(MaxExpLevelsDefault);

	public static readonly float[] HealthByLevel = new float[37]
	{
		0f, 9f, 10f, 10f, 11f, 11f, 12f, 13f, 13f, 14f,
		14f, 15f, 16f, 16f, 17f, 17f, 18f, 19f, 19f, 20f,
		20f, 21f, 22f, 22f, 23f, 23f, 24f, 25f, 25f, 26f,
		26f, 27f, 27f, 27f, 27f, 27f, 27f
	};

	public bool isMenu;

	public bool isConnectScene;

	public SaltedInt currentLevelForEditor = new SaltedInt(14592942, 0);

	public const int maxLevel = 36;

	public int[,] limitsLeveling = new int[6, 2]
	{
		{ 1, 6 },
		{ 7, 11 },
		{ 12, 16 },
		{ 17, 21 },
		{ 22, 26 },
		{ 27, 36 }
	};

	public static int[,] accessByLevels = new int[36, 36];

	public Texture2D[] marks;

	private SaltedInt currentExperience = new SaltedInt(12512238, 0);

	private static int[] _addCoinsFromLevelsDefault = new int[37]
	{
		0, 5, 10, 15, 20, 25, 25, 25, 30, 30,
		30, 35, 35, 40, 40, 40, 45, 45, 50, 50,
		50, 55, 55, 60, 60, 60, 65, 65, 70, 70,
		70, 75, 75, 80, 80, 80, 0
	};

	private static int[] _addCoinsFromLevels = InitAddCoinsFromLevels(_addCoinsFromLevelsDefault);

	private static int[] _addGemsFromLevelsDefault = new int[37]
	{
		0, 4, 4, 5, 5, 6, 6, 7, 7, 8,
		8, 9, 9, 10, 10, 11, 11, 12, 12, 13,
		13, 14, 14, 15, 15, 16, 16, 17, 17, 18,
		18, 19, 19, 20, 20, 21, 0
	};

	private static int[] _addGemsFromLevels = InitAddGemsFromLevels(_addGemsFromLevelsDefault);

	private static bool _storagerKeysInitialized = false;

	private bool _isShowRanks = true;

	public bool isShowNextPlashka;

	public Vector2 posRanks = Vector2.zero;

	private int oldCurrentExperience;

	public int oldCurrentLevel;

	public bool isShowAdd;

	public AudioClip exp_1;

	public AudioClip exp_2;

	public AudioClip exp_3;

	public AudioClip Tierup;

	public static ExperienceController sharedController;

	private PlayerProgress _currentProgress;

	private bool _initialized;

	private static readonly StringBuilder _sharedStringBuilder = new StringBuilder();

	public int AddHealthOnCurLevel
	{
		get
		{
			int num = currentLevel;
			if (HealthByLevel.Length > num && num > 0)
			{
				return (int)(HealthByLevel[num] - HealthByLevel[num - 1]);
			}
			return 0;
		}
	}

	public int currentLevel
	{
		get
		{
			return currentLevelForEditor.Value;
		}
		private set
		{
			bool flag = false;
			if (currentLevelForEditor.Value != value)
			{
				flag = true;
			}
			currentLevelForEditor.Value = value;
			if (value >= 4)
			{
				ReviewController.CheckActiveReview();
			}
			if (flag && ExperienceController.onLevelChange != null)
			{
				ExperienceController.onLevelChange();
			}
		}
	}

	public static int[] addCoinsFromLevels
	{
		get
		{
			return _addCoinsFromLevels;
		}
	}

	public static int[] addGemsFromLevels
	{
		get
		{
			return _addGemsFromLevels;
		}
	}

	public int CurrentExperience
	{
		get
		{
			return currentExperience.Value;
		}
	}

	public bool isShowRanks
	{
		get
		{
			return _isShowRanks;
		}
		set
		{
			if (_isShowRanks != value)
			{
				_isShowRanks = value;
				if (ExpController.Instance != null)
				{
					ExpController.Instance.InterfaceEnabled = value;
				}
			}
		}
	}

	public PlayerProgress CurrentProgress
	{
		get
		{
			if (_currentProgress == null)
			{
				_currentProgress = new PlayerProgress(string.Empty, currentLevel, CurrentExperience, 0, exp_2, exp_3, Tierup);
			}
			return _currentProgress;
		}
	}

	public static event Action onLevelChange;

	public static event Action<PlayerProgress> OnPlayerProgressChanged;

	public static int[] InitMaxLevelMass(int[] _mass)
	{
		int[] array = new int[_mass.Length];
		Array.Copy(_mass, array, _mass.Length);
		return array;
	}

	public static void RefreshExpControllers()
	{
		if (sharedController != null)
		{
			sharedController.Refresh();
		}
		else
		{
			UnityEngine.Debug.LogError("RefreshExpControllers ExperienceController.sharedController == null");
		}
	}

	public static int[] InitAddCoinsFromLevels(int[] _mass)
	{
		int[] array = new int[_mass.Length];
		Array.Copy(_mass, array, _mass.Length);
		return array;
	}

	public static int[] InitAddGemsFromLevels(int[] _mass)
	{
		int[] array = new int[_mass.Length];
		Array.Copy(_mass, array, _mass.Length);
		return array;
	}

	public static void ResetLevelingOnDefault()
	{
		MaxExpLevels = InitMaxLevelMass(MaxExpLevelsDefault);
		_addCoinsFromLevels = InitAddCoinsFromLevels(_addCoinsFromLevelsDefault);
		_addGemsFromLevels = InitAddGemsFromLevels(_addGemsFromLevelsDefault);
	}

	public static void RewriteLevelingParametersForLevel(int _level, int _exp, int _coins, int _gems)
	{
		MaxExpLevels[_level] = _exp;
		_addCoinsFromLevels[_level] = _coins;
		_addGemsFromLevels[_level] = _gems;
	}

	public static void RewriteLevelingParametersForLevel(int _level, int _coins, int _gems)
	{
		_addCoinsFromLevels[_level] = _coins;
		_addGemsFromLevels[_level] = _gems;
	}

	public void SetCurrentExperience(int _exp)
	{
		currentExperience = _exp;
		Storager.setInt("currentExperience", _exp);
		UnityEngine.Debug.Log(currentExperience.Value);
	}

	private static void InitializeStoragerKeysIfNeeded()
	{
		if (!_storagerKeysInitialized)
		{
			if (!Storager.hasKey("currentLevel1"))
			{
				Storager.setInt("currentLevel1", 1);
			}
			_storagerKeysInitialized = true;
		}
	}

	public static int GetCurrentLevelWithUpdateCorrection()
	{
		InitializeStoragerKeysIfNeeded();
		int num = GetCurrentLevel();
		if (num < 36 && Storager.getInt("currentExperience") >= MaxExpLevels[num])
		{
			num++;
		}
		return num;
	}

	public static int GetCurrentLevel()
	{
		int result = 1;
		for (int i = 1; i <= 36; i++)
		{
			string currentLevelKey = GetCurrentLevelKey(i);
			if (Storager.getInt(currentLevelKey) == 1)
			{
				result = i;
				Storager.setInt(currentLevelKey, 1);
			}
		}
		return result;
	}

	public void Refresh()
	{
		currentExperience = Storager.getInt("currentExperience");
		currentLevel = GetCurrentLevel();
	}

	public ExperienceController()
	{
		currentLevel = 1;
	}

	private void AddCurrenciesForLevelUP()
	{
		int count = addGemsFromLevels[currentLevel - 1];
		BankController.canShowIndication = false;
		BankController.AddGems(count, false);
		if (currentLevel == 2 && BalanceController.startCapitalEnabled)
		{
			BankController.AddGems(BalanceController.startCapitalGems, false);
		}
		StartCoroutine(BankController.WaitForIndicationGems("GemsCurrency"));
		BankController.AddCoins(addCoinsFromLevels[currentLevel - 1], false);
		if (currentLevel == 2 && BalanceController.startCapitalEnabled)
		{
			BankController.AddCoins(BalanceController.startCapitalCoins, false);
		}
		StartCoroutine(BankController.WaitForIndicationGems("Coins"));
	}

	private void Awake()
	{
		sharedController = this;
		CoroutineRunner.Instance.StartCoroutine(WaitInitializationThenLogProgressInExperience());
	}

	private IEnumerator WaitInitializationThenLogProgressInExperience()
	{
		while (!_initialized)
		{
			yield return null;
		}
		while (AnalyticsFacade.FlurryFacade == null)
		{
			yield return null;
		}
		int levelBase = currentLevel;
		int tierBase = ExpController.OurTierForAnyPlace() + 1;
		AnalyticsStuff.LogProgressInExperience(levelBase, tierBase);
	}

	public IEnumerable<float> InitController()
	{
		for (int i = 0; i < 36; i++)
		{
			for (int j = 0; j < 36; j++)
			{
				accessByLevels[i, j] = 0;
			}
		}
		for (int k = 0; k < 36; k++)
		{
			for (int l = limitsLeveling.GetLowerBound(0); l <= limitsLeveling.GetUpperBound(0); l++)
			{
				int num = limitsLeveling[l, 0] - 1;
				int num2 = limitsLeveling[l, 1] - 1;
				if (k >= num && k <= num2)
				{
					for (int m = num; m <= num2; m++)
					{
						accessByLevels[k, m] = 1;
					}
					break;
				}
			}
		}
		yield return 0f;
		try
		{
			InitializeStoragerKeysIfNeeded();
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			Refresh();
			if (currentLevel < 36 && currentExperience.Value >= MaxExpLevels[currentLevel])
			{
				currentExperience = 0;
				currentLevel++;
				Storager.setInt(GetCurrentLevelKey(currentLevel), 1);
				Storager.setInt("currentExperience", currentExperience.Value);
				BankController.GiveInitialNumOfCoins();
				AddCurrenciesForLevelUP();
				BankController.canShowIndication = true;
				AnalyticsFacade.LevelUp(currentLevel);
			}
			isShowRanks = false;
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogError("<<< ExperienceController.Start() failed.");
			UnityEngine.Debug.LogException(exception);
		}
		finally
		{
			_initialized = true;
		}
	}

	public static void SendAnalyticsForLevelsFromCloud(int levelBefore)
	{
		if (sharedController == null)
		{
			UnityEngine.Debug.LogError("SendAnalyticsForLevelsFromCloud ExperienceController.sharedController == null");
			return;
		}
		for (int i = levelBefore + 1; i <= sharedController.currentLevel; i++)
		{
			AnalyticsFacade.LevelUp(i);
		}
	}

	public void AddExperience(int incrementExperience)
	{
		if (currentLevel == 36)
		{
			return;
		}
		oldCurrentLevel = currentLevel;
		oldCurrentExperience = currentExperience.Value;
		if (currentLevel < 36 && incrementExperience >= MaxExpLevels[currentLevel] - currentExperience.Value + MaxExpLevels[currentLevel + 1])
		{
			incrementExperience = MaxExpLevels[currentLevel + 1] + MaxExpLevels[currentLevel] - currentExperience.Value - 5;
		}
		string key = "Statistics.ExpInMode.Level" + sharedController.currentLevel;
		if (PlayerPrefs.HasKey(key) && Initializer.lastGameMode != -1)
		{
			string key2 = Initializer.lastGameMode.ToString();
			string @string = PlayerPrefs.GetString(key, "{}");
			try
			{
				Dictionary<string, object> dictionary = (Json.Deserialize(@string) as Dictionary<string, object>) ?? new Dictionary<string, object>();
				object value;
				if (dictionary.TryGetValue(key2, out value))
				{
					int num = Convert.ToInt32(value) + incrementExperience;
					dictionary[key2] = num;
				}
				else
				{
					dictionary.Add(key2, incrementExperience);
				}
				string value2 = Json.Serialize(dictionary);
				PlayerPrefs.SetString(key, value2);
			}
			catch (OverflowException exception)
			{
				UnityEngine.Debug.LogError("Cannot deserialize exp-in-mode: " + @string);
				UnityEngine.Debug.LogException(exception);
			}
			catch (Exception exception2)
			{
				UnityEngine.Debug.LogError("Unknown exception: " + @string);
				UnityEngine.Debug.LogException(exception2);
			}
		}
		currentExperience = currentExperience.Value + incrementExperience;
		Storager.setInt("currentExperience", currentExperience.Value);
		string idOfGivenPreviousTierBestArmor = string.Empty;
		if (currentLevel < 36 && currentExperience.Value >= MaxExpLevels[currentLevel])
		{
			DateTime utcNow = DateTime.UtcNow;
			PlayerPrefs.SetString("Statistics.TimeInRank.Level" + (currentLevel + 1), utcNow.ToString("s"));
			PlayerPrefs.GetInt("Statistics.MatchCount.Level" + sharedController.currentLevel, 0);
			PlayerPrefs.GetInt("Statistics.WinCount.Level" + sharedController.currentLevel, 0);
			AnalyticsFacade.SendCustomEventToAppsFlyer("af_level_achieved", new Dictionary<string, string> { 
			{
				"af_level",
				currentLevel.ToString()
			} });
			AnalyticsFacade.SendCustomEventToFacebook("level_reached", new Dictionary<string, object> { { "level", currentLevel } });
			currentExperience = currentExperience.Value - MaxExpLevels[currentLevel];
			currentLevel++;
			PlayerPrefs.SetString(LeaderboardScript.LeaderboardsResponseCacheTimestamp, DateTime.UtcNow.Subtract(TimeSpan.FromHours(2.0)).ToString("s", CultureInfo.InvariantCulture));
			if (currentLevel == 3)
			{
				AnalyticsStuff.TrySendOnceToAppsFlyer("levelup_3");
			}
			if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShopCompleted && currentLevel > 1)
			{
				if (Storager.getInt("Training.NoviceArmorUsedKey") == 1)
				{
					Storager.setInt("Training.ShouldRemoveNoviceArmorInShopKey", 1);
					if (HintController.instance != null)
					{
						HintController.instance.ShowHintByName("shop_remove_novice_armor", 2.5f);
					}
				}
				TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.FirstMatchCompleted;
				AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Finished);
				if (!Storager.hasKey("Analytics:tutorial_levelup"))
				{
					Storager.setString("Analytics:tutorial_levelup", "{}");
					AnalyticsFacade.SendCustomEventToAppsFlyer("tutorial_levelup", new Dictionary<string, string>());
					Storager.setString("Analytics:af_tutorial_completion", "{}");
					AnalyticsFacade.SendCustomEventToAppsFlyer("af_tutorial_completion", new Dictionary<string, string>());
				}
				try
				{
					if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.playerWeapons != null)
					{
						IEnumerable<Weapon> source = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>();
						if (source.FirstOrDefault((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 3) == null)
						{
							WeaponManager.sharedManager.EquipWeapon(WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().First((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", "") == WeaponManager.SimpleFlamethrower_WN));
						}
						if (source.FirstOrDefault((Weapon w) => w.weaponPrefab.GetComponent<WeaponSounds>().categoryNabor - 1 == 5) == null)
						{
							WeaponManager.sharedManager.EquipWeapon(WeaponManager.sharedManager.allAvailablePlayerWeapons.OfType<Weapon>().First((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", "") == WeaponManager.Rocketnitza_WN));
						}
					}
				}
				catch (Exception ex)
				{
					UnityEngine.Debug.LogError("Exception in gequipping flamethrower and rocketniza: " + ex);
				}
			}
			if (currentLevel == 2)
			{
				try
				{
					EggData eggData = Singleton<EggsManager>.Instance.GetEggData("egg_champion_steel");
					Singleton<EggsManager>.Instance.AddEgg(eggData);
				}
				catch (Exception ex2)
				{
					UnityEngine.Debug.LogErrorFormat("Exception in giving after-training steel league egg: {0}", ex2);
				}
			}
			idOfGivenPreviousTierBestArmor = GiveArmorOnTierUp();
			Storager.setInt(GetCurrentLevelKey(currentLevel), 1);
			Storager.setInt("currentExperience", currentExperience.Value);
			BankController.GiveInitialNumOfCoins();
			AddCurrenciesForLevelUP();
			FriendsController.sharedController.rank = currentLevel;
			FriendsController.sharedController.SendOurData();
			FriendsController.sharedController.UpdatePopularityMaps();
			AnalyticsFacade.LevelUp(currentLevel);
		}
		if (Defs.isSoundFX)
		{
			NGUITools.PlaySound(exp_1);
		}
		_currentProgress = new PlayerProgress(idOfGivenPreviousTierBestArmor, oldCurrentLevel, oldCurrentExperience, incrementExperience, exp_2, exp_3, Tierup);
		if (ExperienceController.OnPlayerProgressChanged != null)
		{
			ExperienceController.OnPlayerProgressChanged(_currentProgress);
		}
	}

	private string GiveArmorOnTierUp()
	{
		int num = Array.IndexOf(ExpController.LevelsForTiers, currentLevel);
		if (num > 0)
		{
			int num2 = num * 3 - 1;
			int count = Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0].Count;
			if (num2 < count)
			{
				string text = Wear.wear[ShopNGUIController.CategoryNames.ArmorCategory][0][num2];
				if (Storager.getInt(text) == 0)
				{
					ShopNGUIController.ProvideItem(ShopNGUIController.CategoryNames.ArmorCategory, text, 1, true, 0, null, null, true, Storager.getString(Defs.ArmorNewEquppedSN) != Defs.ArmorNewNoneEqupped);
					try
					{
						if (PersConfigurator.currentConfigurator != null)
						{
							PersConfigurator.currentConfigurator.UpdateWear();
						}
					}
					catch (Exception ex)
					{
						UnityEngine.Debug.LogErrorFormat("Exception in AddExperience, updating pers configurator: {0}", ex);
					}
					return text;
				}
			}
		}
		return string.Empty;
	}

	private void HideNextPlashka()
	{
		isShowNextPlashka = false;
		isShowAdd = false;
	}

	private void DoOnGUI()
	{
	}

	public static void SetEnable(bool enable)
	{
		if (!(sharedController == null))
		{
			sharedController.isShowRanks = enable;
		}
	}

	private static string GetCurrentLevelKey(int levelIndex)
	{
		_sharedStringBuilder.Length = 0;
		_sharedStringBuilder.Append("currentLevel").Append(levelIndex);
		string result = _sharedStringBuilder.ToString();
		_sharedStringBuilder.Length = 0;
		return result;
	}
}
