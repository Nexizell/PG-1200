using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Holoville.HOTween;
using Holoville.HOTween.Core;
using Holoville.HOTween.Plugins;
using Rilisoft;
using UnityEngine;

public sealed class ProfileController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CExitCallbacksCoroutine_003Ed__59 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ProfileController _003C_003E4__this;

		private int _003Ci_003E5__1;

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
		public _003CExitCallbacksCoroutine_003Ed__59(int _003C_003E1__state)
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
				_003Ci_003E5__1 = 1;
				break;
			case 1:
			{
				_003C_003E1__state = -1;
				int num = _003Ci_003E5__1 + 1;
				_003Ci_003E5__1 = num;
				break;
			}
			}
			if (_003Ci_003E5__1 < _003C_003E4__this._exitCallbacks.Length)
			{
				_003C_003E4__this._exitCallbacks[_003Ci_003E5__1]();
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			_003C_003E4__this._exitCallbacks = new Action[0];
			_003C_003E4__this.InterfaceEnabled = false;
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

	public static SaltedInt countGameTotalKills = new SaltedInt(640178770);

	public static SaltedInt countGameTotalDeaths = new SaltedInt(371743314);

	public static SaltedInt countGameTotalShoot = new SaltedInt(623401554);

	public static SaltedInt countGameTotalHit = new SaltedInt(606624338);

	public static SaltedInt countLikes = new SaltedInt(606624338);

	public ProfileView profileView;

	[SerializeField]
	protected internal Camera _camera3D;

	public const string keyChooseDefaultName = "keyChooseDefaultName";

	public static string[] DefaultKeyNames = new string[19]
	{
		"Key_2020", "Key_2021", "Key_2022", "Key_2023", "Key_2024", "Key_2025", "Key_2026", "Key_2027", "Key_2028", "Key_2029",
		"Key_2030", "Key_2031", "Key_2032", "Key_2033", "Key_2034", "Key_2035", "Key_2036", "Key_2037", "Key_2038"
	};

	private static string _defaultPlayerName = null;

	private static ProfileController _instance;

	[SerializeField]
	protected internal CategoryButtonsController _statsTabButtonsController;

	private IDisposable _backSubscription;

	private const string NicknameKey = "NamePlayer";

	private bool _dirty;

	private bool _escapePressed;

	private Action[] _exitCallbacks = new Action[0];

	private float _idleTimeStart;

	private Quaternion _initialLocalRotation;

	private float _lastTime;

	private Rect? _touchZone;

	private Color? _storedAmbientLight;

	private bool _isNicknameSubmit;

	public Camera Camera3D
	{
		get
		{
			return _camera3D;
		}
	}

	public static ProfileController Instance
	{
		get
		{
			return _instance;
		}
	}

	public string DesiredWeaponTag { get; set; }

	public static int CurOrderCup
	{
		get
		{
			int currentLevel = ExperienceController.sharedController.currentLevel;
			for (int i = 0; i < ExpController.LevelsForTiers.Length; i++)
			{
				if (currentLevel >= MinLevelTir(i) && currentLevel <= MaxLevelTir(i))
				{
					return i;
				}
			}
			return -1;
		}
	}

	public bool InterfaceEnabled
	{
		get
		{
			if (profileView != null && profileView.interfaceHolder != null)
			{
				return profileView.interfaceHolder.gameObject.activeInHierarchy;
			}
			return false;
		}
		private set
		{
			if (!(profileView != null) || !(profileView.interfaceHolder != null))
			{
				return;
			}
			profileView.interfaceHolder.gameObject.SetActive(value);
			if (value)
			{
				Refresh();
				if (ExperienceController.sharedController != null && ExpController.Instance != null)
				{
					ExperienceController.sharedController.isShowRanks = true;
					ExpController.Instance.InterfaceEnabled = true;
				}
				if (_backSubscription != null)
				{
					_backSubscription.Dispose();
				}
				_backSubscription = BackSystem.Instance.Register(HandleEscape, "Profile Controller");
			}
			else
			{
				DesiredWeaponTag = string.Empty;
				if (_backSubscription != null)
				{
					_backSubscription.Dispose();
					_backSubscription = null;
				}
			}
			FreeAwardShowHandler.CheckShowChest(value);
		}
	}

	public static string defaultPlayerName
	{
		get
		{
			if (!PlayerPrefs.HasKey("keyChooseDefaultName"))
			{
				_defaultPlayerName = GetRandomName();
				PlayerPrefs.SetString("keyChooseDefaultName", _defaultPlayerName);
			}
			if (_defaultPlayerName == null)
			{
				_defaultPlayerName = PlayerPrefs.GetString("keyChooseDefaultName");
			}
			return _defaultPlayerName;
		}
	}

	public event EventHandler<ProfileView.InputEventArgs> NicknameInput
	{
		add
		{
			if (profileView != null)
			{
				profileView.NicknameInput += value;
			}
		}
		remove
		{
			if (profileView != null)
			{
				profileView.NicknameInput -= value;
			}
		}
	}

	public event EventHandler BackRequested
	{
		add
		{
			if (profileView != null)
			{
				profileView.BackButtonPressed += value;
			}
			EscapePressed += value;
		}
		remove
		{
			if (profileView != null)
			{
				profileView.BackButtonPressed -= value;
			}
			EscapePressed -= value;
		}
	}

	private event EventHandler EscapePressed;

	public void HandleBankButton()
	{
		if (BankController.Instance != null)
		{
			EventHandler handleBackFromBank = null;
			handleBackFromBank = delegate
			{
				BankController.Instance.BackRequested -= handleBackFromBank;
				BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(false, null);
				InterfaceEnabled = true;
			};
			BankController.Instance.BackRequested += handleBackFromBank;
			BankController.Instance.SetInterfaceEnabledWithDesiredCurrency(true, null);
			InterfaceEnabled = false;
		}
		else
		{
			UnityEngine.Debug.LogWarning("BankController.Instance == null");
		}
	}

	public void HandleAchievementsButton()
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Achievements] button pressed");
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (!Application.isEditor)
			{
				if (Social.localUser.authenticated)
				{
					Social.ShowAchievementsUI();
				}
				else
				{
					GameCenterSingleton.Instance.updateGameCenter();
				}
			}
			break;
		case RuntimePlatform.Android:
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				Social.ShowAchievementsUI();
				break;
			}
			Defs.RuntimeAndroidEdition androidEdition = Defs.AndroidEdition;
			int num = 1;
			break;
		}
		}
	}

	public void HandleLeaderboardsButton()
	{
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("[Leaderboards] button pressed");
		}
		switch (BuildSettings.BuildTargetPlatform)
		{
		case RuntimePlatform.IPhonePlayer:
			if (!Application.isEditor)
			{
				if (Social.localUser.authenticated)
				{
					Social.ShowLeaderboardUI();
				}
				else
				{
					GameCenterSingleton.Instance.updateGameCenter();
				}
			}
			break;
		case RuntimePlatform.Android:
		{
			if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				Social.ShowLeaderboardUI();
				break;
			}
			Defs.RuntimeAndroidEdition androidEdition = Defs.AndroidEdition;
			int num = 1;
			break;
		}
		}
	}

	public static int MinLevelTir(int curTir)
	{
		if (curTir >= 0 && curTir < ExpController.LevelsForTiers.Length)
		{
			return ExpController.LevelsForTiers[curTir];
		}
		return -1;
	}

	public static int MaxLevelTir(int curTir)
	{
		if (curTir >= 0 && curTir < ExpController.LevelsForTiers.Length)
		{
			if (curTir == ExpController.LevelsForTiers.Length - 1)
			{
				return 36;
			}
			return ExpController.LevelsForTiers[curTir + 1];
		}
		return -1;
	}

	public static float GetPerFillProgress(int order, int lev)
	{
		float num = 0f;
		if (order < ExpController.LevelsForTiers.Length)
		{
			int num2 = MinLevelTir(order);
			int num3 = MaxLevelTir(order);
			float num4 = lev - num2;
			float num5 = num3 - num2;
			if (num4 > 0f)
			{
				return num4 / num5;
			}
			return 0f;
		}
		return 0f;
	}

	public void ShowInterface(params Action[] exitCallbacks)
	{
		FriendsController.sharedController.GetOurWins();
		InterfaceEnabled = true;
		_exitCallbacks = exitCallbacks ?? new Action[0];
	}

	public void SetStaticticTab(ProfileStatTabType tabType)
	{
		_statsTabButtonsController.BtnClicked(tabType.ToString());
	}

	private void Awake()
	{
		_instance = this;
	}

	private void Start()
	{
		BackRequested += HandleBackRequest;
		if (profileView != null)
		{
			profileView.nicknameInput.defaultText = defaultPlayerName;
			UIInputRilisoft nicknameInput = profileView.nicknameInput;
			nicknameInput.onFocus = (UIInputRilisoft.OnFocus)Delegate.Combine(nicknameInput.onFocus, new UIInputRilisoft.OnFocus(OnFocusNickname));
			UIInputRilisoft nicknameInput2 = profileView.nicknameInput;
			nicknameInput2.onFocusLost = (UIInputRilisoft.OnFocusLost)Delegate.Combine(nicknameInput2.onFocusLost, new UIInputRilisoft.OnFocusLost(onFocusLostNickname));
		}
		if (profileView != null)
		{
			profileView.Nickname = GetPlayerNameOrDefault();
			profileView.NicknameInput += HandleNicknameInput;
			_initialLocalRotation = profileView.characterView.character.localRotation;
			switch (BuildSettings.BuildTargetPlatform)
			{
			case RuntimePlatform.IPhonePlayer:
				UpdateButton(profileView.achievementsButton, "gamecntr");
				UpdateButton(profileView.leaderboardsButton, "gamecntr");
				break;
			case RuntimePlatform.Android:
				if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
				{
					UpdateButton(profileView.achievementsButton, "google");
					UpdateButton(profileView.leaderboardsButton, "google");
				}
				else if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
				{
					UpdateButton(profileView.achievementsButton, "amazon");
					UpdateButton(profileView.leaderboardsButton, "amazon");
				}
				else
				{
					profileView.achievementsButton.gameObject.SetActive(false);
				}
				break;
			default:
				profileView.achievementsButton.gameObject.SetActive(false);
				break;
			}
		}
		InterfaceEnabled = false;
		FriendsController.OurInfoUpdated += HandleOurInfoUpdated;
	}

	private void HandleOurInfoUpdated()
	{
		if (InterfaceEnabled)
		{
			Refresh(false);
		}
	}

	private void UpdateButton(UIButton button, string spriteName)
	{
		if (!(button == null))
		{
			button.normalSprite = spriteName;
			button.pressedSprite = spriteName + "_n";
			button.hoverSprite = spriteName;
			button.disabledSprite = spriteName;
		}
	}

	private void OnDestroy()
	{
		FriendsController.OurInfoUpdated -= HandleOurInfoUpdated;
		if (profileView != null)
		{
			profileView.NicknameInput -= HandleNicknameInput;
			UIInputRilisoft nicknameInput = profileView.nicknameInput;
			nicknameInput.onFocus = (UIInputRilisoft.OnFocus)Delegate.Remove(nicknameInput.onFocus, new UIInputRilisoft.OnFocus(OnFocusNickname));
			UIInputRilisoft nicknameInput2 = profileView.nicknameInput;
			nicknameInput2.onFocusLost = (UIInputRilisoft.OnFocusLost)Delegate.Remove(nicknameInput2.onFocusLost, new UIInputRilisoft.OnFocusLost(onFocusLostNickname));
		}
	}

	private void Refresh(bool updateWeapon = true)
	{
		if (profileView != null)
		{
			profileView.Nickname = GetPlayerNameOrDefault();
			Dictionary<string, object> dictionary = ((FriendsController.sharedController != null && FriendsController.sharedController.ourInfo != null && FriendsController.sharedController.ourInfo.ContainsKey("wincount") && FriendsController.sharedController.ourInfo["wincount"] != null) ? (FriendsController.sharedController.ourInfo["wincount"] as Dictionary<string, object>) : null);
			profileView.CheckBtnCopy();
			profileView.DeathmatchWinCount = Storager.getInt(Defs.RatingDeathmatch).ToString();
			profileView.TeamBattleWinCount = Storager.getInt(Defs.RatingTeamBattle).ToString();
			profileView.DeadlyGamesWinCount = Storager.getInt(Defs.RatingHunger).ToString();
			profileView.FlagCaptureWinCount = Storager.getInt(Defs.RatingFlag).ToString();
			profileView.CapturePointWinCount = Storager.getInt(Defs.RatingCapturePoint).ToString();
			profileView.DuelWinCount = Storager.getInt(Defs.RatingDuel).ToString();
			profileView.TotalWinCount = (Storager.getInt(Defs.RatingDeathmatch) + Storager.getInt(Defs.RatingTeamBattle) + Storager.getInt(Defs.RatingHunger) + Storager.getInt(Defs.RatingFlag) + Storager.getInt(Defs.RatingCapturePoint) + Storager.getInt(Defs.RatingDuel)).ToString();
			profileView.PixelgunFriendsID = ((FriendsController.sharedController != null && FriendsController.sharedController.id != null) ? FriendsController.sharedController.id : string.Empty);
			object value;
			profileView.TotalWeeklyWinCount = ((dictionary != null && dictionary.TryGetValue("weekly", out value)) ? ((long)value) : 0).ToString();
			profileView.CoopTimeSurvivalPointCount = Storager.getInt(Defs.COOPScore).ToString();
			profileView.GameTotalKills = countGameTotalKills.Value.ToString();
			float num = 0f;
			num = ((countGameTotalDeaths.Value != 0) ? ((float)countGameTotalKills.Value / (1f * (float)countGameTotalDeaths.Value)) : ((float)countGameTotalKills.Value));
			num = (float)Math.Round(num, 2);
			profileView.GameKillrate = num.ToString();
			float num2 = 0f;
			if (countGameTotalHit.Value != 0)
			{
				num2 = (float)(100 * countGameTotalHit.Value) / (1f * (float)countGameTotalShoot.Value);
			}
			num2 = (float)Math.Round(num2, 2);
			profileView.GameAccuracy = num2.ToString();
			profileView.GameLikes = countLikes.Value.ToString();
			profileView.WaveCountLabel = PlayerPrefs.GetInt(Defs.WavesSurvivedMaxS, 0).ToString();
			profileView.KilledCountLabel = PlayerPrefs.GetInt(Defs.KilledZombiesMaxSett, 0).ToString();
			profileView.SurvivalScoreLabel = PlayerPrefs.GetInt(Defs.SurvivalScoreSett, 0).ToString();
			profileView.Box1StarsLabel = InitializeStarCountLabelForBox(0);
			profileView.Box2StarsLabel = InitializeStarCountLabelForBox(1);
			profileView.Box3StarsLabel = InitializeStarCountLabelForBox(2);
			profileView.SecretCoinsLabel = InitializeSecretBonusCountLabel(VirtualCurrencyBonusType.Coin);
			profileView.SecretGemsLabel = InitializeSecretBonusCountLabel(VirtualCurrencyBonusType.Gem);
			if (updateWeapon && WeaponManager.sharedManager != null)
			{
				Weapon[] array = WeaponManager.sharedManager.playerWeapons.OfType<Weapon>().ToArray();
				if (array.Length != 0)
				{
					string desiredPrefabName = null;
					if (!string.IsNullOrEmpty(DesiredWeaponTag))
					{
						ItemRecord byTag = ItemDb.GetByTag(DesiredWeaponTag);
						if (byTag != null)
						{
							desiredPrefabName = byTag.PrefabName;
						}
					}
					if (!string.IsNullOrEmpty(desiredPrefabName) && array.Any((Weapon w) => w.weaponPrefab.name.Replace("(Clone)", "") == desiredPrefabName))
					{
						profileView.SetWeaponAndSkin(DesiredWeaponTag, false);
					}
					else
					{
						int num3 = new System.Random(Time.frameCount).Next(array.Length);
						Weapon weapon = array[num3];
						profileView.SetWeaponAndSkin(ItemDb.GetByPrefabName(weapon.weaponPrefab.name.Replace("(Clone)", "")).Tag, false);
					}
				}
				else
				{
					profileView.SetWeaponAndSkin("Knife", false);
				}
			}
			if (Storager.getString(Defs.HatEquppedSN) != Defs.HatNoneEqupped)
			{
				profileView.UpdateHat(Storager.getString(Defs.HatEquppedSN));
			}
			else
			{
				profileView.RemoveHat();
			}
			if (Storager.getString("MaskEquippedSN") != "MaskNoneEquipped")
			{
				profileView.UpdateMask(Storager.getString("MaskEquippedSN"));
			}
			else
			{
				profileView.RemoveMask();
			}
			if (Storager.getString(Defs.BootsEquppedSN) != Defs.BootsNoneEqupped)
			{
				profileView.UpdateBoots(Storager.getString(Defs.BootsEquppedSN));
			}
			else
			{
				profileView.RemoveBoots();
			}
			if (Storager.getString(Defs.ArmorNewEquppedSN) != Defs.ArmorNoneEqupped)
			{
				profileView.UpdateArmor(Storager.getString(Defs.ArmorNewEquppedSN));
			}
			else
			{
				profileView.RemoveArmor();
			}
			if (Storager.getString(Defs.CapeEquppedSN) != Defs.CapeNoneEqupped)
			{
				profileView.UpdateCape(Storager.getString(Defs.CapeEquppedSN));
			}
			else
			{
				profileView.RemoveCape();
			}
			if (FriendsController.sharedController != null)
			{
				profileView.SetClanLogo(FriendsController.sharedController.clanLogo ?? string.Empty);
			}
			else
			{
				profileView.SetClanLogo(string.Empty);
			}
		}
		_idleTimeStart = Time.realtimeSinceStartup + 1000000f;
	}

	private void OnEnable()
	{
		Refresh();
	}

	private void Update()
	{
		EventHandler escapePressed = this.EscapePressed;
		if (_escapePressed && escapePressed != null)
		{
			escapePressed(this, EventArgs.Empty);
			_escapePressed = false;
		}
		if (Time.realtimeSinceStartup - _idleTimeStart > ShopNGUIController.IdleTimeoutPers)
		{
			ReturnCharacterToInitialState();
		}
	}

	private void LateUpdate()
	{
		if (profileView != null && InterfaceEnabled && !HOTween.IsTweening(profileView.characterView.character))
		{
			float rotationRateForCharacterInMenues = RilisoftRotator.RotationRateForCharacterInMenues;
			if (!_touchZone.HasValue)
			{
				_touchZone = new Rect(0f, 0.1f * (float)Screen.height, 0.5f * (float)Screen.width, 0.8f * (float)Screen.height);
			}
			RilisoftRotator.RotateCharacter(profileView.characterView.character, rotationRateForCharacterInMenues, _touchZone.Value, ref _idleTimeStart, ref _lastTime);
		}
	}

	private void HandleEscape()
	{
		if (!(BankController.Instance != null) || !BankController.Instance.InterfaceEnabled)
		{
			if (InfoWindowController.IsActive)
			{
				InfoWindowController.HideCurrentWindow();
			}
			else
			{
				_escapePressed = true;
			}
		}
	}

	private void ReturnCharacterToInitialState()
	{
		if (profileView == null)
		{
			UnityEngine.Debug.LogWarning("profileView == null");
			return;
		}
		int num = HOTween.Kill(profileView.characterView.character);
		if (num > 0 && Application.isEditor)
		{
			UnityEngine.Debug.LogWarning("Tweens killed: " + num);
		}
		_idleTimeStart += 1000000f;
		HOTween.To(profileView.characterView.character, 0.5f, new TweenParms().Prop("localRotation", new PlugQuaternion(_initialLocalRotation)).UpdateType(UpdateType.TimeScaleIndependentUpdate).Ease(EaseType.Linear)
			.OnComplete((TweenDelegate.TweenCallback)delegate
			{
				_idleTimeStart += 1000000f;
			}));
	}

	private string InitializeStarCountLabelForBox(int boxIndex)
	{
		if (boxIndex >= LevelBox.campaignBoxes.Count)
		{
			UnityEngine.Debug.LogWarning("Box index is out of range:    " + boxIndex);
			return string.Empty;
		}
		LevelBox levelBox = LevelBox.campaignBoxes[boxIndex];
		List<CampaignLevel> levels = levelBox.levels;
		Dictionary<string, int> value;
		if (!CampaignProgress.boxesLevelsAndStars.TryGetValue(levelBox.name, out value))
		{
			UnityEngine.Debug.LogWarning("ProfileController: Box not found in dictionary: " + levelBox.name);
			value = new Dictionary<string, int>();
		}
		int num = 0;
		for (int i = 0; i != levels.Count; i++)
		{
			string sceneName = levels[i].sceneName;
			int value2 = 0;
			value.TryGetValue(sceneName, out value2);
			num += value2;
		}
		return string.Concat(num, '/', levels.Count * 3);
	}

	private string InitializeSecretBonusCountLabel(VirtualCurrencyBonusType bonusType)
	{
		List<string> levelsWhereGotBonus = CoinBonus.GetLevelsWhereGotBonus(bonusType);
		return string.Concat(Math.Min(20, levelsWhereGotBonus.Count), '/', 20);
	}

	private void HandleNicknameInput(object sender, ProfileView.InputEventArgs e)
	{
		SaveNamePlayer(e.Input);
	}

	public void SaveNamePlayer(string namePlayer)
	{
		namePlayer = FilterBadWorld.FilterString(namePlayer);
		if (string.IsNullOrEmpty(namePlayer) || namePlayer.Trim() == string.Empty)
		{
			namePlayer = defaultPlayerName;
			profileView.nicknameInput.label.text = namePlayer;
		}
		if (Application.isEditor)
		{
			UnityEngine.Debug.Log("Saving new name:    " + namePlayer);
		}
		PlayerPrefs.SetString("NamePlayer", namePlayer);
		if (WeaponManager.sharedManager != null && WeaponManager.sharedManager.myTable != null)
		{
			NetworkStartTable component = WeaponManager.sharedManager.myTable.GetComponent<NetworkStartTable>();
			if (component != null)
			{
				component.SetNewNick();
			}
		}
		_dirty = true;
		_isNicknameSubmit = true;
	}

	private void HandleBackRequest(object sender, EventArgs e)
	{
		if (_dirty && FriendsController.sharedController != null)
		{
			FriendsController.sharedController.SendOurData();
			_dirty = false;
		}
		Action action = _exitCallbacks.FirstOrDefault();
		if (action != null)
		{
			action();
		}
		StartCoroutine(ExitCallbacksCoroutine());
	}

	private IEnumerator ExitCallbacksCoroutine()
	{
		int i = 1;
		while (i < _exitCallbacks.Length)
		{
			_exitCallbacks[i]();
			yield return null;
			int num = i + 1;
			i = num;
		}
		_exitCallbacks = new Action[0];
		InterfaceEnabled = false;
	}

	private void OnFocusNickname()
	{
		_isNicknameSubmit = false;
	}

	private void onFocusLostNickname()
	{
		if (!_isNicknameSubmit && profileView != null)
		{
			profileView.nicknameInput.value = GetPlayerNameOrDefault();
		}
	}

	public static void ResaveStatisticToKeychain()
	{
		Storager.setInt("keyGameTotalKills", countGameTotalKills.Value);
		Storager.setInt("keyGameDeath", countGameTotalDeaths.Value);
		Storager.setInt("keyGameShoot", countGameTotalShoot.Value);
		Storager.setInt("keyGameHit", countGameTotalHit.Value);
		Storager.setInt("keyCountLikes", countLikes.Value);
	}

	public static void LoadStatisticFromKeychain()
	{
		if (!Storager.hasKey("keyGameTotalKills"))
		{
			Storager.setInt("keyGameTotalKills", 0);
		}
		if (!Storager.hasKey("keyGameDeath"))
		{
			Storager.setInt("keyGameDeath", 0);
		}
		if (!Storager.hasKey("keyGameShoot"))
		{
			Storager.setInt("keyGameShoot", 0);
		}
		if (!Storager.hasKey("keyGameHit"))
		{
			Storager.setInt("keyGameHit", 0);
		}
		countGameTotalKills.Value = Storager.getInt("keyGameTotalKills");
		countGameTotalDeaths.Value = Storager.getInt("keyGameDeath");
		countGameTotalShoot.Value = Storager.getInt("keyGameShoot");
		countGameTotalHit.Value = Storager.getInt("keyGameHit");
		countLikes.Value = Storager.getInt("keyCountLikes");
	}

	public static void OnGameTotalKills()
	{
		countGameTotalKills.Value++;
	}

	public static void OnGameDeath()
	{
		countGameTotalDeaths.Value++;
	}

	public static void OnGameShoot()
	{
		countGameTotalShoot.Value++;
	}

	public static void OnGameHit()
	{
		countGameTotalHit.Value++;
	}

	public static void OnGetLike()
	{
		countLikes.Value++;
	}

	private static string GetRandomName()
	{
		if (DefaultKeyNames != null && DefaultKeyNames.Length != 0)
		{
			int num = UnityEngine.Random.Range(0, DefaultKeyNames.Length);
			return LocalizationStore.Get(DefaultKeyNames[num]);
		}
		return "Player";
	}

	public static string GetPlayerNameOrDefault()
	{
		if (PlayerPrefs.HasKey("NamePlayer"))
		{
			string text = PlayerPrefs.GetString("NamePlayer");
			if (text != null)
			{
				if (text.Length > 20)
				{
					text = text.Substring(0, 20);
				}
				return text;
			}
		}
		string text2 = PlayerPrefs.GetString("SocialName", string.Empty);
		if (Social.localUser != null && Social.localUser.authenticated && !string.IsNullOrEmpty(Social.localUser.userName))
		{
			if (!text2.Equals(Social.localUser.userName))
			{
				text2 = Social.localUser.userName;
				PlayerPrefs.SetString("SocialName", text2);
			}
			return text2;
		}
		Defs.RuntimeAndroidEdition androidEdition = Defs.AndroidEdition;
		int num = 1;
		if (!string.IsNullOrEmpty(text2))
		{
			return text2;
		}
		return defaultPlayerName;
	}
}
