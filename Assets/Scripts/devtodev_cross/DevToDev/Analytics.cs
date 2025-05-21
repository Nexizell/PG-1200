using System.Collections.Generic;
using DevToDev.Logic;

namespace DevToDev
{
	public class Analytics
	{
		private static readonly string SDK_VERSION = "2.2";

		public static string UserId
		{
			get
			{
				return SDKClient.Instance.ActiveUserId;
			}
			set
			{
				SDKClient.Instance.ActiveUserId = value;
			}
		}

		public static string SDKVersion
		{
			get
			{
				return SDK_VERSION;
			}
		}

		public static string ApplicationVersion
		{
			get
			{
				return SDKClient.Instance.ApplicationVersion;
			}
			set
			{
				SDKClient.Instance.ApplicationVersion = value;
			}
		}

		public static People ActiveUser
		{
			get
			{
				return SDKClient.Instance.ActiveUser;
			}
		}

		public static void SetActiveLog(bool isActive)
		{
			SDKClient.Instance.SetActiveLog(isActive);
		}

		public static void Initialize(string appKey, string secretKey)
		{
			SDKClient.Instance.Initialize(appKey, secretKey);
		}

		public static void SendBufferedEvents()
		{
			SDKClient.Instance.SendBufferedEvents();
		}

		public static void StartSession()
		{
			SDKClient.Instance.StartSession();
		}

		public static void EndSession()
		{
			SDKClient.Instance.EndSession(0L);
		}

		public static void CurrencyAccrual(int amount, string currencyName, AccrualType accrualType)
		{
			SDKClient.Instance.CurrencyAccrual(currencyName, amount, accrualType);
		}

		public static void LevelUp(int newLevel)
		{
			SDKClient.Instance.LevelUp(newLevel, null);
		}

		public static void Referral(IDictionary<ReferralProperty, string> referralData)
		{
			SDKClient.Instance.Referral(referralData);
		}

		public static void LevelUp(int newLevel, Dictionary<string, int> values)
		{
			SDKClient.Instance.LevelUp(newLevel, values);
		}

		public static void InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, int purchasePrice, string purchaseCurrency)
		{
			Dictionary<string, int> dictionary = new Dictionary<string, int>();
			dictionary.Add(purchaseCurrency, purchasePrice);
			SDKClient.Instance.InAppPurchase(purchaseId, purchaseType, purchaseAmount, dictionary);
		}

		public static void InAppPurchase(string purchaseId, string purchaseType, int purchaseAmount, Dictionary<string, int> values)
		{
			SDKClient.Instance.InAppPurchase(purchaseId, purchaseType, purchaseAmount, values);
		}

		public static void Tutorial(int step)
		{
			SDKClient.Instance.Tutorial(step);
		}

		public static void RealPayment(string transactionId, float inAppPrice, string inAppName, string inAppCurrencyISOCode)
		{
			SDKClient.Instance.RealPayment(transactionId, inAppPrice, inAppName, inAppCurrencyISOCode);
		}

		public static void SocialNetworkConnect(SocialNetwork network)
		{
			SDKClient.Instance.SocialNetworkConnect(network);
		}

		public static void SocialNetworkPost(SocialNetwork network, string reason)
		{
			SDKClient.Instance.SocialNetworkPost(network, reason);
		}

		public static void CustomEvent(string eventName)
		{
			SDKClient.Instance.CustomEvent(eventName, null);
		}

		public static void CustomEvent(string eventName, CustomEventParams eventParams)
		{
			SDKClient.Instance.CustomEvent(eventName, eventParams);
		}

		public static void StartProgressionEvent(string eventId, ProgressionEventParams eventParams)
		{
			SDKClient.Instance.StartProgressionEvent(eventId, eventParams);
		}

		public static void EndProgressionEvent(string eventId, ProgressionEventParams eventParams)
		{
			SDKClient.Instance.EndProgressionEvent(eventId, eventParams);
		}

		public static void CurrentLevel(int currentLevel)
		{
			SDKClient.Instance.SetCurrentLevel(currentLevel);
		}

		public static void ReplaceUserId(string fromUserId, string toUserId)
		{
			SDKClient.Instance.ReplaceUserId(fromUserId, toUserId);
		}
	}
}
