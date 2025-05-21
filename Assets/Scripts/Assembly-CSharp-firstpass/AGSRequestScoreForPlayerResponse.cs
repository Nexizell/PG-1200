using System;
using System.Collections;

public class AGSRequestScoreForPlayerResponse : AGSRequestScoreResponse
{
	public string playerId;

	public new static AGSRequestScoreForPlayerResponse FromJSON(string json)
	{
		try
		{
			AGSRequestScoreForPlayerResponse aGSRequestScoreForPlayerResponse = new AGSRequestScoreForPlayerResponse();
			Hashtable hashtable = json.hashtableFromJson();
			aGSRequestScoreForPlayerResponse.error = (hashtable.ContainsKey("error") ? hashtable["error"].ToString() : "");
			aGSRequestScoreForPlayerResponse.userData = (hashtable.ContainsKey("userData") ? int.Parse(hashtable["userData"].ToString()) : 0);
			aGSRequestScoreForPlayerResponse.leaderboardId = (hashtable.ContainsKey("leaderboardId") ? hashtable["leaderboardId"].ToString() : "");
			aGSRequestScoreForPlayerResponse.rank = (hashtable.ContainsKey("rank") ? int.Parse(hashtable["rank"].ToString()) : (-1));
			aGSRequestScoreForPlayerResponse.score = (hashtable.ContainsKey("score") ? long.Parse(hashtable["score"].ToString()) : (-1));
			aGSRequestScoreForPlayerResponse.scope = (LeaderboardScope)Enum.Parse(typeof(LeaderboardScope), hashtable["scope"].ToString());
			aGSRequestScoreForPlayerResponse.playerId = (hashtable.Contains("playerId") ? hashtable["playerId"].ToString() : "");
			return aGSRequestScoreForPlayerResponse;
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError(ex.ToString());
			return GetBlankResponseWithError("ERROR_PARSING_JSON");
		}
	}

	public static AGSRequestScoreForPlayerResponse GetBlankResponseWithError(string error, string leaderboardId = "", string playerId = "", LeaderboardScope scope = LeaderboardScope.GlobalAllTime, int userData = 0)
	{
		return new AGSRequestScoreForPlayerResponse
		{
			error = error,
			playerId = playerId,
			userData = userData,
			leaderboardId = leaderboardId,
			scope = scope,
			rank = -1,
			score = -1L
		};
	}

	public static AGSRequestScoreForPlayerResponse GetPlatformNotSupportedResponse(string leaderboardId, string playerId, LeaderboardScope scope, int userData)
	{
		return GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, playerId, scope, userData);
	}
}
