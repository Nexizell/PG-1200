using System;
using System.Collections.Generic;
using UnityEngine;

public class AppsFlyer : MonoBehaviour
{
	public static void validateReceipt(string publicKey, string purchaseData, string signature, string price, string currency, Dictionary<string, string> extraParams)
	{
	}

	public static void validateReceipt(string productIdentifier, string price, string currency, string transactionId, Dictionary<string, string> additionalParametes)
	{
	}

	public static void handlePushNotification(Dictionary<string, string> payload)
	{
	}

	public static void registerUninstall(byte[] token)
	{
	}

	public static void setCollectIMEI(bool shouldCollect)
	{
	}

	public static void createValidateInAppListener(string aObject, string callbackMethod, string callbackFailedMethod)
	{
	}

	public static void init(string devKey)
	{
	}

	public static void setGCMProjectNumber(string googleGCMNumber)
	{
	}

	public static void setImeiData(string imeiData)
	{
	}

	public static void trackEvent(string eventName, string eventValue)
	{
	}

	public static void setCurrencyCode(string currencyCode)
	{
	}

	public static void setCustomerUserID(string customerUserID)
	{
	}

	public static void loadConversionData(string callbackObject)
	{
	}

	[Obsolete("Use loadConversionData(string callbackObject)")]
	public static void loadConversionData(string callbackObject, string callbackMethod, string callbackFailedMethod)
	{
	}

	public static void setAppsFlyerKey(string key)
	{
	}

	public static void trackAppLaunch()
	{
	}

	public static void setAppID(string appleAppId)
	{
	}

	public static void trackRichEvent(string eventName, Dictionary<string, string> eventValues)
	{
	}

	public static void setIsDebug(bool isDebug)
	{
	}

	public static void setIsSandbox(bool isSandbox)
	{
	}

	public static void getConversionData()
	{
	}

	public static string getAppsFlyerId()
	{
		return null;
	}

	public static void handleOpenUrl(string url, string sourceApplication, string annotation)
	{
	}
}
