using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestScoresResponse : AGSRequestResponse
{
	public string leaderboardId;

	public AGSLeaderboard leaderboard;

	public LeaderboardScope scope;

	public List<AGSScore> scores;

	public static AGSRequestScoresResponse FromJSON(string json)
	{
		try
		{
			AGSRequestScoresResponse aGSRequestScoresResponse = new AGSRequestScoresResponse();
			Hashtable hashtable = json.hashtableFromJson();
			aGSRequestScoresResponse.error = (hashtable.ContainsKey("error") ? hashtable["error"].ToString() : "");
			aGSRequestScoresResponse.userData = (hashtable.ContainsKey("userData") ? int.Parse(hashtable["userData"].ToString()) : 0);
			aGSRequestScoresResponse.leaderboardId = (hashtable.ContainsKey("leaderboardId") ? hashtable["leaderboardId"].ToString() : "");
			if (hashtable.ContainsKey("leaderboard"))
			{
				aGSRequestScoresResponse.leaderboard = AGSLeaderboard.fromHashtable(hashtable["leaderboard"] as Hashtable);
			}
			else
			{
				aGSRequestScoresResponse.leaderboard = AGSLeaderboard.GetBlankLeaderboard();
			}
			aGSRequestScoresResponse.scores = new List<AGSScore>();
			if (hashtable.Contains("scores"))
			{
				foreach (Hashtable item in hashtable["scores"] as ArrayList)
				{
					aGSRequestScoresResponse.scores.Add(AGSScore.fromHashtable(item));
				}
			}
			aGSRequestScoresResponse.scope = (LeaderboardScope)Enum.Parse(typeof(LeaderboardScope), hashtable["scope"].ToString());
			return aGSRequestScoresResponse;
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError(ex.ToString());
			return GetBlankResponseWithError("ERROR_PARSING_JSON");
		}
	}

	public static AGSRequestScoresResponse GetBlankResponseWithError(string error, string leaderboardId = "", LeaderboardScope scope = LeaderboardScope.GlobalAllTime, int userData = 0)
	{
		return new AGSRequestScoresResponse
		{
			error = error,
			userData = userData,
			leaderboardId = leaderboardId,
			scope = scope,
			scores = new List<AGSScore>()
		};
	}

	public static AGSRequestScoresResponse GetPlatformNotSupportedResponse(string leaderboardId, LeaderboardScope scope, int userData)
	{
		return GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, scope, userData);
	}
}
