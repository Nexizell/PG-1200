using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Rilisoft;
using UnityEngine;

public sealed class coinsShop : MonoBehaviour
{
	public static coinsShop thisScript;

	public static bool showPlashkuPriExit = false;

	private bool productsReceived;

	public Action onResumeFronNGUI;

	private static readonly HashSet<string> _loggedPackages = new HashSet<string>();

	private static DateTime? _etcFileTimestamp;

	private Action _drawInnerInterface;

	public static bool IsStoreAvailable
	{
		get
		{
			if (!IsWideLayoutAvailable)
			{
				return !IsNoConnection;
			}
			return false;
		}
	}

	public static bool IsWideLayoutAvailable
	{
		get
		{
			if (!CheckAndroidHostsTampering() && !CheckLuckyPatcherInstalled() && !IsBangerrySupported())
			{
				return HasTamperedProducts;
			}
			return true;
		}
	}

	internal static bool HasTamperedProducts { private get; set; }

	public static bool IsBillingSupported
	{
		get
		{
			return false;
		}
	}

	public static bool IsNoConnection
	{
		get
		{
			if (thisScript == null)
			{
				return true;
			}
			if (!thisScript.productsReceived)
			{
				return true;
			}
			return !IsBillingSupported;
		}
	}

	private void SetProducts()
	{
		productsReceived = true;
	}

	private void OnEnable()
	{
		if (Application.loadedLevelName != "Loading")
		{
			ActivityIndicator.IsActiveIndicator = false;
		}
	}

	private void OnDisable()
	{
		ActivityIndicator.IsActiveIndicator = false;
	}

	private void OnDestroy()
	{
		thisScript = null;
		StoreKitEventListener.productListReceivedEvent -= SetProducts;
	}

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (Application.isEditor)
		{
			productsReceived = true;
		}
		thisScript = this;
		StoreKitEventListener.productListReceivedEvent += SetProducts;
		RefreshProductsIfNeed("coinsShop.Awake()");
	}

	private void Start()
	{
		LifetimeEventsRaiser.Instance.ApplicationPause += HandleApplicationPause;
	}

	public static void TryToFireCurrenciesAddEvent(string currency)
	{
		try
		{
			CoinsMessage.FireCoinsAddedEvent(currency == "GemsCurrency");
		}
		catch (Exception ex)
		{
			Debug.LogError("coinsShop.TryToFireCurrenciesAddEvent: FireCoinsAddedEvent( currency == Defs.Gems ): " + ex);
		}
	}

	public void HandlePurchaseButton(AbstractBankViewItem item)
	{
		int index = item.purchaseInfo.Index;
		PurchaseEventArgs.PurchaseType type = item.purchaseInfo.Type;
		ButtonClickSound.Instance.PlayClick();
		if ((type == PurchaseEventArgs.PurchaseType.Coins && (index >= StoreKitEventListener.coinIds.Length || index >= VirtualCurrencyHelper.coinInappsQuantity.Length)) || (type == PurchaseEventArgs.PurchaseType.Gems && (index >= StoreKitEventListener.gemsIds.Length || index >= VirtualCurrencyHelper.gemsInappsQuantity.Length)) || (type == PurchaseEventArgs.PurchaseType.Offer && index >= StoreKitEventListener.inappOffersIds.Length))
		{
			Debug.LogError("Index of purchase is out of range: " + index);
			return;
		}
		string productId = string.Empty;
		switch (type)
		{
		case PurchaseEventArgs.PurchaseType.Coins:
			productId = StoreKitEventListener.coinIds[index];
			break;
		case PurchaseEventArgs.PurchaseType.Gems:
			productId = StoreKitEventListener.gemsIds[index];
			break;
		case PurchaseEventArgs.PurchaseType.Offer:
			productId = StoreKitEventListener.inappOffersIds[index];
			break;
		default:
			Debug.LogError("HandlePurchaseButton: Unknown purchase type: " + type);
			return;
		}
		if (InappBonuessController.Instance.InappBonusAlreadyBought(item.InappBonusParameters))
		{
			return;
		}
		StoreKitEventListener.purchaseInProcess = true;
		InappBonuessController.Instance.RememberCurrentBonusForInapp(productId, item.InappBonusParameters);
	}

	private void GivePurchaseImmediately(PurchaseEventArgs.PurchaseType purchaseType, string productId, int index)
	{
	}

	public static void showCoinsShop()
	{
		thisScript.enabled = true;
		coinsPlashka.hideButtonCoins = true;
		coinsPlashka.showPlashka();
	}

	public static void hideCoinsShop()
	{
		if (thisScript != null)
		{
			thisScript.enabled = false;
			Resources.UnloadUnusedAssets();
		}
	}

	public static void ExitFromShop(bool performOnExitActs)
	{
		hideCoinsShop();
		if (showPlashkuPriExit)
		{
			coinsPlashka.hidePlashka();
		}
		coinsPlashka.hideButtonCoins = false;
		if (performOnExitActs && thisScript.onResumeFronNGUI != null)
		{
			thisScript.onResumeFronNGUI();
			thisScript.onResumeFronNGUI = null;
			coinsPlashka.hidePlashka();
		}
	}

	internal static bool CheckAndroidHostsTampering()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			return false;
		}
		if (Defs.AndroidEdition == Defs.RuntimeAndroidEdition.GoogleLite)
		{
			if (!File.Exists("/etc/hosts"))
			{
				return false;
			}
			try
			{
				return new string[0].Where((string l) => l.TrimStart().StartsWith("127.")).Any((string l) => l.Contains("android.clients.google.com") || l.Contains("mtalk.google.com "));
			}
			catch (Exception message)
			{
				Debug.LogError(message);
				return false;
			}
		}
		return false;
	}

	internal static bool CheckLuckyPatcherInstalled()
	{
		if (BuildSettings.BuildTargetPlatform != RuntimePlatform.Android)
		{
			return false;
		}
		return (from bytes in new string[3] { "Y29tLmRpbW9udmlkZW8ubHVja3lwYXRjaGVy", "Y29tLmNoZWxwdXMubGFja3lwYXRjaA==", "Y29tLmZvcnBkYS5scA==" }.Select(Convert.FromBase64String)
			where bytes != null
			select Encoding.UTF8.GetString(bytes, 0, bytes.Length)).Any(PackageExists);
	}

	private static bool PackageExists(string packageName)
	{
		if (packageName == null)
		{
			throw new ArgumentNullException("packageName");
		}
		bool isEditor = Application.isEditor;
		return false;
	}

	private static string ConvertFromBase64(string s)
	{
		byte[] array = Convert.FromBase64String(s);
		return Encoding.UTF8.GetString(array, 0, array.Length);
	}

	private static bool IsBangerrySupported()
	{
		try
		{
			return false;
		}
		catch (Exception ex)
		{
			Debug.LogWarningFormat("Exception in {0}: {1}", "IsBangerrySupported", ex);
			return false;
		}
	}

	private static DateTime? GetHostsTimestamp()
	{
		return null;
	}

	internal static bool CheckHostsTimestamp()
	{
		if (_etcFileTimestamp.HasValue)
		{
			DateTime? hostsTimestamp = GetHostsTimestamp();
			if (hostsTimestamp.HasValue && _etcFileTimestamp.Value != hostsTimestamp.Value)
			{
				Debug.LogError(string.Format("Timestamp check failed: {0:s} expcted, but actual value is {1:s}.", new object[2] { _etcFileTimestamp.Value, hostsTimestamp.Value }));
				return false;
			}
		}
		return true;
	}

	public void RefreshProductsIfNeed(string callerName, bool force = false)
	{
		string callee = string.Format(CultureInfo.InvariantCulture, "{0} > RefreshProductsIfNeed(), productsReceived: {1}, force: {2}", callerName ?? string.Empty, productsReceived, force);
		using (new ScopeLogger(callee, Defs.IsDeveloperBuild))
		{
			if (!productsReceived || force)
			{
				StoreKitEventListener.RefreshProducts();
			}
		}
	}

	private void HandleApplicationPause(bool pause)
	{
		if (Defs.IsDeveloperBuild)
		{
			Debug.LogFormat("coinsShop.HandleApplicationPause(), pause: {0}, IsBillingSupported: {1}", pause, IsBillingSupported);
		}
		if (!pause)
		{
			if (!IsBillingSupported)
			{
				StoreKitEventListener.InitializeGoogleIab();
			}
			else
			{
				RefreshProductsIfNeed("coinsShop.HandleApplicationPause()");
			}
		}
	}
}
