using System;
using System.Collections;
using System.Collections.Generic;

public class AGSRequestPercentilesResponse : AGSRequestResponse
{
	public string leaderboardId;

	public AGSLeaderboard leaderboard;

	public List<AGSLeaderboardPercentile> percentiles;

	public int userIndex;

	public LeaderboardScope scope;

	public static AGSRequestPercentilesResponse FromJSON(string json)
	{
		try
		{
			AGSRequestPercentilesResponse aGSRequestPercentilesResponse = new AGSRequestPercentilesResponse();
			Hashtable hashtable = json.hashtableFromJson();
			aGSRequestPercentilesResponse.error = (hashtable.ContainsKey("error") ? hashtable["error"].ToString() : "");
			aGSRequestPercentilesResponse.userData = (hashtable.ContainsKey("userData") ? int.Parse(hashtable["userData"].ToString()) : 0);
			aGSRequestPercentilesResponse.leaderboardId = (hashtable.ContainsKey("leaderboardId") ? hashtable["leaderboardId"].ToString() : "");
			if (hashtable.ContainsKey("leaderboard"))
			{
				aGSRequestPercentilesResponse.leaderboard = AGSLeaderboard.fromHashtable(hashtable["leaderboard"] as Hashtable);
			}
			else
			{
				aGSRequestPercentilesResponse.leaderboard = AGSLeaderboard.GetBlankLeaderboard();
			}
			aGSRequestPercentilesResponse.percentiles = new List<AGSLeaderboardPercentile>();
			if (hashtable.Contains("percentiles"))
			{
				foreach (Hashtable item in hashtable["percentiles"] as ArrayList)
				{
					aGSRequestPercentilesResponse.percentiles.Add(AGSLeaderboardPercentile.fromHashTable(item));
				}
			}
			aGSRequestPercentilesResponse.userIndex = (hashtable.ContainsKey("userIndex") ? int.Parse(hashtable["userIndex"].ToString()) : (-1));
			aGSRequestPercentilesResponse.scope = (LeaderboardScope)Enum.Parse(typeof(LeaderboardScope), hashtable["scope"].ToString());
			return aGSRequestPercentilesResponse;
		}
		catch (Exception ex)
		{
			AGSClient.LogGameCircleError(ex.ToString());
			return GetBlankResponseWithError("ERROR_PARSING_JSON");
		}
	}

	public static AGSRequestPercentilesResponse GetBlankResponseWithError(string error, string leaderboardId = "", LeaderboardScope scope = LeaderboardScope.GlobalAllTime, int userData = 0)
	{
		AGSRequestPercentilesResponse aGSRequestPercentilesResponse = new AGSRequestPercentilesResponse();
		aGSRequestPercentilesResponse.error = error;
		aGSRequestPercentilesResponse.userData = userData;
		aGSRequestPercentilesResponse.leaderboardId = leaderboardId;
		aGSRequestPercentilesResponse.scope = scope;
		aGSRequestPercentilesResponse.leaderboard = AGSLeaderboard.GetBlankLeaderboard();
		aGSRequestPercentilesResponse.percentiles = new List<AGSLeaderboardPercentile>();
		aGSRequestPercentilesResponse.userIndex = -1;
		aGSRequestPercentilesResponse.scope = scope;
		return aGSRequestPercentilesResponse;
	}

	public static AGSRequestPercentilesResponse GetPlatformNotSupportedResponse(string leaderboardId, LeaderboardScope scope, int userData)
	{
		return GetBlankResponseWithError("PLATFORM_NOT_SUPPORTED", leaderboardId, scope, userData);
	}
}
