using System;
using System.Collections;

public class AGSRequestScoreResponse : AGSRequestResponse
{
	public string leaderboardId;

	public LeaderboardScope scope;

	public int rank;

	public long score;

	public static AGSRequestScoreResponse FromJSON(string json)
	{
		try
		{
			AGSRequestScoreResponse aGSRequestScoreResponse = new AGSRequestScoreResponse();
			Hashtable hashtable = json.hashtableFromJson();
			aGSRequestScoreResponse.error = (hashtable.ContainsKey("error") ? hashtable["error"].ToString() : "");
			aGSRequestScoreResponse.userData = (hashtable.ContainsKey("userData") ? int.Parse(hashtable["userData"].ToString()) : 0);
			aGSRequestScoreResponse.leaderboardId = (hashtable.ContainsKey("leaderboardId") ? hashtable["leaderboardId"].ToString() : "");
			aGSRequestScoreResponse.rank = (hashtable.ContainsKey("rank") ? int.Parse(hashtable["rank"].ToString()) : (-1));
			aGSRequestScoreResponse.score = (hashtable.ContainsKey("score") ? long.Parse(hashtable["score"].ToString()) : (-1));
			aGSRequestScoreResponse.scope = (LeaderboardScope)Enum.Parse(typeof(LeaderboardScope), hashtable["scope"].ToString());
			return aGSRequestScoreResponse;
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError(ex.ToString());
			return GetBlankResponseWithError("ERROR_PARSING_JSON");
		}
	}

	public static AGSRequestScoreResponse GetBlankResponseWithError(string error, string leaderboardId = "", LeaderboardScope scope = LeaderboardScope.GlobalAllTime, int userData = 0)
	{
		return new AGSRequestScoreResponse
		{
			error = error,
			userData = userData,
			leaderboardId = leaderboardId,
			scope = scope,
			rank = -1,
			score = -1L
		};
	}

	public static AGSRequestScoreResponse GetPlatformNotSupportedResponse(string leaderboardId, LeaderboardScope scope, int userData)
	{
		return GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, scope, userData);
	}
}
