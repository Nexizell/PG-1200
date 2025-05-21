using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSAchievementsClient : MonoBehaviour
{
	public static event Action<AGSUpdateAchievementResponse> UpdateAchievementCompleted;

	public static event Action<AGSRequestAchievementsResponse> RequestAchievementsCompleted;

	public static event Action<AGSRequestAchievementsForPlayerResponse> RequestAchievementsForPlayerCompleted;

	[Obsolete("UpdateAchievementFailedEvent is deprecated. Use UpdateAchievementCompleted instead.")]
	public static event Action<string, string> UpdateAchievementFailedEvent;

	[Obsolete("UpdateAchievementSucceededEvent is deprecated. Use UpdateAchievementCompleted instead.")]
	public static event Action<string> UpdateAchievementSucceededEvent;

	[Obsolete("RequestAchievementsSucceededEvent is deprecated. Use RequestAchievementsCompleted instead.")]
	public static event Action<List<AGSAchievement>> RequestAchievementsSucceededEvent;

	[Obsolete("RequestAchievementsFailedEvent is deprecated. Use RequestAchievementsCompleted instead.")]
	public static event Action<string> RequestAchievementsFailedEvent;

	static AGSAchievementsClient()
	{
	}

	public static void UpdateAchievementProgress(string achievementId, float progress, int userData = 0)
	{
		AGSUpdateAchievementResponse platformNotSupportedResponse = AGSUpdateAchievementResponse.GetPlatformNotSupportedResponse(achievementId, userData);
		if (AGSAchievementsClient.UpdateAchievementFailedEvent != null)
		{
			AGSAchievementsClient.UpdateAchievementFailedEvent(platformNotSupportedResponse.achievementId, platformNotSupportedResponse.error);
		}
		if (AGSAchievementsClient.UpdateAchievementCompleted != null)
		{
			AGSAchievementsClient.UpdateAchievementCompleted(platformNotSupportedResponse);
		}
	}

	public static void RequestAchievements(int userData = 0)
	{
		AGSRequestAchievementsResponse platformNotSupportedResponse = AGSRequestAchievementsResponse.GetPlatformNotSupportedResponse(userData);
		if (AGSAchievementsClient.RequestAchievementsFailedEvent != null)
		{
			AGSAchievementsClient.RequestAchievementsFailedEvent(platformNotSupportedResponse.error);
		}
		if (AGSAchievementsClient.RequestAchievementsCompleted != null)
		{
			AGSAchievementsClient.RequestAchievementsCompleted(platformNotSupportedResponse);
		}
	}

	public static void RequestAchievementsForPlayer(string playerId, int userData = 0)
	{
		if (AGSAchievementsClient.RequestAchievementsForPlayerCompleted != null)
		{
			AGSAchievementsClient.RequestAchievementsForPlayerCompleted(AGSRequestAchievementsForPlayerResponse.GetPlatformNotSupportedResponse(playerId, userData));
		}
	}

	public static void ShowAchievementsOverlay()
	{
	}

	public static void RequestAchievementsSucceeded(string json)
	{
		AGSRequestAchievementsResponse aGSRequestAchievementsResponse = AGSRequestAchievementsResponse.FromJSON(json);
		if (!aGSRequestAchievementsResponse.IsError() && AGSAchievementsClient.RequestAchievementsSucceededEvent != null)
		{
			AGSAchievementsClient.RequestAchievementsSucceededEvent(aGSRequestAchievementsResponse.achievements);
		}
		if (AGSAchievementsClient.RequestAchievementsCompleted != null)
		{
			AGSAchievementsClient.RequestAchievementsCompleted(aGSRequestAchievementsResponse);
		}
	}

	public static void RequestAchievementsFailed(string json)
	{
		AGSRequestAchievementsResponse aGSRequestAchievementsResponse = AGSRequestAchievementsResponse.FromJSON(json);
		if (aGSRequestAchievementsResponse.IsError() && AGSAchievementsClient.RequestAchievementsFailedEvent != null)
		{
			AGSAchievementsClient.RequestAchievementsFailedEvent(aGSRequestAchievementsResponse.error);
		}
		if (AGSAchievementsClient.RequestAchievementsCompleted != null)
		{
			AGSAchievementsClient.RequestAchievementsCompleted(aGSRequestAchievementsResponse);
		}
	}

	public static void UpdateAchievementFailed(string json)
	{
		AGSUpdateAchievementResponse aGSUpdateAchievementResponse = AGSUpdateAchievementResponse.FromJSON(json);
		if (aGSUpdateAchievementResponse.IsError() && AGSAchievementsClient.UpdateAchievementFailedEvent != null)
		{
			AGSAchievementsClient.UpdateAchievementFailedEvent(aGSUpdateAchievementResponse.achievementId, aGSUpdateAchievementResponse.error);
		}
		if (AGSAchievementsClient.UpdateAchievementCompleted != null)
		{
			AGSAchievementsClient.UpdateAchievementCompleted(aGSUpdateAchievementResponse);
		}
	}

	public static void UpdateAchievementSucceeded(string json)
	{
		AGSUpdateAchievementResponse aGSUpdateAchievementResponse = AGSUpdateAchievementResponse.FromJSON(json);
		if (!aGSUpdateAchievementResponse.IsError() && AGSAchievementsClient.UpdateAchievementSucceededEvent != null)
		{
			AGSAchievementsClient.UpdateAchievementSucceededEvent(aGSUpdateAchievementResponse.achievementId);
		}
		if (AGSAchievementsClient.UpdateAchievementCompleted != null)
		{
			AGSAchievementsClient.UpdateAchievementCompleted(aGSUpdateAchievementResponse);
		}
	}

	public static void RequestAchievementsForPlayerComplete(string json)
	{
		if (AGSAchievementsClient.RequestAchievementsForPlayerCompleted != null)
		{
			AGSAchievementsClient.RequestAchievementsForPlayerCompleted(AGSRequestAchievementsForPlayerResponse.FromJSON(json));
		}
	}
}
