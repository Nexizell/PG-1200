using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Rilisoft;
using Rilisoft.MiniJson;
using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class AppsMenu : MonoBehaviour
{
	public Texture androidFon;

	public Texture riliFon;

	public Texture commicsFon;

	public Material fadeMaterial;

	public GameObject activityIndikatorPrefab;

	public string intendedSignatureHash;

	private Texture currentFon;

	private static Material m_Material = null;

	private static int _startFrameIndex;

	internal volatile object _preventAggressiveOptimisation;

	private const string ExternalStoragePermission = "android.permission.WRITE_EXTERNAL_STORAGE";

	private static volatile uint _preventInlining = 3565584061u;

	private const string _suffix = "Scene";

	private IDisposable _backSubscription;

	private readonly TaskCompletionSource<bool> _storagePermissionGrantedPromise = new TaskCompletionSource<bool>();

	private bool _storagePermissionRequested;

	internal static bool ApplicationBinarySplitted
	{
		get
		{
			return false;
		}
	}

	private Task<bool> StoragePermissionFuture
	{
		get
		{
			return _storagePermissionGrantedPromise.Task;
		}
	}

	internal IEnumerable<float> AppsMenuAwakeCoroutine()
	{
		yield return 0.1f;
		Device.isPixelGunLow = Device.isPixelGunLowDevice;
		Application.targetFrameRate = (120);
		_startFrameIndex = Time.frameCount;
		yield return 0.2f;
		if (!Launcher.UsingNewLauncher)
		{
			m_Material = fadeMaterial;
		}
	}

	private static IEnumerator MeetTheCoroutine(string sceneName, long abuseTicks, long nowTicks)
	{
		TimeSpan timeSpan = TimeSpan.FromTicks(Math.Abs(nowTicks - abuseTicks));
		if (Defs.IsDeveloperBuild)
		{
			if (timeSpan.TotalMinutes < 3.0)
			{
				yield break;
			}
		}
		else if (timeSpan.TotalDays < 1.0)
		{
			yield break;
		}
		float seconds = new System.Random(nowTicks.GetHashCode()).Next(15, 60);
		yield return new WaitForSeconds(seconds);
		SceneManager.LoadScene(sceneName);
	}

	private static string GetAbuseKey_53232de5(uint pad)
	{
		uint num = 0x97C95CDCu ^ pad;
		_preventInlining++;
		return num.ToString("x");
	}

	private static string GetAbuseKey_21493d18(uint pad)
	{
		uint num = 0xE5A34C21u ^ pad;
		_preventInlining++;
		return num.ToString("x");
	}

	private static string GetTerminalSceneName_4de1(uint gamma)
	{
		return "Closing4de1Scene".Replace(gamma.ToString("x"), string.Empty);
	}

	private void OnEnable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
		}
		_backSubscription = BackSystem.Instance.Register(Application.Quit, "AppsMenu");
	}

	private void OnDisable()
	{
		if (_backSubscription != null)
		{
			_backSubscription.Dispose();
			_backSubscription = null;
		}
	}

	private void Awake()
	{
		try
		{
			if (Storager.getInt("CampaignBoxes.SettedBoughtKeysOnUpdate") == 0)
			{
				ChooseBox.UnlockReachedBoxes();
				Storager.setInt("CampaignBoxes.SettedBoughtKeysOnUpdate", 1);
			}
		}
		catch (Exception ex)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in setting campaign boxes bought after update: {0}", ex);
		}
		try
		{
			CloudSyncController.Instance.Pull(true);
		}
		catch (Exception ex2)
		{
			UnityEngine.Debug.LogErrorFormat("Exception in AppsMenu CloudSyncController.Instance.Pull: {0}", ex2);
		}
		if (!Storager.hasKey("BalanceController.SHOWN_ACTION_IDS_KEY"))
		{
			Storager.setString("BalanceController.SHOWN_ACTION_IDS_KEY", Json.Serialize(new List<string>()));
		}
		FreeTicketsController instance = FreeTicketsController.Instance;
		LogsManager.Initialize();
		WeaponManager.FirstTagForOurTier(WeaponTags.PistolTag);
		if (Application.platform == RuntimePlatform.IPhonePlayer)
		{
			foreach (GadgetInfo.GadgetCategory item in Enum.GetValues(typeof(GadgetInfo.GadgetCategory)).OfType<GadgetInfo.GadgetCategory>())
			{
				if (item != GadgetInfo.GadgetCategory.Throwing && !Storager.hasKey(GadgetsInfo.SNForCategory(item)))
				{
					Storager.setString(GadgetsInfo.SNForCategory(item), "");
				}
			}
		}
		if (!Storager.hasKey(GadgetsInfo.SNForCategory(GadgetInfo.GadgetCategory.Throwing)))
		{
			Storager.setString(GadgetsInfo.SNForCategory(GadgetInfo.GadgetCategory.Throwing), "gadget_fraggrenade");
			GadgetsInfo.ProvideGadget("gadget_fraggrenade");
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.ShootingRangeCompleted && PlayerPrefs.GetInt("shop_tutorial_state_passed_VER_12_1", 0) >= 9)
		{
			TrainingController.CompletedTrainingStage = TrainingController.NewTrainingCompletedStage.ShopCompleted;
			AnalyticsStuff.Tutorial(AnalyticsConstants.TutorialState.Back_Shop);
		}
		currentFon = riliFon;
		if (ActivityIndicator.instance == null && activityIndikatorPrefab != null)
		{
			UnityEngine.Object.DontDestroyOnLoad(UnityEngine.Object.Instantiate(activityIndikatorPrefab));
		}
		ActivityIndicator.SetLoadingFon(currentFon);
		ActivityIndicator.IsShowWindowLoading = true;
	}

	private IEnumerator Start()
	{
		yield return null;
		Switcher.timer.Start();
		if (Defs.IsDeveloperBuild && Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			UnityEngine.Debug.Log(new StringBuilder("[Rilisoft] Trying to instantiate `android.os.AsyncTask`... ").ToString());
		}
		yield return null;
		if (!Storager.hasKey(Defs.PremiumEnabledFromServer))
		{
			Storager.setInt(Defs.PremiumEnabledFromServer, 0);
		}
		ActivityIndicator.IsActiveIndicator = false;
		foreach (float step in AppsMenuAwakeCoroutine())
		{
			Switcher.timer.Reset();
			Switcher.timer.Start();
			yield return null;
			_preventAggressiveOptimisation = step;
		}
		Switcher.timer.Reset();
		Switcher.timer.Start();
		if (Launcher.UsingNewLauncher)
		{
			yield break;
		}
		// if (Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		// {
		// 	Action<string> action = delegate(string sceneName)
		// 	{
		// 		if (Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		// 		{
		// 			string abuseKey_21493d = GetAbuseKey_21493d18(558447896u);
		// 			long num = DateTime.UtcNow.Ticks >> 1;
		// 			long result = num;
		// 			if (!Storager.hasKey(abuseKey_21493d))
		// 			{
		// 				Storager.setString(abuseKey_21493d, num.ToString());
		// 			}
		// 			else if (long.TryParse(Storager.getString(abuseKey_21493d), out result))
		// 			{
		// 				Storager.setString(abuseKey_21493d, Math.Min(num, result).ToString());
		// 			}
		// 			else
		// 			{
		// 				Storager.setString(abuseKey_21493d, num.ToString());
		// 			}
		// 			CoroutineRunner.Instance.StartCoroutine(MeetTheCoroutine(sceneName, result << 1, num << 1));
		// 		}
		// 	};
		// 	LicenseVerificationController.PackageInfo value = default(LicenseVerificationController.PackageInfo);
		// 	try
		// 	{
		// 		value = LicenseVerificationController.GetPackageInfo();
		// 		Launcher.PackageInfo = value;
		// 	}
		// 	catch (Exception ex)
		// 	{
		// 		UnityEngine.Debug.Log("LicenseVerificationController.GetPackageInfo() failed:    " + ex);
		// 		action(GetTerminalSceneName_4de1(19937u));
		// 	}
		// 	finally
		// 	{
		// 		if (value.SignatureHash == null)
		// 		{
		// 			UnityEngine.Debug.Log("actualPackageInfo.SignatureHash == null");
		// 			action(GetTerminalSceneName_4de1(19937u));
		// 		}
		// 	}
		// 	string packageName = value.PackageName;
		// 	if (string.Compare(packageName, Defs.GetIntendedAndroidPackageName(), StringComparison.Ordinal) != 0)
		// 	{
		// 		UnityEngine.Debug.LogWarning("Verification FakeBundleDetected:    " + packageName);
		// 		action(GetTerminalSceneName_4de1(19937u));
		// 	}
		// 	else
		// 	{
		// 		UnityEngine.Debug.Log("Package check passed.");
		// 	}
		// 	if (string.IsNullOrEmpty(intendedSignatureHash))
		// 	{
		// 		UnityEngine.Debug.LogWarning("String.IsNullOrEmpty(intendedSignatureHash)");
		// 		action(GetTerminalSceneName_4de1(19937u));
		// 	}
		// 	string signatureHash = value.SignatureHash;
		// 	if (string.Compare(signatureHash, intendedSignatureHash, StringComparison.Ordinal) != 0)
		// 	{
		// 		UnityEngine.Debug.LogWarning("Verification FakeSignatureDetected:    " + signatureHash);
		// 		Switcher.AppendAbuseMethod(AbuseMetod.AndroidPackageSignature);
		// 		action(GetTerminalSceneName_4de1(19937u));
		// 	}
		// 	else
		// 	{
		// 		UnityEngine.Debug.Log("Signature check passed.");
		// 	}
		// }
		yield return StartCoroutine(WaitForObbResolved());
		yield return null;
		StartCoroutine(Fade(1f, 1f));
		SetCurrentLanguage();
	}

	private IEnumerator WaitForStoragePermissionRequested()
	{
		RuntimePlatform buildTargetPlatform = BuildSettings.BuildTargetPlatform;
		yield break;
	}

	private static void OpenSettings()
	{
	}

	private IEnumerator WaitForObbResolved()
	{
		bool applicationBinarySplitted = ApplicationBinarySplitted;
		yield break;
	}

	private static int SafeGetSdkLevel()
	{
		try
		{
			return AndroidSystem.GetSdkVersion();
		}
		catch (Exception message)
		{
			UnityEngine.Debug.LogWarning(message);
			return 0;
		}
	}

	private void HandleStoragePermissionDialog(bool permissionGranted)
	{
		_storagePermissionGrantedPromise.TrySetResult(permissionGranted);
		NoodlePermissionGranter.PermissionRequestCallback = null;
	}

	private static void CheckRenameOldLanguageName()
	{
		if (Storager.IsInitialized(Defs.ChangeOldLanguageName))
		{
			return;
		}
		Storager.SetInitialized(Defs.ChangeOldLanguageName);
		string @string = PlayerPrefs.GetString(Defs.CurrentLanguage, string.Empty);
		if (!string.IsNullOrEmpty(@string))
		{
			switch (@string)
			{
			case "Français":
				PlayerPrefs.SetString(Defs.CurrentLanguage, "French");
				PlayerPrefs.Save();
				break;
			case "Deutsch":
				PlayerPrefs.SetString(Defs.CurrentLanguage, "German");
				PlayerPrefs.Save();
				break;
			case "日本人":
				PlayerPrefs.SetString(Defs.CurrentLanguage, "Japanese");
				PlayerPrefs.Save();
				break;
			case "Español":
				PlayerPrefs.SetString(Defs.CurrentLanguage, "Spanish");
				PlayerPrefs.Save();
				break;
			}
		}
	}

	internal static void SetCurrentLanguage()
	{
		CheckRenameOldLanguageName();
		string @string = PlayerPrefs.GetString(Defs.CurrentLanguage);
		if (string.IsNullOrEmpty(@string))
		{
			@string = LocalizationStore.CurrentLanguage;
		}
		else
		{
			LocalizationStore.CurrentLanguage = @string;
		}
	}

	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive)
	{
		UnityEngine.Debug.LogFormat("GameThrive HandleNotification('{0}', ..., {1})", message, isActive);
	}

	private void LoadLoading()
	{
		Switcher.timer.Reset();
		Switcher.timer.Start();
		GlobalGameController.currentLevel = -1;
		SceneManager.LoadSceneAsync("Loading");
	}

	private void DrawQuad(Color aColor, float aAlpha)
	{
		aColor.a = aAlpha;
		if (m_Material != null && m_Material.SetPass(0))
		{
			GL.PushMatrix();
			GL.LoadOrtho();
			GL.Begin(7);
			GL.Color(aColor);
			GL.Vertex3(0f, 0f, -1f);
			GL.Vertex3(0f, 1f, -1f);
			GL.Vertex3(1f, 1f, -1f);
			GL.Vertex3(1f, 0f, -1f);
			GL.End();
			GL.PopMatrix();
		}
		else
		{
			UnityEngine.Debug.LogWarning("Couldnot set pass for material.");
		}
	}

	private IEnumerator Fade(float aFadeOutTime, float aFadeInTime)
	{
		Color aColor = Color.black;
		float tm2 = 0f;
		while (tm2 < aFadeInTime)
		{
			tm2 += Time.deltaTime;
			DrawQuad(aColor, Mathf.Clamp01(tm2 / aFadeInTime));
			yield return new WaitForEndOfFrame();
		}
		if (!TrainingController.TrainingCompleted && TrainingController.CompletedTrainingStage == TrainingController.NewTrainingCompletedStage.None)
		{
			currentFon = commicsFon;
			if (ActivityIndicator.instance != null)
			{
				ActivityIndicator.instance.legendLabel.gameObject.SetActive(true);
				string text = LocalizationStore.Get("Key_1925");
				bool num = "Key_1925" == text;
				string text2 = (num ? "Please reboot your device if frozen.".ToUpperInvariant() : text);
				if (num)
				{
					ActivityIndicator.instance.legendLabel.ambigiousFont = ActivityIndicator.instance.bitmapFont;
					ActivityIndicator.instance.legendLabel.ProcessText();
				}
				ActivityIndicator.instance.legendLabel.text = text2;
			}
			else
			{
				UnityEngine.Debug.LogWarning("ActivityIndicator.instance is null.");
			}
			ActivityIndicator.IsActiveIndicator = false;
		}
		else
		{
			currentFon = ((BackgroundsManager.LoadingBackgroudTexture != null) ? BackgroundsManager.LoadingBackgroudTexture : androidFon);
			ActivityIndicator.IsActiveIndicator = true;
		}
		ActivityIndicator.SetLoadingFon(currentFon);
		tm2 = aFadeInTime;
		while (tm2 > 0f)
		{
			tm2 -= Time.deltaTime;
			DrawQuad(aColor, Mathf.Clamp01(tm2 / aFadeInTime));
			yield return new WaitForEndOfFrame();
		}
		LoadLoading();
	}

	public static Rect RiliFonRect()
	{
		float num = (float)Screen.height * 1.7766234f;
		return new Rect((float)Screen.width / 2f - num / 2f, 0f, num, Screen.height);
	}

	private void OnGUI()
	{
		bool usingNewLauncher = Launcher.UsingNewLauncher;
	}
}
