using System;
using System.Collections.Generic;
using UnityEngine;

public class AGSLeaderboardsClient : MonoBehaviour
{
	public static event Action<AGSSubmitScoreResponse> SubmitScoreCompleted;

	public static event Action<AGSRequestLeaderboardsResponse> RequestLeaderboardsCompleted;

	public static event Action<AGSRequestScoreResponse> RequestLocalPlayerScoreCompleted;

	public static event Action<AGSRequestScoreForPlayerResponse> RequestScoreForPlayerCompleted;

	public static event Action<AGSRequestScoresResponse> RequestScoresCompleted;

	public static event Action<AGSRequestPercentilesResponse> RequestPercentileRanksCompleted;

	public static event Action<AGSRequestPercentilesForPlayerResponse> RequestPercentileRanksForPlayerCompleted;

	[Obsolete("SubmitScoreFailedEvent is deprecated. Use SubmitScoreCompleted instead.")]
	public static event Action<string, string> SubmitScoreFailedEvent;

	[Obsolete("SubmitScoreSucceededEvent is deprecated. Use SubmitScoreCompleted instead.")]
	public static event Action<string> SubmitScoreSucceededEvent;

	[Obsolete("RequestLeaderboardsFailedEvent is deprecated. Use RequestLeaderboardsCompleted instead.")]
	public static event Action<string> RequestLeaderboardsFailedEvent;

	[Obsolete("RequestLeaderboardsSucceededEvent is deprecated. Use RequestLeaderboardsCompleted instead.")]
	public static event Action<List<AGSLeaderboard>> RequestLeaderboardsSucceededEvent;

	[Obsolete("RequestLocalPlayerScoreFailedEvent is deprecated. Use RequestLocalPlayerScoreCompleted instead.")]
	public static event Action<string, string> RequestLocalPlayerScoreFailedEvent;

	[Obsolete("RequestLocalPlayerScoreSucceededEvent is deprecated. Use RequestLocalPlayerScoreCompleted instead.")]
	public static event Action<string, int, long> RequestLocalPlayerScoreSucceededEvent;

	[Obsolete("RequestPercentileRanksFailedEvent is deprecated. Use RequestPercentileRanksCompleted instead.")]
	public static event Action<string, string> RequestPercentileRanksFailedEvent;

	[Obsolete("RequestPercentileRanksSucceededEvent is deprecated. Use RequestPercentileRanksCompleted instead.")]
	public static event Action<AGSLeaderboard, List<AGSLeaderboardPercentile>, int> RequestPercentileRanksSucceededEvent;

	static AGSLeaderboardsClient()
	{
	}

	public static void SubmitScore(string leaderboardId, long score, int userData = 0)
	{
		AGSSubmitScoreResponse platformNotSupportedResponse = AGSSubmitScoreResponse.GetPlatformNotSupportedResponse(leaderboardId, userData);
		if (AGSLeaderboardsClient.SubmitScoreFailedEvent != null)
		{
			AGSLeaderboardsClient.SubmitScoreFailedEvent(platformNotSupportedResponse.leaderboardId, platformNotSupportedResponse.error);
		}
		if (AGSLeaderboardsClient.SubmitScoreCompleted != null)
		{
			AGSLeaderboardsClient.SubmitScoreCompleted(platformNotSupportedResponse);
		}
	}

	public static void ShowLeaderboardsOverlay()
	{
	}

	public static void RequestLeaderboards(int userData = 0)
	{
		AGSRequestLeaderboardsResponse platformNotSupportedResponse = AGSRequestLeaderboardsResponse.GetPlatformNotSupportedResponse(userData);
		if (AGSLeaderboardsClient.RequestLeaderboardsFailedEvent != null)
		{
			AGSLeaderboardsClient.RequestLeaderboardsFailedEvent(platformNotSupportedResponse.error);
		}
		if (AGSLeaderboardsClient.RequestLeaderboardsCompleted != null)
		{
			AGSLeaderboardsClient.RequestLeaderboardsCompleted(platformNotSupportedResponse);
		}
	}

	public static void RequestLocalPlayerScore(string leaderboardId, LeaderboardScope scope, int userData = 0)
	{
		AGSRequestScoreResponse platformNotSupportedResponse = AGSRequestScoreResponse.GetPlatformNotSupportedResponse(leaderboardId, scope, userData);
		if (AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent != null)
		{
			AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent(platformNotSupportedResponse.leaderboardId, platformNotSupportedResponse.error);
		}
		if (AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted != null)
		{
			AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted(platformNotSupportedResponse);
		}
	}

	public static void RequestScoreForPlayer(string leaderboardId, string playerId, LeaderboardScope scope, int userData = 0)
	{
		if (AGSLeaderboardsClient.RequestScoreForPlayerCompleted != null)
		{
			AGSLeaderboardsClient.RequestScoreForPlayerCompleted(AGSRequestScoreForPlayerResponse.GetPlatformNotSupportedResponse(leaderboardId, playerId, scope, userData));
		}
	}

	public static void RequestScores(string leaderboardId, LeaderboardScope scope, int userData = 0)
	{
		if (AGSLeaderboardsClient.RequestScoresCompleted != null)
		{
			AGSLeaderboardsClient.RequestScoresCompleted(AGSRequestScoresResponse.GetPlatformNotSupportedResponse(leaderboardId, scope, userData));
		}
	}

	public static void RequestPercentileRanks(string leaderboardId, LeaderboardScope scope, int userData = 0)
	{
		AGSRequestPercentilesResponse platformNotSupportedResponse = AGSRequestPercentilesResponse.GetPlatformNotSupportedResponse(leaderboardId, scope, userData);
		if (AGSLeaderboardsClient.RequestPercentileRanksFailedEvent != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksFailedEvent(platformNotSupportedResponse.leaderboardId, platformNotSupportedResponse.error);
		}
		if (AGSLeaderboardsClient.RequestPercentileRanksCompleted != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksCompleted(platformNotSupportedResponse);
		}
	}

	public static void RequestPercentileRanksForPlayer(string leaderboardId, string playerId, LeaderboardScope scope, int userData = 0)
	{
		if (AGSLeaderboardsClient.RequestPercentileRanksForPlayerCompleted != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksForPlayerCompleted(AGSRequestPercentilesForPlayerResponse.GetPlatformNotSupportedResponse(leaderboardId, playerId, scope, userData));
		}
	}

	public static void SubmitScoreFailed(string json)
	{
		AGSSubmitScoreResponse aGSSubmitScoreResponse = AGSSubmitScoreResponse.FromJSON(json);
		if (aGSSubmitScoreResponse.IsError() && AGSLeaderboardsClient.SubmitScoreFailedEvent != null)
		{
			AGSLeaderboardsClient.SubmitScoreFailedEvent(aGSSubmitScoreResponse.leaderboardId, aGSSubmitScoreResponse.error);
		}
		if (AGSLeaderboardsClient.SubmitScoreCompleted != null)
		{
			AGSLeaderboardsClient.SubmitScoreCompleted(aGSSubmitScoreResponse);
		}
	}

	public static void SubmitScoreSucceeded(string json)
	{
		AGSSubmitScoreResponse aGSSubmitScoreResponse = AGSSubmitScoreResponse.FromJSON(json);
		if (!aGSSubmitScoreResponse.IsError() && AGSLeaderboardsClient.SubmitScoreSucceededEvent != null)
		{
			AGSLeaderboardsClient.SubmitScoreSucceededEvent(aGSSubmitScoreResponse.leaderboardId);
		}
		if (AGSLeaderboardsClient.SubmitScoreCompleted != null)
		{
			AGSLeaderboardsClient.SubmitScoreCompleted(aGSSubmitScoreResponse);
		}
	}

	public static void RequestLeaderboardsFailed(string json)
	{
		AGSRequestLeaderboardsResponse aGSRequestLeaderboardsResponse = AGSRequestLeaderboardsResponse.FromJSON(json);
		if (aGSRequestLeaderboardsResponse.IsError() && AGSLeaderboardsClient.RequestLeaderboardsFailedEvent != null)
		{
			AGSLeaderboardsClient.RequestLeaderboardsFailedEvent(aGSRequestLeaderboardsResponse.error);
		}
		if (AGSLeaderboardsClient.RequestLeaderboardsCompleted != null)
		{
			AGSLeaderboardsClient.RequestLeaderboardsCompleted(aGSRequestLeaderboardsResponse);
		}
	}

	public static void RequestLeaderboardsSucceeded(string json)
	{
		AGSRequestLeaderboardsResponse aGSRequestLeaderboardsResponse = AGSRequestLeaderboardsResponse.FromJSON(json);
		if (!aGSRequestLeaderboardsResponse.IsError() && AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent != null)
		{
			AGSLeaderboardsClient.RequestLeaderboardsSucceededEvent(aGSRequestLeaderboardsResponse.leaderboards);
		}
		if (AGSLeaderboardsClient.RequestLeaderboardsCompleted != null)
		{
			AGSLeaderboardsClient.RequestLeaderboardsCompleted(aGSRequestLeaderboardsResponse);
		}
	}

	public static void RequestLocalPlayerScoreFailed(string json)
	{
		AGSRequestScoreResponse aGSRequestScoreResponse = AGSRequestScoreResponse.FromJSON(json);
		if (aGSRequestScoreResponse.IsError() && AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent != null)
		{
			AGSLeaderboardsClient.RequestLocalPlayerScoreFailedEvent(aGSRequestScoreResponse.leaderboardId, aGSRequestScoreResponse.error);
		}
		if (AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted != null)
		{
			AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted(aGSRequestScoreResponse);
		}
	}

	public static void RequestLocalPlayerScoreSucceeded(string json)
	{
		AGSRequestScoreResponse aGSRequestScoreResponse = AGSRequestScoreResponse.FromJSON(json);
		if (!aGSRequestScoreResponse.IsError() && AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent != null)
		{
			AGSLeaderboardsClient.RequestLocalPlayerScoreSucceededEvent(aGSRequestScoreResponse.leaderboardId, aGSRequestScoreResponse.rank, aGSRequestScoreResponse.score);
		}
		if (AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted != null)
		{
			AGSLeaderboardsClient.RequestLocalPlayerScoreCompleted(aGSRequestScoreResponse);
		}
	}

	public static void RequestScoreForPlayerComplete(string json)
	{
		if (AGSLeaderboardsClient.RequestScoreForPlayerCompleted != null)
		{
			AGSLeaderboardsClient.RequestScoreForPlayerCompleted(AGSRequestScoreForPlayerResponse.FromJSON(json));
		}
	}

	public static void RequestScoresSucceeded(string json)
	{
		if (AGSLeaderboardsClient.RequestScoresCompleted != null)
		{
			AGSLeaderboardsClient.RequestScoresCompleted(AGSRequestScoresResponse.FromJSON(json));
		}
	}

	public static void RequestScoresFailed(string json)
	{
		if (AGSLeaderboardsClient.RequestScoresCompleted != null)
		{
			AGSLeaderboardsClient.RequestScoresCompleted(AGSRequestScoresResponse.FromJSON(json));
		}
	}

	public static void RequestPercentileRanksFailed(string json)
	{
		AGSRequestPercentilesResponse aGSRequestPercentilesResponse = AGSRequestPercentilesResponse.FromJSON(json);
		if (aGSRequestPercentilesResponse.IsError() && AGSLeaderboardsClient.RequestPercentileRanksFailedEvent != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksFailedEvent(aGSRequestPercentilesResponse.leaderboardId, aGSRequestPercentilesResponse.error);
		}
		if (AGSLeaderboardsClient.RequestPercentileRanksCompleted != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksCompleted(aGSRequestPercentilesResponse);
		}
	}

	public static void RequestPercentileRanksSucceeded(string json)
	{
		AGSRequestPercentilesResponse aGSRequestPercentilesResponse = AGSRequestPercentilesResponse.FromJSON(json);
		if (!aGSRequestPercentilesResponse.IsError() && AGSLeaderboardsClient.RequestPercentileRanksSucceededEvent != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksSucceededEvent(aGSRequestPercentilesResponse.leaderboard, aGSRequestPercentilesResponse.percentiles, aGSRequestPercentilesResponse.userIndex);
		}
		if (AGSLeaderboardsClient.RequestPercentileRanksCompleted != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksCompleted(aGSRequestPercentilesResponse);
		}
	}

	public static void RequestPercentileRanksForPlayerComplete(string json)
	{
		if (AGSLeaderboardsClient.RequestPercentileRanksForPlayerCompleted != null)
		{
			AGSLeaderboardsClient.RequestPercentileRanksForPlayerCompleted(AGSRequestPercentilesForPlayerResponse.FromJSON(json));
		}
	}
}
