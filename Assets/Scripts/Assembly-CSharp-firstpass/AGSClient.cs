using System;
using UnityEngine;

public class AGSClient : MonoBehaviour
{
	public const string serviceName = "AmazonGameCircle";

	public const AmazonLogging.AmazonLoggingLevel errorLevel = AmazonLogging.AmazonLoggingLevel.Verbose;

	public static bool ReinitializeOnFocus;

	private static bool IsReady;

	private static bool supportsAchievements;

	private static bool supportsLeaderboards;

	private static bool supportsWhispersync;

	private static readonly string serviceUnavailableOnPlatform;

	public static event Action ServiceReadyEvent;

	public static event Action<string> ServiceNotReadyEvent;

	static AGSClient()
	{
		ReinitializeOnFocus = false;
		IsReady = false;
		supportsAchievements = false;
		supportsLeaderboards = false;
		supportsWhispersync = false;
		serviceUnavailableOnPlatform = "Amazon GameCircle is not available on current platform.";
	}

	public static void Init()
	{
		Init(supportsLeaderboards, supportsAchievements, supportsWhispersync);
	}

	public static void Init(bool supportsLeaderboards, bool supportsAchievements, bool supportsWhispersync)
	{
		ReinitializeOnFocus = true;
		AGSClient.supportsAchievements = supportsAchievements;
		AGSClient.supportsLeaderboards = supportsLeaderboards;
		AGSClient.supportsWhispersync = supportsWhispersync;
		ServiceNotReady(serviceUnavailableOnPlatform);
	}

	public static void SetPopUpEnabled(bool enabled)
	{
	}

	public static void SetPopUpLocation(GameCirclePopupLocation location)
	{
	}

	public static void ServiceReady(string empty)
	{
		Log("Client GameCircle - Service is ready");
		IsReady = true;
		if (AGSClient.ServiceReadyEvent != null)
		{
			AGSClient.ServiceReadyEvent();
		}
	}

	public static bool IsServiceReady()
	{
		return IsReady;
	}

	public static void release()
	{
	}

	public static void Shutdown()
	{
	}

	public static void ServiceNotReady(string param)
	{
		IsReady = false;
		if (AGSClient.ServiceNotReadyEvent != null)
		{
			AGSClient.ServiceNotReadyEvent(param);
		}
	}

	public static void ShowGameCircleOverlay()
	{
	}

	public static void ShowSignInPage()
	{
	}

	public static void LogGameCircleError(string errorMessage)
	{
		AmazonLogging.LogError(AmazonLogging.AmazonLoggingLevel.Verbose, "AmazonGameCircle", errorMessage);
	}

	public static void LogGameCircleWarning(string errorMessage)
	{
		AmazonLogging.LogWarning(AmazonLogging.AmazonLoggingLevel.Verbose, "AmazonGameCircle", errorMessage);
	}

	public static void Log(string message)
	{
		AmazonLogging.Log(AmazonLogging.AmazonLoggingLevel.Verbose, "AmazonGameCircle", message);
	}
}
