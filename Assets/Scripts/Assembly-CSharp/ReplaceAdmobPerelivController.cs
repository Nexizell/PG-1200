using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Rilisoft;
using UnityEngine;

public sealed class ReplaceAdmobPerelivController : MonoBehaviour
{
	[CompilerGenerated]
	internal sealed class _003CLoadDataCoroutine_003Ed__25 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public ReplaceAdmobPerelivController _003C_003E4__this;

		public FakeInterstitialConfigMemento fakeInterstitialConfig;

		public int index;

		private WWW _003CimageRequest_003E5__1;

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
		public _003CLoadDataCoroutine_003Ed__25(int _003C_003E1__state)
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
			{
				_003C_003E1__state = -1;
				_003C_003E4__this.DataLoading = true;
				if (fakeInterstitialConfig.ImageUrls.Count == 0)
				{
					UnityEngine.Debug.LogWarning("LoadDataCoroutine(): fakeInterstitialConfig.ImageUrls.Count == 0");
					return false;
				}
				string urlString = fakeInterstitialConfig.ImageUrls[index % fakeInterstitialConfig.ImageUrls.Count];
				string imageURLForOurQuality = _003C_003E4__this.GetImageURLForOurQuality(urlString);
				_003CimageRequest_003E5__1 = Tools.CreateWwwIfNotConnected(imageURLForOurQuality);
				if (_003CimageRequest_003E5__1 == null)
				{
					_003C_003E4__this.DataLoading = false;
					return false;
				}
				_003C_003E2__current = _003CimageRequest_003E5__1;
				_003C_003E1__state = 1;
				return true;
			}
			case 1:
				_003C_003E1__state = -1;
				if (!string.IsNullOrEmpty(_003CimageRequest_003E5__1.error))
				{
					UnityEngine.Debug.LogWarningFormat("ReplaceAdmobPerelivController: {0}", _003CimageRequest_003E5__1.error);
					_003C_003E4__this.DataLoading = false;
					return false;
				}
				if (!_003CimageRequest_003E5__1.texture)
				{
					_003C_003E4__this.DataLoading = false;
					UnityEngine.Debug.LogWarning("ReplaceAdmobPerelivController: imageRequest.texture = null. returning...");
					return false;
				}
				if (_003CimageRequest_003E5__1.texture.width < 20)
				{
					_003C_003E4__this.DataLoading = false;
					UnityEngine.Debug.LogWarning("ReplaceAdmobPerelivController: imageRequest.texture is dummy. returning...");
					return false;
				}
				_003C_003E4__this._image = _003CimageRequest_003E5__1.texture;
				if (fakeInterstitialConfig.RedirectUrls.Count == 0)
				{
					UnityEngine.Debug.LogWarning("LoadDataCoroutine(): fakeInterstitialConfig.RedirectUrls.Count == 0");
					return false;
				}
				_003C_003E4__this._adUrl = fakeInterstitialConfig.RedirectUrls[index % fakeInterstitialConfig.RedirectUrls.Count];
				_003C_003E4__this.DataLoading = false;
				return false;
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

	private Texture2D _image;

	private string _adUrl;

	private static int _timesWantToShow = -1;

	private static int _timesShown = 0;

	private long _timeSuspended;

	public static ReplaceAdmobPerelivController sharedController { get; private set; }

	public static bool ShouldShowAtThisTime
	{
		get
		{
			if (PromoActionsManager.ReplaceAdmobPereliv == null)
			{
				return false;
			}
			if (PromoActionsManager.ReplaceAdmobPereliv.ShowEveryTimes <= 0)
			{
				return false;
			}
			return _timesWantToShow % PromoActionsManager.ReplaceAdmobPereliv.ShowEveryTimes == 0;
		}
	}

	public Texture2D Image
	{
		get
		{
			return _image;
		}
	}

	public string AdUrl
	{
		get
		{
			return _adUrl;
		}
	}

	public bool DataLoaded
	{
		get
		{
			if (_image != null)
			{
				return _adUrl != null;
			}
			return false;
		}
	}

	public bool DataLoading { get; private set; }

	public bool ShouldShowInLobby { get; set; }

	public static void IncreaseTimesCounter()
	{
		_timesWantToShow++;
	}

	public static void TryShowPereliv(string context)
	{
		DateTime? serverTime = FriendsController.GetServerTime();
		if (!serverTime.HasValue)
		{
			UnityEngine.Debug.LogWarning("Server time is not received.");
		}
		else if (sharedController != null && sharedController.Image != null && sharedController.AdUrl != null)
		{
			AdmobPerelivWindow.admobTexture = sharedController.Image;
			AdmobPerelivWindow.admobUrl = sharedController.AdUrl;
			AdmobPerelivWindow.Context = context;
			PlayerPrefs.SetString(Defs.LastTimeShowBanerKey, serverTime.Value.ToString("s"));
			FyberFacade.Instance.IncrementCurrentDailyInterstitialCount(serverTime.Value);
			_timesShown++;
			InterstitialCounter.Instance.IncrementFakeInterstitialCount();
		}
	}

	public void DestroyImage()
	{
		if (Image != null)
		{
			_image = null;
		}
	}

	public void LoadPerelivData()
	{
		try
		{
			if (DataLoading)
			{
				UnityEngine.Debug.LogWarning("ReplaceAdmobPerelivController: data is already loading. returning...");
				return;
			}
			if (_image != null)
			{
				UnityEngine.Object.Destroy(_image);
			}
			_image = null;
			_adUrl = null;
			if (AdsConfigManager.Instance.LastLoadedConfig == null)
			{
				UnityEngine.Debug.LogWarning("LoadPerelivData(): AdsConfigManager.Instance.LastLoadedConfig == null");
				return;
			}
			FakeInterstitialConfigMemento fakeInterstitialConfig = AdsConfigManager.Instance.LastLoadedConfig.FakeInterstitialConfig;
			int count = fakeInterstitialConfig.ImageUrls.Count;
			if (count <= 0)
			{
				UnityEngine.Debug.LogWarning("LoadPerelivData(): fakeInterstitialConfig.ImageUrls.Count == 0");
				return;
			}
			int index = UnityEngine.Random.Range(0, count);
			StartCoroutine(LoadDataCoroutine(fakeInterstitialConfig, index));
		}
		catch (Exception exception)
		{
			UnityEngine.Debug.LogException(exception);
		}
	}

	private string GetImageURLForOurQuality(string urlString)
	{
		string value = string.Empty;
		if (Screen.height >= 500)
		{
			value = "-Medium";
		}
		if (Screen.height >= 900)
		{
			value = "-Hi";
		}
		urlString = urlString.Insert(urlString.LastIndexOf("."), value);
		return urlString;
	}

	private IEnumerator LoadDataCoroutine(FakeInterstitialConfigMemento fakeInterstitialConfig, int index)
	{
		DataLoading = true;
		if (fakeInterstitialConfig.ImageUrls.Count == 0)
		{
			UnityEngine.Debug.LogWarning("LoadDataCoroutine(): fakeInterstitialConfig.ImageUrls.Count == 0");
			yield break;
		}
		string urlString = fakeInterstitialConfig.ImageUrls[index % fakeInterstitialConfig.ImageUrls.Count];
		string imageURLForOurQuality = GetImageURLForOurQuality(urlString);
		WWW imageRequest = Tools.CreateWwwIfNotConnected(imageURLForOurQuality);
		if (imageRequest == null)
		{
			DataLoading = false;
			yield break;
		}
		yield return imageRequest;
		if (!string.IsNullOrEmpty(imageRequest.error))
		{
			UnityEngine.Debug.LogWarningFormat("ReplaceAdmobPerelivController: {0}", imageRequest.error);
			DataLoading = false;
			yield break;
		}
		if (!imageRequest.texture)
		{
			DataLoading = false;
			UnityEngine.Debug.LogWarning("ReplaceAdmobPerelivController: imageRequest.texture = null. returning...");
			yield break;
		}
		if (imageRequest.texture.width < 20)
		{
			DataLoading = false;
			UnityEngine.Debug.LogWarning("ReplaceAdmobPerelivController: imageRequest.texture is dummy. returning...");
			yield break;
		}
		_image = imageRequest.texture;
		if (fakeInterstitialConfig.RedirectUrls.Count == 0)
		{
			UnityEngine.Debug.LogWarning("LoadDataCoroutine(): fakeInterstitialConfig.RedirectUrls.Count == 0");
			yield break;
		}
		_adUrl = fakeInterstitialConfig.RedirectUrls[index % fakeInterstitialConfig.RedirectUrls.Count];
		DataLoading = false;
	}

	private void Awake()
	{
		sharedController = this;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	private void OnApplicationPause(bool pausing)
	{
		if (!pausing)
		{
			if (PromoActionsManager.CurrentUnixTime - _timeSuspended > 3600)
			{
				_timesShown = 0;
				InterstitialCounter.Instance.Reset();
			}
		}
		else
		{
			_timeSuspended = PromoActionsManager.CurrentUnixTime;
		}
	}
}
