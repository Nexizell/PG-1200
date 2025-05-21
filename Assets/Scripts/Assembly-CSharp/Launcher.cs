using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;
using UnityEngine.SocialPlatforms.GameCenter;
using UnityEngine.UI;

public sealed class Launcher : MonoBehaviour
{
	internal struct Bounds
	{
		private readonly float _lower;

		private readonly float _upper;

		public float Lower
		{
			get
			{
				return _lower;
			}
		}

		public float Upper
		{
			get
			{
				return _upper;
			}
		}

		public Bounds(float lower, float upper)
		{
			_lower = Mathf.Min(lower, upper);
			_upper = Mathf.Max(lower, upper);
		}

		private float Clamp(float value)
		{
			return Mathf.Clamp(value, _lower, _upper);
		}

		public float Lerp(float value, float t)
		{
			return Mathf.Lerp(Clamp(value), _upper, t);
		}

		public float Lerp(float t)
		{
			return Lerp(_lower, t);
		}
	}

	public string intendedSignatureHash;

	public GameObject inAppGameObjectPrefab;

	public Canvas Canvas;

	public Slider ProgressSlider;

	public Text ProgressLabel;

	public RawImage SplashScreen;

	public GameObject amazonIapManagerPrefab;

	private GameObject amazonGameCircleManager;

	private static float? _progress;

	private bool _amazonGamecircleManagerInitialized;

	private bool _amazonIapManagerInitialized;

	private bool _crossfadeFinished;

	private static bool? _usingNewLauncher;

	private string _leaderboardId = string.Empty;

	private int _targetFramerate = 30;

	internal static LicenseVerificationController.PackageInfo? PackageInfo { get; set; }

	internal static bool UsingNewLauncher
	{
		get
		{
			if (_usingNewLauncher.HasValue)
			{
				return _usingNewLauncher.Value;
			}
			return false;
		}
	}

	private void Awake()
	{
		if (Application.platform == RuntimePlatform.Android || Tools.RuntimePlatform == RuntimePlatform.MetroPlayerX64)
		{
			Application.targetFrameRate = 30;
		}
		_targetFramerate = ((Application.targetFrameRate == -1) ? 300 : Mathf.Clamp(Application.targetFrameRate, 30, 60));
		if (!_usingNewLauncher.HasValue)
		{
			_usingNewLauncher = SceneLoader.ActiveSceneName.Equals("Launcher");
		}
		if (ProgressLabel != null)
		{
			ProgressLabel.text = 0f.ToString("P0");
		}
	}

	private IEnumerable<float> SplashScreenFadeOut()
	{
		if (SplashScreen != null)
		{
			int splashScreenFadeOutFrameCount = _targetFramerate;
			SplashScreen.gameObject.SetActive(true);
			int i = 0;
			while (i != splashScreenFadeOutFrameCount)
			{
				Color color = Color.Lerp(Color.white, Color.black, (float)i / (float)splashScreenFadeOutFrameCount);
				SplashScreen.color = color;
				yield return 0f;
				int num = i + 1;
				i = num;
			}
			SplashScreen.color = Color.black;
			yield return 1f;
		}
	}

	private IEnumerable<float> LoadingProgressFadeIn()
	{
		if (SplashScreen != null)
		{
			int loadingFadeInFrameCount = _targetFramerate;
			Color transparentColor = new Color(0f, 0f, 0f, 0f);
			int i = 0;
			while (i != loadingFadeInFrameCount)
			{
				float t = Mathf.Pow((float)i / (float)loadingFadeInFrameCount, 2.2f);
				Color color = Color.Lerp(Color.black, transparentColor, t);
				SplashScreen.color = color;
				yield return 0.5f;
				int num = i + 1;
				i = num;
			}
			UnityEngine.Object.Destroy(SplashScreen.gameObject);
			yield return 1f;
		}
		_crossfadeFinished = true;
	}

	private IEnumerator LoadingProgressFadeInCoroutine()
	{
		foreach (float item in LoadingProgressFadeIn())
		{
			float num = item;
			yield return null;
		}
	}

	private IEnumerator Start()
	{
		if (!_progress.HasValue)
		{
			foreach (float item in SplashScreenFadeOut())
			{
				float num = item;
				yield return null;
			}
			foreach (float item2 in LoadingProgressFadeIn())
			{
				float num2 = item2;
				yield return null;
			}
			_progress = 0f;
			FrameStopwatchScript stopwatch = GetComponent<FrameStopwatchScript>();
			if (stopwatch == null)
			{
				stopwatch = gameObject.AddComponent<FrameStopwatchScript>();
			}
			foreach (float item3 in InitRootCoroutine())
			{
				if (item3 >= 0f)
				{
					_progress = item3;
				}
				if (stopwatch != null)
				{
					float secondsSinceFrameStarted = stopwatch.GetSecondsSinceFrameStarted();
					if (item3 >= 0f && secondsSinceFrameStarted < 1.618f / (float)_targetFramerate)
					{
						continue;
					}
				}
				if (ProgressSlider != null)
				{
					ProgressSlider.value = _progress.Value;
				}
				if (ProgressLabel != null)
				{
					ProgressLabel.text = _progress.Value.ToString("P0");
				}
				if (!ActivityIndicator.IsActiveIndicator)
				{
					ActivityIndicator.IsActiveIndicator = _crossfadeFinished;
				}
				yield return null;
			}
			if (Canvas != null)
			{
				UnityEngine.Object.Destroy(Canvas.gameObject);
			}
			UnityEngine.Object.Destroy(gameObject);
		}
		else
		{
			while (_progress < 1f)
			{
				yield return null;
			}
		}
	}

	private static void LogMessageWithBounds(string prefix, Bounds bounds)
	{
		UnityEngine.Debug.Log(string.Format("{0}: [{1:P0}, {2:P0}]\t\t{3}", prefix, bounds.Lower, bounds.Upper, Time.frameCount));
	}

	private IEnumerable<float> InitRootCoroutine()
	{
		Bounds bounds7 = new Bounds(0f, 0.04f);
		LogMessageWithBounds("AppsMenuAwakeCoroutine()", bounds7);
		Bounds bounds6 = new Bounds(0.05f, 0.09f);
		LogMessageWithBounds("AppsMenuStartCoroutine()", bounds6);
		foreach (float item in AppsMenuStartCoroutine())
		{
			yield return bounds6.Lerp(item);
		}
		Bounds bounds5 = new Bounds(0.1f, 0.19f);
		LogMessageWithBounds("InAppInstancerStartCoroutine()", bounds5);
		foreach (float item2 in InAppInstancerStartCoroutine())
		{
			yield return bounds5.Lerp(item2);
		}
		Bounds bounds4 = new Bounds(0.2f, 0.24f);
		LogMessageWithBounds("Application.LoadLevelAdditiveAsync(\"AppCenter\")", bounds4);
		AsyncOperation loadingCoroutine2 = Application.LoadLevelAdditiveAsync("AppCenter");
		while (!loadingCoroutine2.isDone)
		{
			yield return bounds4.Lerp(loadingCoroutine2.progress);
		}
		yield return -1f;
		Bounds bounds3 = new Bounds(0.25f, 0.29f);
		LogMessageWithBounds("Application.LoadLevelAdditiveAsync(\"Loading\")", bounds3);
		AsyncOperation loadingCoroutine = Application.LoadLevelAdditiveAsync("Loading");
		while (!loadingCoroutine.isDone)
		{
			yield return bounds3.Lerp(loadingCoroutine.progress);
		}
		yield return -1f;
		Switcher switcher = UnityEngine.Object.FindObjectOfType<Switcher>();
		if (switcher != null)
		{
			Bounds bounds2 = new Bounds(0.3f, 0.89f);
			LogMessageWithBounds("Switcher.InitializeSwitcher()", bounds2);
			foreach (float item3 in switcher.InitializeSwitcher())
			{
				yield return (item3 < 0f) ? item3 : bounds2.Lerp(item3);
			}
		}
		Bounds bounds = new Bounds(0.9f, 0.99f);
		LogMessageWithBounds("Switcher.LoadMainMenu()", bounds);
		foreach (float item4 in switcher.LoadMainMenu())
		{
			yield return bounds.Lerp(item4);
		}
		yield return 1f;
	}

	private static string GetTerminalSceneName_3afcc97c(uint gamma)
	{
		return "ClosingScene";
	}

	private IEnumerable<float> AppsMenuStartCoroutine()
	{
		if (Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			LicenseVerificationController.PackageInfo value = default(LicenseVerificationController.PackageInfo);
			try
			{
				value = LicenseVerificationController.GetPackageInfo();
				PackageInfo = value;
			}
			catch (Exception ex)
			{
				UnityEngine.Debug.Log("LicenseVerificationController.GetPackageInfo() failed:    " + ex);
				Singleton<SceneLoader>.Instance.LoadScene(GetTerminalSceneName_3afcc97c(989645180u));
			}
			finally
			{
				if (value.SignatureHash == null)
				{
					UnityEngine.Debug.Log("actualPackageInfo.SignatureHash == null");
					Singleton<SceneLoader>.Instance.LoadScene(GetTerminalSceneName_3afcc97c(989645180u));
				}
			}
			string packageName = value.PackageName;
			if (string.Compare(packageName, Defs.GetIntendedAndroidPackageName(), StringComparison.Ordinal) != 0)
			{
				UnityEngine.Debug.LogWarning("Verification FakeBundleDetected:    " + packageName);
				Singleton<SceneLoader>.Instance.LoadScene(GetTerminalSceneName_3afcc97c(989645180u));
			}
			else
			{
				UnityEngine.Debug.Log("Package check passed.");
			}
			if (string.IsNullOrEmpty(intendedSignatureHash))
			{
				UnityEngine.Debug.LogWarning("String.IsNullOrEmpty(intendedSignatureHash)");
				Singleton<SceneLoader>.Instance.LoadScene(GetTerminalSceneName_3afcc97c(989645180u));
			}
			string signatureHash = value.SignatureHash;
			if (string.Compare(signatureHash, intendedSignatureHash, StringComparison.Ordinal) != 0)
			{
				UnityEngine.Debug.LogWarning("Verification FakeSignatureDetected:    " + signatureHash);
				Singleton<SceneLoader>.Instance.LoadScene(GetTerminalSceneName_3afcc97c(989645180u));
			}
			else
			{
				UnityEngine.Debug.Log("Signature check passed.");
			}
			yield return 0.2f;
		}
		yield return 0.8f;
		AppsMenu.SetCurrentLanguage();
		yield return 1f;
	}

	private IEnumerable<float> InAppInstancerStartCoroutine()
	{
		if (!GameObject.FindGameObjectWithTag("InAppGameObject"))
		{
			UnityEngine.Object.Instantiate(inAppGameObjectPrefab, Vector3.zero, Quaternion.identity);
			yield return 0.1f;
		}
		if (amazonIapManagerPrefab == null)
		{
			UnityEngine.Debug.LogWarning("amazonIapManager == null");
		}
		else if (!_amazonIapManagerInitialized)
		{
			UnityEngine.Object.Instantiate(amazonIapManagerPrefab, Vector3.zero, Quaternion.identity);
			_amazonIapManagerInitialized = true;
			yield return 0.2f;
		}
		if (Application.platform == RuntimePlatform.Android && Defs.AndroidEdition == Defs.RuntimeAndroidEdition.Amazon)
		{
			if (amazonGameCircleManager == null)
			{
				UnityEngine.Debug.LogWarning("amazonGamecircleManager == null");
			}
			else if (!_amazonGamecircleManagerInitialized)
			{
				UnityEngine.Object.DontDestroyOnLoad(amazonGameCircleManager);
				_amazonGamecircleManagerInitialized = true;
			}
		}
		else if (BuildSettings.BuildTargetPlatform == RuntimePlatform.IPhonePlayer)
		{
			GameCenterPlatform.ShowDefaultAchievementCompletionBanner(true);
		}
		yield return 1f;
	}

	private static void HandleNotification(string message, Dictionary<string, object> additionalData, bool isActive)
	{
		UnityEngine.Debug.Log(string.Format("GameThrive HandleNotification(“{0}”, ..., {1})", new object[2] { message, isActive }));
	}

	private void HandleAmazonGamecircleServiceReady()
	{
	}

	private void HandleAmazonPotentialProgressConflicts()
	{
		UnityEngine.Debug.Log("HandleAmazonPotentialProgressConflicts()");
	}

	private void HandleAmazonSyncFailed()
	{
	}

	private void HandleAmazonThrottled()
	{
		UnityEngine.Debug.LogWarning("HandleAmazonThrottled().");
	}

	private void HandleAmazonGamecircleServiceNotReady(string message)
	{
		UnityEngine.Debug.LogError("Amazon GameCircle service is not ready:\n" + message);
	}

	private void HandleAmazonSubmitScoreSucceeded(string leaderbordId)
	{
		if (UnityEngine.Debug.isDebugBuild)
		{
			UnityEngine.Debug.Log("Submit score succeeded for leaderboard " + leaderbordId);
		}
	}

	private void HandleAmazonSubmitScoreFailed(string leaderbordId, string error)
	{
		UnityEngine.Debug.LogError(string.Format("Submit score failed for leaderboard {0}:\n{1}", new object[2] { leaderbordId, error }));
	}
}
