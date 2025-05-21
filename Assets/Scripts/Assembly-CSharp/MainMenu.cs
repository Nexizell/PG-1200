using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Prime31;
using Rilisoft;
using UnityEngine;

public sealed class MainMenu : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CWaitForExperienceGuiAndAdd_003Ed__33 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ExperienceController legacyExperienceController;

		public int addend;

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
		public _003CWaitForExperienceGuiAndAdd_003Ed__33(int _003C_003E1__state)
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
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (ExpController.Instance == null)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			legacyExperienceController.AddExperience(addend);
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

	public static MainMenu sharedMenu = null;

	public GameObject JoysticksUIRoot;

	public static bool BlockInterface = false;

	public static bool IsAdvertRun = false;

	private bool isShowDeadMatch;

	private bool isShowCOOP;

	public bool isFirstFrame = true;

	public bool isInappWinOpen;

	private bool musicOld;

	private bool fxOld;

	public Texture inAppFon;

	public GUIStyle puliInApp;

	public GUIStyle healthInApp;

	public GUIStyle pulemetInApp;

	public GUIStyle crystalSwordInapp;

	public GUIStyle elixirInapp;

	private bool showUnlockDialog;

	private bool isPressFullOnMulty;

	private float _timeWhenPurchShown;

	public GameObject skinsManagerPrefab;

	public GameObject weaponManagerPrefab;

	public GUIStyle backBut;

	private ExperienceController expController;

	private AdvertisementController _advertisementController;

	public bool isShowAvard;

	public static readonly string iTunesEnderManID = "811995374";

	private static bool firstEnterLobbyAtThisLaunch = true;

	private bool _skinsMakerQuerySucceeded;

	public static int FontSizeForMessages
	{
		get
		{
			return Mathf.RoundToInt((float)Screen.height * 0.03f);
		}
	}

	public static string RateUsURL
	{
		get
		{
			if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
			{
				return "https://play.google.com/store/apps/details?id=com.pixel.gun3d&hl=en";
			}
			return Defs2.ApplicationUrl;
		}
	}

	public static float iOSVersion
	{
		get
		{
			float result = -1f;
			if (Application.platform == RuntimePlatform.IPhonePlayer)
			{
				float.TryParse(SystemInfo.operatingSystem.Replace("iPhone OS ", "").Substring(0, 1), out result);
			}
			return result;
		}
	}

	private void completionHandler(string error, object result)
	{
		if (error != null)
		{
			UnityEngine.Debug.LogError(error);
		}
		else
		{
			Utils.logObject(result);
		}
	}

	private void Awake()
	{
		if (firstEnterLobbyAtThisLaunch)
		{
			firstEnterLobbyAtThisLaunch = false;
			GlobalGameController.SetMultiMode();
			return;
		}
		using (new StopwatchLogger("MainMenu.Awake()"))
		{
			GlobalGameController.SetMultiMode();
			WeaponManager.sharedManager.Reset();
		}
	}

	private IEnumerator WaitForExperienceGuiAndAdd(ExperienceController legacyExperienceController, int addend)
	{
		while (ExpController.Instance == null)
		{
			yield return null;
		}
		legacyExperienceController.AddExperience(addend);
	}

	private void Start()
	{
		using (new StopwatchLogger("MainMenu.Start()"))
		{
			sharedMenu = this;
			StoreKitEventListener.State.Mode = "In_main_menu";
			StoreKitEventListener.State.PurchaseKey = "In shop";
			StoreKitEventListener.State.Parameters.Clear();
			if (!FriendsController.sharedController.dataSent)
			{
				FriendsController.sharedController.InitOurInfo();
				FriendsController.sharedController.StartCoroutine(FriendsController.sharedController.WaitForReadyToOperateAndUpdatePlayer());
				FriendsController.sharedController.dataSent = true;
			}
			if (NotificationController.isGetEveryDayMoney)
			{
				isShowAvard = true;
			}
			expController = ExperienceController.sharedController;
			if (expController == null)
			{
				UnityEngine.Debug.LogError("MainMenu.Start():    expController == null");
			}
			if (expController != null)
			{
				expController.isMenu = true;
			}
			float coef = Defs.Coef;
			if (expController != null)
			{
				expController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
			}
			if (PlayerPrefs.GetString(Defs.ShouldReoeatActionSett, "").Equals(Defs.GoToProfileAction))
			{
				PlayerPrefs.SetString(Defs.ShouldReoeatActionSett, "");
				PlayerPrefs.Save();
			}
			Storager.setInt(Defs.EarnedCoins, 0);
			Invoke("setEnabledGUI", 0.1f);
			ActivityIndicator.IsActiveIndicator = true;
			PlayerPrefs.SetInt("typeConnect__", -1);
			if (!GameObject.FindGameObjectWithTag("SkinsManager") && (bool)skinsManagerPrefab)
			{
				UnityEngine.Object.Instantiate(skinsManagerPrefab, Vector3.zero, Quaternion.identity);
			}
			if (!WeaponManager.sharedManager && (bool)weaponManagerPrefab)
			{
				UnityEngine.Object.Instantiate(weaponManagerPrefab, Vector3.zero, Quaternion.identity);
			}
			GlobalGameController.ResetParameters();
			GlobalGameController.Score = 0;
			if (PlayerPrefs.GetInt(Defs.ShouldEnableShopSN, 0) == 1)
			{
				PlayerPrefs.SetInt(Defs.ShouldEnableShopSN, 0);
				PlayerPrefs.Save();
			}
		}
	}

	private void SetInApp()
	{
		isInappWinOpen = !isInappWinOpen;
		if (expController != null)
		{
			expController.isShowRanks = !isInappWinOpen;
			expController.isMenu = !isInappWinOpen;
		}
		if (isInappWinOpen)
		{
			if (StoreKitEventListener.restoreInProcess)
			{
				ActivityIndicator.IsActiveIndicator = true;
			}
			if (!Defs.isMulti)
			{
				Time.timeScale = 0f;
			}
		}
		else
		{
			ActivityIndicator.IsActiveIndicator = false;
		}
	}

	public static bool SkinsMakerSupproted()
	{
		bool result = BuildSettings.BuildTargetPlatform != RuntimePlatform.MetroPlayerX64;
		if (BuildSettings.BuildTargetPlatform == RuntimePlatform.Android)
		{
			result = Defs.AndroidEdition != Defs.RuntimeAndroidEdition.GoogleLite;
		}
		return result;
	}

	private void Update()
	{
		float num = ((float)Screen.width - 42f * Defs.Coef - Defs.Coef * (672f + (float)(SkinsMakerSupproted() ? 262 : 0))) / (SkinsMakerSupproted() ? 3f : 2f);
		if (expController != null)
		{
			expController.posRanks = new Vector2(21f * Defs.Coef, 21f * Defs.Coef);
		}
	}

	private void OnDestroy()
	{
		sharedMenu = null;
		if (expController != null)
		{
			expController.isShowRanks = false;
			expController.isMenu = false;
		}
	}
}
